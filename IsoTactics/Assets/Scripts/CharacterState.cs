using System.Collections;
using System.Collections.Generic;
using IsoTactics;
using UnityEngine;
using CharacterInfo = IsoTactics.CharacterInfo;

public class CharacterState
{
    public string SetState(CharacterInfo character, OverlayTile nextTile, bool isMoving, string currentSuffix)
    {
        var back = "_B";
        var front = "_F";
        if (nextTile != null)
        {
            currentSuffix = nextTile.transform.position.y < character.transform.position.y ? front : back;
        }
        if (isMoving)
        {
            character.GetComponent<SpriteRenderer>().flipX = nextTile.transform.position.x < character.transform.position.x;
            character.GetComponent<Animator>().Play($"Walking{currentSuffix}");
        }
        else
        {
            character.GetComponent<Animator>().Play($"Idle{currentSuffix}");
        }

        return currentSuffix;


        // character.GetComponent<SpriteRenderer>().sprite = 
    }
}
