using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class CustomPathTile : TileBase {
    public Sprite customSprite;
    public bool Movable;
    public Color customColor = Color.white;
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = customSprite;
        tileData.colliderType = Tile.ColliderType.Sprite; // Optional: Set collider type
        tileData.color = customColor;
        // You can customize other properties here if needed
        base.GetTileData(location, tilemap, ref tileData);
    }
}