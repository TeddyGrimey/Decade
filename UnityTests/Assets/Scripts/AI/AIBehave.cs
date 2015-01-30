using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBehave : MonoBehaviour {

	public string destination = "null";

	public GameObject currentNode = null;

	public List<string> listOfDestinations = new List<string>();

	public float turnSpeed = 0.1f;

	private Vector3 rand = new Vector3();
	private float randSpeed = 0;

	private CharacterController controller;

	private float angularVelocity = 12.0f;

	private float slow = 1;

	void Start() {
		rand.x = Random.Range(-0.0f, 0.0f);
		rand.z = Random.Range(-0.0f, 0.0f);
		randSpeed = Random.Range(0.3f,0.5f);
		controller = this.GetComponent<CharacterController>();
		//destination = listOfDestinations[Random.Range(0, listOfDestinations.Count)];
	}

	void Update () {



		listOfDestinations = currentNode.GetComponent<Node>().pathName;

		
		if(currentNode != null){
			Vector3 tempCurrentNode = currentNode.transform.position;
			if(destination != currentNode.GetComponent<Node>().nodeName){
				tempCurrentNode.x += rand.x;
				tempCurrentNode.x += rand.z;
			}
			if(Vector3.Distance(this.transform.position, tempCurrentNode) > 0.2f){

				Vector3 dir = tempCurrentNode - this.transform.position;
				float angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;

				float angleRads = this.transform.rotation.eulerAngles.y * Mathf.PI / 180;

				bool right = false;
				bool left = false;
				bool front = false;
				RaycastHit hit4;
				if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads + 0.7f),0,Mathf.Cos(angleRads + 0.7f)) ,out hit4, 0.15f)){
					Debug.DrawLine(this.transform.position, hit4.point,Color.red);
					transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle - 90f, Vector3.up), turnSpeed);
					right = true;
				}
				else if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads - 0.7f),0,Mathf.Cos(angleRads - 0.7f)) ,out hit4, 0.15f)){
					Debug.DrawLine(this.transform.position, hit4.point,Color.red);
					transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle + 90f, Vector3.up), turnSpeed);
					right = true;
				}
				if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads + 1.5f),0,Mathf.Cos(angleRads + 1.5f)) ,out hit4, 0.13f)){
					Debug.DrawLine(this.transform.position, hit4.point,Color.red);
					transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle - 90f, Vector3.up), turnSpeed);
					//right = true;
				}
				else if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads - 1.5f),0,Mathf.Cos(angleRads - 1.5f)) ,out hit4, 0.13f)){
					Debug.DrawLine(this.transform.position, hit4.point,Color.red);
					transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle + 90f, Vector3.up), turnSpeed);
					//right = true;
				}
				if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads + 0.4f),0,Mathf.Cos(angleRads + 0.4f)) ,out hit4, 0.2f)){
					Debug.DrawLine(this.transform.position, hit4.point,Color.red);
					transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle - 60f, Vector3.up), turnSpeed);
					//right = true;
				}
				else if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads - 0.4f),0,Mathf.Cos(angleRads - 0.4f)) ,out hit4, 0.2f)){
					Debug.DrawLine(this.transform.position, hit4.point,Color.red);
					transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle + 60f, Vector3.up), turnSpeed);
					//right = true;
				}

				if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads),0,Mathf.Cos(angleRads)) ,out hit4, 0.3f)){
					//Debug.DrawLine(this.transform.position, hit4.point,Color.red);
					if(!right){
						//transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle - 45f, Vector3.up), turnSpeed);
					}
					else if(!left){
						//transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle + 45f, Vector3.up), turnSpeed);
					}
					if(left && right){
						//transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle + 180, Vector3.up), 1);

					}
					front = true;
				}
				if(front){
					//transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle + 180, Vector3.up), turnSpeed);
					//slow = Mathf.Lerp(slow, 0.8f, 0.01f);
				}
				else{
					//slow = Mathf.Lerp(slow, 1f, 0.01f);
				}
				if(front){
					//transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), 0.001f);
					slow = Mathf.Lerp(slow, 0f, 0.001f);
				}
				else{
					slow = Mathf.Lerp(slow, 1f, 0.01f);
				}
				transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), turnSpeed);
				controller.Move(new Vector3(Mathf.Sin(angleRads)* randSpeed,-7f,Mathf.Cos(angleRads)* randSpeed) * Time.deltaTime * slow);


				float difAngle = 0.01f;

				//Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z), new Vector3(Mathf.Sin(angleRads),-0.1f,Mathf.Cos(angleRads)),Color.red);


			}
			else if(destination != "null"){

				if(destination != currentNode.GetComponent<Node>().nodeName && currentNode.GetComponent<Node>().pathName.Contains(destination)){ 
					//print (currentNode.GetComponent<Node>().pathName.IndexOf(destination));
					currentNode = currentNode.GetComponent<Node>().pathParent[currentNode.GetComponent<Node>().pathName.IndexOf(destination)];
				}
				if(destination == currentNode.GetComponent<Node>().nodeName){
					destination = listOfDestinations[Random.Range(0, listOfDestinations.Count)];
				}
			}

		}
	}
}
