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

    private Orchestrator _orch;

    public void MovePlayer(InputAction.CallbackContext ctx)
    {
        panInput = ctx.ReadValue<Vector2>();
    }



    public void Select(InputAction.CallbackContext ctx)
    {
        // Makes ray from mouse position through screen
        // Starts from the camera because rays have to originate from a thing
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // If the ray has a hit
        // ReadValueAsButton() return a boolean
        // Without this check, the event fires on button down and up
        if (hit.collider != null && ctx.ReadValueAsButton())
        {
            // If the hit has the Pawn comp
            // The C# modifier out basically spits whatever its modifying out into the scope as a var
            if (hit.collider.TryGetComponent(out Pawn pawn))
            {
                pawn.GotClicked();
            }

            // If the hit has the TilemapCollider2D comp AKA the tilemap
            // This could be an else statement since we don't actually use the tilemapCollider var
            // But this way makes sure that what we hit is a tilemap collider and prevents unintended effects
            // Also we'll probably build on this, so keeping it open ended is good
            if (hit.collider.TryGetComponent(out TilemapCollider2D tilemapCollider))
            {
                _orch.SelectTile(hit.point);
            }
        }

    }

    void Start()
    {
        // Grabs the CharacterController to whatever this is attached to
        _characterController = GetComponent<CharacterController>();
        // This searches the scene for a GO with this tag, should only ever be one Orch
        _orch = GameObject.FindWithTag("Orch").GetComponent<Orchestrator>();
    }

    void FixedUpdate()
    {
        _characterController.Move(panSpeed * Time.deltaTime * panInput);
    }

}
