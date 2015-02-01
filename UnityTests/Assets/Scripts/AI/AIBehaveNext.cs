using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBehaveNext : MonoBehaviour {

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

	public bool start = true;

	public bool collisionMode = true;

	public GameObject player = null;
	public float viewDist = 8;

	void Start() {
		player = GameObject.Find("Player");
		rand.x = Random.Range(-0.0f, 0.0f);
		rand.z = Random.Range(-0.0f, 0.0f);
		randSpeed = Random.Range(0.3f,0.6f);
		controller = this.GetComponent<CharacterController>();
		//listOfDestinations = currentNode.GetComponent<Node>().pathName;
		//destination = listOfDestinations[Random.Range(0, listOfDestinations.Count)];
	}

	void Update () {

		if(Vector3.Distance(this.transform.position, player.transform.position) > viewDist){
			collisionMode = false;
		}
		else{
			collisionMode = true;
		}
		listOfDestinations = currentNode.GetComponent<Node>().pathName;

		if(start){
			destination = listOfDestinations[Random.Range(0, listOfDestinations.Count)]; 
			start = false;
		}

		if(currentNode != null){
			if(collisionMode){
				controller.radius = 0.5f;
				controller.stepOffset = 0.05f;
				turnSmooth = 0.1f;
				//transform.renderer.enabled = true;
			}
			else{
				controller.radius = 0f;
				controller.stepOffset = 10;
				turnSmooth = 1f;
				//transform.renderer.enabled = false;
			}
			Vector3 tempCurrentNode = currentNode.transform.position;
			if(destination != currentNode.GetComponent<Node>().nodeName){
				tempCurrentNode.x += rand.x;
				tempCurrentNode.x += rand.z;
			}
			if(Vector3.Distance(this.transform.position, tempCurrentNode) > 0.3f){

				Quaternion rotationDirection;
				Vector3 dir = tempCurrentNode - this.transform.position;
				float angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;
				rotationDirection = Quaternion.AngleAxis(angle, Vector3.up);


				float angleRads = this.transform.rotation.eulerAngles.y * Mathf.PI / 180;

				Vector3 moveDirection = Vector3.zero;


				moveDirection = new Vector3(Mathf.Sin(angleRads)* randSpeed,-7f,Mathf.Cos(angleRads)* randSpeed) * slow;

				rotationDirection = Quaternion.AngleAxis(angle, Vector3.up);

				if(collisionMode){
					Collider[] surrounding = Physics.OverlapSphere (this.transform.position, 0.2F);
					Vector3 averagePosition = Vector3.zero;
					int amount = 0;;
					for(int i = 0; i < surrounding.Length; i++){
						if(surrounding[i] != this.collider){
							if(surrounding[i].GetComponent<AIBehaveNext>() || surrounding[i].GetComponent<CharacterControllerScript>()){
								//Debug.DrawRay(transform.position, (surrounding[i].transform.position - transform.position).normalized, Color.grey);
								RaycastHit hit;
								if(Physics.Raycast(transform.position, (surrounding[i].transform.position - transform.position).normalized, out hit, 4f)){

									if(hit.collider == surrounding[i]){
										//print ("hitMyself");
										averagePosition += surrounding[i].transform.position;
										//Debug.DrawLine(this.transform.position, surrounding[i].transform.position, Color.yellow);

										Debug.DrawLine(this.transform.position, hit.point, Color.yellow);
										amount++;
									}
								}
							}
						}
					}

					if(amount > 0){
						averagePosition /= amount;
						dir = this.transform.position - averagePosition;
						angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;
						
						Quaternion rotationDirection2 = Quaternion.AngleAxis(angle, Vector3.up);


						angleRads = rotationDirection2.eulerAngles.y * Mathf.PI / 180;
						float angleDif = Vector3.Angle(transform.forward, new Vector3(Mathf.Sin(angleRads)* randSpeed,-7f,Mathf.Cos(angleRads)* 0.05f));
						if(Vector3.Distance(this.transform.position, averagePosition) < 0.2f && angleDif > 1f){
							Debug.DrawLine(this.transform.position, averagePosition, Color.magenta);
							rotationDirection = Quaternion.Lerp(rotationDirection,rotationDirection2, 0.2f);
							moveDirection += new Vector3(Mathf.Sin(angleRads)* randSpeed,-7f,Mathf.Cos(angleRads)* 0.01f); 
							//moveDirection /= 2;
						}
					}

				}
				//Rotate to direction
				transform.rotation = Quaternion.Lerp(this.transform.rotation, rotationDirection, turnSmooth); 

				//Draw a ray in direction facing
				//Debug.DrawRay(transform.position, transform.forward * 0.2f, Color.red);

				//Move in direction
				controller.Move(moveDirection * Time.deltaTime);


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
