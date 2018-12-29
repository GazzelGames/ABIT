using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueTransmitter : MonoBehaviour {

    [HideInInspector]
    public static PlayerDialogueTransmitter instance;

    public GameObject objectReference;

    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    Animator anim;

    //so what i need to do is create a list of characters that are in range of the player that the player can  talk to
    //the update function will need to decide with of the objects is closest to the target. 
    // then you can press the c key to talk to that target

    // Use this for initialization
    void Start () {
        CreateSingleton();
        anim = GetComponentInParent<Animator>();
        PlayerMangerListener.GameStopped += DisableTalking;
    }

    private void OnApplicationQuit()
    {
        PlayerMangerListener.GameStopped -= DisableTalking;
    }

    private void SetTalkingStance()
    {
        if (objectReference != null)
        {
            Vector3 displacement = gameObject.transform.parent.position - objectReference.transform.position;
            float x = displacement.x;
            float y = displacement.y;
            float stance = 0;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x > 0)
                {
                    stance = 1;
                }
                else
                {
                    stance = 2;
                }
            }
            else
            {
                if (y > 0)
                {
                    stance = 0;
                }
                else
                {
                    stance = 3;
                }
            }

            anim.SetFloat("IdleStance", stance);
        }

    }

    private void DisableTalking()
    {
        if (objectReference != null)
        {
            DialogueManager.instance.ClearText();
            objectReference = null;
        }
    }
}
