using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairPhysics : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController.instance.playerSpeed *= 0.5f;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController.instance.playerSpeed /= 0.5f;
    }
}
