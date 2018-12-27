using UnityEngine;

public class Frozen : MonoBehaviour {

    private float iceHP;

    private void Start()
    {
        iceHP = 3;
    }

    //this will allow the player to push the ice cube and destory it with either sword or rock punching gauntlet
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerSword")
        {
            iceHP = iceHP - PauseMenuManager.instance.SwordDamage();
        }
    }

}
