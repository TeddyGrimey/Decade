using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPathFinding : MonoBehaviour {

	//The order the AI will run when starting the game
	public int order = 0;
	public int thisOrder = 0;

	//Has the search completed
	public bool completed = false;

	//Store the current node the AI script is linked with
	private Node thisNode;

	//This will store the information about all the nodes and parents
	private List<GameObject> nodeList = new List<GameObject>();
	private List<GameObject> nodeParentList = new List<GameObject>();

	private int state = 0;

	private int timer = 0;
	public int debugTimer = 30;

	void Update () {

		//Is the search completed
		if(!completed && order == thisOrder){
			if(state == 0){
				//Get the node script on this
				thisNode = this.GetComponent<Node>();

				//Get all the surrounding nodes of this node and place them in the nodeList 
				for(int n = 0; n < thisNode.surroundingNodes.Count; n++){
					//Check to see if the nodes adding are not already in the nodeList
					if(!nodeList.Contains(thisNode.surroundingNodes[n])){
						//Add them to the nodeList
						nodeList.Add(thisNode.surroundingNodes[n]);
						//Add this path to the nodes pathName 
						thisNode.surroundingNodes[n].GetComponent<Node>().pathName.Add(thisNode.nodeName);
						//Add this node to their parent node
						thisNode.surroundingNodes[n].GetComponent<Node>().pathParent.Add(this.gameObject);
					}
				}
				state ++;
			}
			if(state == 1){
				//Debug timer to slow down the process 
				timer ++;
				if(timer > debugTimer){
					timer = 0;
					//For all the nodes in the node list;
					for(int n = 0; n < nodeList.Count; n++){
						//For all the surrounding nodes of the nodeList[n]:
						for(int s = 0; s < nodeList[n].GetComponent<Node>().surroundingNodes.Count; s++){
							//Check to see if they already exist in the nodeList
							if(!nodeList.Contains(nodeList[n].GetComponent<Node>().surroundingNodes[s])){
								//Add them to the nodeList
								nodeList.Add(nodeList[n].GetComponent<Node>().surroundingNodes[s]);
								//Add this path to the nodes pathName 
								nodeList[n].GetComponent<Node>().surroundingNodes[s].GetComponent<Node>().pathName.Add(thisNode.nodeName);
								//Add this node to their parent node
								nodeList[n].GetComponent<Node>().surroundingNodes[s].GetComponent<Node>().pathParent.Add(nodeList[n]);
							}
						}
					}
				}
			}



			//completed = true;
		}


	}
}
