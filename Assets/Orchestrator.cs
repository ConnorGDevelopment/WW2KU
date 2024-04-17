using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Orchestrator : MonoBehaviour
{
    private Pawn _selectedPawn;
    public Pawn SelectedPawn { get { return _selectedPawn; } }

    private Image _selectPreview;

    private HighlightGridController _highlightGrid;

    public UnityEvent PawnSelected = new();

    public void SelectPawn(Pawn pawn)
    {
        Debug.Log($"Pawn: {pawn.pawnName} passed to Orch");
        _selectedPawn = pawn;
        PawnSelected.Invoke();
    }

    void Start()
    {
        _selectPreview = GameObject.FindWithTag("SelectPreview").GetComponent<Image>();
        _highlightGrid = GameObject.FindWithTag("HighlightGrid").GetComponent<HighlightGridController>();
    }

    // Apparently its important to destroy event listeners when you're done with a thing, prevents memory leaks
    // It may be irrelevant here bc the orch shouldn't be destroyed, but maybe between scenes and stuff it is, idk
    void OnDestroy()
    {
        PawnSelected.RemoveAllListeners();
    }
}
