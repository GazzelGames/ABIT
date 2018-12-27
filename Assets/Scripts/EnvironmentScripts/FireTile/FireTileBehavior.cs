using UnityEngine;

public class FireTileBehavior : MonoBehaviour {

    AudioClip fireSizzel;

    public bool test = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        test = true;
        string tagName = collision.tag;
        if(tagName == "IceSpell")
        {
            Destroy(gameObject,0.1f);
        }
    }
}

namespace FireTile
{
    public enum ObjectTagFireTile { IceSpell }

    public class FireTileTrait
    {
        //what do i want to add to this behavior
        

        public void DestoryTile()
        {

        }
    }
}