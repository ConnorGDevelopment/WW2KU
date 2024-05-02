using UnityEngine;
using UnityEngine.Events;

public partial class Orchestrator
{
    private Card _selectedCard;

    public Card SelectedCard
    {
        get { return _selectedCard; }
        set { _selectedCard = value; }
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
                Debug.Log($"Orch: Card {card.CardData.name} selected");

                DeselectTile();
                SelectedCard = card;
                CardSelected.Invoke();
            }
        }
        else
        {
            Debug.Log($"Orch: Card {card.CardData.name} selected");

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

    public UnityEvent CardSelected = new();
    public UnityEvent CardDeselected = new();
}
