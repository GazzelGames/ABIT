using UnityEngine;

[System.Serializable]
public class BasicItemValues {
    [TextArea(3, 10)]
    public string discription;

    public string itemName;
    public int moneyValue;
    public Sprite itemImage;
}
