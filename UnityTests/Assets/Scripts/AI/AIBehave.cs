using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBehave : MonoBehaviour {

	public string destination = "null";

	public GameObject currentNode = null;

	public List<string> listOfDestinations = new List<string>();

	private Vector3 rand = new Vector3();
	private float randSpeed = 0;

	void Start() {
		rand.x = Random.Range(-0.1f, 0.1f);
		rand.z = Random.Range(-0.1f, 0.1f);
		randSpeed = Random.Range(0.002f,0.01f);
	}

	void Update () {
		if(currentNode != null){
			if(Vector3.Distance(this.transform.position, currentNode.transform.position) > 0.15f){
				if(destination == currentNode.GetComponent<Node>().nodeName){
					this.transform.position = Vector3.MoveTowards(this.transform.position, currentNode.transform.position, randSpeed);
				}
				else{
					this.transform.position = Vector3.MoveTowards(this.transform.position, currentNode.transform.position + rand, randSpeed);
				}
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
