using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtonScript : MonoBehaviour {

    public delegate void ButtonDelegate();
    public ButtonDelegate buttonState;

    bool buttonPressed;
    public bool ButtonPressed { get { return buttonPressed; } set { buttonPressed = value; }}
#pragma warning disable 649
    [SerializeField]
    private Sprite[] sprites;
#pragma warning restore 649

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        buttonPressed = true;
        GetComponent<SpriteRenderer>().sprite = sprites[1];
        buttonState();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        buttonPressed = false;
        GetComponent<SpriteRenderer>().sprite = sprites[0];
        buttonState();
    }
}
