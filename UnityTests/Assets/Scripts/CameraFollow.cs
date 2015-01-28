using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {


	public Transform player = null;
	private Vector3 playerPos = new Vector3();

	void Start () {
		//playerPos = player.transform.localPosition;
		playerPos = new Vector3(player.transform.position.x - this.transform.position.x,player.transform.position.y - this.transform.position.y, player.transform.position.z - this.transform.position.z);
	}
	

	void Update () {
		 
		this.transform.position = new Vector3(player.transform.position.x - playerPos.x, this.transform.position.y, player.transform.position.z - playerPos.z);

		print ("test");
	}
}
