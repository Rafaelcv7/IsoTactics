using System.Collections.Generic;
using IsoTactics;
using UnityEngine;
using CharacterInfo = IsoTactics.CharacterInfo;

public class Spawner : MonoBehaviour
{
    public List<CharacterInfo> charactersPrefabs;
    
    [Header("Events")]
    public GameEvents onCharacterSpawn;

    public GameEvents ChangePhase;

    public GameEvents SetCursorSilhouette;

    private OverlayTile _focusedTile;

    private GameObject _cursor;
    

    private void Update()
    {
        if (charactersPrefabs.Count > 0 && _focusedTile)
        {
            SetCursorSilhouette.Raise(this, charactersPrefabs[0].GetComponent<SpriteRenderer>().sprite);
            
            if (Input.GetMouseButtonDown(0))
            {
                var newCharacter = Instantiate(charactersPrefabs[0]).GetComponent<CharacterInfo>();
                charactersPrefabs.RemoveAt(0);
                SpawnCharacter(newCharacter, _focusedTile);
                
                if (charactersPrefabs.Count == 0)
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

    private void SpawnCharacter(CharacterInfo newCharacter, OverlayTile tile)
    {
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
