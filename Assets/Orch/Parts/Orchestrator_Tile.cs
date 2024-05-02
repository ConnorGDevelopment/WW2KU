using UnityEngine;
using UnityEngine.Events;

public partial class Orchestrator
{
    private Vector3? _selectedTile;

    public Vector3? SelectedTile
    {
        get
        {
            if (_selectedTile.HasValue)
            {
                return _selectedTile;
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (value.HasValue)
            {
                var cellPosition = _highlightGrid.Tilemap.WorldToCell((Vector3)value);
                _selectedTile = _highlightGrid.Tilemap.GetCellCenterWorld(cellPosition);
                Debug.Log($"Orch: Tile {_selectedTile.Value} selected");
            }
        }
    }

    public void SelectTile(Vector3 cords)
    {
        SelectedTile = cords;
        // If a Pawn is currently selected and a tile is clicked
        if (SelectedCard && SelectedPawn && SelectedTile.HasValue)
        {
            Debug.Log(
                $"Orch: Pawn {SelectedPawn.PawnName} attacking Tile {SelectedTile.Value} with ${SelectedCard}"
            );
        }
        else if (SelectedPawn && SelectedTile.HasValue)
        {
            Debug.Log($"Orch: Pawn {SelectedPawn.PawnName} going to Tile {SelectedTile.Value}");
            // Move Pawn to clicked tile if its within their movement
            SelectedPawn.MovePawn(SelectedTile.Value);
        }
        else
        {
            DeselectPawn();
        }
    }

    public void DeselectTile()
    {
        if (SelectedTile.HasValue)
        {
            Debug.Log($"Orch: Tile deselected");
            SelectedTile = null;
            TileDeselected.Invoke();
        }
    }

    public UnityEvent TileSelected = new();
    public UnityEvent TileDeselected = new();
}
