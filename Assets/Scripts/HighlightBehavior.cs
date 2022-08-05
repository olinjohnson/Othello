using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightBehavior : MonoBehaviour
{
    private SpriteRenderer srenderer;

    private void Start()
    {
        srenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        srenderer.color = new Color(0, 0, 0, 100 / 255f);
    }
    private void OnMouseExit()
    {
        srenderer.color = new Color(0, 0, 0, 50 / 255f);
    }
    private void OnMouseDown()
    {
        string name = gameObject.name;
        BoardBehavior.board = BoardBehavior.game.MakeMove(BoardBehavior.board, int.Parse(name.Substring(9, name.Length - 9)));
    }
}
