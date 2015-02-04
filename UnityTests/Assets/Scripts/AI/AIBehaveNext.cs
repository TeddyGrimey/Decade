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
	public float turnSmooth = 10f; 

	public bool start = true;

	public bool collisionMode = true;

	public GameObject player = null;
	public float viewDist = 8;

	private Color color;

	void Start() {

		//transform.Find("Basic Person 01").renderer.material.color = color;
		player = GameObject.Find("Player");

		//listOfDestinations = currentNode.GetComponent<Node>().pathName;
		//destination = listOfDestinations[Random.Range(0, listOfDestinations.Count)];
	}

	void Update () {
		if(player.GetComponent<CharacterControllerScript>().gameState == "awake"){
			if(Vector3.Distance(this.transform.position, player.transform.position) > viewDist){
				collisionMode = false;
			}
			else{
				collisionMode = true;
			}
			listOfDestinations = currentNode.GetComponent<Node>().pathName;

			if(start){
				color = new Color(Random.Range(0f,1f), Random.Range(0f,1f),Random.Range(0f,1f));
				player.GetComponent<CharacterControllerScript>().cityPop ++;
				rand.x = Random.Range(-0.0f, 0.0f);
				rand.z = Random.Range(-0.0f, 0.0f);
				randSpeed = Random.Range(0.3f,0.6f);
				controller = this.GetComponent<CharacterController>();
				destination = listOfDestinations[Random.Range(0, listOfDestinations.Count)]; 
				start = false;
			}

			if(currentNode != null){
				if(collisionMode){
					controller.radius = 0.5f;
					controller.stepOffset = 0.05f;
					this.GetComponent<CharacterController>().enabled = true;
					//turnSmooth = 0.1f;
					//transform.renderer.enabled = true;

				}
				else{
					this.GetComponent<CharacterController>().enabled = false;
					controller.radius = 0f;
					controller.stepOffset = 10;
					//turnSmooth = 1f;
					//transform.renderer.enabled = false;

				}
				Vector3 tempCurrentNode = currentNode.transform.position;
				if(Vector3.Distance(this.transform.position, tempCurrentNode) > 0.3f){
					if(collisionMode){
						Quaternion rotationDirection;
						Vector3 dir = tempCurrentNode - this.transform.position;
						float angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;
						rotationDirection = Quaternion.AngleAxis(angle, Vector3.up);
						
						
						float angleRads = this.transform.rotation.eulerAngles.y * Mathf.PI / 180;
						
						Vector3 moveDirection = Vector3.zero;
						
						
						moveDirection = new Vector3(Mathf.Sin(angleRads)* randSpeed,-7f,Mathf.Cos(angleRads)* randSpeed) * slow;
						
						rotationDirection = Quaternion.AngleAxis(angle, Vector3.up);

						Collider[] surrounding = Physics.OverlapSphere (this.transform.position, 0.4F);
						Vector3 averagePosition = Vector3.zero;
						int amount = 0;;
						for(int i = 0; i < surrounding.Length; i++){
							if(surrounding[i] != this.collider){
								if(surrounding[i].GetComponent<CharacterControllerScript>()){
									this.GetComponent<Sound>().time += 10;
								}
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

							//audio.enabled = true;
							//this.GetComponent<Sound>().time ++;
							this.GetComponent<Sound>().play = true;

							averagePosition /= amount;

							dir = this.transform.position - averagePosition;

							angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;
							
							Quaternion rotationDirection2 = Quaternion.AngleAxis(angle, Vector3.up);


							angleRads = rotationDirection2.eulerAngles.y * Mathf.PI / 180;
							float angleDif = Quaternion.Angle(this.transform.rotation, rotationDirection2);
							if(Vector3.Distance(this.transform.position, averagePosition) < 0.4f && angleDif > 90f){
								Debug.DrawLine(this.transform.position, averagePosition, color);
								rotationDirection = Quaternion.Lerp(rotationDirection,rotationDirection2, turnSmooth * Time.smoothDeltaTime * 4f * player.GetComponent<CharacterControllerScript>().gameSpeed);
								if(Vector3.Distance(this.transform.position, averagePosition) < 0.2f){
									moveDirection += new Vector3(Mathf.Sin(angleRads)* randSpeed,-7f,Mathf.Cos(angleRads)* 0.1f); 
								}
								moveDirection /= amount * 1.1f;
							}
						}
						else{
							//audio.enabled = false;
							this.GetComponent<Sound>().play = false;
						}
						transform.rotation = Quaternion.Lerp(this.transform.rotation, rotationDirection, turnSmooth * Time.smoothDeltaTime * player.GetComponent<CharacterControllerScript>().gameSpeed);
						controller.Move(moveDirection * Time.smoothDeltaTime * player.GetComponent<CharacterControllerScript>().gameSpeed);

					}
					else{
						transform.position = Vector3.MoveTowards(transform.position, new Vector3(tempCurrentNode.x,tempCurrentNode.y + 0.2f,tempCurrentNode.z),randSpeed * Time.deltaTime * player.GetComponent<CharacterControllerScript>().gameSpeed);
					}
					//Rotate to direction
					//transform.rotation = Quaternion.Lerp(this.transform.rotation, rotationDirection, turnSmooth); 


					//Draw a ray in direction facing
					//Debug.DrawRay(transform.position, transform.forward * 0.2f, Color.red);

					//Move in direction
					//controller.Move(moveDirection * Time.deltaTime);


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
}
