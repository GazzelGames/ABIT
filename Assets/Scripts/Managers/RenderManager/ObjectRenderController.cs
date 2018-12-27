﻿using UnityEngine;

public class ObjectRenderController : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void Update()
    {
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
