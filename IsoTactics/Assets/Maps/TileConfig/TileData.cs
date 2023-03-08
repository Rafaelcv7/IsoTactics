using System.Collections.Generic;
using IsoTactics.Enums;
using IsoTactics.Stats;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace IsoTactics.TileConfig
{
    [CreateAssetMenu(menuName = "ScriptableObjects/TileData")]
    public class TileData : ScriptableObject
    {
        public List<TileBase> tilesTextures;
        public TileTypes type = TileTypes.Traversable;
        public ScriptableEffect effect;
    }
}