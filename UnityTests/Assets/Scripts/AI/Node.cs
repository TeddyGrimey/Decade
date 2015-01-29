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

}
