using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedPreview : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Orchestrator _orch;
    public int previewScale = 50;

    private void ChangeSprite()
    {
        if (_orch.SelectedPawn != null)
        {
            var sprite = _orch.SelectedPawn.GetComponent<SpriteRenderer>().sprite;
            _spriteRenderer.sprite = sprite;
            gameObject.GetComponent<RectTransform>().sizeDelta = sprite.bounds.size * previewScale;
        }
        else
        {
            _spriteRenderer.sprite = null;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }

    }

    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _orch = GameObject.FindWithTag("Orch").GetComponent<Orchestrator>();
        _orch.PawnSelected.AddListener(ChangeSprite);
    }

}
