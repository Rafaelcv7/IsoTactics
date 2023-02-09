using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;
using static IsoTactics.ArrowTranslator;

namespace IsoTactics
{
    public class MouseController : MonoBehaviour
    {
        public GameObject cursor;
        public GameObject characterPrefab;

        public CharacterInfo character;
        private CharacterStateManager _stateManager;
        private PathFinder _pathFinder;
        private RangeFinder _rangeFinder;
        private ArrowTranslator _arrowTranslator;
        private List<OverlayTile> _path;
        private List<OverlayTile> _rangeFinderTiles;
        private bool _isMoving;
        private OverlayTile _tile;
        private TurnManager _turnManager;
        private Vector3 _previousMousePosition;

        private void Start()
        {
            _turnManager = gameObject.GetComponent<TurnManager>();
            _pathFinder = new PathFinder();
            _rangeFinder = new RangeFinder();
            _arrowTranslator = new ArrowTranslator();
            _path = new List<OverlayTile>();
            _isMoving = false;
            _rangeFinderTiles = new List<OverlayTile>();
            cursor = Instantiate(cursor);
            _previousMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        void Update()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                _tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                cursor.transform.position = _tile.transform.position;
                cursor.gameObject.GetComponentsInChildren<SpriteRenderer>()[0].sortingOrder =
                    cursor.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder =
                        _tile.GetComponent<SpriteRenderer>().sortingOrder;

                if (_turnManager.onPosPhase && _turnManager.charactersPrefabs.Count != 0)
                {
                    PositionCharacter();
                }
                else if (character && character.movementPoints != 0)
                {
                    GetInRangeTiles();
                    if (_rangeFinderTiles.Contains(_tile) && !_isMoving)
                    {
                        _path = _pathFinder.FindPath(character.standingOnTile, _tile, _rangeFinderTiles);

                        foreach (var item in _rangeFinderTiles)
                        {
                            MapManager.Instance.Map[item.Grid2DLocation].SetSprite(ArrowTranslator.ArrowDirection.None);
                        }

                        for (var i = 0; i < _path.Count; i++)
                        {
                            var previousTile = i > 0 ? _path[i - 1] : character.standingOnTile;
                            var futureTile = i < _path.Count - 1 ? _path[i + 1] : null;

                            var arrow = _arrowTranslator.TranslateDirection(previousTile, _path[i], futureTile);
                            _path[i].SetSprite(arrow);
                        }
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        _tile.ShowTile();
                        _isMoving = true;
                        _tile.HideTile();
                    } 
                }
            }

            if (_path.Count > 0 && _isMoving && _rangeFinderTiles.Contains(_tile))// && characterMovementPoints
            {
                _rangeFinderTiles.ForEach(x => x.HideTile());
                _path.ForEach(x => x.SetSprite(ArrowDirection.None));
                character.standingOnTile.isBlocked = false;
                MoveAlongPath();
            }
            else
            {
                _isMoving = false;
            }
        }

        private void MoveAlongPath()
        {
            var step = character.movementSpeed * Time.deltaTime;
            
            _stateManager.EvaluateState(_path[0], _isMoving);
            
            
            var zIndex = _path[0].transform.position.z;
            character.transform.position = Vector2.MoveTowards(character.transform.position, _path[0].transform.position, step);
            character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

            if (Vector2.Distance(character.transform.position, _path[0].transform.position) < 0.00001f)
            {
                PositionCharacterOnLine(_path[0]);
                _path.RemoveAt(0);
                character.movementPoints--;
            }

            if (_path.Count == 0)
            {
                character.standingOnTile.isBlocked = true;
                _isMoving = false;
                GetInRangeTiles();
                _stateManager.EvaluateState(null, _isMoving);
                
                if (character.movementPoints == 0)
                {
                    _turnManager.PassTurn();
                    character = null;
                }
            }
        }

        private void PositionCharacterOnLine(OverlayTile tile)
        {
            character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
            character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
            character.standingOnTile = tile;
        }

        private static RaycastHit2D? GetFocusedOnTile()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousePos2D = new Vector2(mousePos.x, mousePos.y);

            var hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            if (hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }

        private void GetInRangeTiles()
        {
            _rangeFinderTiles = _rangeFinder.GetTilesInRange(new Vector2Int(character.standingOnTile.gridLocation.x, character.standingOnTile.gridLocation.y), character.movementPoints, character.jumpHeight);

            foreach (var item in _rangeFinderTiles.Where(item => character.movementPoints > 0))
            {
                item.ShowTile();
            }
        }

        public void StartTurn(CharacterInfo currentCharacter)
        {
            character = currentCharacter;
            _stateManager = character.GetComponent<CharacterStateManager>();
            character.RestartStats();
        }

        private void PositionCharacter()
        {
            cursor.GetComponentsInChildren<SpriteRenderer>()[1].sprite =
                characterPrefab.GetComponent<SpriteRenderer>().sprite;
            
            if (Input.GetMouseButtonDown(0))
            {
                _turnManager.charactersContainer.Add(Instantiate(characterPrefab).GetComponent<CharacterInfo>());
                character = _turnManager.charactersContainer[_turnManager.currentTurn];
                PositionCharacterOnLine(_tile);
                _tile.isBlocked = true;
                cursor.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].sprite = null;
                _stateManager = character.GetComponent<CharacterStateManager>();
                character = null;
                _turnManager.PassTurn();
            } 
        }
    }
}
