using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPickUP : MonoBehaviour {

#pragma warning disable 649
    [SerializeField]
    private int currencyAmount;
#pragma warning restore 649

    private void Start()
    {
        PlayerMangerListener.PlayerDead += PlayerDied;
    }

    private void OnDestroy()
    {
        PlayerMangerListener.PlayerDead -= PlayerDied;
    }

    void PlayerDied()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HudCanvas.instance.CurrentCurrency = currencyAmount;
            AudioClip moneySound = Resources.Load<AudioClip>("Audio/MoneyPickUp");
            AudioSource.PlayClipAtPoint(moneySound, transform.position, 1);
            Destroy(gameObject);
        }
    }
}
