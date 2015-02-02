using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {
	
	public AudioClip sound;

	public bool play = false;

	public int time = 0;
	int max = 0;

	public Sound[] sounds;
	
	void Start() {

	}

	void Update () {

	

		if(play){
			audio.pitch = Random.Range(1F, 1.50F);
			if(time > max){
				audio.PlayOneShot(sound);	
				time = 0;
				max = Random.Range(200, 300);
			}
			time++;
		}
	}
}
