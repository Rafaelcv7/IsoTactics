using System.Collections;
using System.Collections.Generic;
using IsoTactics;
using TMPro;
using UnityEngine;

public class TileReader : MonoBehaviour
{
    private TMP_Text _tileInfo;

    private OverlayTile _tile;
    // Start is called before the first frame update
    void Start()
    {
        _tileInfo = gameObject.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_tile)
            _tileInfo.text = $"({_tile.Grid2DLocation.x},{_tile.Grid2DLocation.y})";
    }

    public void ReadTile(Component sender, object data)
    {
        _tile = data as OverlayTile;
    }
}
