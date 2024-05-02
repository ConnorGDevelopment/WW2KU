using UnityEngine;
using UnityEngine.Events;

public partial class Orchestrator
{
    private Pawn _selectedPawn;
    public Pawn SelectedPawn
    {
        get { return _selectedPawn; }
        set
        {
            // Remove all listeners, then change value
            // I don't know if this is needed, but its safe


            _selectedPawn = value;

            // If Pawn, not null, log and add listeners (each Pawn is separate)
            if (value)
            {
                Debug.Log($"Orch: Pawn {_selectedPawn.PawnName} selected");
            }
        }
    }

    public UnityEvent PawnSelected = new();
    public UnityEvent PawnDeselected = new();
    public UnityEvent PawnMoved = new();

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
            Debug.Log($"Orch: Pawn {SelectedPawn.PawnName} deselected");
            SelectedPawn = null;
            PawnDeselected.Invoke();
        }
    }
}
