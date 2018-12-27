using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretWallScript : MonoBehaviour {

    SpriteRenderer sprite;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        while (sprite.color.a<0.99)
        {
            float alpha = sprite.color.a;
            alpha = Mathf.Lerp(alpha, 1, 0.05f);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            yield return null;
        }
        yield return null;
    }
    IEnumerator FadeOut()
    {
        while (sprite.color.a>0.001f)
        {
            float alpha = sprite.color.a;
            alpha = Mathf.Lerp(alpha, 0, 0.07f);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            yield return null;
        }
        yield return null;
    }
}

