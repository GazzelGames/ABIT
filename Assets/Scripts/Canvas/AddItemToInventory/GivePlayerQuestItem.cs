using UnityEngine;

public class GivePlayerQuestItem : MonoBehaviour {

    public QuestItem questItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            PauseMenuManager.instance.CreateQuestItem(questItem);
            //PlayerMangerListener.instance.Talking = false;
            Destroy(gameObject);
        }
    }
}
