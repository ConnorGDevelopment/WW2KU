using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CardData", menuName = "CardData", order = 0)]
public class CardData : ScriptableObject
{
    public int damage;
    public int range;
    public Vector3Int[] shape;
}

