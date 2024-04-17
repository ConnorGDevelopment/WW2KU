using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public string pawnName;
    public int movement;

    private Orchestrator _orch;

    void Start()
    {
        _orch = GameObject.FindWithTag("Orch").GetComponent<Orchestrator>();
    }

    void OnMouseDown()
    {
        Debug.Log($"Pawn: {pawnName} clicked");
        _orch.SelectPawn(this);
    }
}
