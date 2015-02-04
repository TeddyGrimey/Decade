using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {
	
	public AudioClip sound;
	public AudioClip sound2;
	public AudioClip sound3;
	public float voicePitch;

	public bool play = false;

	public int time = 0;
	int max = 0;
	
	void Start() {
		max = Random.Range(100, 600);
		voicePitch = Random.Range (0.95F, 1.05F);
		audio.pitch = voicePitch;
		//audio.clip = sound;
		audio.Play();	
	}

	void Update () {
		if(play){
			audio.pitch = voicePitch;
			if(time > max){
				int rand = Random.Range (0, 2);
				if (rand == 0){
					audio.PlayOneShot(sound);	
				}
				if (rand == 1){
					audio.PlayOneShot(sound2);
				}
				if (rand == 2){
					audio.PlayOneShot (sound3);
				}
				time = 0;
				max = Random.Range(100, 600);
			
			}
			time++;
		}
	}
}
