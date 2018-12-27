using System.Collections;
using UnityEngine;

public class ShieldSpell : MonoBehaviour {

    private KeyCode keyBind;

    public void SetKeyCode(KeyCode newKeyCode)
    {
        keyBind = newKeyCode;
    }

    private void OnEnable()
    {
        decideKeyBind();
        PlayerMangerListener.instance.HasControl = false;
        StartCoroutine(RunShield());
    }

    IEnumerator RunShield()
    {
        do
        {
            transform.Rotate(new Vector3(0, 0, 720 * Time.deltaTime));
            yield return null;
        }
        while (Input.GetKey(keyBind));
        PlayerMangerListener.instance.HasControl = true;
        Destroy(gameObject);
    }

    void decideKeyBind()
    {
        if (Input.GetKey(KeyCode.Keypad1))
        {
            keyBind = KeyCode.Keypad1;
        }
        else if (Input.GetKey(KeyCode.Keypad2))
        {
            keyBind = KeyCode.Keypad2;
        }
        else if (Input.GetKey(KeyCode.Keypad3))
        {
            keyBind = KeyCode.Keypad3;
        }

    }
}
