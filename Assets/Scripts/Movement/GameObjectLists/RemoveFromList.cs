using UnityEngine;

public class RemoveFromList : MonoBehaviour {

    public GameObject shield;
    bool isQuit;

    public void InitializeVariables(GameObject objReference)
    {
        shield = objReference;
    }

    private void OnApplicationQuit()
    {
        isQuit = true;
    }

    private void OnDisable()
    {
        if (isQuit == false)
        {
            if (shield != null) {
                if (shield.GetComponent<NecroShields>())
                {
                    shield.GetComponent<NecroShields>().RemoveListItem(gameObject);
                }
                else if (shield.GetComponent<SelfShield>())
                {
                    shield.GetComponent<SelfShield>().RemoveListItem(gameObject);
                }
            }
            Destroy(gameObject, 1);
        }

    }
}