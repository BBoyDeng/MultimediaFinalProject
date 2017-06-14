using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller_3X3X3 : MonoBehaviour {

	public GameObject[] Cube = new GameObject[130];
	public CubeMove[] cm = new CubeMove[130];
	public GameObject Camera; 
	public CameraMove cam;
	private float cur_time;
	private float prev_time;
	private int Bound = 3;
	private int times = 10;
	private bool MOVING;
	private Vector3 vec;
	private int temp;

	static int LENGTH = 3;
	int numOfOBJ = 27;
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
	int curCamera;
	int total_score;

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
		if (!restart) {
			Camera = GameObject.Find("Camera_1"); 
			cam = Camera.GetComponent<CameraMove> ();
			curCamera = 1;
			camSwap (curCamera);
		}
		cur_cubeNum = 0;
		int count = 2;
		while (count>0) {
			int x = Random.Range (1, numOfOBJ+1);  
			if (score [x] == 0 && x != 23 && x != 14 && x != 5) {
				count--;
				setCubeColor (cm [x], 2);
				cm [x].gameObject.SetActive (true);
				score [x] = 2;
			}
		}
		gameover = false;
		restart = false;
		UNDO = false;
		total_score = 0;
	}

	// Update is called once per frame
	void Update () {
		cur_time = Time.time;
		if (!MOVING && !cam.isMoving) {
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
				dir = "Y";
				All_Cube_Move ();
				temp = times;
				MOVING = true;
			} else if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
				dir = "-Y";
				All_Cube_Move ();
				temp = times;
				MOVING = true;
			} else if (Input.GetKeyDown (KeyCode.Escape)) {
				restart = true;
				Start ();
			} else if (Input.GetKeyDown (KeyCode.Space)) {
				UNDO_move ();
			} else if(Input.GetKeyDown (KeyCode.Z)){ 
				curCamera--;
				if (curCamera == 0) curCamera = 4;
				cam.move ("Z", curCamera);
				camSwap (curCamera);

			} else if(Input.GetKeyDown (KeyCode.X)){ 
				curCamera++;
				if (curCamera == 5) curCamera = 1;
				cam.move ("X", curCamera);
				camSwap(curCamera); 
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
						if (curCamera == 1) {
							if (dir == "-Z" && v [2] >= -1 * Bound) {
								cm [i].transform.Translate (0, 0, -1 * moveRange [i] / times);
							} else if (dir == "Z" && v [2] <= Bound) {
								cm [i].transform.Translate (0, 0, moveRange [i] / times);
							} else if (dir == "X" && v [0] <= Bound) {
								cm [i].transform.Translate (moveRange [i] / times, 0, 0);
							} else if (dir == "-X" && v [0] >= -1 * Bound) {
								cm [i].transform.Translate (-1 * moveRange [i] / times, 0, 0);
							} else if (dir == "Y" && v [1] <= 9) {
								cm [i].transform.Translate (0, moveRange [i] / times, 0);
							} else if (dir == "-Y" && v [1] >= 3) {
								cm [i].transform.Translate (0, -1 * moveRange [i] / times, 0);
							}
						} else if (curCamera == 2) {
							if (dir == "-X" && v [2] >= -1 * Bound) {
								cm [i].transform.Translate (0, 0, -1 * moveRange [i] / times);
							} else if (dir == "X" && v [2] <= Bound) {
								cm [i].transform.Translate (0, 0, moveRange [i] / times);
							} else if (dir == "Y" && v [1] <= 9) {
								cm [i].transform.Translate (0, moveRange [i] / times, 0);
							} else if (dir == "-Y" && v [1] >= 3) {
								cm [i].transform.Translate (0, -1 * moveRange [i] / times, 0);
							}
						} else if (curCamera == 3) {
							if (dir == "Z" && v [2] >= -1 * Bound) {
								cm [i].transform.Translate (0, 0, -1 * moveRange [i] / times);
							} else if (dir == "-Z" && v [2] <= Bound) {
								cm [i].transform.Translate (0, 0, moveRange [i] / times);
							} else if (dir == "-X" && v [0] <= Bound) {
								cm [i].transform.Translate (moveRange [i] / times, 0, 0);
							} else if (dir == "X" && v [0] >= -1 * Bound) {
								cm [i].transform.Translate (-1 * moveRange [i] / times, 0, 0);
							} else if (dir == "Y" && v [1] <= 9) {
								cm [i].transform.Translate (0, moveRange [i] / times, 0);
							} else if (dir == "-Y" && v [1] >= 3) {
								cm [i].transform.Translate (0, -1 * moveRange [i] / times, 0);
							}
						} else if (curCamera == 4) {
							if (dir == "X" && v [2] >= -1 * Bound) {
								cm [i].transform.Translate (0, 0, -1 * moveRange [i] / times);
							} else if (dir == "-X" && v [2] <= Bound) {
								cm [i].transform.Translate (0, 0, moveRange [i] / times);
							} else if (dir == "Y" && v [1] <= 9) {
								cm [i].transform.Translate (0, moveRange [i] / times, 0);
							} else if (dir == "-Y" && v [1] >= 3) {
								cm [i].transform.Translate (0, -1 * moveRange [i] / times, 0);
							}
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
			if (curCamera == 1) {
				for (int i = 25; i >= 1; i -= LENGTH * LENGTH) {
					for (int j = i; j < i + LENGTH; j++) {
						int cur = j;
						while (cur-1>=i && score [cur - 1] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
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
				for (int i = numOfOBJ - LENGTH + 1; i >= 1; i -= LENGTH * LENGTH) {
					for (int j = i; j < i + LENGTH; j++) {
						if (j+1 < i+LENGTH && score [j] == score [j + 1] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j +1] * 2;
							total_score += score [j];
							score [j + 1] = 0;
							moveRange [j + 1] += LENGTH;
							movedTo [j + 1] = j;
							for (int k = j + 1; k < i + LENGTH-1; k++) {
								score [k] = score [k + 1];
								score [k + 1] = 0;
								moveRange [k + 1] += LENGTH;
								movedTo [k + 1] = j;
							}
							break;
						}
					}
				}
			} else if (curCamera == 2) {
				for (int i = 19; i >= 1; i -= LENGTH * LENGTH) {
					for (int j = i; j <= i + LENGTH*2; j+=LENGTH) {
						int cur = j;
						while ((cur - LENGTH) >= i && score [cur - LENGTH] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur - LENGTH] = score [cur];
							score [cur] = 0;
							cur-=LENGTH;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}
					}
				}
				///combine same score
				for (int i = 19; i >= 1; i -= LENGTH * LENGTH) {
					for (int j = i; j <= i + LENGTH*2; j+=LENGTH) {
						if (j + LENGTH <= i + LENGTH*2 && score [j] == score [j + LENGTH] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j + LENGTH] * 2;
							total_score += score [j];
							score [j + LENGTH] = 0;
							moveRange [j + LENGTH] += LENGTH;
							movedTo [j + LENGTH] = j;
							for (int k = j + LENGTH; k < i + LENGTH*2; k+=LENGTH) {
								score [k] = score [k + LENGTH];
								score [k + LENGTH] = 0;
								moveRange [k + LENGTH] += LENGTH;
								movedTo [k + LENGTH] = j;
							}
							break;
						}
					}
				}
			} else if (curCamera == 3) {
				for (int i = 21; i >= 1; i -= LENGTH * LENGTH) {
					for (int j = i; j > i - LENGTH; j--) {
						int cur = j;
						while (cur + 1 <= i && score [cur + 1] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur + 1] = score [cur];
							score [cur] = 0;
							cur++;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}
					}
				}
				///combine same score
				for (int i = 21; i >= 1; i -= LENGTH * LENGTH) {
					for (int j = i; j > i - LENGTH; j--) {
						if (j-1 > i-LENGTH && score [j] == score [j - 1] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j -1] * 2;
							total_score += score [j];
							score [j - 1] = 0;
							moveRange [j - 1] += LENGTH;
							movedTo [j - 1] = j;
							for (int k = j - 1 ; k > i - LENGTH+1; k--) {
								score [k] = score [k - 1];
								score [k - 1] = 0;
								moveRange [k - 1] += LENGTH;
								movedTo [k - 1] = j;
							}
							break;
						}
					}
				}
			} else if (curCamera == 4) {
				for (int i = numOfOBJ ; i >= 1; i -= LENGTH * LENGTH) {
					for (int j = i; j >= i - LENGTH*2; j-=LENGTH) {
						int cur = j;
						while ((cur + LENGTH) <= i && score [cur + LENGTH] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur + LENGTH] = score [cur];
							score [cur] = 0;
							cur+=LENGTH;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}
					}
				}
				///combine same score
				for (int i = numOfOBJ ; i >= 1; i -= LENGTH * LENGTH) {
					for (int j = i; j >= i - LENGTH*2; j-=LENGTH) {
						if (j - LENGTH >= i - LENGTH*2 && score [j] == score [j - LENGTH] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j - LENGTH] * 2;
							total_score += score [j];
							score [j - LENGTH] = 0;
							moveRange [j - LENGTH] += LENGTH;
							movedTo [j - LENGTH] = j;
							for (int k = j - LENGTH; k >= i - LENGTH; k-=LENGTH) {
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

		} else if (dir == "-X") {
			if (curCamera == 1) {
				for (int i = LENGTH*LENGTH; i <= numOfOBJ; i += LENGTH*LENGTH) {
					for (int j = i; j > i - LENGTH; j--) {
						int cur = j;
						while ((cur + 1) % LENGTH != 1 && score [cur + 1] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur + 1] = score [cur];
							score [cur] = 0;
							cur++;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}

					}
				}
				///combine same score
				for (int i = LENGTH*LENGTH; i <= numOfOBJ; i += LENGTH*LENGTH) {
					for (int j = i; j > i - LENGTH; j--) {
						if (j-1 > i-LENGTH && score [j] == score [j - 1] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j - 1] * 2;
							total_score += score [j];
							score [j - 1] = 0;
							moveRange [j - 1] += LENGTH;
							movedTo [j - 1] = j;
							for (int k = j - 1; k > i - LENGTH+1; k--) {
								score [k] = score [k - 1];
								score [k - 1] = 0;
								moveRange [k - 1] += LENGTH;
								movedTo [k - 1] = j;
							}
							break;
						}
					}
				}

			} else if (curCamera == 2) {
				for (int i = 25; i >= 1; i -= LENGTH*LENGTH) {
					for (int j = i; j >= i-2*LENGTH ; j-=LENGTH) {
						int cur = j;
						while ((cur + LENGTH) <= i && score [cur + LENGTH] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur + LENGTH] = score [cur];
							score [cur] = 0;
							cur+=LENGTH;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}

					}
				}
				///combine same score
				for (int i = 25; i >= 1; i -= LENGTH*LENGTH) {
					for (int j = i; j >= i-2*LENGTH ; j-=LENGTH) {
						if (j - LENGTH >= i-2*LENGTH && score [j] == score [j - LENGTH] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j - LENGTH] * 2;
							total_score += score [j];
							score [j - LENGTH] = 0;
							moveRange [j - LENGTH] += LENGTH;
							movedTo [j - LENGTH] = j;
							for (int k = j - LENGTH; k >= i - LENGTH; k-=LENGTH) {
								score [k] = score [k - LENGTH];
								score [k - LENGTH] = 0;
								moveRange [k - LENGTH] += LENGTH;
								movedTo [k - LENGTH] = j;
							}
							break;
						}
					}
				}
			} else if (curCamera == 3) {
				for (int i = 19; i >= 1 ; i -= LENGTH*LENGTH) {
					for (int j = i; j < i + LENGTH; j++) {
						int cur = j;
						while (cur - 1 >= i && score [cur - 1] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
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
				for (int i = 19; i >= 1 ; i -= LENGTH*LENGTH) {
					for (int j = i; j < i + LENGTH; j++) {
						if (j+1 < i+ LENGTH && score [j] == score [j + 1] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j + 1] * 2;
							total_score += score [j];
							score [j + 1] = 0;
							moveRange [j + 1] += LENGTH;
							movedTo [j + 1] = j;
							for (int k = j+1; k < i + LENGTH - 1; k++) {
								score [k] = score [k + 1];
								score [k + 1] = 0;
								moveRange [k + 1] += LENGTH;
								movedTo [k + 1] = j;
							}
							break;
						}
					}
				}
			} else if (curCamera == 4) {
				for (int i = 21; i >= 1; i -= LENGTH*LENGTH) {
					for (int j = i; j <= i+2*LENGTH ; j+=LENGTH) {
						int cur = j;
						while ((cur - LENGTH) >= i && score [cur - LENGTH] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur - LENGTH] = score [cur];
							score [cur] = 0;
							cur-=LENGTH;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}

					}
				}
				///combine same score
				for (int i = 21; i >= 1; i -= LENGTH*LENGTH) {
					for (int j = i; j <= i+2*LENGTH ; j+=LENGTH) {
						if (j + LENGTH <= i+2*LENGTH && score [j] == score [j + LENGTH] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j + LENGTH] * 2;
							total_score += score [j];
							score [j + LENGTH] = 0;
							moveRange [j + LENGTH] += LENGTH;
							movedTo [j + LENGTH] = j;
							for (int k = j + LENGTH; k <= i+LENGTH ; k+=LENGTH) {
								score [k] = score [k + LENGTH];
								score [k + LENGTH] = 0;
								moveRange [k + LENGTH] += LENGTH;
								movedTo [k + LENGTH] = j;
							}
							break;
						}
					}
				}
			}
		} else if (dir == "Y") { 
			if (curCamera == 1) {
				for (int i = numOfOBJ; i >= numOfOBJ - LENGTH + 1; i--) {
					for (int j = i; j >= 1; j -= LENGTH * LENGTH) {
						int cur = j;
						while (cur + LENGTH * LENGTH <= numOfOBJ && score [cur + LENGTH * LENGTH] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur + LENGTH * LENGTH] = score [cur];
							score [cur] = 0;
							cur += LENGTH * LENGTH;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}

					}
				}
				///combine same score
				for (int i = numOfOBJ; i >= numOfOBJ - LENGTH + 1; i--) {
					for (int j = i; j >= 1; j -= LENGTH * LENGTH) {
						if (j - LENGTH * LENGTH >=1 && score [j] == score [j - LENGTH * LENGTH] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j - LENGTH * LENGTH] * 2;
							total_score += score [j];
							score [j - LENGTH * LENGTH] = 0;
							moveRange [j - LENGTH * LENGTH] += LENGTH;
							movedTo [j - LENGTH * LENGTH] = j;
							for (int k = j - LENGTH * LENGTH; k - LENGTH * LENGTH >= 1; k -= LENGTH * LENGTH) {
								score [k] = score [k - LENGTH * LENGTH];
								score [k - LENGTH * LENGTH] = 0;
								moveRange [k - LENGTH * LENGTH] += LENGTH;
								movedTo [k - LENGTH * LENGTH] = j;
							}
							break;
						}
					}
				}
			} else if (curCamera == 2) {
				for (int i = 25; i >= numOfOBJ-2 - LENGTH*2; i-=LENGTH) {
					for (int j = i; j >= 1; j -= LENGTH * LENGTH) {
						int cur = j;
						while (cur + LENGTH * LENGTH <= numOfOBJ && score [cur + LENGTH * LENGTH] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur + LENGTH * LENGTH] = score [cur];
							score [cur] = 0;
							cur += LENGTH * LENGTH;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}

					}
				}
				///combine same score
				for (int i = 25; i >= numOfOBJ-2 - LENGTH*2; i-=LENGTH) {
					for (int j = i; j >= 1; j -= LENGTH * LENGTH) {
						if (j - LENGTH * LENGTH >=1 && score [j] == score [j - LENGTH * LENGTH] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j - LENGTH * LENGTH] * 2;
							total_score += score [j];
							score [j - LENGTH * LENGTH] = 0;
							moveRange [j - LENGTH * LENGTH] += LENGTH;
							movedTo [j - LENGTH * LENGTH] = j;
							for (int k = j - LENGTH * LENGTH; k - LENGTH * LENGTH >= 1; k -= LENGTH * LENGTH) {
								score [k] = score [k - LENGTH * LENGTH];
								score [k - LENGTH * LENGTH] = 0;
								moveRange [k - LENGTH * LENGTH] += LENGTH;
								movedTo [k - LENGTH * LENGTH] = j;
							}
							break;
						}
					}
				}
			} else if (curCamera == 3) {
				for (int i = 21; i >= numOfOBJ - 2 - LENGTH*2 ; i--) {
					for (int j = i; j >= 1; j -= LENGTH * LENGTH) {
						int cur = j;
						while (cur + LENGTH * LENGTH <= numOfOBJ && score [cur + LENGTH * LENGTH] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur + LENGTH * LENGTH] = score [cur];
							score [cur] = 0;
							cur += LENGTH * LENGTH;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}

					}
				}
				///combine same score
				for (int i = 21; i >= numOfOBJ - 2 - LENGTH*2 ; i--) {
					for (int j = i; j >= 1; j -= LENGTH * LENGTH) {
						if (j - LENGTH * LENGTH >=1 && score [j] == score [j - LENGTH * LENGTH] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j - LENGTH * LENGTH] * 2;
							total_score += score [j];
							score [j - LENGTH * LENGTH] = 0;
							moveRange [j - LENGTH * LENGTH] += LENGTH;
							movedTo [j - LENGTH * LENGTH] = j;
							for (int k = j - LENGTH * LENGTH; k >= LENGTH * LENGTH; k -= LENGTH * LENGTH) {
								score [k] = score [k - LENGTH * LENGTH];
								score [k - LENGTH * LENGTH] = 0;
								moveRange [k - LENGTH * LENGTH] += LENGTH;
								movedTo [k - LENGTH * LENGTH] = j;
							}
							break;
						}
					}
				}
			} else if (curCamera == 4) {
				for (int i = numOfOBJ; i >= numOfOBJ-LENGTH*2; i-=LENGTH) {
					for (int j = i; j >= 1; j -= LENGTH * LENGTH) {
						int cur = j;
						while (cur + LENGTH * LENGTH <= numOfOBJ && score [cur + LENGTH * LENGTH] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur + LENGTH * LENGTH] = score [cur];
							score [cur] = 0;
							cur += LENGTH * LENGTH;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}

					}
				}
				///combine same score
				for (int i = numOfOBJ; i >= numOfOBJ-LENGTH*2; i-=LENGTH) {
					for (int j = i; j >= 1; j -= LENGTH * LENGTH) {
						if (j - LENGTH * LENGTH >=1 && score [j] == score [j - LENGTH * LENGTH] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j - LENGTH * LENGTH] * 2;
							total_score += score [j];
							score [j - LENGTH * LENGTH] = 0;
							moveRange [j - LENGTH * LENGTH] += LENGTH;
							movedTo [j - LENGTH * LENGTH] = j;
							for (int k = j - LENGTH * LENGTH; k - LENGTH * LENGTH >= 1; k -= LENGTH * LENGTH) {
								score [k] = score [k - LENGTH * LENGTH];
								score [k - LENGTH * LENGTH] = 0;
								moveRange [k - LENGTH * LENGTH] += LENGTH;
								movedTo [k - LENGTH * LENGTH] = j;
							}
							break;
						}
					}
				}
			}
		} else if (dir == "-Y") { 
			if (curCamera == 1) {
				for (int i = LENGTH*LENGTH; i >= LENGTH*LENGTH - LENGTH + 1; i--) {
					for (int j = i; j <= numOfOBJ; j += LENGTH * LENGTH) {
						int cur = j;
						while (cur - LENGTH * LENGTH >= 1 && score [cur - LENGTH * LENGTH] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur - LENGTH * LENGTH] = score [cur];
							score [cur] = 0;
							cur -= LENGTH * LENGTH;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}

					}
				}
				///combine same score
				for (int i = LENGTH*LENGTH; i >= LENGTH*LENGTH - LENGTH + 1; i--) {
					for (int j = i; j <= numOfOBJ; j += LENGTH * LENGTH) {
						if (score [j] == score [j + LENGTH * LENGTH] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j + LENGTH * LENGTH] * 2;
							total_score += score [j];
							score [j + LENGTH * LENGTH] = 0;
							moveRange [j + LENGTH * LENGTH] += LENGTH;
							movedTo [j + LENGTH * LENGTH] = j;
							for (int k = j + LENGTH * LENGTH; k <= numOfOBJ; k += LENGTH * LENGTH) {
								score [k] = score [k + LENGTH * LENGTH];
								score [k + LENGTH * LENGTH] = 0;
								moveRange [k + LENGTH * LENGTH] += LENGTH;
								movedTo [k + LENGTH * LENGTH] = j;
							}
							break;
						}
					}
				}
			} else if (curCamera == 2) {
				for (int i = 7; i >= 1; i-=LENGTH) {
					for (int j = i; j <= numOfOBJ; j += LENGTH * LENGTH) {
						int cur = j;
						while (cur - LENGTH * LENGTH >= 1 && score [cur - LENGTH * LENGTH] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur - LENGTH * LENGTH] = score [cur];
							score [cur] = 0;
							cur -= LENGTH * LENGTH;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}

					}
				}
				///combine same score
				for (int i = 7; i >= 1; i-=LENGTH) {
					for (int j = i; j <= numOfOBJ; j += LENGTH * LENGTH) {
						if (j <= numOfOBJ && score [j] == score [j + LENGTH * LENGTH] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j + LENGTH * LENGTH] * 2;
							total_score += score [j];
							score [j + LENGTH * LENGTH] = 0;
							moveRange [j + LENGTH * LENGTH] += LENGTH;
							movedTo [j + LENGTH * LENGTH] = j;
							for (int k = j + LENGTH * LENGTH; k <= numOfOBJ - LENGTH*LENGTH; k += LENGTH * LENGTH) {
								score [k] = score [k + LENGTH * LENGTH];
								score [k + LENGTH * LENGTH] = 0;
								moveRange [k + LENGTH * LENGTH] += LENGTH;
								movedTo [k + LENGTH * LENGTH] = j;
							}
							break;
						}
					}
				}
			} else if (curCamera == 3) {
				for (int i = 1; i <= LENGTH; i++) {
					for (int j = i; j <= numOfOBJ; j += LENGTH * LENGTH) {
						int cur = j;
						while (cur - LENGTH * LENGTH >= 1 && score [cur - LENGTH * LENGTH] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur - LENGTH * LENGTH] = score [cur];
							score [cur] = 0;
							cur -= LENGTH * LENGTH;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}

					}
				}
				///combine same score
				for (int i = 1; i <= LENGTH; i++) {
					for (int j = i; j <= numOfOBJ; j += LENGTH * LENGTH) {
						if (score [j] == score [j + LENGTH * LENGTH] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j + LENGTH * LENGTH] * 2;
							total_score += score [j];
							score [j + LENGTH * LENGTH] = 0;
							moveRange [j + LENGTH * LENGTH] += LENGTH;
							movedTo [j + LENGTH * LENGTH] = j;
							for (int k = j + LENGTH * LENGTH; k <= numOfOBJ - LENGTH * LENGTH; k += LENGTH * LENGTH) {
								score [k] = score [k + LENGTH * LENGTH];
								score [k + LENGTH * LENGTH] = 0;
								moveRange [k + LENGTH * LENGTH] += LENGTH;
								movedTo [k + LENGTH * LENGTH] = j;
							}
							break;
						}
					}
				}
			} else if (curCamera == 4) {
				for (int i = 3; i <= LENGTH*LENGTH; i+=LENGTH) {
					for (int j = i; j <= numOfOBJ; j += LENGTH * LENGTH) {
						int cur = j;
						while (cur - LENGTH * LENGTH >= 1 && score [cur - LENGTH * LENGTH] == 0 && score [cur] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							score [cur - LENGTH * LENGTH] = score [cur];
							score [cur] = 0;
							cur -= LENGTH * LENGTH;
							movedTo [j] = cur;
							moveRange [j] += LENGTH;
						}

					}
				}
				///combine same score
				for (int i = 3; i <= LENGTH*LENGTH; i+=LENGTH) {
					for (int j = i; j <= numOfOBJ; j += LENGTH * LENGTH) {
						if (j <= numOfOBJ && score [j] == score [j + LENGTH * LENGTH] && score [j] != 0) {
							if (!isMoved)
								save_prev_score ();
							isMoved = true;
							cur_cubeNum--;
							score [j] = score [j + LENGTH * LENGTH] * 2;
							total_score += score [j];
							score [j + LENGTH * LENGTH] = 0;
							moveRange [j + LENGTH * LENGTH] += LENGTH;
							movedTo [j + LENGTH * LENGTH] = j;
							for (int k = j + LENGTH * LENGTH; k <= numOfOBJ - LENGTH*LENGTH; k += LENGTH * LENGTH) {
								score [k] = score [k + LENGTH * LENGTH];
								score [k + LENGTH * LENGTH] = 0;
								moveRange [k + LENGTH * LENGTH] += LENGTH;
								movedTo [k + LENGTH * LENGTH] = j;
							}
							break;
						}
					}
				}
			}
		} else if (dir == "Z") { 
			
		} else if (dir == "-Z") { 

		}


	}

	Material mMaterial;
	void setCubeColor(CubeMove cm, int value){
		if (value == 2) {
			cm.GetComponent<Renderer> ().material.color = Color.cyan;
			if (curCamera == 1) {
				mMaterial = Resources.Load ("2", typeof(Material)) as Material;
			} else mMaterial = Resources.Load ("2p", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 4) {
			if (curCamera == 1) {
				mMaterial = Resources.Load ("4", typeof(Material)) as Material;
			} else mMaterial = Resources.Load ("4p", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 8) {
			if (curCamera == 1) {
				mMaterial = Resources.Load ("8", typeof(Material)) as Material;
			} else mMaterial = Resources.Load ("8p", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 16) {
			if (curCamera == 1) {
				mMaterial = Resources.Load ("16", typeof(Material)) as Material;
			} else mMaterial = Resources.Load ("16p", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 32) {
			if (curCamera == 1) {
				mMaterial = Resources.Load ("32", typeof(Material)) as Material;
			} else mMaterial = Resources.Load ("32p", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 64) {
			if (curCamera == 1) {
				mMaterial = Resources.Load ("64", typeof(Material)) as Material;
			} else mMaterial = Resources.Load ("64p", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 128) {
			if (curCamera == 1) {
				mMaterial = Resources.Load ("128", typeof(Material)) as Material;
			} else mMaterial = Resources.Load ("128p", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 256) {
			if (curCamera == 1) {
				mMaterial = Resources.Load ("256", typeof(Material)) as Material;
			} else mMaterial = Resources.Load ("256p", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 512) {
			if (curCamera == 1) {
				mMaterial = Resources.Load ("512", typeof(Material)) as Material;
			} else mMaterial = Resources.Load ("512p", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 1024) {
			if (curCamera == 1) {
				mMaterial = Resources.Load ("1024", typeof(Material)) as Material;
			} else mMaterial = Resources.Load ("1024p", typeof(Material)) as Material;
			cm.GetComponent<Renderer> ().material = mMaterial;
		} else if (value == 2048) {
			if (curCamera == 1) {
				mMaterial = Resources.Load ("2048", typeof(Material)) as Material;
			} else mMaterial = Resources.Load ("2048p", typeof(Material)) as Material;
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
			if (score [x] == 0 && x!=23 && x!=14 && x!=5){
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
		int t = cur - 9*((cur-1)/9);
		int x = (3-(t+2)%3)*3-6;
		int y = 3*(((cur-1)/9)+1);
		int z = 3*(3-(t-1)/3)-6;
		cm [cur].transform.position = new Vector3(x, y, z);
	}

	void camSwap(int currentCam){   
		/*foreach (GameObject cams in cameras){ 
			Camera theCam = cams.GetComponent<Camera>() as Camera; 
			theCam.enabled = false; 
		}      

		string oneToUse = "Camera_"+currentCam; 	
		Camera usedCam = GameObject.Find(oneToUse).GetComponent<Camera>() as Camera; 
		usedCam.enabled = true; */
		for (int i = 1; i <= numOfOBJ; i++) {
			setCubeColor (cm[i], score[i]);
		}

	}

}

	
