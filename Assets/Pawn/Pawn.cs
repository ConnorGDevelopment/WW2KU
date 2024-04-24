using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Pawn : MonoBehaviour
{
    public string pawnName;
    public int movement;

    public int health = 20;

    private Orchestrator _orch;
    private Tilemap _terrain;

    void Start()
    {
        _orch = GameObject.FindWithTag("Orch").GetComponent<Orchestrator>();
        _terrain = GameObject.FindWithTag("Terrain").GetComponent<Tilemap>();

        // Correct starting position to grid
        var position = gameObject.transform.position;
        var cellPosition = _terrain.WorldToCell(position);
        var cellCenter = _terrain.GetCellCenterWorld(cellPosition);
        gameObject.transform.position = cellCenter;
    }

    void OnMouseDown()
    {
        Debug.Log($"Pawn: {pawnName} clicked");
        _orch.SelectPawn(gameObject.GetComponent<Pawn>());
    }

    public UnityEvent PawnMoved = new();

    public void MovePawn(Vector3 cords)
    {
        Debug.Log($"Pawn: {pawnName} moving to {cords}");
        var cellPosition = _terrain.WorldToCell(cords);
        var cellCenter = _terrain.GetCellCenterWorld(cellPosition);
        gameObject.transform.position = cellCenter;
        PawnMoved.Invoke();
    }

    public void Attack(Pawn target)
    {
        Debug.Log($"Pawn: {pawnName} is attacking {target.pawnName} with {_orch.SelectedCard.cardData.name} for {_orch.SelectedCard.cardData.damage}");
        if (target.health - _orch.SelectedCard.cardData.damage > 20)
        {
            target.health = 20;
        }
        else if (target.health - _orch.SelectedCard.cardData.damage < 0)
        {
            target.health = 0;
        }
        else
        {
            target.health = target.health - _orch.SelectedCard.cardData.damage;
        }
    }
}
