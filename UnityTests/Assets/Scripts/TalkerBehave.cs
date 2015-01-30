using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TalkerBehave : MonoBehaviour {

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
		rand.x = Random.Range(-0.4f, 0.4f);
		rand.z = Random.Range(-0.4f, 0.4f);
		randSpeed = Random.Range(0.3f,0.5f);
		controller = this.GetComponent<CharacterController>();
		destination = listOfDestinations[Random.Range(0, listOfDestinations.Count)];
		bool conversation = false;
	}

	void Update () {

		bool conversation = this.GetComponent<HelloTrigger> ().Conversation;

			if (conversation == true){
		}

		if (conversation == false){

		if(currentNode != null){
			Vector3 tempCurrentNode = currentNode.transform.position;
			if(destination != currentNode.GetComponent<Node>().nodeName){
				tempCurrentNode.x += rand.x;
				tempCurrentNode.x += rand.z;
			}
			if(Vector3.Distance(this.transform.position, tempCurrentNode) > 0.1f){

				Vector3 dir = tempCurrentNode - this.transform.position;
				float angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;

				float angleRads = this.transform.rotation.eulerAngles.y * Mathf.PI / 180;


				transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), turnSpeed);
				controller.Move(new Vector3(Mathf.Sin(angleRads)* randSpeed,-7f,Mathf.Cos(angleRads)* randSpeed) * Time.deltaTime * slow);
				float difAngle = 0.3f;
				RaycastHit hit1;
				RaycastHit hit2;
				RaycastHit hit3;
				if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads),0,Mathf.Cos(angleRads)), out hit1, 0.2f) || 
				   Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads + difAngle),0,Mathf.Cos(angleRads + difAngle)), out hit2, 0.2f)||
				   Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads - difAngle),0,Mathf.Cos(angleRads - difAngle)), out hit3, 0.2f)){
					//Debug.DrawLine(this.transform.position, hit1.point,Color.green);

					RaycastHit hit4;
					RaycastHit hit5;

					if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads + difAngle),0,Mathf.Cos(angleRads + difAngle)), out hit4, 0.3f) == false){
						//Debug.DrawLine(this.transform.position, hit4.point,Color.blue);
						transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle + 90, Vector3.up), turnSpeed);
						slow = Mathf.Lerp(slow, 0.5f, 0.01f);
					}
					else if(Physics.Raycast(this.transform.position, new Vector3(Mathf.Sin(angleRads - difAngle),0,Mathf.Cos(angleRads - difAngle)), out hit5, 0.3f) == false){
						//Debug.DrawLine(this.transform.position, hit5.point,Color.blue);
						transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle - 90, Vector3.up), turnSpeed);
						slow = Mathf.Lerp(slow, 0.5f, 0.01f);
					}
					else{
						slow = Mathf.Lerp(slow, 0, 0.01f);
					}
				}
				else{
					slow = Mathf.Lerp(slow, 1, 0.1f);
				}
				//Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z), new Vector3(Mathf.Sin(angleRads),-0.1f,Mathf.Cos(angleRads)),Color.red);


			}
			else if(destination != "null"){

				if(destination != currentNode.GetComponent<Node>().nodeName && currentNode.GetComponent<Node>().pathName.Contains(destination)){ 
					print (currentNode.GetComponent<Node>().pathName.IndexOf(destination));
					currentNode = currentNode.GetComponent<Node>().pathParent[currentNode.GetComponent<Node>().pathName.IndexOf(destination)];
				}
				if(destination == currentNode.GetComponent<Node>().nodeName){
					destination = listOfDestinations[Random.Range(0, listOfDestinations.Count)];
				}
			}

		}
	}
	}
}
