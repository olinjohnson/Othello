using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBehavior : MonoBehaviour
{
    private static int height = 8;
    private static int width = 8;
    private static float margin = 0.05f;
    private float y_offset = -3.5f - (3 * margin);
    private float x_offset = 3.5f + (3 * margin);

    public GameObject boardContainer;
    public GameObject tilePrefab;
    public GameObject validMovePrefab;
    public GameObject highlights;

    public GameObject blackCoinPrefab;
    public GameObject whiteCoinPrefab;

    public static Board board;
    public static Game game;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTileGrid();
        game = new Game();
        board = new Board();

        InitiateAIGame();
    }


    void InitiateAIGame()
    {
        StartCoroutine(AIMoveCycle());
    }

    IEnumerator AIMoveCycle()
    {
        while(!game.DetectEnding(board))
        {
            // White to move
            if(!board.turn)
            {
                ulong valid_moves = game.GetValidMoves(board);
                HighlightValidMoves(valid_moves);
                if (valid_moves == 0) { board.turn = true; }
                yield return new WaitUntil(() => board.turn);
                UpdateBoard(board);
                foreach (Transform h in highlights.transform)
                {
                    Destroy(h.gameObject);
                }
            }
            // Black to move
            else
            {
                ulong valid_moves = game.GetValidMoves(board);
                HighlightValidMoves(valid_moves);
                if(valid_moves == 0) { board.turn = false; }
                yield return new WaitUntil(() => !board.turn);
                UpdateBoard(board);
                foreach (Transform h in highlights.transform)
                {
                    Destroy(h.gameObject);
                }
            }
        }
    }

    void GenerateTileGrid()
    {
        for(int x = 0; x < width; x++)
        {
            for(int i = 0; i < height; i++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(-x - (x * margin) + x_offset, i + (i * margin) + y_offset, 0), Quaternion.identity);
                newTile.name = $"Tile{(x * 8) + i}";
                newTile.transform.parent = boardContainer.transform;
            }
        }
    }

    void HighlightValidMoves(ulong moves)
    {
        ulong initial_mask = 1;
        for(int i = 0; i < 64; i++)
        {
            if((moves & initial_mask) > 0)
            {
                GameObject highlight = Instantiate(validMovePrefab, GameObject.Find($"Tile{i}").GetComponent<Transform>().position, Quaternion.identity);
                highlight.name = $"Highlight{i}";
                highlight.transform.parent = highlights.transform;
            }
            initial_mask = initial_mask << 1;
        }
    }

    void UpdateBoard(Board b)
    {
        GameObject pieceParentContainer = GameObject.Find("Pieces");
        foreach (Transform p in pieceParentContainer.transform)
        {
            Destroy(p.gameObject);
        }

        ulong initial_mask = 1;
        for (int i = 0; i < 64; i++)
        {
            if ((b.white_pieces & initial_mask) > 0)
            {
                GameObject piece = Instantiate(whiteCoinPrefab, GameObject.Find($"Tile{i}").GetComponent<Transform>().position, Quaternion.identity);
                piece.name = $"WhitePiece{i}";
                piece.transform.parent = pieceParentContainer.transform;
            }
            else if((b.black_pieces & initial_mask) > 0)
            {
                GameObject piece = Instantiate(blackCoinPrefab, GameObject.Find($"Tile{i}").GetComponent<Transform>().position, Quaternion.identity);
                piece.name = $"BlackPiece{i}";
                piece.transform.parent = pieceParentContainer.transform;
            }
            initial_mask = initial_mask << 1;
        }
    }
}
