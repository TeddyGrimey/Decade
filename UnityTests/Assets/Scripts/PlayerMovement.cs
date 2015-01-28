using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

	//Public variables
	public GameObject cam = null;
	public float turnSpeed = 50;
	public float moveSpeed = 50;

	public Shader shader1;
	public Shader shader2;

	//Private variables
	private Vector3 playerPos = new Vector3();
	private Vector3 playerRot = new Vector3();
	private Vector3 camPos = new Vector3();

	private float angularVelocity = 12.0f;
	private float radialDeadZone  = 0.25f;
	
	private GameObject currentHit = null;
	private List<Transform> fadeDown = new List<Transform>();
	private List<Transform> fadeUp = new List<Transform>();

	void Start () {
	
		playerPos = this.transform.localPosition;
		camPos = new Vector3(this.transform.position.x - cam.transform.position.x,this.transform.position.y - cam.transform.position.y, this.transform.position.z - cam.transform.position.z);

		shader1 = Shader.Find("Diffuse");
		shader2 = Shader.Find("TransparentDiffuse");
	}



	void Update () {
	
		Vector3 screenPos = cam.camera.WorldToScreenPoint (this.transform.position);
		Ray ray = cam.camera.ScreenPointToRay(screenPos);
		//Debug.DrawRay (ray.origin, ray.direction *  50, Color.yellow);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 100.0F)){
			Debug.DrawLine(ray.origin, hit.point);


			if(!fadeDown.Contains(hit.transform)){
				fadeDown.Add(hit.transform);
			}

			for(int i = 0; i < fadeDown.Count; i++){
				if(fadeDown[i] != hit.transform){
					fadeUp.Add(fadeDown[i]);
					fadeDown.Remove(fadeDown[i]);
				}
			}

			if(hit.transform.gameObject != currentHit){


				/*if(currentHit != null){
					currentHit.transform.gameObject.renderer.material.shader = shader1;
				}*/

				currentHit = hit.transform.gameObject;
			}
		}
		/*else if(currentHit != null){
			//currentHit.transform.gameObject.renderer.material.shader = shader1;
			//currentHit = null;
		}*/
		else{
			for(int i = 0; i < fadeDown.Count; i++){
				fadeUp.Add(fadeDown[i]);
				fadeDown.Remove(fadeDown[i]);
			}
		}

		for(int i = 0; i < fadeDown.Count; i++){
			fadeDown[i].transform.gameObject.renderer.material.shader = shader2;
			if(fadeDown[i].transform.renderer.material.color.a > 0.7){
				fadeDown[i].transform.renderer.material.color = new Color(fadeDown[i].transform.renderer.material.color.r,fadeDown[i].transform.renderer.material.color.g,fadeDown[i].transform.renderer.material.color.b,fadeDown[i].transform.renderer.material.color.a - 0.01f); 
			}
		}
		for(int i = 0; i < fadeUp.Count; i++){

			fadeUp[i].transform.renderer.material.color = new Color(fadeUp[i].transform.renderer.material.color.r,fadeUp[i].transform.renderer.material.color.g,fadeUp[i].transform.renderer.material.color.b,fadeUp[i].transform.renderer.material.color.a + 0.01f); 
			if(fadeUp[i].transform.renderer.material.color.a >= 1){
				fadeUp[i].transform.gameObject.renderer.material.shader = shader1;
				fadeUp.Remove(fadeUp[i]);
			}

		}



		//playerPos = new Vector3();

		float angleRads = this.transform.rotation.eulerAngles.y * Mathf.PI / 180;

		//print (this.transform.rotation.eulerAngles.y);

		if(Input.GetAxis("Vertical") >  0){
			//playerPos.z +=  Mathf.Cos(angleRads) * Time.deltaTime * moveSpeed;
			//playerPos.x +=  Mathf.Sin(angleRads) * Time.deltaTime * moveSpeed;
		}

		//playerRot.y += Input.GetAxis("Horizontal") * Time.deltaTime * turnSpeed;

		Vector3 direction = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
		Quaternion currentRotation = Quaternion.LookRotation(Vector3.up, direction);
		if (direction.magnitude > radialDeadZone){
			this.transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, Time.deltaTime * angularVelocity);
			playerPos.z -=  Mathf.Cos(angleRads) * Time.deltaTime * moveSpeed;
			playerPos.x -=  Mathf.Sin(angleRads) * Time.deltaTime * moveSpeed;
		}

		//playerPos.z += Input.GetAxis("Vertical");


		this.transform.position = playerPos;


		cam.transform.position = new Vector3(this.transform.position.x - camPos.x, this.transform.position.y - camPos.y, this.transform.position.z - camPos.z);

		if(Input.GetButton("Cancel")){
			Application.Quit();
			print("quit");
		}

	}
}
