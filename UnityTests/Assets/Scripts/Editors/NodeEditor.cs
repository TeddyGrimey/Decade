using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof (Node))]
public class NodeEditor : Editor {

	public override void OnInspectorGUI(){
		if(GUILayout.Button("Select Nodes")){

		}
	}

}
