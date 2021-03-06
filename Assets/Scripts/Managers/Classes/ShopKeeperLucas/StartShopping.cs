﻿using UnityEngine;

public class StartShopping : MonoBehaviour {

    public GameObject shoppingMenu;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMangerListener.instance.StateOf = GameState.StateOfGame.PlayerShopping;
        shoppingMenu.SetActive(true);
        CameraController.instance.variablePos = transform.parent.gameObject.transform.position;
        if (GetComponentInParent<Animator>())
        {
            GetComponentInParent<Animator>().enabled = false;
        }
        //PlayerController.instance.enabled = false;
    }
}
