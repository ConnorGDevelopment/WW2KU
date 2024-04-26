using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightGridController : MonoBehaviour
{
    public Tilemap HighlightTilemap { get; private set; }

    private Orchestrator _orch;

    public Tile MovementTile;
    public Tile AttackTile;

    void Start()
    {
        HighlightTilemap = gameObject.GetComponent<Tilemap>();
        _orch = GameObject.FindWithTag("Orch").GetComponent<Orchestrator>();
        _orch.PawnSelected.AddListener(HighlightPawn);
        _orch.PawnDeselected.AddListener(ClearHighlight);
        _orch.TileDeselected.AddListener(ClearHighlight);
        _orch.CardDeselected.AddListener(ClearHighlight);
        _orch.CardSelected.AddListener(HighlightPawn);
    }

    private void Highlight(Vector3Int cellPosition, Tile tileToUse)
    {
        HighlightTilemap.SetTile(cellPosition, tileToUse);
    }

    private void ClearHighlight()
    {
        HighlightTilemap.ClearAllTiles();
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
            Vector3Int pawnPosition = HighlightTilemap.WorldToCell(pawn.transform.position);

            // If pawn has movement, highlight tiles in each direction
            if (pawn.movement > 0)
            {
                // There should be a better way to do this, but a lot of the methods available suck
                for (int x = 1; x <= pawn.movement; x++)
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
            Vector3Int pawnPosition = HighlightTilemap.WorldToCell(pawn.transform.position);

            for (int x = 1; x <= _orch.SelectedCard.cardData.range; x++)
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

    public bool IsTileHighlighted(Vector3 cords)
    {
        var _cellPosition = HighlightTilemap.WorldToCell(cords);


        if (HighlightTilemap.HasTile(_cellPosition))
        {
            return HighlightTilemap.GetTile(_cellPosition) == MovementTile;
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
        if (IsTileHighlighted(cords))
        {
            _orch.SelectTile(cords);
        }
    }
}