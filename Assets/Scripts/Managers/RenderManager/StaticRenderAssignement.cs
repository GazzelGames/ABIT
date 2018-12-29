using UnityEngine;

public class StaticRenderAssignement : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        if (transform.parent.transform.position.y < 0)
        {
            spriteRenderer.sortingOrder = Mathf.Abs(Mathf.RoundToInt(transform.parent.transform.position.y));
        }
        else
        {
            spriteRenderer.sortingOrder = -Mathf.RoundToInt(transform.parent.transform.position.y);
        }
    }
}
