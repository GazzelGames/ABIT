using System.Collections;
using UnityEngine;

public class BridgeListener : MonoBehaviour {

    public GameObject[] buttons;
    bool[] buttonState;

    private void OnEnable()
    {
        buttonState = new bool[buttons.Length];
        for(int i =0; i<buttons.Length; i++)
        {
            buttons[i].GetComponent <FloorButtonScript>().buttonState += CheckBoolStates;
        }
        
    }

    private void OnApplicationQuit()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<FloorButtonScript>().buttonState -= CheckBoolStates;
        }

    }

    void CheckBoolStates()
    {
        for (int i=0; i<buttons.Length; i++)
        {
            buttonState[i] = buttons[i].GetComponent<FloorButtonScript>().ButtonPressed;
        }

        if (BoolFunction())
        {
            StartCoroutine(MoveCamera());
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    IEnumerator MoveCamera()
    {
        PlayerMangerListener.instance.HasControl = false;
        CameraController.instance.GoToTransformPosition(transform.position);
        yield return new WaitUntil(() => CameraController.instance.targetReached);
        //player unlock music
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        CameraController.instance.followPlayer = true;
        PlayerMangerListener.instance.HasControl = true;
    }

    bool BoolFunction()
    {
        for (int i = 0;i<buttonState.Length;i++)
        {
            if (!buttonState[i])
            {
                return false;
            }
        }
        return true;
    }

    public void RespawnEnemies()
    {

    }

}
