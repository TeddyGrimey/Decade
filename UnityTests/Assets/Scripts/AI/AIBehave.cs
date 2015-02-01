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
	public float turnVel = 10;
	public float turnSmooth = 0.1f; 
	

	void Start() {
		rand.x = Random.Range(-0.0f, 0.0f);
		rand.z = Random.Range(-0.0f, 0.0f);
		randSpeed = Random.Range(0.3f,0.5f);
		controller = this.GetComponent<CharacterController>();
		destination = listOfDestinations[Random.Range(0, listOfDestinations.Count)];
	}

	void Update () {



		listOfDestinations = currentNode.GetComponent<Node>().pathName;

		
		if(currentNode != null){
			Vector3 tempCurrentNode = currentNode.transform.position;
			if(destination != currentNode.GetComponent<Node>().nodeName){
				tempCurrentNode.x += rand.x;
				tempCurrentNode.x += rand.z;
			}
			if(Vector3.Distance(this.transform.position, tempCurrentNode) > 0.3f){

				Vector3 dir = tempCurrentNode - this.transform.position;
				float angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;

				float angleRads = this.transform.rotation.eulerAngles.y * Mathf.PI / 180;

				bool right = false;
				bool left = false;
				bool front = false;
				RaycastHit hit4;
				int num = 0;
				Quaternion newRot = this.transform.rotation;
				//Casting raycast infront of the AI
				if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads + 0.7f),0,Mathf.Cos(angleRads + 0.7f)) ,out hit4, 0.1f)){
					Debug.DrawLine(this.transform.position, hit4.point,Color.red);
					//transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle - turnVel, Vector3.up), turnSmooth);
					//right = true;
					newRot.y = Mathf.LerpAngle(newRot.y, newRot.y - 1f, 0.01f); 
					num++;
				}
				if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads - 0.7f),0,Mathf.Cos(angleRads - 0.7f)) ,out hit4, 0.1f)){
					Debug.DrawLine(this.transform.position, hit4.point,Color.red);
					//transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle + turnVel, Vector3.up), turnSmooth);
					//left = true;
					newRot.y = Mathf.LerpAngle(newRot.y, newRot.y + 1f, 0.01f); 
					num++;
				}
				if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads + 1.5f),0,Mathf.Cos(angleRads + 1.5f)) ,out hit4, 0.1f)){
					Debug.DrawLine(this.transform.position, hit4.point,Color.red);
					//transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle - turnVel, Vector3.up), turnSmooth);
					newRot.y = Mathf.LerpAngle(newRot.y, newRot.y - 1f, 0.01f); 
					num++;
				}
				if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads - 1.5f),0,Mathf.Cos(angleRads - 1.5f)) ,out hit4, 0.1f)){
					Debug.DrawLine(this.transform.position, hit4.point,Color.red);
					//transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle + turnVel, Vector3.up), turnSmooth);
					newRot.y = Mathf.LerpAngle(newRot.y, newRot.y + 1f, 0.01f); 
					num++;
				}

				float closest = Mathf.Infinity;
				string direct = "null";
				if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads + 0.2f),0,Mathf.Cos(angleRads + 0.2f)) ,out hit4, 0.3f)){

					if(Vector3.Distance(this.transform.position, hit4.point) < closest){
						if(hit4.collider.GetComponent<AIBehave>() || hit4.collider.GetComponent<CharacterControllerScript>()){
							Debug.DrawLine(this.transform.position, hit4.point,Color.red);
							closest = Vector3.Distance(this.transform.position, hit4.point);
							direct = "right";
						}
					}
				}
				if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads - 0.2f),0,Mathf.Cos(angleRads - 0.2f)) ,out hit4, 0.3f)){

					if(Vector3.Distance(this.transform.position, hit4.point) < closest){
						if(hit4.collider.GetComponent<AIBehave>()|| hit4.collider.GetComponent<CharacterControllerScript>()){
							Debug.DrawLine(this.transform.position, hit4.point,Color.red);
							closest = Vector3.Distance(this.transform.position, hit4.point);
							direct = "left";
						}
					}
				}

				if(direct == "right"){
					newRot.y = Mathf.LerpAngle(newRot.y, newRot.y - 0.4f, 0.1f); 
					//transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y - 180,transform.rotation.eulerAngles.z), turnSmooth);
					slow = Mathf.Lerp(slow, 0.1f, 0.01f);
					//transform.rotation = newRot;
					num++;
				}
				else if(direct == "left"){
					newRot.y = Mathf.LerpAngle(newRot.y, newRot.y + 0.4f, 0.1f); 
					//transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y + 180,transform.rotation.eulerAngles.z), turnSmooth);
					slow = Mathf.Lerp(slow, 0.1f, 0.01f);
					//transform.rotation = newRot;
					num++; 
				}
				else{
					slow = Mathf.Lerp(slow, 1f, 0.1f);
				}

				if(num >= 3){
					newRot.y = Mathf.LerpAngle(newRot.y, newRot.y + Random.Range(-4, 4), 0.1f); 
				}
				transform.rotation = newRot;
				transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), turnSmooth);
				controller.Move(new Vector3(Mathf.Sin(angleRads)* randSpeed,-7f,Mathf.Cos(angleRads)* randSpeed) * Time.deltaTime * slow);


			}
			else if(destination != "null"){

				if(destination != currentNode.GetComponent<Node>().nodeName && currentNode.GetComponent<Node>().pathName.Contains(destination)){ 
					currentNode = currentNode.GetComponent<Node>().pathParent[currentNode.GetComponent<Node>().pathName.IndexOf(destination)];
				}

				//Changes to a random destination if reached destination
				if(destination == currentNode.GetComponent<Node>().nodeName){
					destination = listOfDestinations[Random.Range(0, listOfDestinations.Count)];
				}


			}

		}
	}
}
