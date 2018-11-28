using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRunner : MonoBehaviour {
    public Tilemap tilemap;
    private int widthMax;
    private int heightMax;
    private int widthMin;
    private int heightMin;
    public Vector3 startingPosition;

    TileBase[] tiles;
    // Use this for initialization
    void Start ()
    {
        tilemap.transform.position = new Vector3(100, 100);
        BoundsInt tilemapSize = tilemap.cellBounds;
        widthMax = tilemapSize.xMax;
        heightMax = tilemapSize.yMax;
        widthMin = tilemapSize.xMin;
        heightMin = tilemapSize.yMin;
        Debug.Log("Points for the tilemap: " + widthMax + " " + heightMax + " : " + widthMin + " " + heightMin);
        tiles = tilemap.GetTilesBlock(tilemapSize);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartDungeonRun()
    {
        TileBase[] surroundingTiles = GetSurroundingTiles(startingPosition);
        Vector3Int tilePosition = new Vector3Int(0,0,0);
        TileData tileData = new TileData();
        surroundingTiles[0].GetTileData(tilePosition, null, ref tileData);
        Debug.Log(tileData.sprite);
    }

    private TileBase[] GetSurroundingTiles(Vector3 currentPosition)
    {
        List<TileBase> tiles = new List<TileBase>();

        GetTile(currentPosition, Vector3.up, tiles);
        GetTile(currentPosition, Vector3.down, tiles);
        GetTile(currentPosition, Vector3.left, tiles);
        GetTile(currentPosition, Vector3.right, tiles);
        Debug.Log(tiles.Count);
        return tiles.ToArray();
    }

    private void GetTile(Vector3 currentPosition, Vector3 direction, List<TileBase> tiles)
    {
        Vector3 checkPosition = currentPosition + direction;
        if (DoesPositionExist(checkPosition))
        {
            tiles.Add(GetTileAt(checkPosition));
        }
    }

    private bool DoesPositionExist(Vector3 checkPosition)
    {
        if(checkPosition.x > widthMin && checkPosition.y > heightMin && checkPosition.x < widthMax && checkPosition.y < heightMax)
        {
            return true;
        }
        return false;
    }

    TileBase GetTileAt(Vector3 pos)
    {
        Debug.Log((int)pos.x + (int)pos.y * (Mathf.Abs(widthMin) + widthMax) + " " + tiles.Length);
        Debug.Log(Mathf.Abs(widthMin) + widthMax);
        return tiles[(int)pos.x + (int)pos.y * (Mathf.Abs(widthMin) + widthMax)];
    }
}
