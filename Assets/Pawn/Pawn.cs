using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Pawn : MonoBehaviour
{
    public string PawnName;
    public int Movement;

    public int Health = 20;

    private Orchestrator _orch;
    private Tilemap _terrain;

    public Material HighlightMaterial;
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
        Debug.Log($"Pawn: Highlighting {PawnName}");
        // TODO: This is a bad way to check if this is the currently selected pawn, but InstanceID was not working
        if (_orch.SelectedPawn && _orch.SelectedPawn.PawnName == PawnName)
        {
            _spriteRenderer.material = HighlightMaterial;
        }
        else
        {
            _spriteRenderer.material = _defaultMaterial;
        }
    }

    // Originally I used OnMouseDown, but it was making the hitbox wonky for selecting
    public void OnSelect()
    {
        Debug.Log($"Pawn: {PawnName} clicked");

        _orch.SelectPawn(gameObject.GetComponent<Pawn>());
    }

    // void OnMouseDown()
    // {
    //     Debug.Log($"Pawn: {pawnName} clicked");

    //     _orch.SelectPawn(gameObject.GetComponent<Pawn>());
    // }

    public void MovePawn(Vector3 cords)
    {
        Debug.Log($"Pawn: {PawnName} moving to {cords}");
        var cellPosition = _terrain.WorldToCell(cords);
        var cellCenter = _terrain.GetCellCenterWorld(cellPosition);
        gameObject.transform.position = cellCenter;
        _orch.PawnMoved.Invoke();
    }

    public void Attack(Pawn target)
    {
        Debug.Log(
            $"Pawn: {PawnName} is attacking {target.PawnName} with {_orch.SelectedCard.CardData.name} for {_orch.SelectedCard.CardData.Damage}"
        );
        if (target.Health - _orch.SelectedCard.CardData.Damage > 20)
        {
            target.Health = 20;
        }
        else if (target.Health - _orch.SelectedCard.CardData.Damage < 0)
        {
            target.Health = 0;
        }
        else
        {
            target.Health = target.Health - _orch.SelectedCard.CardData.Damage;
        }
    }
}
