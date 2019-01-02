using System.Collections;
using UnityEngine;

public class Mushroom : MonoBehaviour {

    public ActionItem shield;
    public QuestItem questMushroom;
    public NormalDialogue[] nd;
    public NormalDialogue nothinghappened;

    public GameObject shieldSpell; //this reference is for the iron sword gameobject

    private GameObject player;  // this reference is the player object
    public GameObject witch;

    private void Start()
    {
        PlayerMangerListener.instance.HasControl = false;
        player = GameObject.Find("Player");
        witch = GameObject.Find("Enchantress");
        transform.position = player.transform.position + new Vector3(0, 2, 0);
        player.GetComponent<Animator>().SetBool("VictoryPose", true);
        PlayerMangerListener.instance.StateOf = GameState.StateOfGame.PlayerTalking;

        try
        {
            if ((transform.position - witch.transform.position).magnitude < 8f)
            {
                player.GetComponent<Animator>().SetBool("VictoryPose", true);
                Invoke("DisableRender", 1); //return player back to normal stance with the this invoke
                StartCoroutine(GetShieldScene());
                if (witch.transform.GetChild(0).gameObject.GetComponent<WitchDialogue>())
                {
                    witch.transform.GetChild(0).gameObject.GetComponent<WitchDialogue>().waitingForMushroom = false;
                    witch.transform.GetChild(0).gameObject.GetComponent<WitchDialogue>().mushRoomRecieved = true;
                }
            }
            else
            {
                StartCoroutine(NothingThere());
            }
        }
        catch
        {
            StartCoroutine(NothingThere());
        }

    }

    void DisableRender()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<Animator>().SetBool("VictoryPose", false);
    }

    IEnumerator GetShieldScene()
    {
        PlayerMangerListener.instance.InScene = true;

        DialogueManager.instance.StartTalking(nd[0]);
        yield return new WaitUntil(() => DialogueManager.instance.enabled == false);

        GameObject sSpell = Instantiate(shieldSpell, player.transform.position + new Vector3(0, 5, 0), transform.rotation);   //spawns the sword about the player
        player.GetComponent<Animator>().SetBool("VictoryPose", true);

        print("play victory music then make coroutine wait until its finished");
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<Animator>().SetBool("VictoryPose", false);
        sSpell.transform.position = player.transform.position;
        PauseMenuManager.instance.AddActionItem(2, shield);
        Destroy(sSpell);

        PlayerMangerListener.instance.InScene = false;

        DialogueManager.instance.StartTalking(nd[1]);
        yield return new WaitUntil(() => DialogueManager.instance.enabled == false);

        PauseMenuManager.instance.EndQuestItem(questMushroom);

        PlayerMangerListener.instance.HasControl = true;
        Destroy(gameObject, 0.5f);
        yield return null;
    }

    IEnumerator NothingThere()
    {
        player.GetComponent<Animator>().SetBool("VictoryPose", true);
        yield return new WaitForSeconds(1);

        DialogueManager.instance.StartTalking(nothinghappened);
        yield return new WaitUntil(() => DialogueManager.instance.enabled == false);

        player.GetComponent<Animator>().SetBool("VictoryPose", false);
        PlayerMangerListener.instance.HasControl = true;
        Destroy(gameObject);
        yield return null;
    }
}

