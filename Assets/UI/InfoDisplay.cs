using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI hpDisplay;
    public Pawn pawn;
    void Start()
    {
        if (pawn && hpDisplay)
        {
            hpDisplay.text = pawn.health.ToString();
        }
    }

    void Update()
    {
        hpDisplay.text = pawn.health.ToString();
    }
}
