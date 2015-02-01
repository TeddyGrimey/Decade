using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

	public string nodeName = "null";

	//An array of the surroudning nodes
	public List<GameObject> surroundingNodes = new List<GameObject>();

	//An array of paths and the way to get to the next node from here
	public List<string> pathName = new List<string>();
	public List<GameObject> pathParent = new List<GameObject>();

	public bool debug = true;

	void Start(){
		this.renderer.enabled = false;
		this.transform.FindChild("Sphere").renderer.enabled = false;

		Collider[] sphereHit = Physics.OverlapSphere (this.transform.position, 1.0F);

		for(int i = 0; i<sphereHit.Length; i++){
			if(sphereHit[i].collider.transform.gameObject.GetComponent<Node>() && !surroundingNodes.Contains(sphereHit[i].collider.transform.gameObject) && sphereHit[i].collider.transform.gameObject != this.gameObject){
				surroundingNodes.Add(sphereHit[i].collider.gameObject);
			
			}
		}

	}

	void Update(){
		if(debug){
			for(int i = 0; i < surroundingNodes.Count; i++){
				Debug.DrawLine(this.transform.position, surroundingNodes[i].transform.position);
			}
		}
	}

}
