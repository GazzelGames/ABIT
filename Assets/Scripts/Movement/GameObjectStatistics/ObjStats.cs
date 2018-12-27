using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjStats : MonoBehaviour {

#pragma warning disable 649
    [SerializeField]
    private int health, status, emotionStatus;
    //[SerializeField]
    //private float movementSpeed;
    [SerializeField]
    private bool isTalking, onPatrol, fight;
#pragma warning restore 649

    private void Awake()
    {
        isTalking = true;
    }

    //this stuff will all be handled in properties essentially eliminating the need for update and the stuff

    //here is where we will assign our properties
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

   // public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }

    public bool canTalk;
    public bool IsTalking {
        get
        {
            return isTalking;
        }
        set
        {
            isTalking = value;
        }
    }
    public bool OnPatrol { get { return onPatrol; } set { onPatrol = value; } }
    public bool Fight { get { return fight; } set { fight = value; } }


}
