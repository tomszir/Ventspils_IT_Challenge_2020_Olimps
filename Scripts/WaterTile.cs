using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : MonoBehaviour
{
    public GameObject waterTile;
    public Vector2 startPos;
    public float offset;
    public int xTiles;
    public int yTiles;
    void Start()
    {
        for(int x = 0; x < xTiles; x++)
        {
            for(int y = 0; y < yTiles; y++)
            {
                Instantiate(waterTile, new Vector3(startPos.x - offset * x, 0, startPos.y - offset * y), Quaternion.identity, transform);
            }
        }
    }
}
