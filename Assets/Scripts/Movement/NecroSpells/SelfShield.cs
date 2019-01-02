using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfShield : MonoBehaviour {

    public GameObject[] preFabs;
    public Vector3[] offsets;
    public GameObject summonedBlock;

    public GameObject necroMancer;

#pragma warning disable 649
    [SerializeField]
    List<GameObject> shields;
#pragma warning restore 649

    IEnumerator CreateList()
    {
        foreach(GameObject obj in shields)
        {
            obj.SetActive(true);
            yield return null;
        }
    }

    public void RemoveListItem(GameObject objReference)
    {
        foreach (GameObject obj in shields)
        {
            print("removeList function is working in the overshield");
            if (obj.activeInHierarchy)
            {
                print("the function returns");
                return;
            }
        }

        if (gameObject.activeInHierarchy)
        {
            Invoke("DisableShield", 0.25f);
        }

    }

    void DisableShield()
    {
        necroMancer.GetComponent<CapsuleCollider2D>().enabled = true;
        //this.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void Awake()
    {
        necroMancer = transform.parent.gameObject;
        GameObject block = Instantiate(summonedBlock, GameObject.Find("SummonedBlockTransform").transform) as GameObject;
        block.transform.position = GameObject.Find("SummonedBlockTransform").transform.position;
        block.AddComponent<DestroyWithPlayer>();
    }

    // Use this for initialization
    void OnEnable () {
        necroMancer.GetComponent<CapsuleCollider2D>().enabled = false;
        StartCoroutine(CreateList());
	}

    private void OnApplicationQuit()
    {
        isQuit = true;
    }

    bool isQuit;
    private void OnDisable()
    {
        if (isQuit == false&&HudCanvas.instance.CurrentHP>0)
        {
            necroMancer.GetComponent<CapsuleCollider2D>().enabled = true;
            necroMancer.GetComponent<NecroMancerBehavior>().canHit = true;
        }
    }

}
