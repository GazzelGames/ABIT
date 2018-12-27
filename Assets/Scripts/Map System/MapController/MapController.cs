using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    public GameObject mapObj;
    public float clampX,clampY;
    public float clampScale;
    public float xPos;
    public float yPos;

    RectTransform mapTransform;

    public float speed;

    private void OnEnable()
    {
        clampX = 140f;
        clampY = 130f;

    }

    // Update is called once per frame
    void Update () {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        xPos = mapObj.transform.localPosition.x;
        yPos = mapObj.transform.localPosition.y;
        Vector3 currentVector = new Vector3(x, y, 0f);
        if (Input.GetButton("Fire1"))
        {
           ZoomMap();
        }else if(Input.GetButton("Fire2"))
        {
           ZoomOutMap();
        }
        mapObj.transform.localPosition = new Vector3(Mathf.Clamp(mapObj.transform.localPosition.x, -clampX, clampX), Mathf.Clamp(mapObj.transform.localPosition.y, -clampY, clampY), 0f);
        mapObj.transform.Translate(currentVector * Time.deltaTime * speed);
    }

    void ZoomMap()
    {
        if (mapObj.transform.localScale.x < 2)
        {
            mapObj.transform.localScale += new Vector3(1f*Time.deltaTime, 1f * Time.deltaTime, 0);
            clampScale = mapObj.transform.localScale.x;
            ScalingClamp();
        }
    }
    void ZoomOutMap()
    {
        if (mapObj.transform.localScale.x > 1)
        {
            mapObj.transform.localScale -= new Vector3(1f * Time.deltaTime, 1f * Time.deltaTime, 0);
            clampScale = mapObj.transform.localScale.x;
            ScalingClamp();
        }
    }

    void ScalingClamp()
    {
        clampX = Mathf.Lerp(140, 1000, clampScale - 1);
        clampY = Mathf.Lerp(130, 500, clampScale - 1);
    }
}
