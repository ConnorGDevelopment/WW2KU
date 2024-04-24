using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Orchestrator : MonoBehaviour
{
    // GameObject Refs
    private HighlightGridController _highlightGrid;
    public Camera MainCam { get; private set; }


    // private Image _selectPreview;

    // Select Bins
    private Pawn _selectedPawn;
    public Pawn SelectedPawn
    {
        get { return _selectedPawn; }
        set
        {
            // Remove all listeners, then change value
            // I don't know if this is needed, but its safe
            if (_selectedPawn)
            {
                _selectedPawn.PawnMoved.RemoveAllListeners();
            }

            _selectedPawn = value;

            // If Pawn, not null, log and add listeners (each Pawn is separate)
            if (value)
            {
                Debug.Log($"Orch: Pawn {_selectedPawn.pawnName} selected");
                _selectedPawn.PawnMoved.AddListener(DeselectPawn);

            }


        }
    }

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
                var cellPosition = _highlightGrid.HighlightTilemap.WorldToCell((Vector3)value);
                _selectedTile = _highlightGrid.HighlightTilemap.GetCellCenterWorld(cellPosition);
                Debug.Log($"Orch: Tile {_selectedTile.Value} selected");
            }

        }
    }

    private Card _selectedCard;

    public Card SelectedCard
    {
        get { return _selectedCard; }
        set { _selectedCard = value; }
    }

    // Unity Events
    public UnityEvent PawnSelected = new();
    public UnityEvent PawnDeselected = new();

    public UnityEvent TileSelected = new();
    public UnityEvent TileDeselected = new();

    public UnityEvent CardSelected = new();
    public UnityEvent CardDeselected = new();

    // Methods
    public void SelectPawn(Pawn pawn)
    {
        if (SelectedPawn && SelectedCard && (SelectedPawn.GetInstanceID() != pawn.GetInstanceID()))
        {
            SelectedPawn.Attack(pawn);
        }
        else if (SelectedPawn)
        {
            if (SelectedPawn.GetInstanceID() == pawn.GetInstanceID())
            {
                DeselectPawn();
            }
            else
            {
                // This runs through the setter above
                SelectedPawn = pawn;
                // This basically shouts and anything with a listener attached to this event will do its thing
                PawnSelected.Invoke();
            }
        }
        else
        {
            // This runs through the setter above
            SelectedPawn = pawn;
            // This basically shouts and anything with a listener attached to this event will do its thing
            PawnSelected.Invoke();
        }

    }

    private void DeselectPawn()
    {
        if (SelectedPawn)
        {
            SelectedPawn = null;
            PawnDeselected.Invoke();
        }

    }

    public void SelectTile(Vector3 cords)
    {

        SelectedTile = cords;
        // If a Pawn is currently selected and a tile is clicked
        if (SelectedCard && SelectedPawn && SelectedTile.HasValue)
        {
            Debug.Log($"Orch: Pawn {SelectedPawn.pawnName} attacking Tile {SelectedTile.Value} with ${SelectedCard}");
        }
        else if (SelectedPawn && SelectedTile.HasValue)
        {
            Debug.Log($"Orch: Pawn {SelectedPawn.pawnName} going to Tile {SelectedTile.Value}");
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



    public void SelectCard(Card card)
    {
        if (SelectedCard)
        {
            if (SelectedCard.GetInstanceID() == card.GetInstanceID())
            {
                DeselectCard();
            }
            else
            {
                Debug.Log($"Orch: Card {card.cardData.name} selected");

                DeselectTile();
                SelectedCard = card;
                CardSelected.Invoke();
            }
        }
        else
        {
            Debug.Log($"Orch: Card {card.cardData.name} selected");

            DeselectTile();
            SelectedCard = card;
            CardSelected.Invoke();
        }

    }
    public void DeselectCard()
    {
        Debug.Log($"Orch: Card deselected");
        SelectedCard = null;
        CardDeselected.Invoke();
    }

    void Start()
    {
        // _selectPreview = GameObject.FindWithTag("SelectPreview").GetComponent<Image>();
        _highlightGrid = GameObject.FindWithTag("HighlightGrid").GetComponent<HighlightGridController>();
        MainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    // Apparently its important to destroy event listeners when you're done with a thing, prevents memory leaks
    // It may be irrelevant here bc the orch shouldn't be destroyed, but maybe between scenes and stuff it is, idk
    void OnDestroy()
    {
        PawnSelected.RemoveAllListeners();
    }
}
