using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedPreview : MonoBehaviour
{
    private Image _selectedPreview;
    private Orchestrator _orch;
    public int previewScale = 50;

    private void ChangeSprite()
    {
        if (_orch.SelectedPawn != null)
        {
            var sprite = _orch.SelectedPawn.GetComponent<SpriteRenderer>().sprite;
            _selectedPreview.sprite = sprite;
            gameObject.GetComponent<RectTransform>().sizeDelta = sprite.bounds.size * previewScale;
        }
        else
        {
            _selectedPreview.sprite = null;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }

    }

    void Start()
    {
        _selectedPreview = gameObject.GetComponent<Image>();
        _orch = GameObject.FindWithTag("Orch").GetComponent<Orchestrator>();
        _orch.PawnSelected.AddListener(ChangeSprite);
    }

}
