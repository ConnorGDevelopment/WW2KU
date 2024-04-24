using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CameraControls : MonoBehaviour
{
    private CharacterController _characterController;
    public float panSpeed = 5.0f;
    private Vector2 panInput = Vector2.zero;

    public void MovePlayer(InputAction.CallbackContext ctx)
    {
        panInput = ctx.ReadValue<Vector2>();
    }

    // public void SelectTile(InputAction.CallbackContext ctx)
    // {
    //     // Makes ray from mouse position through screen
    //     // Starts from the camera because rays have to originate from a thing
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

    //     // If the ray has a hit
    //     // ReadValueAsButton() return a boolean
    //     // Without this check, the event fires on button down and up
    //     if (hit.collider != null && ctx.ReadValueAsButton())
    //     {
    //         // If the hit has the Pawn comp
    //         // The C# modifier out basically spits whatever its modifying out into the scope as a var
    //         if (hit.collider.TryGetComponent(out Pawn pawn))
    //         {
    //             SelectPawn(pawn);
    //         }

    //         // If the hit has the TilemapCollider2D comp AKA the tilemap
    //         if (hit.collider.TryGetComponent(out TilemapCollider2D tilemapCollider))
    //         {
    //             // The position of the tilemap is at the origin, so we have to do some steps to get an actual grid location
    //             Tilemap tilemap = tilemapCollider.GetComponent<Tilemap>();
    //             // Converts a point in the world to a grid location
    //             Vector3Int cellPosition = tilemap.WorldToCell(hit.point);
    //             // Gets the center of the tile at that grid location
    //             Vector2 cellCenter = tilemap.GetCellCenterWorld(cellPosition);
    //             MovePawn(cellCenter);
    //         }
    //     }

    // }

    // private Pawn _activePawn;
    // private Image _selectPreview;
    // private HighlightGridController _highlightGridController;
    // public int previewScale = 50;

    // private void SelectPawn(Pawn pawn)
    // {
    //     _activePawn = pawn;

    //     if (_selectPreview != null)
    //     {
    //         // Sets the sprite of the preview to the currently selected Pawn
    //         _selectPreview.sprite = pawn.gameObject.GetComponent<SpriteRenderer>().sprite;
    //         // Changes the size of the preview to the pawn's size * a scalar because UI size is different
    //         _selectPreview.rectTransform.sizeDelta = _selectPreview.sprite.bounds.size * previewScale;

    //     }
    // }

    // private void MovePawn(Vector2 cellCenter)
    // {
    //     // If there is a pawn selected, then change its position and clear the selection
    //     if (_activePawn != null)
    //     {
    //         // Moves the pawn and deselects it
    //         _activePawn.transform.position = cellCenter;
    //         _activePawn = null;

    //         // Resets the preview
    //         _selectPreview.sprite = null;
    //         _selectPreview.rectTransform.sizeDelta = new(0, 0);

    //         // Clears movement highlights
    //         _highlightGridController.ClearHighlight();
    //     }
    // }

    void Start()
    {
        // Grabs the CharacterController to whatever this is attached to
        _characterController = GetComponent<CharacterController>();
        // This searches the scene for a GO with this tag, should only ever be one
        // _selectPreview = GameObject.FindWithTag("SelectPreview").GetComponent<Image>();
        // _highlightGridController = GameObject.FindWithTag("HighlightGrid").GetComponent<HighlightGridController>();
    }

    void FixedUpdate()
    {
        _characterController.Move(panSpeed * Time.deltaTime * panInput);
    }

}
