using UnityEngine;
using System.Collections;

public class HelloTrigger : MonoBehaviour {

	public AudioSource AudioSource;
	public bool Conversation;

	// Use this for initialization
	void Start () {
		Conversation = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject player = GameObject.Find ("Player");
		float distance = Vector3.Distance (transform.position, player.transform.position);

		if (distance <= 0.5) {
						if (Input.GetKeyDown (KeyCode.Space)) {
								AudioSource.Play ();
								Conversation = true;
						}
				} else {
			Conversation = false;
				}

		}


	
	}

