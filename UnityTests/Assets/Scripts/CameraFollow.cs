using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {


	public Transform player = null;
	private Vector3 playerPos = new Vector3();
	private Vector3 startRotation;
	private Vector3 startPosition;

	public bool view = false;

	public float view2D = 10;
	public float view3D = 3;
	public float smooth = 1;

	void OnValidate(){
		smooth = Mathf.Clamp(smooth, 0, 1);
	}

	void Start () {
		playerPos = new Vector3(player.transform.position.x - this.transform.position.x,player.transform.position.y - this.transform.position.y, player.transform.position.z - this.transform.position.z);
		startRotation = this.transform.rotation.eulerAngles;
		startPosition = this.transform.position;
	}
	

	void Update () {

		if(Input.GetButtonDown("Fire1")){
			//view = !view;
		}

		if(view){
			this.transform.position = Vector3.Lerp(this.transform.position,new Vector3(player.transform.position.x - playerPos.x, 0.5f, player.transform.position.z - playerPos.z), smooth);
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation,  Quaternion.Euler(10, this.transform.rotation.eulerAngles.y,this.transform.rotation.eulerAngles.z), smooth);
			this.camera.orthographicSize = Mathf.Lerp (this.camera.orthographicSize, view2D, smooth);
		}
		else{
			this.transform.position = Vector3.Lerp(this.transform.position,new Vector3(player.transform.position.x - playerPos.x, startPosition.y, player.transform.position.z - playerPos.z), smooth);
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation,  Quaternion.Euler(startRotation), smooth);
			this.camera.orthographicSize = Mathf.Lerp (this.camera.orthographicSize, view3D, smooth);
			this.camera.orthographic = true;
		}

	}
}
