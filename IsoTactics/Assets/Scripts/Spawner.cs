using System;
using System.Collections.Generic;
using System.Linq;
using IsoTactics;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    public List<CharacterPrefabsContainer> charactersPrefabs;
    public List<CharacterPrefabsContainer> enemyPrefabs;

    [Header("Events")]
    public GameEvents onCharacterSpawn;

    public GameEvents ChangePhase;

    public GameEvents SetCursorSilhouette;

    private OverlayTile _focusedTile;

    private GameObject _cursor;

    private List<CharacterPrefabsContainer> _charactersList;

    private void Start()
    {
        _charactersList = charactersPrefabs.Concat(enemyPrefabs).ToList();
    }

    private void Update()
    {
        if (_charactersList.Count > 0 && _focusedTile)
        {
            SetCursorSilhouette.Raise(this, _charactersList[0].characterPrefab.GetComponent<SpriteRenderer>().sprite);
            
            if (Input.GetMouseButtonDown(0))
            {
                if (!_focusedTile.isBlocked)
                {
                    var newCharacter = Instantiate(_charactersList[0].characterPrefab).GetComponent<Character>();
                    newCharacter.isAi = _charactersList[0].isAi;
                    newCharacter.teamId = _charactersList[0].teamId;
                    _charactersList.RemoveAt(0);
                
                    SpawnCharacter(newCharacter, _focusedTile);
                
                    if (_charactersList.Count == 0)
                    {
                        SetCursorSilhouette.Raise(this, null);
                        if(ChangePhase)
                            ChangePhase.Raise(this, "Turn");
                    }
            
                    if(onCharacterSpawn)
                        onCharacterSpawn.Raise(this, newCharacter);
                }
                
            }
        }
        
    }

    private void SpawnCharacter(Character newCharacter, OverlayTile tile)
    {
        newCharacter.isAlive = true;
        newCharacter.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        newCharacter.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        newCharacter.LinkCharacterToTile(tile);
    }
    
    //Called by MouseController.
    public void NewFocusedTile(Component sender, object data)
    {
        if (data is OverlayTile tile)
        {
            _focusedTile = tile;
        }
    }
    
}
