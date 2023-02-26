using System.Collections.Generic;
using IsoTactics.TileConfig;
using UnityEngine;
using UnityEngine.Serialization;
using static IsoTactics.ArrowTranslator;

namespace IsoTactics
{
    public class OverlayTile : MonoBehaviour
    {
        public int G;
        public int H;
        public int F => G + H;
        public Character activeCharacter;
        public bool isBlocked = false;

        public OverlayTile previousTile;
        public Vector3Int gridLocation;
        public Vector2Int Grid2DLocation => new(gridLocation.x, gridLocation.y);
        public TileData tileData;

        public List<Sprite> arrows;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
            }
        }

        public void HideTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }

        public void ShowTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        public void SetSprite(ArrowTranslator.ArrowDirection d)
        {
            if (d == ArrowTranslator.ArrowDirection.None)
                GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);
            else
            {
                GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 1);
                GetComponentsInChildren<SpriteRenderer>()[1].sprite = arrows[(int)d];
                GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
            }
        }

    }
}
