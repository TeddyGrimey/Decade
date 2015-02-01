using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {
	
	public AudioClip sound;

	public bool play = false;

	public int time = 0;
	int max = 0;
	
	void Start() {

	}

	void Update () {
		if(play){
			audio.pitch = Random.Range(1.00F, 4.00F);
			if(time > max){
				audio.PlayOneShot(sound);	
				time = 0;
				max = Random.Range(100, 300);
			}
			time++;
		}
	}
}
