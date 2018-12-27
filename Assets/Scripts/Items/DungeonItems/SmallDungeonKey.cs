using UnityEngine;

public class SmallDungeonKey : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MoutianDungeonManager.instance.KeyCounter++;
        Destroy(gameObject);
        
    }
}
