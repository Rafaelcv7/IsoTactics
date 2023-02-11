using UnityEngine;
using System.Linq;


namespace IsoTactics
{
    public class MouseController : MonoBehaviour
    {
        public GameObject cursor;
        private OverlayTile _tile;
        
        [Header("Events:")]
        public GameEvents onNewFocusedTile;
        
        private void Start()
        {
            cursor = Instantiate(cursor);
        }

        void Update()
        {
            var hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                _tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                cursor.transform.position = _tile.transform.position;
                cursor.gameObject.GetComponentsInChildren<SpriteRenderer>()[0].sortingOrder =
                    cursor.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder =
                        _tile.GetComponent<SpriteRenderer>().sortingOrder;

                if (onNewFocusedTile)
                    onNewFocusedTile.Raise(this, _tile);
            }
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

        //Called by Spawner.
        public void SetCursorSilhouette(Component sender, object data)
        {
            if (data is Sprite sprite)
            {
                cursor.GetComponentsInChildren<SpriteRenderer>()[1].sprite = sprite;
                
            }
            else
            {
                cursor.GetComponentsInChildren<SpriteRenderer>()[1].sprite = null;
            }
        }
    }
}
