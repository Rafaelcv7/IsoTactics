using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using static IsoTactics.ArrowTranslator;

namespace IsoTactics
{
    public class MovementController : MonoBehaviour
    {
        public Character activeCharacter;
        [Header("Events:")]
        public GameEvents characterMoving;
        private PathFinder _pathFinder;
        private RangeFinder _rangeFinder;
        private ArrowTranslator _arrowTranslator;
        private List<OverlayTile> _path;
        private List<OverlayTile> _inRangeTiles;
        private bool _isMoving;
        private bool _isMovementEnabled = true;
        private bool _Ai;

        private OverlayTile _focusedTile;

        void Start()
        {
            _pathFinder = new PathFinder();
            _rangeFinder = new RangeFinder();
            _arrowTranslator = new ArrowTranslator();
            _path = new List<OverlayTile>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isMovementEnabled) return; // Event Activated.
            
            if (activeCharacter && !activeCharacter.isAi && activeCharacter.Stats.movementPoints.statValue != 0 && GamePhases.CurrentPhase.Equals("Turn") && _focusedTile && !_Ai)
            {
                GetInRangeTiles();
                if (_inRangeTiles.Contains(_focusedTile) && !_isMoving)
                {
                    _path = _pathFinder.FindPath(activeCharacter.activeTile, _focusedTile, _inRangeTiles);

                    foreach (var item in _inRangeTiles)
                    {
                        MapManager.Instance.Map[item.Grid2DLocation].SetSprite(ArrowDirection.None);
                    }

                    for (var i = 0; i < _path.Count; i++)
                    {
                        var previousTile = i > 0 ? _path[i - 1] : activeCharacter.activeTile;
                        var futureTile = i < _path.Count - 1 ? _path[i + 1] : null;

                        var arrow = _arrowTranslator.TranslateDirection(previousTile, _path[i], futureTile);
                        _path[i].SetSprite(arrow);
                    }
                }
                
                if (Input.GetMouseButtonDown(0) && _inRangeTiles.Contains(_focusedTile))
                {
                    _focusedTile.ShowTile();
                    _isMoving = true;
                    _focusedTile.HideTile();
                    if (characterMoving) { characterMoving.Raise(this, true); }
                }
            }
            if (_path?.Count > 0 && _isMoving)// && characterMovementPoints
            {
                // _inRangeTiles?.ForEach(x => x.HideTile());
                _path.ForEach(x => x.SetSprite(ArrowDirection.None));
                MoveAlongPath();
            }
        }
        
        private void MoveAlongPath()
        {
            var step = 3 * Time.deltaTime;
            
            activeCharacter.State.EvaluateMovingState(_path[0], _isMoving);
            
            
            var zIndex = _path[0].transform.position.z;
            activeCharacter.transform.position = Vector2.MoveTowards(activeCharacter.transform.position, _path[0].transform.position, step);
            activeCharacter.transform.position = new Vector3(activeCharacter.transform.position.x, activeCharacter.transform.position.y, zIndex);

            if (Vector2.Distance(activeCharacter.transform.position, _path[0].transform.position) < 0.00001f)
            {
                PositionCharacterOnLine(_path[0]);
                _path.RemoveAt(0);
                activeCharacter.Stats.movementPoints.statValue--;
            }

            if (_path.Count == 0)
            {
                _isMoving = false;
                _Ai = false;
                GetInRangeTiles();
                activeCharacter.State.EvaluateMovingState(null, _isMoving);
                if (characterMoving) { characterMoving.Raise(this, false); }
            }
        }
        private void PositionCharacterOnLine(OverlayTile tile)
        {
            activeCharacter.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
            activeCharacter.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
            activeCharacter.LinkCharacterToTile(tile);
        }

        private void GetInRangeTiles()
        {
            _inRangeTiles = _rangeFinder.GetTilesInRange(
                activeCharacter.activeTile.Grid2DLocation,
                activeCharacter.Stats.movementPoints.statValue);

            // foreach (var item in _inRangeTiles.Where(item => activeCharacter.movementPoints > 0))
            // {
            //     item.ShowTile();
            // }
        }

        private void CancelMovement()
        {
            _inRangeTiles?.ForEach(x => x.HideTile());
            _path?.ForEach(x => x.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1,1,1,0));
            _inRangeTiles = _path = null;
            _focusedTile = null;
        }
        
        //Called by MouseController Event
        public void NewFocusedTile(Component sender, object data)
        {
            if (data is OverlayTile tile)
            {
                _focusedTile = tile;
            }
            else if(!_Ai)
            {
                CancelMovement();
            }
        }
        //Called by TurnManager
        public void SetActiveCharacter(Component sender, object data)
        {
            _inRangeTiles?.ForEach(x => x.HideTile());
            _path?.ForEach(x => x.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1,1,1,0));
            if (data is Character newCharacter)
            {
                activeCharacter = newCharacter;
            }
        }

        public void MoveAiCommand(Component sender, object data)
        {
            if (data is List<OverlayTile> pathToFollow)
            {
                if (activeCharacter)
                {
                    _isMoving = true;
                    _Ai = true;
                    if (pathToFollow.Count > 0)
                        _path = pathToFollow;
                }
            }
        }

        public void ToggleMovement(Component sender, object data)
        {
            _isMovementEnabled = !_isMovementEnabled;
            CancelMovement();
        }
    }
}

