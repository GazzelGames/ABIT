using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public static CameraController instance;

	public GameObject Player;
	public float cameraSpeed;
	//float CameraSize;
    [HideInInspector]
	public bool followPlayer;
	public Vector3 variablePos;

    //[HideInInspector]
    public bool targetReached;

    // Use this for initialization
    void Awake () {
		MakeInstince();
		//MainCam.orthographicSize = 2.7f;
		InitializeCamera();
        targetReached = false;
		followPlayer = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (followPlayer){
            FollowPlayer();
        }
        else if(!targetReached)
        {
            TargetTransform();
        }
	}

    public float x;
    public float y;
    private void LateUpdate()
    {
        //I Need to move camera to closest unit
        Vector3 position = transform.position;
        x = position.x = Mathf.Round(position.x * 32)/32;
        y = position.y = Mathf.Round(position.y * 32)/32;
        transform.position = new Vector3(position.x,position.y,-5);

    }

    void MakeInstince(){
		if (instance == null)
		{
			instance = this;
        }

	}	
	void FollowPlayer(){
		Vector3 playerOffset = Player.transform.position;
		playerOffset = new Vector3(playerOffset.x, playerOffset.y, -5);
		Vector3 currentVector = playerOffset - transform.position;
        transform.Translate(currentVector * Time.deltaTime * cameraSpeed);

        //transform.position += (currentVector * Time.deltaTime * cameraSpeed);

	}
	void TargetTransform(){
        Vector3 currentVec = variablePos - transform.position;
        Vector3 vectorOffset = new Vector3(currentVec.x, currentVec.y, 0);
        if (vectorOffset.magnitude > 0.1f)
        {
            transform.position += (vectorOffset * Time.deltaTime * cameraSpeed/2);
        }
        else
        {
            targetReached = true;
        }

    }
	void InitializeCamera(){
		transform.position = new Vector3 (Player.transform.position.x, Player.transform.position.y, -10);
	}

    public void GoToTransformPosition(Vector3 position)
    {
        followPlayer = false;
        targetReached = false;
        variablePos = position;
    }

    public void SetVariablePos(Vector3 vector)
    {
        Vector3 variablePos = vector;
        variablePos = new Vector3(variablePos.x, variablePos.y, 0);
    }

	public void SetTransform(Transform UpdatedPos){
		transform.position = UpdatedPos.position;
	}

}
