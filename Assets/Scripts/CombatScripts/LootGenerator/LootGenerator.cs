using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGenerator : MonoBehaviour {

    //array of loot GameObjects
#pragma warning disable 649
    [SerializeField]
    private GameObject[] loot;
#pragma warning restore 649

    private void OnEnable()
    {
        StartCoroutine(GenerateLoot());
    }

    IEnumerator GenerateLoot()
    {

        bool fullHealth = false;

        int lootElement;

        lootElement = Random.Range(0, loot.Length + 3);

        if (HudCanvas.instance.CurrentHP == HudCanvas.instance.MaxHP)
        {
            fullHealth = true;
        }
        while(lootElement==0 && fullHealth)
        {
            lootElement = Random.Range(1, loot.Length + 3);
        }

        if (lootElement < loot.Length)
        {
            Instantiate(loot[lootElement], transform.position, transform.rotation, null);
        }
        Destroy(gameObject);
        yield return null;
    }

}
