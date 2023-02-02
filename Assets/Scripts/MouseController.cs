using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Vector3 _destination;
    private CharacterInfo _character;
    private PathFinder _pathFinder;
    private RangeFinder _rangeFinder;
    private List<OverlayTile> _path;
    private List<OverlayTile> _inRangeTiles = new List<OverlayTile>();
    private ArrowTranslator _arrowTranslator;

    public GameObject characterPrefab;
    private float moveSpeed;

    private void Start()
    {
        _pathFinder = new PathFinder();
        _rangeFinder = new RangeFinder();
        _arrowTranslator = new ArrowTranslator();
    }

    private bool _isMoving = false;

    // Update is called once per frame
    void LateUpdate()
    {
        var focusedTileHit = GetFocusedOnTile();
        
        if (focusedTileHit.HasValue)
        {
            OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
            transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder =
                overlayTile.GetComponent<SpriteRenderer>().sortingOrder -1;

            if (_inRangeTiles.Contains(overlayTile) && !_isMoving)
            {
                _path = _pathFinder.FindPath(_character.activeTile, overlayTile, _inRangeTiles);

                foreach (var item in _inRangeTiles)
                {
                    item.SetArrowSprite(ArrowTranslator.ArrowDirection.None);
                }

                for (int i = 0; i < _path.Count; i++)
                {
                    var previousTile = i > 0 ? _path[i - 1] : _character.activeTile;
                    var futureTile = i < _path.Count - 1 ? _path[i + 1] : null;

                    var arrowDir = _arrowTranslator.TranslateDirection(previousTile, _path[i], futureTile);
                    _path[i].SetArrowSprite(arrowDir);
                }
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (_character == null)
                {
                    _character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                    moveSpeed = _character.movementSpeed;
                    PositionCharacterOnTile(overlayTile);
                    GetInRangeTiles();
                }
                else
                {
                    _isMoving = true;
                }
            }
        }

        if (_path.Count > 0 && _isMoving) // && _character.movePoints > 0
        {
            MoveAlongPath();
        }
        
    }

    private void GetInRangeTiles()
    {
        foreach (var item in _inRangeTiles)
        {
            item.HideTile();
        }
        
        _inRangeTiles = _rangeFinder.GetTilesInRange(_character.activeTile, _character.movePoints);

        foreach (var item in _inRangeTiles)
        {
            item.ShowTile();
        }
    }

    private void MoveAlongPath()
    {
        var step = moveSpeed * Time.deltaTime;

        var zIndex = _path[0].transform.position.z;
        if (_character.GetComponent<SpriteRenderer>().sortingOrder <
            _path[0].GetComponent<SpriteRenderer>().sortingOrder)
        {
            _character.GetComponent<SpriteRenderer>().sortingOrder = _path[0].GetComponent<SpriteRenderer>().sortingOrder;
        }
        var position = _character.transform.position;
        position = Vector2.MoveTowards(
            position, 
            _path[0].transform.position,
            step
            );
        position =
            new Vector3(position.x, position.y, zIndex);
        _character.transform.position = position;

        if (Vector2.Distance(_character.transform.position, _path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOnTile(_path[0]);
            _path.RemoveAt(0);
            // _character.movePoints--;
        }

        if (_path.Count == 0)
        {
            GetInRangeTiles();
            _isMoving = false;
        }
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(mousePosition.x, mousePosition.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderBy(i => i.collider.transform.position.z).First();
        }
        return null;
    }

    private void PositionCharacterOnTile(OverlayTile tile)
    {
        var tilePos = tile.transform.position;
        _character.transform.position = new Vector3(tilePos.x, tilePos.y - 0.0001f, tilePos.z);
        _character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        _character.activeTile = tile;
    }
}
