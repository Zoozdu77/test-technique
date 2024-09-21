using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour 
{
    //Les tiles en jeu
    [HideInInspector] public int typeId;
    [HideInInspector] public float speedInMult;
    [HideInInspector] public float speedOutMult;
    [HideInInspector] public bool canWalk;
    [HideInInspector] public bool slippery;
    [HideInInspector] public bool ContainSmtg;
    //[HideInInspector] public Tile[] nodes;
}

