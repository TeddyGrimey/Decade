using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterControllerScript : MonoBehaviour {

	private CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
	public float speed = 1.5f;
	private float radialDeadZone  = 0.25f;
	private float angularVelocity = 12.0f;
	public float gravity = 0.5f;

	public int cityPop = 0;
	public int colPop = 0;

	public float gameSpeed = 1;

	public List<GameObject> nodeList = new List<GameObject>();
	public float currentNode = 0;

	public string gameState = "load";

	private bool sim = false;
	private int simTime = 0;

	public Text canvas; 

	// Use this for initialization
	void Start () {
		controller = gameObject.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

		if(nodeList.Count ==  currentNode){
			gameState = "awake";
			sim = true;
			canvas.text = "";
		}
		else{
			canvas.text = "Loading..." + (Mathf.Round(Mathf.InverseLerp(0, nodeList.Count,currentNode) * 100).ToString());
		}
		if(sim && simTime <= 100){
			gameSpeed = 1;
			simTime ++;
		}
		else if(simTime == 101){
			gameSpeed = 1;

		}



		moveDirection = Vector3.zero;

		Vector3 direction = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
		float angleRads = this.transform.rotation.eulerAngles.y * Mathf.PI / 180;
		Quaternion currentRotation = Quaternion.LookRotation(Vector3.up, direction);
		if (direction.magnitude > radialDeadZone){
			this.transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, Time.smoothDeltaTime * angularVelocity);
			
			Vector3 movement = moveDirection * Time.smoothDeltaTime * speed;
			moveDirection = direction;
		}

		moveDirection.y = -gravity;


		moveDirection *= Time.smoothDeltaTime * speed;

		controller.Move(moveDirection);


	}
}
