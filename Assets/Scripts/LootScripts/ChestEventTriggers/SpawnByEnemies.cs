using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnByEnemies : MonoBehaviour {

    public List<GameObject> enemies;
#pragma warning disable 649
    [SerializeField]
    GameObject lootBox;
#pragma warning restore 649

    // Use this for initialization
    void Start () {
        lootBox.SetActive(false);
	}
	
    public void Remove(GameObject enemy)
    {
        print("item has been removed");
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            StartCoroutine(ShowDestination());
        }
    }

    IEnumerator ShowDestination()
    {
        yield return new WaitForSeconds(0.5f);
        CameraController.instance.GoToTransformPosition(lootBox.transform.position);
        yield return new WaitUntil(() => CameraController.instance.targetReached);

        lootBox.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        CameraController.instance.followPlayer = true;
        //player revealing music

        yield return null;
    }
}
