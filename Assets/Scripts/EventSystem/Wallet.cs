using System.Collections;
using UnityEngine;

public class Wallet : MonoBehaviour {

    public NormalDialogue nd;
    public GameObject moneyValue;
#pragma warning disable 649
    [SerializeField]
    private Equipment wallet;
#pragma warning restore 649

    private void Start()
    {
        moneyValue = GameObject.Find("HudCanvas").transform.GetChild(2).gameObject;
        moneyValue.SetActive(true);
        GetComponent<AudioSource>().volume = GameManager.instance.VolumeModifier;
        GetComponent<AudioSource>().Play();
        HudCanvas.instance.MaxCurrency = wallet.itemValues.moneyValue;
        transform.position += new Vector3(0, 5, 0);
        StartCoroutine(ItemDiscription());
        PauseMenuManager.instance.ChangeEquipmentItem(3, wallet);
    }

    IEnumerator ItemDiscription()
    {
        yield return new WaitForSeconds(1.0f);
        DialogueManager.instance.StartTalking(nd);
        PlayerMangerListener.instance.InScene = false;
        yield return new WaitUntil(() =>DialogueManager.instance.speechBox.activeInHierarchy==false);
        //yield return DialogueManager.instance.SceneDialogue(nd);
        Destroy(gameObject);
        yield return null;
    }

}
