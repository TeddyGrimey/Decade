using UnityEngine;
using System.Collections;

public class CharacterControllerScript : MonoBehaviour {

	private CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
	public float speed = 1.5f;
	private float radialDeadZone  = 0.25f;
	private float angularVelocity = 12.0f;
	public float gravity = 0.5f;

	// Use this for initialization
	void Start () {
		controller = gameObject.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
	
		moveDirection = Vector3.zero;

		Vector3 direction = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
		float angleRads = this.transform.rotation.eulerAngles.y * Mathf.PI / 180;
		Quaternion currentRotation = Quaternion.LookRotation(Vector3.up, direction);
		if (direction.magnitude > radialDeadZone){
			this.transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, Time.deltaTime * angularVelocity);
			
			Vector3 movement = moveDirection * Time.deltaTime * speed;
			moveDirection = direction;
		}

		moveDirection.y = -gravity;


		moveDirection *= Time.deltaTime * speed;

		controller.Move(moveDirection);


	}
}
