using UnityEngine;
using UnityEngine.Tilemaps;

public class Pawn : MonoBehaviour
{
    public string PawnName;
    public int Movement;

    public int Health = 20;

    private Orch _orch;
    private Tilemap _terrain;

    public Material GlowMat;
    private Material _defaultMat;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _orch = GameObject.FindWithTag("Orch").GetComponent<Orch>();
        _terrain = _orch.Terrain;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _defaultMat = gameObject.GetComponent<SpriteRenderer>().material;

        // Correct starting position to grid
        var position = gameObject.transform.position;
        var cellPosition = _terrain.WorldToCell(position);
        var cellCenter = _terrain.GetCellCenterWorld(cellPosition);
        gameObject.transform.position = cellCenter;

        _orch.PawnManager.OnSelect.AddListener(ToggleHighlight);
        _orch.PawnManager.OnSelect.AddListener(ToggleHighlight);
    }

    private void ToggleHighlight()
    {
        Debug.Log($"Pawn: Highlighting {PawnName}");

        _spriteRenderer.material =
            (
                _orch.PawnManager.Selected
                && _orch.PawnManager.Selected.GetInstanceID() == gameObject.GetInstanceID()
            )
                ? GlowMat
                : _defaultMat;
    }

    public void Highlight(bool toggle)
    {
        _spriteRenderer.material = toggle ? GlowMat : _defaultMat;
    }

    // Originally I used OnMouseDown, but it was making the hitbox wonky for selecting
    public void OnSelect()
    {
        Debug.Log($"Pawn: {PawnName} clicked");

        _orch.PawnManager.Select(gameObject);
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
