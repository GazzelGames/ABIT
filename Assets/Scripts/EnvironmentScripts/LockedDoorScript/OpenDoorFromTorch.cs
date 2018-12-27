using System.Collections;
using UnityEngine;

public class OpenDoorFromTorch : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private Sprite[] sprite;
#pragma warning restore 649

    //the doors that need to open
    private GameObject[] otherDoor;
    //the torches that i will be listening to;
    public GameObject[] torches;

    private AudioSource audioSource;

    private void Awake()
    {
        otherDoor = new GameObject[2];
        otherDoor[0] = transform.GetChild(0).gameObject;
        otherDoor[1] = transform.GetChild(1).gameObject;
    }

    private void OnEnable()
    {
        foreach (GameObject obj in torches)
        {
            obj.GetComponent<FireTorchState>().torchState += CheckTorchState;
        }
    }

    private void OnDisable()
    {
        foreach(GameObject obj in torches)
        {
            obj.GetComponent<FireTorchState>().torchState -= CheckTorchState;
        }
    }

    void CheckTorchState()
    {
        foreach (GameObject obj in torches)
        {
            if (obj.GetComponent<FireTorchState>())
            {
                if (obj.GetComponent<FireTorchState>().torchOnFire == false)
                {
                    return;
                }
            }

        }

        StartCoroutine(OpenDoor());
    }

    public IEnumerator OpenDoor()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        SpriteRenderer sprite1 = otherDoor[0].GetComponent<SpriteRenderer>();
        SpriteRenderer sprite2 = otherDoor[1].GetComponent<SpriteRenderer>();
        for (int i = 0; i < sprite.Length; i++)
        {
            sprite1.sprite = sprite[i];
            sprite2.sprite = sprite[i];
            yield return new WaitForSeconds(0.1f);
        }
        audioSource.Stop();
        audioSource.enabled = false;
        BoxCollider2D[] doorColliders = new BoxCollider2D[GetComponentsInChildren<BoxCollider2D>().Length];
        doorColliders = GetComponentsInChildren<BoxCollider2D>();


        AudioClip victoryJingle = Resources.Load<AudioClip>("Audio/Jingle_Achievement_00");
        AudioSource.PlayClipAtPoint(victoryJingle, transform.position);

        foreach (BoxCollider2D collider in doorColliders)
        {
            
            collider.enabled = false;
        }
    }
}