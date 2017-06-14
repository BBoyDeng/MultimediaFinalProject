using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	private GameObject target;
	private float speed = 20;
	private Vector3 cameraPosition;
	private float number;
	private float radius;
	public bool isMoving;
	public float rotate;
	public bool start;
	public int direction;
	float times;
	float count;
	float d,r;
	Vector3 v;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Cube_14");
		cameraPosition = transform.position;
		radius = Vector3.Distance (target.transform.position, transform.position);
	}

	
	// Update is called once per frame
	void Update () {

		if(direction==1 && count > 0){     ///順時鐘
			isMoving = true;
			count--;
			if (rotate == 360) {
				if(count !=0) v = new Vector3(transform.position.x - d , 6 , transform.position.z - d );
				else v = new Vector3(0 , 6 , -12);
				transform.position = v;
				Quaternion quate = Quaternion.identity;
				quate.eulerAngles = new Vector3 (0, 270 + r*(times+1-count), 0);
				if(count ==0) quate.eulerAngles = new Vector3 (0, 0, 0);
				transform.rotation = quate;

			} else if (rotate == 90) {
				if(count !=0) v = new Vector3(transform.position.x - d , 6 , transform.position.z + d );
				else v = new Vector3(-12 , 6 , 0);
				transform.position = v;
				Quaternion quate = Quaternion.identity;
				quate.eulerAngles = new Vector3 (0, r*(times+1-count), 0);
				if(count ==0) quate.eulerAngles = new Vector3 (0, 90, 0);
				transform.rotation = quate;
			} else if (rotate == 180) {
				if(count !=0) v = new Vector3(transform.position.x + d , 6 , transform.position.z + d );
				else v = new Vector3(0 , 6 , 12);
				transform.position = v;
				Quaternion quate = Quaternion.identity;
				quate.eulerAngles = new Vector3 (0, 90 + r*(times+1-count), 0);
				if(count ==0) quate.eulerAngles = new Vector3 (0, 180, 0);
				transform.rotation = quate;
			} else if (rotate == 270) {
				if(count !=0) v = new Vector3(transform.position.x + d , 6 , transform.position.z - d );
				else v = new Vector3(12 , 6 , 0);
				transform.position = v;
				Quaternion quate = Quaternion.identity;
				quate.eulerAngles = new Vector3 (0, 180 + r*(times+1-count), 0);
				if(count ==0) quate.eulerAngles = new Vector3 (0, 270, 0);
				transform.rotation = quate;
			}

			if (count == 0) {
				direction = 0;
				isMoving = false;
			}

		} else if(direction==2 && count > 0){   ///逆時鐘
			isMoving = true;
			count--;
			if (rotate == 0) {
				if(count !=0) v = new Vector3(transform.position.x + d , 6 , transform.position.z - d );
				else v = new Vector3(0 , 6 , -12);
				transform.position = v;
				Quaternion quate = Quaternion.identity;
				quate.eulerAngles = new Vector3 (0, 90 - r*(times+1-count), 0);
				if(count ==0) quate.eulerAngles = new Vector3 (0, 0, 0);
				transform.rotation = quate;

			} else if (rotate == 90) {
				if(count !=0) v = new Vector3(transform.position.x - d , 6 , transform.position.z - d );
				else v = new Vector3(-12 , 6 , 0);
				transform.position = v;
				Quaternion quate = Quaternion.identity;
				quate.eulerAngles = new Vector3 (0, 180 - r*(times+1-count), 0);
				if(count ==0) quate.eulerAngles = new Vector3 (0, 90, 0);
				transform.rotation = quate;
			} else if (rotate == 180) {
				if(count !=0) v = new Vector3(transform.position.x - d , 6 , transform.position.z + d );
				else v = new Vector3(0 , 6 , 12);
				transform.position = v;
				Quaternion quate = Quaternion.identity;
				quate.eulerAngles = new Vector3 (0, 270 - r*(times+1-count), 0);
				if(count ==0) quate.eulerAngles = new Vector3 (0, 180, 0);
				transform.rotation = quate;
			} else if (rotate == 270) {
				if(count !=0) v = new Vector3(transform.position.x + d , 6 , transform.position.z + d );
				else v = new Vector3(12 , 6 , 0);
				transform.position = v;
				Quaternion quate = Quaternion.identity;
				quate.eulerAngles = new Vector3 (0, 0 -  r*(times+1-count), 0);
				if(count ==0) quate.eulerAngles = new Vector3 (0, 270, 0);
				transform.rotation = quate;
			}

			if (count == 0) {
				direction = 0;
				isMoving = false;
			}
		}

	}

	public void move(string dir, int curCamera){
		start = true;
		times = 18;
		count = times;
		d = 12 / times;
		r = 90 / times;
		if (dir == "Z") {
			direction = 1;
			rotate = ((5-curCamera) * 90);
		} else if (dir == "X") {
			rotate =  ((5-curCamera) * 90)%360;
			direction = 2;
		}
	}
}
 