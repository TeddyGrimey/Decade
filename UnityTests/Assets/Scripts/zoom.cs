using UnityEngine;
using System.Collections;

public class zoom : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		camera.orthographicSize += 0.002f;
	
		print (10);
		print (20);
	}
}
