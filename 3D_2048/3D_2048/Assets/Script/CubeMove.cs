using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class CubeMove : MonoBehaviour {

	private float cur_time;
	private float prev_time;
	private int Bound = 4;
	public float pos_x;
	public float pos_y;
	public float pos_z;
	private Vector3 vec;
	private int temp;

	/*public int value;

	// Use this for initialization
	void Start () {
		prev_time = 0;
		value = 0;
		pos_x = transform.position.x;
		pos_y = transform.position.y;
		pos_z = transform.position.z;
		//GetComponent<Renderer> ().material.color = Color.red;
	}*/
	
	// Update is called once per frame
	/*void Update () {
		
	}*/

	public Material mMaterial;
	bool flag = false;

	// Use this for initialization
	void Start () {
		/*mMaterial = Resources.Load ("2", typeof(Material)) as Material;
		GetComponent<Renderer> ().material = mMaterial;*/
	}

	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown (KeyCode.Escape)) {
			if (!flag) {
				mMaterial = Resources.Load ("2", typeof(Material)) as Material;
				GetComponent<Renderer> ().material = mMaterial;
				flag = true;
			} else {
				mMaterial = Resources.Load ("4", typeof(Material)) as Material;
				GetComponent<Renderer> ().material = mMaterial;
				flag = false;
			}
		}*/
	}




}
