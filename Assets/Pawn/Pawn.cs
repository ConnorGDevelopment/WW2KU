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

    public Material highlightMaterial;
    private Material _defaultMaterial;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _orch = GameObject.FindWithTag("Orch").GetComponent<Orchestrator>();
        _terrain = GameObject.FindWithTag("Terrain").GetComponent<Tilemap>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _defaultMaterial = gameObject.GetComponent<SpriteRenderer>().material;

        // Correct starting position to grid
        var position = gameObject.transform.position;
        var cellPosition = _terrain.WorldToCell(position);
        var cellCenter = _terrain.GetCellCenterWorld(cellPosition);
        gameObject.transform.position = cellCenter;

        _orch.PawnSelected.AddListener(ToggleHighlight);
        _orch.PawnDeselected.AddListener(ToggleHighlight);
    }

    private void ToggleHighlight()
    {
        Debug.Log($"Pawn: Highlighting {pawnName}");
        // TODO: This is a bad way to check if this is the currently selected pawn, but InstanceID was not working
        if (_orch.SelectedPawn && _orch.SelectedPawn.pawnName == pawnName)
        {
            _spriteRenderer.material = highlightMaterial;

        }
        else
        {
            _spriteRenderer.material = _defaultMaterial;
        }
    }

    // Originally I used OnMouseDown, but it was making the hitbox wonky for selecting
    public void GotClicked()
    {
        Debug.Log($"Pawn: {pawnName} clicked");

        _orch.SelectPawn(gameObject.GetComponent<Pawn>());
    }

    // void OnMouseDown()
    // {
    //     Debug.Log($"Pawn: {pawnName} clicked");

    //     _orch.SelectPawn(gameObject.GetComponent<Pawn>());
    // }

    public void MovePawn(Vector3 cords)
    {
        Debug.Log($"Pawn: {pawnName} moving to {cords}");
        var cellPosition = _terrain.WorldToCell(cords);
        var cellCenter = _terrain.GetCellCenterWorld(cellPosition);
        gameObject.transform.position = cellCenter;
        _orch.PawnMoved.Invoke();
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
