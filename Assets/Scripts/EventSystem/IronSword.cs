using UnityEngine;

public class IronSword : MonoBehaviour {

#pragma warning disable 649
    [SerializeField]
    private Equipment ironSword;

    public NormalDialogue nd;
#pragma warning restore 649
    private void Start()
    {
        GetComponent<AudioSource>().volume = GameManager.instance.VolumeModifier;
        GetComponent<AudioSource>().Play();
        nd.dialogue = new string[1];
        nd.dialogue[0] = ironSword.itemValues.discription;
        DialogueManager.instance.StartTalking(nd);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PauseMenuManager.instance.ChangeEquipmentItem(2, ironSword);
            PlayerMangerListener.instance.HasSword = true;
            Destroy(gameObject);
        }
    }
}
