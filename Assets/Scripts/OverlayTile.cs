using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArrowTranslator;

public class OverlayTile : MonoBehaviour
{
    public int G; //Distance from starting point.
    public int H; //Distance from destination point.
    public int F { get { return G + H; } }

    public bool isBlocked;

    public OverlayTile previousTile;

    public Vector3Int gridLocation;
    public Vector2Int grid2DLocation => (Vector2Int)gridLocation;

    public List<Sprite> arrows;


    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    
    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        SetArrowSprite(ArrowTranslator.ArrowDirection.None);
    }

    public void SetArrowSprite(ArrowTranslator.ArrowDirection d)
    {
        var arrow = GetComponentsInChildren<SpriteRenderer>()[1];
        if (d == ArrowTranslator.ArrowDirection.None)
        {
            arrow.color = new Color(1, 1, 1, 0);
        }
        else
        {
            arrow.color = new Color(1, 1, 1, 1);
            arrow.sprite = arrows[(int)d];
            arrow.sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
        }
    }
}
