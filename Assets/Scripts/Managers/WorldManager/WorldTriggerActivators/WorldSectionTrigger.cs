using System.Collections.Generic;
using UnityEngine;

public class WorldSectionTrigger : MonoBehaviour {
    [HideInInspector]
    public List<GameObject> areaObjects = new List<GameObject>();

    public WorldState.WorldStateEnum stateEnum;

    private void Awake()
    {
        int childCount = transform.childCount;
        for(int i=0;i<childCount; i++)
        {
            areaObjects.Add(transform.GetChild(i).gameObject);
            areaObjects[i].SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (stateEnum != WorldManager.instance.worldState)
            {
                foreach (GameObject objs in areaObjects)
                {
                    objs.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (GameObject objs in areaObjects)
            {
                objs.SetActive(false);
            }
        }

    }

}
