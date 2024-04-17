using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightGridController : MonoBehaviour
{
    private Tilemap _tilemap;

    private Orchestrator _orch;

    public Tile HighlightTile;

    void Start()
    {
        _tilemap = gameObject.GetComponent<Tilemap>();
        _orch = GameObject.FindWithTag("Orch").GetComponent<Orchestrator>();
        _orch.PawnSelected.AddListener(HighlightPawnMovement);
    }

    public void Highlight(Vector2 cellCenter)
    {
        Vector3Int cellPosition = _tilemap.WorldToCell(cellCenter);
        _tilemap.SetTile(cellPosition, HighlightTile);
    }
    // This is an overload, basically means you can call this function with either a Vector2 or a Vector3Int and it'll work
    public void Highlight(Vector3Int cellPosition)
    {
        _tilemap.SetTile(cellPosition, HighlightTile);
    }

    public void ClearHighlight()
    {
        _tilemap.ClearAllTiles();
    }

    public void HighlightPawnMovement()
    {
        Pawn pawn = _orch.SelectedPawn;

        if (pawn)
        {
            Vector3Int pawnPosition = _tilemap.WorldToCell(pawn.transform.position);

            // If pawn has movement, highlight tiles in each direction
            if (pawn.movement > 0)
            {
                // There should be a better way to do this, but a lot of the methods available suck
                for (int i = 1; i <= pawn.movement; i++)
                {
                    Vector3Int deltaX = new(i, 0, 0);
                    Vector3Int deltaY = new(0, i, 0);

                    // Cardinal Movement
                    Highlight(pawnPosition + deltaX);
                    Highlight(pawnPosition - deltaX);
                    Highlight(pawnPosition + deltaY);
                    Highlight(pawnPosition - deltaY);

                    // Diagonal Movement
                    // Highlight(pawnPosition + deltaX + deltaY);
                    // Highlight(pawnPosition + deltaX - deltaY);
                    // Highlight(pawnPosition - deltaX + deltaY);
                    // Highlight(pawnPosition - deltaX - deltaY);

                    // Diagonal Positions w/o Diagonal Movement
                    Vector3Int deltaDiagX = new(i, i - 1, 0);
                    Vector3Int deltaDiagY = new(i - 1, i, 0);
                    Highlight(pawnPosition + deltaDiagX);
                    Highlight(pawnPosition - deltaDiagX);
                    Highlight(pawnPosition + deltaDiagY);
                    Highlight(pawnPosition - deltaDiagY);

                }
            }
        }


    }
}
