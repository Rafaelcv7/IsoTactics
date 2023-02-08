using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static IsoTactics.ArrowTranslator;

namespace IsoTactics
{
    public class MouseController : MonoBehaviour
    {
        public GameObject cursor;
        public GameObject characterPrefab;

        private CharacterInfo _character;
        private CharacterState _stateMachine;
        private PathFinder _pathFinder;
        private RangeFinder _rangeFinder;
        private ArrowTranslator _arrowTranslator;
        private List<OverlayTile> _path;
        private List<OverlayTile> _rangeFinderTiles;
        private bool _isMoving;
        private OverlayTile _tile;
        private string suffix = "";

        private void Start()
        {
            _pathFinder = new PathFinder();
            _rangeFinder = new RangeFinder();
            _arrowTranslator = new ArrowTranslator();
            _path = new List<OverlayTile>();
            _isMoving = false;
            _rangeFinderTiles = new List<OverlayTile>();
            _stateMachine = new CharacterState();
            cursor = Instantiate(cursor);
            cursor.GetComponentsInChildren<SpriteRenderer>()[1].sprite =
                characterPrefab.GetComponent<SpriteRenderer>().sprite;
        }

        void LateUpdate()
        {
            RaycastHit2D? hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                _tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                cursor.transform.position = _tile.transform.position;
                cursor.gameObject.GetComponentsInChildren<SpriteRenderer>()[0].sortingOrder =
                    cursor.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder =
                        _tile.GetComponent<SpriteRenderer>().sortingOrder;

                if (_rangeFinderTiles.Contains(_tile) && !_isMoving)
                {
                    _path = _pathFinder.FindPath(_character.standingOnTile, _tile, _rangeFinderTiles);

                    foreach (var item in _rangeFinderTiles)
                    {
                        MapManager.Instance.Map[item.Grid2DLocation].SetSprite(ArrowTranslator.ArrowDirection.None);
                    }

                    for (var i = 0; i < _path.Count; i++)
                    {
                        var previousTile = i > 0 ? _path[i - 1] : _character.standingOnTile;
                        var futureTile = i < _path.Count - 1 ? _path[i + 1] : null;

                        var arrow = _arrowTranslator.TranslateDirection(previousTile, _path[i], futureTile);
                        _path[i].SetSprite(arrow);
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    _tile.ShowTile();

                    if (_character == null)
                    {
                        _character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                        PositionCharacterOnLine(_tile);
                        cursor.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);
                        GetInRangeTiles();
                    }
                    else
                    {
                        _isMoving = true;
                        _tile.gameObject.GetComponent<OverlayTile>().HideTile();
                    }
                }
            }

            if (_path.Count > 0 && _isMoving && _rangeFinderTiles.Contains(_tile))// && characterMovementPoints
            {
                _rangeFinderTiles.ForEach(x => x.HideTile());
                MoveAlongPath();
            }
            else
            {
                _isMoving = false;
            }
        }

        private void MoveAlongPath()
        {
            var step = _character.movementSpeed * Time.deltaTime;
            
            suffix = _stateMachine.SetState(_character, _path[0], _isMoving,suffix);
            
            
            var zIndex = _path[0].transform.position.z;
            _character.transform.position = Vector2.MoveTowards(_character.transform.position, _path[0].transform.position, step);
            _character.transform.position = new Vector3(_character.transform.position.x, _character.transform.position.y, zIndex);

            if (Vector2.Distance(_character.transform.position, _path[0].transform.position) < 0.00001f)
            {
                PositionCharacterOnLine(_path[0]);
                _path.RemoveAt(0);
                // _character.MovementPoints--;
            }

            if (_path.Count == 0)
            {
                GetInRangeTiles();
                _isMoving = false;
                _stateMachine.SetState(_character, null, _isMoving,suffix);
            }

        }

        private void PositionCharacterOnLine(OverlayTile tile)
        {
            _character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
            _character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
            _character.standingOnTile = tile;
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
            _rangeFinderTiles = _rangeFinder.GetTilesInRange(new Vector2Int(_character.standingOnTile.gridLocation.x, _character.standingOnTile.gridLocation.y), _character.movementPoints, _character.jumpHeight);

            foreach (var item in _rangeFinderTiles)
            {
                item.ShowTile();
            }
        }
    }
}
