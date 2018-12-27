using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoorDungeon : MonoBehaviour {

#pragma warning disable 649
    [SerializeField]
    private Sprite[] sprite;
    private GameObject door2;
    private GameObject door1;
    private AudioSource audioSource;
#pragma warning restore 649

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //need to add key here
        if (collision.gameObject.CompareTag("Player")&&MoutianDungeonManager.instance.KeyCounter>0)
        {
            audioSource = GetComponent<AudioSource>();
            door1 = transform.GetChild(0).gameObject;
            door2 = transform.GetChild(1).gameObject;
            StartCoroutine(OpenLockedDoors());
            MoutianDungeonManager.instance.KeyCounter--;
        }
    }

    public IEnumerator OpenLockedDoors()
    {
        audioSource.Play();
        SpriteRenderer sprite1 = door1.GetComponent<SpriteRenderer>();
        SpriteRenderer sprite2 = door2.GetComponent<SpriteRenderer>();

        for (int i =0; i<sprite.Length; i++)
        {
            sprite1.sprite = sprite[i];
            sprite2.sprite = sprite[i];
            yield return new WaitForSeconds(0.1f);
        }
        audioSource.Stop();
        audioSource.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return null;
    }
}
