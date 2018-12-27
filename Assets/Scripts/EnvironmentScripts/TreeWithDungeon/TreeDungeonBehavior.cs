using UnityEngine;

public class TreeDungeonBehavior : MonoBehaviour {

#pragma warning disable 649
    [SerializeField]
    private Sprite openDoorSprite;
#pragma warning restore 649

    private SpriteRenderer spriteRenderer;
    private GameObject entrance;

    private void Start()
    {
        entrance = transform.parent.transform.GetChild(0).gameObject;
        entrance.SetActive(false);
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    public bool test;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.sprite = openDoorSprite;
        entrance.SetActive(true);
    }

    public void ChangeSprite()
    {
        spriteRenderer.sprite = openDoorSprite;
    }
}
