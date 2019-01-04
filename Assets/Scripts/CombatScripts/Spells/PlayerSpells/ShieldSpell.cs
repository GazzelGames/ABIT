using UnityEngine;

public class ShieldSpell : MonoBehaviour {

    private KeyCode keyBind;

    private void Awake()
    {
        DecideKeyBind();
        PlayerMangerListener.instance.HasControl = false;
        HudCanvas.instance.magicDrain = true;
        HudCanvas.instance.magicDrainValue = 0.75f;
    }

    private void LateUpdate()
    {
        transform.Rotate(new Vector3(0, 0, -360 * Time.deltaTime));
        if (!Input.GetKey(keyBind)||(HudCanvas.instance.CurrentMagic<=0))
        {
            PlayerMangerListener.instance.HasControl = true;
            HudCanvas.instance.magicDrain = false;
            HudCanvas.instance.magicDrainValue = 0f;
            Destroy(gameObject);
        }
    }

    void DecideKeyBind()
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
        }else if (Input.GetKey(KeyCode.I))
        {
            keyBind = KeyCode.I;
        }
        else if (Input.GetKey(KeyCode.O))
        {
            keyBind = KeyCode.O;
        }
        else if (Input.GetKey(KeyCode.P))
        {
            keyBind = KeyCode.P;
        }
    }
}
