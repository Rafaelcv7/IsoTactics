using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IsoTactics;
using UnityEngine;
using CharacterInfo = IsoTactics.CharacterInfo;
using static IsoTactics.ArrowTranslator;

namespace IsoTactics
{
    public class MovementController : MonoBehaviour
    {
        public CharacterInfo activeCharacter;
        private CharacterStateManager _characterStateManager;
        private PathFinder _pathFinder;
        private RangeFinder _rangeFinder;
        private ArrowTranslator _arrowTranslator;
        private List<OverlayTile> _path;
        private List<OverlayTile> _inRangeTiles;
        private bool _isMoving;

        private OverlayTile _focusedTile;

        [Header("Events:")] public GameEvents PassTurn;


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

            if (activeCharacter && activeCharacter.movementPoints != 0 && GamePhases.CurrentPhase.Equals("Turn"))
            {
                GetInRangeTiles();
                if (_inRangeTiles.Contains(_focusedTile) && !_isMoving)
                {
                    _path = _pathFinder.FindPath(activeCharacter.activeTile, _focusedTile, _inRangeTiles);

                    foreach (var item in _inRangeTiles)
                    {
                        MapManager.Instance.Map[item.Grid2DLocation].SetSprite(ArrowTranslator.ArrowDirection.None);
                    }

                    for (var i = 0; i < _path.Count; i++)
                    {
                        var previousTile = i > 0 ? _path[i - 1] : activeCharacter.activeTile;
                        var futureTile = i < _path.Count - 1 ? _path[i + 1] : null;

                        var arrow = _arrowTranslator.TranslateDirection(previousTile, _path[i], futureTile);
                        _path[i].SetSprite(arrow);
                    }
                }
                
                if (Input.GetMouseButtonDown(0))
                {
                    _focusedTile.ShowTile();
                    _isMoving = true;
                    _focusedTile.HideTile();
                } 
                
                if (_path.Count > 0 && _isMoving && _inRangeTiles.Contains(_focusedTile))// && characterMovementPoints
                {
                    _inRangeTiles.ForEach(x => x.HideTile());
                    _path.ForEach(x => x.SetSprite(ArrowDirection.None));
                    MoveAlongPath();
                }
                else
                {
                    _isMoving = false;
                }
            }
        }
        
        private void MoveAlongPath()
        {
            var step = activeCharacter.movementSpeed * Time.deltaTime;
            
            _characterStateManager.EvaluateState(_path[0], _isMoving);
            
            
            var zIndex = _path[0].transform.position.z;
            activeCharacter.transform.position = Vector2.MoveTowards(activeCharacter.transform.position, _path[0].transform.position, step);
            activeCharacter.transform.position = new Vector3(activeCharacter.transform.position.x, activeCharacter.transform.position.y, zIndex);

            if (Vector2.Distance(activeCharacter.transform.position, _path[0].transform.position) < 0.00001f)
            {
                PositionCharacterOnLine(_path[0]);
                _path.RemoveAt(0);
                activeCharacter.movementPoints--;
            }

            if (_path.Count == 0)
            {
                _isMoving = false;
                GetInRangeTiles();
                _characterStateManager.EvaluateState(null, _isMoving);
                
                if (activeCharacter.movementPoints == 0)
                {
                    if(PassTurn)
                        PassTurn.Raise(this, null);
                    activeCharacter = null;
                }
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
                new Vector2Int(activeCharacter.activeTile.gridLocation.x, activeCharacter.activeTile.gridLocation.y),
                activeCharacter.movementPoints, activeCharacter.jumpHeight);

            foreach (var item in _inRangeTiles.Where(item => activeCharacter.movementPoints > 0))
            {
                item.ShowTile();
            }
        }
        //Called by MouseController Event
        public void NewFocusedTile(Component sender, object data)
        {
            if (data is OverlayTile tile)
            {
                _focusedTile = tile;
            }
        }
        //Called by TurnManager
        public void SetActiveCharacter(Component sender, object data)
        {
            if (data is CharacterInfo newCharacter)
            {
                activeCharacter = newCharacter;
                _characterStateManager = activeCharacter.GetComponent<CharacterStateManager>();
            }
        }
    }
}

