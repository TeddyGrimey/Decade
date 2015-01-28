using UnityEngine;
using System.Collections;

public class zoom : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		camera.orthographicSize += 0.001f;
		print (10);
	}
}
