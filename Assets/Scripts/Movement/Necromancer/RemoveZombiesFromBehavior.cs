using UnityEngine;

public class RemoveZombiesFromBehavior : MonoBehaviour {

    [HideInInspector]
    public GameObject parent;
    bool isQuitting;

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDisable()
    {
        if (isQuitting == false)
        {
            parent.GetComponent<NecroMancerBehavior>().zombies.Remove(gameObject);
            //Destroy(gameObject,1);
        }
    }
}
