using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardData cardData;
    private Orchestrator _orch;

    void Start()
    {
        _orch = GameObject.FindWithTag("Orch").GetComponent<Orchestrator>();
    }


}
