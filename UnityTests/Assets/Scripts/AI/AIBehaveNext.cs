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


	void Start() {
		player = GameObject.Find("Player");
	}

	void Update () {

		//if the game state is awake
		if(player.GetComponent<CharacterControllerScript>().gameState == "awake"){
			if(Vector3.Distance(this.transform.position, player.transform.position) > viewDist){
				collisionMode = false;
			}
			else{
				collisionMode = true;
			}
			listOfDestinations = currentNode.GetComponent<Node>().pathName;

			if(start){
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
				}
				else{
					this.GetComponent<CharacterController>().enabled = false;
					controller.radius = 0f;
					controller.stepOffset = 10;
				}
				Vector3 tempCurrentNode = currentNode.transform.position;
				if(Vector3.Distance(this.transform.position, tempCurrentNode) > 0.6f){
					if(collisionMode){

						//things with angles
						Quaternion rotationDirection;
						Vector3 dir = tempCurrentNode - this.transform.position;
						float angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;
						rotationDirection = Quaternion.AngleAxis(angle, Vector3.up);
						float angleRads = this.transform.rotation.eulerAngles.y * Mathf.PI / 180;
						Vector3 moveDirection = Vector3.zero;
						moveDirection = new Vector3(Mathf.Sin(angleRads)* randSpeed,-7f,Mathf.Cos(angleRads)* randSpeed) * slow;
						rotationDirection = Quaternion.AngleAxis(angle, Vector3.up);

						//detectig an overlap sphere around the agent and placing every object into an array
						Collider[] surrounding = Physics.OverlapSphere (this.transform.position, 0.35F);

						//declaring the variables for the average position and rotation
						Vector3 averagePosition = Vector3.zero;
						float averageDistance = 0;

						//the var to get the amount of colliders we actually use
						int amount = 0;

						List<Vector3> tempPos = new List<Vector3>();

						//run through the surrounding array
						for(int i = 0; i < surrounding.Length; i++){

							//check that object is not this object
							if(surrounding[i] != this.collider){

								//if the object has a character controller script on then accelerate playing of sound
								if(surrounding[i].GetComponent<CharacterControllerScript>()){
									this.GetComponent<Sound>().time += 10;
								}

								//if the surrounding object has a AIbehave function on it or CharacterControlllerScipt
								if(surrounding[i].GetComponent<AIBehaveNext>() || surrounding[i].GetComponent<CharacterControllerScript>()){

									//draws a ray in direction of object
									//Debug.DrawRay(transform.position, (surrounding[i].transform.position - transform.position).normalized, Color.grey);

									//declare a new raycast hit
									RaycastHit hit;

									//casts a ray to the object and tests if it reaches the object
									if(Physics.Raycast(transform.position, (surrounding[i].transform.position - transform.position).normalized, out hit, 4f)){
										
										//check to see if the hit was the object in question
										if(hit.collider == surrounding[i]){
											Vector3 dir4 = this.transform.position - surrounding[i].transform.position;
											float angle4 = Mathf.Atan2(dir4.x,dir4.z) * Mathf.Rad2Deg;
											Quaternion rotationDirection4 = Quaternion.AngleAxis(angle4, Vector3.up);
											float angleDif = Quaternion.Angle(this.transform.rotation, rotationDirection4);

											if(angleDif > 110){
												averagePosition += surrounding[i].transform.position;
												tempPos.Add(surrounding[i].transform.position);
												//Debug.DrawLine(this.transform.position, surrounding[i].transform.position, Color.yellow);
												Debug.DrawLine(this.transform.position, hit.point, Color.yellow);
												amount++;
											}
										}
									}
								}
							}
						}

						for(int i = 0; i < tempPos.Count; i++){
							for(int i2 = 0; i2 < tempPos.Count; i2++){
								averageDistance += Vector3.Distance(tempPos[i2], tempPos[i]);
							}
						}

						//depending on the mount of objects hit
						if(amount > 0){

							float rotate = 3f;
							float resist = 1;

							if(amount == 1){
								this.GetComponent<Sound>().play = true;
								averagePosition /= amount;
								dir = this.transform.position - averagePosition;
								rotate = 3;
							}
							else if(amount > 1){
								averagePosition /= amount;
								averageDistance /= amount;

								if(averageDistance > 0.4f){
									dir = averagePosition - this.transform.position;
								}

								else{
									dir = this.transform.position - averagePosition;
									rotate = 3;
									resist = 0.8f;
								}
							}



							angle = Mathf.Atan2(dir.x,dir.z) * Mathf.Rad2Deg;
							Quaternion rotationDirection3 = Quaternion.AngleAxis(angle, Vector3.up);


							Vector3 dir2 = this.transform.position - averagePosition;
							float angle2 = Mathf.Atan2(dir2.x,dir2.z) * Mathf.Rad2Deg;
							Quaternion rotationDirection2 = Quaternion.AngleAxis(angle2, Vector3.up);
							angleRads = rotationDirection2.eulerAngles.y * Mathf.PI / 180;
							float angleDif = Quaternion.Angle(this.transform.rotation, rotationDirection2);

							if(Vector3.Distance(this.transform.position, averagePosition) < 0.6f && angleDif > 100){ 
								Debug.DrawLine(this.transform.position, averagePosition, Color.cyan);
								rotationDirection = Quaternion.Lerp(rotationDirection,rotationDirection3, turnSmooth * Time.smoothDeltaTime * rotate * player.GetComponent<CharacterControllerScript>().gameSpeed);
								if(Vector3.Distance(this.transform.position, averagePosition) < 0.1f){
									moveDirection += new Vector3(Mathf.Sin(angleRads)* randSpeed,-7f,Mathf.Cos(angleRads)* 0.1f); 
								}

							}
							if(angleDif > 160){
								moveDirection *= Vector3.Distance(this.transform.position, averagePosition) * resist;
								//moveDirection *= resist;
							}
						}
						else{
							this.GetComponent<Sound>().play = false;
						}
						transform.rotation = Quaternion.Lerp(this.transform.rotation, rotationDirection, turnSmooth * Time.smoothDeltaTime * player.GetComponent<CharacterControllerScript>().gameSpeed);
						controller.Move(moveDirection * Time.smoothDeltaTime * player.GetComponent<CharacterControllerScript>().gameSpeed);
					}
					else{
						transform.position = Vector3.MoveTowards(transform.position, new Vector3(tempCurrentNode.x,tempCurrentNode.y + 0.2f,tempCurrentNode.z),randSpeed * Time.deltaTime * player.GetComponent<CharacterControllerScript>().gameSpeed);
					}

					//Draw a ray in direction facing
					Debug.DrawRay(transform.position, transform.forward * 0.3f, Color.red);

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
