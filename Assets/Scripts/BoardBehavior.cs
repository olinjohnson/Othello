using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBehavior : MonoBehaviour
{
    private static int height = 8;
    private static int width = 8;
    private static float margin = 0.05f;

    private float y_offset = -3.5f - (3 * margin);
    private float x_offset = -3.5f - (3 * margin);

    public GameObject board;
    public GameObject tilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTileGrid();
    }


    void InitiateAIGame()
    {
        StartCoroutine(AIMoveCycle());
    }

    IEnumerator AIMoveCycle()
    {
        yield return new WaitForSeconds(1);
    }

    void GenerateTileGrid()
    {
        for(int i = 0; i < height; i++)
        {
            for(int x = 0; x < width; x++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(x + (x * margin) + x_offset, i + (i * margin) + y_offset, 0), Quaternion.identity);
                newTile.name = $"Tile{x},{i}";
                newTile.transform.parent = board.transform;
            }
        }
    }
}
