using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


	public GameObject cam = null;
	private Vector3 camPos = new Vector3();


	void Start () {
		camPos = new Vector3(this.transform.position.x - cam.transform.position.x,this.transform.position.y - cam.transform.position.y, this.transform.position.z - cam.transform.position.z);


	}
	

	void Update () {

		if(Input.GetKey("w")){
			this.rigidbody.AddRelativeForce(5,0,0);
		}

		if(Input.GetKey("a")){
			//this.transform.rotation = Quaternion.Euler(0,this.transform.rotation.eulerAngles.y - 5,0);
			this.rigidbody.AddTorque(0,-1,0);
		}
		if(Input.GetKey("d")){
			//this.transform.rotation = Quaternion.Euler(0,this.transform.rotation.eulerAngles.y + 5,0);
			this.rigidbody.AddTorque(0,1,0);
		}

		cam.transform.position = new Vector3(this.transform.position.x - camPos.x, this.transform.position.y - camPos.y, this.transform.position.z - camPos.z);


	}
}
