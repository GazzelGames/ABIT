
using UnityEngine;
using IceTile;

public class IceTileBehavior : MonoBehaviour {

#pragma warning disable 649
    [SerializeField]
    IceTileTraits iceTile;
#pragma warning restore 649
    // Use this for initialization
    void Start ()
    {
        iceTile.Constructor(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tagName = collision.tag;
        if(tagName == "PlayerSword")
        {
            iceTile.BlockHit(ObjectTagName.Sword);
        }else if(tagName == "Gauntlets")
        {
            iceTile.BlockHit(ObjectTagName.Gauntlets);
        }else if (tagName == "FireBall")
        {
            iceTile.BlockHit(ObjectTagName.Fireball);
            Destroy(collision.gameObject,0.25f);
        }
        else
        {
            iceTile.BlockHit(ObjectTagName.EverythingElse);
        }
    }
}

namespace IceTile
{
    public enum ObjectTagName { Sword, Gauntlets, Fireball, EverythingElse };

    [System.Serializable]
    public class IceTileTraits
    {
        float hp = 3;
        float HP
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp < 3 / 2 && hp > 0)
                {
                    //Player Ice Craking Sounds
                    //set the sprite to  breaking Ice

                }
                else if (hp <= 0)
                {

                    DisableCube();
                    AudioSource.PlayClipAtPoint(breakingCube, self.transform.position);
                }
                Debug.Log("Current block Health: " + hp.ToString());
            }
        }

#pragma warning disable 649
        [SerializeField]
        Sprite breakingIce;
        [SerializeField]
        AudioClip swordHittingIce;
        [SerializeField]
        AudioClip breakingCube;
        [SerializeField]
        ParticleSystem destructionParticles;
#pragma warning restore 649
        private GameObject self;

        public void Constructor(GameObject newSelf)
        {
            self = newSelf;
        }

        void DisableCube()
        {
            Debug.Log("Cube Destroyed");
            self.SetActive(false);
            //breakingCube.Play();
        }

        public void BlockHit(ObjectTagName enumTag)
        {
            switch (enumTag)
            {
                case ObjectTagName.Sword:
                    {
                        Debug.Log("play Sword Hitting Sound");
                        HP = HP - 0.1f;
                        break;
                    }
                case ObjectTagName.Gauntlets:
                    {
                        Debug.Log("Player Gauntlets Hitting Ice Sound");
                        HP = HP - 2;
                        break;
                    }
                case ObjectTagName.Fireball:
                    {
                        Debug.Log("Play Fire Explosion Sounds");
                        HP = 0;
                        break;
                    }
                case ObjectTagName.EverythingElse:
                    {
                        Debug.Log("Play plinking sound");
                        //player specific sound
                        break;
                    }
            }
        }
    }

}
