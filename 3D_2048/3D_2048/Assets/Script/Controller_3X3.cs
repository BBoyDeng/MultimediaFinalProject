using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller_3X3 : MonoBehaviour {

	public GameObject[] Cube = new GameObject[130];
	public CubeMove[] cm = new CubeMove[130];
	private float cur_time;
	private float prev_time;
	private int Bound = 3;
	private int times = 10;
	private bool MOVING;
	private Vector3 vec;
	private int temp;

	static int LENGTH = 3;
	int numOfOBJ = 9;
	int[] movedTo = new int[130];
	int[] score = new int[130];
	int[] undo_score = new int[130];
	float[] moveRange = new float[130];
	string dir;
	bool isMoved;
	int cur_cubeNum;
	bool gameover;
	bool restart;
	bool UNDO;

	// Use this for initialization
	void Start () {
		for (int i = 1; i <= numOfOBJ; i++) {
			if (!restart) {
				string str = "Cube_" + i.ToString ();
				Cube [i] = GameObject.Find (str);
				cm [i] = Cube [i].GetComponent<CubeMove> ();
				setPosition (i);
			}
			cm [i].gameObject.SetActive (false);
			movedTo [i] = 0;
			score[i] = 0;
			moveRange [i] = 0;
		}
		cur_cubeNum = 2;
		int count = 2;
		while (count>0) {
			int x = Random.Range (1, numOfOBJ+1);  
			if (score [x] == 0) {
				count--;
				setCubeColor (cm[x], 2);
				cm [x].gameObject.SetActive (true);
				score [x] = 2;
			}
		}
		gameover = false;
		restart = false;
		UNDO = false;

	}
	
	// Update is called once per frame
	void Update () {
		cur_time = Time.time;
		if (!MOVING) {
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) {
				dir = "-X";
				All_Cube_Move ();
				temp = times;
				MOVING = true;
			} else if (Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) {
				dir = "X";
				All_Cube_Move ();
				temp = times;
				MOVING = true;
			} else if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
				dir = "Z";
				All_Cube_Move ();
				temp = times;
				MOVING = true;
			} else if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
				dir = "-Z";
				All_Cube_Move ();
				temp = times;
				MOVING = true;
			} else if (Input.GetKeyDown (KeyCode.Escape)) {
				restart = true;
				Start ();
			} else if (Input.GetKeyDown (KeyCode.Space)) {
				UNDO_move ();
			}
		} else if (cur_time - prev_time > 0.01) {
			if (temp == 0) {
				MOVING = false;
				ResetAndShow ();
				RandomCreateCube ();
			} else {
				for (int i = 1; i <= numOfOBJ; i++) {
					if (movedTo [i] != 0) {
						Vector3 v = cm [i].transform.position;
						if (dir == "-Z" && v [2] >= -1*Bound) {
							cm [i].transform.Translate (0, 0, -1 * moveRange [i] / times);
						} else if (dir == "Z" && v [2] <= Bound) {
							cm [i].transform.Translate (0, 0, moveRange [i] / times);
						} else if (dir == "X" && v [0] <= Bound) {
							cm [i].transform.Translate (moveRange [i] / times, 0, 0);
						} else if (dir == "-X" && v [0] >= -1*Bound) {
							cm [i].transform.Translate (-1 * moveRange [i] / times, 0, 0);
						} else if (dir == "Y") {
						} else if (dir == "-Y") {
						}

					}
				}
				prev_time = Time.time;
				temp--;
			}

		} 

	}

	void All_Cube_Move(){
		if (dir == "X") {
			for (int i = 1; i <= numOfOBJ; i+=LENGTH) {
				for (int j = i; j<i+LENGTH ; j ++) {
					int cur = j;

					while ((cur - 1) % LENGTH != 0 && score [cur - 1] == 0 && score [cur] != 0) {
						if(!isMoved) save_prev_score ();
						isMoved = true;
						score [cur - 1] = score [cur];
						score [cur] = 0;
						cur--;
						movedTo [j] = cur;
						moveRange [j] += LENGTH;
					}
				}
			}
			///combine same score
			for (int i = 1; i <= numOfOBJ; i+=LENGTH) {
				for (int j = i; j<i+LENGTH-1 ; j ++) {
					if (score [j] == score [j + 1] && score [j] != 0) {
						if(!isMoved) save_prev_score ();
						isMoved = true;
						cur_cubeNum--;
						score [j] = score [j + 1]*2;
						score [j + 1] = 0;
						moveRange [j + 1] += LENGTH;
						movedTo [j + 1] = j;
						for (int k = j + 1 ; k < i+LENGTH-1; k ++) {
							score [k] = score [k + 1];
							score [k + 1] = 0;
							moveRange [k + 1] += LENGTH;
							movedTo [k + 1] = j;
						}
						break;
					}
				}
			}
		} else if (dir == "-X") {
			for (int i = LENGTH ; i <= numOfOBJ; i+=LENGTH) {
				for (int j = i; j>i-LENGTH ; j --) {
					int cur = j;
					while ( (cur + 1)%LENGTH!=1 && score [cur + 1] == 0 && score [cur] != 0) {
						if(!isMoved) save_prev_score ();
						isMoved = true;
						score [cur + 1] = score [cur];
						score [cur] = 0;
						cur++;
						movedTo[j] = cur;
						moveRange[j]+=LENGTH;
					}

				}
			}
			///combine same score
			for (int i = LENGTH ; i <= numOfOBJ; i+=LENGTH) {
				for (int j = i; j>i-LENGTH+1 ; j --) {
					if (score [j] == score [j - 1] && score [j] != 0) {
						if(!isMoved) save_prev_score ();
						isMoved = true;
						cur_cubeNum--;
						score [j] = score [j - 1]*2;
						score [j - 1] = 0;
						moveRange [j-1] += LENGTH;
						movedTo [j-1] = j;
						for (int k = j - 1 ; k > i-LENGTH+1; k--) {
							score [k] = score [k - 1];
							score [k - 1] = 0;
							moveRange [k - 1] += LENGTH;
							movedTo [k - 1] = j;
						}
						break;
					}
				}
			}
		} else if (dir == "Y") { 
			
		} else if (dir == "-Y") { 
			
		} else if (dir == "Z") { 
			for (int i = 1 ; i <= LENGTH; i++) {
				for (int j = i; j <= numOfOBJ; j += LENGTH) {
					int cur = j;
					while (cur - LENGTH >= 1 && score [cur - LENGTH] == 0 && score [cur] != 0) {
						if(!isMoved) save_prev_score ();
						isMoved = true;
						score [cur - LENGTH] = score [cur];
						score [cur] = 0;
						cur -= LENGTH;
						movedTo[j] = cur;
						moveRange[j]+=LENGTH;
					}

				}
			}

			///combine same score
			for (int i = 1 ; i <= LENGTH; i++) {
				for (int j = i; j <= numOfOBJ - LENGTH; j += LENGTH) {
					if (score [j] == score [j + LENGTH] && score [j] != 0) {
						if(!isMoved) save_prev_score ();
						isMoved = true;
						cur_cubeNum--;
						score [j] = score [j + LENGTH]*2;
						score [j + LENGTH] = 0;
						moveRange [j + LENGTH] += LENGTH;
						movedTo [j + LENGTH] = j;
						for (int k = j + LENGTH ; k <= numOfOBJ - LENGTH; k += LENGTH) {
							score [k] = score [k + LENGTH];
							score [k + LENGTH] = 0;
							moveRange [k + LENGTH] += LENGTH;
							movedTo [k + LENGTH] = j;
						}
						break;
					}
				}
			}

		} else if (dir == "-Z") { 
			for (int i = numOfOBJ ; i > numOfOBJ - LENGTH; i--) {
				for (int j = i; j >= 1; j -= LENGTH) {
					int cur = j;

					while (cur + LENGTH <= numOfOBJ && score [cur + LENGTH] == 0 && score [cur] != 0) {
						if(!isMoved) save_prev_score ();
						isMoved = true;
						score [cur + LENGTH] = score [cur];
						score [cur] = 0;
						cur += LENGTH;
						movedTo[j] = cur;
						moveRange[j]+=LENGTH;
					}

				}
			}
			///combine same score
			for (int i = numOfOBJ ; i > numOfOBJ - LENGTH; i--) {
				for (int j = i; j > LENGTH ; j -= LENGTH) {
					if (score [j] == score [j - LENGTH] && score [j] != 0) {
						if(!isMoved) save_prev_score ();
						isMoved = true;
						cur_cubeNum--;
						score [j] = score [j - LENGTH]*2;
						score [j - LENGTH] = 0;
						moveRange [j - LENGTH] += LENGTH;
						movedTo [j - LENGTH] = j;
						for (int k = j - LENGTH ; k > LENGTH; k -= LENGTH) {
							score [k] = score [k - LENGTH];
							score [k - LENGTH] = 0;
							moveRange [k - LENGTH] += LENGTH;
							movedTo [k - LENGTH] = j;

						}
						break;
					}
				}
			}

		}


	}

	void setCubeColor(CubeMove cm, int value){
		Material mMaterial;
		if (value == 2) {
			cm.GetComponent<Renderer> ().material.color = Color.cyan;
			mMaterial = Resources.Load ("2", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 4) {
			mMaterial = Resources.Load ("4", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 8) {
			mMaterial = Resources.Load ("8", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 16) {
			mMaterial = Resources.Load ("16", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 32) {
			mMaterial = Resources.Load ("32", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 64) {
			mMaterial = Resources.Load ("64", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 128) {
			mMaterial = Resources.Load ("128", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 256) {
			mMaterial = Resources.Load ("256", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 512) {
			mMaterial = Resources.Load ("512", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 1024) {
			mMaterial = Resources.Load ("1024", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 2048) {
			mMaterial = Resources.Load ("2048", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		}
	}

	void ResetAndShow(){
		for (int i = 1; i <= numOfOBJ; i++) {
			if (movedTo [i] != 0) {
				setPosition (i);
				movedTo [i] = 0;
				moveRange [i] = 0;
				cm [i].gameObject.SetActive (false);
			}
			if (score [i] != 0) {
				cm [i].gameObject.SetActive (true);
				setCubeColor (cm [i], score [i]);
			}
		}
			
	}

	void RandomCreateCube(){
		if (isMoved == false)
			return;
		
		while (true) {
			int x = Random.Range (1, numOfOBJ+1);
			if (score [x] == 0) {
				setPosition (x);
				cur_cubeNum++;
				setCubeColor (cm[x], 2);
				cm [x].gameObject.SetActive (true);
				int y = Random.Range (1, 8);
				if (y == 1) score [x] = 4;
				else score [x] = 2;
				setCubeColor (cm[x], score [x]);
				break;
			}
		}

		isMoved = false;
		if (cur_cubeNum == numOfOBJ && isGameOver ()) {
			gameover = true;
			Debug.Log ("GameOver");
		}
	}

	bool isGameOver (){
		///check x-axis
		for (int i = 1; i <= numOfOBJ; i+=LENGTH) {
			for (int j = i; j<i+LENGTH-1 ; j ++) {
				if (score [j] == score [j + 1] && score [j] != 0) {
					return false;
				}
			}
		}
		///check z-axis
		for (int i = 1 ; i <= LENGTH; i++) {
			for (int j = i; j <= numOfOBJ - LENGTH; j += LENGTH) {
				if (score [j] == score [j + LENGTH] && score [j] != 0) {
					return false;
				}
			}
		}
		///check y-axis
		 

		return true;
	}

	void save_prev_score (){
		for (int i = 1; i <= numOfOBJ; i++) {
			undo_score [i] = score [i];
		}
		UNDO = true;
	}

	void UNDO_move(){
		if (UNDO == true) {
			UNDO = false;
			for (int i = 1; i <= numOfOBJ; i++) {
				score [i] = undo_score [i];
				if (score [i] != 0) {
					cm [i].gameObject.SetActive (true);
					setCubeColor (cm [i], score [i]);
				} else
					cm [i].gameObject.SetActive (false);
			}
		}
	}

	void setPosition(int cur){
		int x = (3-(cur+2)%3)*3-6;
		int y = 3;
		int z = 3*(3-(cur-1)/3)-6;
		cm [cur].transform.position = new Vector3(x, y, z);
	}


}
