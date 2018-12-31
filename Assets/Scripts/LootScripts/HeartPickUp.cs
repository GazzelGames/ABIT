using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickUp : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioClip moneySound = Resources.Load<AudioClip>("Audio/MoneyPickUp");
            AudioSource.PlayClipAtPoint(moneySound, transform.position, 1);
            HudCanvas.instance.CurrentHP = 1;
            Destroy(gameObject);
        }
    }
}
