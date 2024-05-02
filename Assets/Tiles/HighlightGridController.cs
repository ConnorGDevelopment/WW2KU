using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightGridController : MonoBehaviour
{
    public Tilemap Tilemap { get; private set; }

    private Orchestrator _orch;

    public Tile MovementTile;
    public Tile AttackTile;

    void Start()
    {
        Tilemap = gameObject.GetComponent<Tilemap>();
        _orch = GameObject.FindWithTag("Orch").GetComponent<Orchestrator>();
        _orch.PawnSelected.AddListener(HighlightPawn);
        _orch.PawnDeselected.AddListener(ClearHighlight);
        _orch.TileDeselected.AddListener(ClearHighlight);
        _orch.CardDeselected.AddListener(ClearHighlight);
        _orch.CardSelected.AddListener(HighlightPawn);
    }

    private void Highlight(Vector3Int cellPosition, Tile tileToUse)
    {
        Tilemap.SetTile(cellPosition, tileToUse);
    }

    private void ClearHighlight()
    {
        Tilemap.ClearAllTiles();
    }

    private void HighlightPawn()
    {
        if (_orch.SelectedCard)
        {
            HighlightPawnAttack();
        }
        else
        {
            HighlightPawnMovement();
        }
    }

    private void HighlightPawnMovement()
    {
        Pawn pawn = _orch.SelectedPawn;

        if (pawn)
        {
            Vector3Int pawnPosition = Tilemap.WorldToCell(pawn.transform.position);

            // If pawn has movement, highlight tiles in each direction
            if (pawn.Movement > 0)
            {
                // There should be a better way to do this, but a lot of the methods available suck
                for (int x = 1; x <= pawn.Movement; x++)
                {
                    for (int y = 0; y <= x; y++)
                    {
                        Vector3Int delta = new(x - y, y, 0);
                        Highlight(pawnPosition + (delta * new Vector3Int(1, 1, 0)), MovementTile);
                        Highlight(pawnPosition + (delta * new Vector3Int(-1, 1, 0)), MovementTile);
                        Highlight(pawnPosition + (delta * new Vector3Int(-1, -1, 0)), MovementTile);
                        Highlight(pawnPosition + (delta * new Vector3Int(1, -1, 0)), MovementTile);
                    }
                }
            }
        }
    }

    private void HighlightPawnAttack()
    {
        Pawn pawn = _orch.SelectedPawn;

        if (pawn)
        {
            Vector3Int pawnPosition = Tilemap.WorldToCell(pawn.transform.position);

            for (int x = 1; x <= _orch.SelectedCard.CardData.Range; x++)
            {
                for (int y = 0; y <= x; y++)
                {
                    Vector3Int delta = new(x - y, y, 0);
                    Highlight(pawnPosition + (delta * new Vector3Int(1, 1, 0)), AttackTile);
                    Highlight(pawnPosition + (delta * new Vector3Int(-1, 1, 0)), AttackTile);
                    Highlight(pawnPosition + (delta * new Vector3Int(-1, -1, 0)), AttackTile);
                    Highlight(pawnPosition + (delta * new Vector3Int(1, -1, 0)), AttackTile);
                }
            }
        }
    }

    public bool IsHighlighted(Vector3 cords)
    {
        var _cellPosition = Tilemap.WorldToCell(cords);

        if (Tilemap.HasTile(_cellPosition))
        {
            return Tilemap.GetTile(_cellPosition) == MovementTile;
        }
        else
        {
            return false;
        }
    }

    void OnMouseDown()
    {
        Debug.Log($"HighlightGrid: Tile {Input.mousePosition} selected");
        var cords = _orch.MainCam.ScreenToWorldPoint(Input.mousePosition);
        cords.z = 0;
        if (IsHighlighted(cords))
        {
            _orch.SelectTile(cords);
        }
    }
}
