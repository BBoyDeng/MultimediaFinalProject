using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Time_Controller_4X4 : MonoBehaviour {

	public GameObject[] Cube = new GameObject[130];
	public CubeMove[] cm = new CubeMove[130];
	public GameObject pauseCanvas;
	public GameObject gameOverCanvas;
	public GameObject scoreCanvas;
	public Text gameOverText;
	public Text scoreText;
	public GameObject lightSource;

	private float cur_time;
	private float prev_time;
	private int Bound = 4 ;
	private int times = 10;
	private bool MOVING;
	private Vector3 vec;
	private int temp;
	private bool isPaused;
	private float lightRotateX;
	private float countDown;

	static int LENGTH = 4;
	int numOfOBJ = 16;
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
	int total_score;
	AudioSource press_music;
	AudioClip CLIP;
	GUIStyle style;
	int prevTotalScore;
	Quaternion quate;
	int prev_cubeNum;

	// Use this for initialization
	void Start () {
		for (int i = 1; i <= numOfOBJ; i++) {
			if (!restart) {
				string str = "Cube_" + i.ToString ();
				Cube [i] = GameObject.Find (str);
				cm [i] = Cube [i].GetComponent<CubeMove> ();
			}
			cm [i].gameObject.SetActive (false);
			movedTo [i] = 0;
			score[i] = 0;
			moveRange [i] = 0;
		}
		if (!restart) {
			press_music = this.gameObject.AddComponent<AudioSource> ();
			CLIP = Resources.Load<AudioClip> ("cursor7");
			press_music.clip = CLIP;
			style = new GUIStyle();
		}
		cur_cubeNum = 2;
		int count = 2;
		while (count>0) {
			int x = Random.Range (1, numOfOBJ+1);  
			if (score [x] == 0) {
				setPosition (x);
				count--;
				setCubeColor (cm[x], 2);
				cm [x].gameObject.SetActive (true);
				score [x] = 2;
			}
		}
		gameover = false;
		restart = false;
		UNDO = false;
		isPaused = false;
		total_score = 0;
		countDown = 360f;
		lightRotateX = 0.05f;

		quate = Quaternion.identity;
		quate.eulerAngles = new Vector3 (270f, 0f, 0f);
		lightSource.transform.rotation = quate;

		pauseCanvas.SetActive (false);
		gameOverCanvas.SetActive (false);
		scoreCanvas.SetActive (true);
	}

	// Update is called once per frame
	void Update () {

		if ((countDown -= lightRotateX) <= 0f) {		// Timeout
			lightRotateX = 0f;
			gameover = true;
		}

		lightSource.transform.Rotate (lightRotateX, 0f, 0f);

		if (!gameover) {
			gameOverCanvas.SetActive (false);
			scoreCanvas.SetActive (true);
			scoreText.text = "Score : " + total_score.ToString ();
		} else {
			scoreCanvas.SetActive (false);
			gameOverCanvas.SetActive (true);
			gameOverText.text = "Game Over...\n" + "Your Score : " + total_score.ToString () + "\n\n"
				+ "Press R to restart\n" + "Press B to mainmenu\n";
		}


		if (Input.GetKeyDown (KeyCode.B)) {
			Time.timeScale = 1f;
			pauseCanvas.SetActive (false);
			Application.LoadLevel ("MainMenu1");
		} else if (Input.GetKeyDown (KeyCode.R)) {
			Time.timeScale = 1f;
			restart = true;
			Start ();
			press_music.Play ();
		} else if (Input.GetKeyDown (KeyCode.Escape) && !gameover) {
			if (!isPaused) {
				lightRotateX = 0f;
				Time.timeScale = 0;
				pauseCanvas.SetActive (true);
				isPaused = true;
			} else {
				lightRotateX = 0.05f;
				Time.timeScale = 1f;
				pauseCanvas.SetActive (false);
				isPaused = false;
			}
		}

		if (!isPaused && !gameover) {
			cur_time = Time.time;
			if (!MOVING) {
				if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) {
					dir = "-X";
					All_Cube_Move ();
					temp = times;
					MOVING = true;
					if (isMoved)
						press_music.Play ();
				} else if (Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) {
					dir = "X";
					All_Cube_Move ();
					temp = times;
					MOVING = true;
					if (isMoved)
						press_music.Play ();
				} else if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
					dir = "Z";
					All_Cube_Move ();
					temp = times;
					MOVING = true;
					if (isMoved)
						press_music.Play ();
				} else if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
					dir = "-Z";
					All_Cube_Move ();
					temp = times;
					MOVING = true;
					if (isMoved)
						press_music.Play ();
				} else if (Input.GetKeyDown (KeyCode.R)) {
					restart = true;
					Start ();
					press_music.Play ();
				} else if (Input.GetKeyDown (KeyCode.Space)) {
					UNDO_move ();
					press_music.Play ();
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
							if (dir == "-Z" && v [2] >= -4.5) {
								cm [i].transform.Translate (0, 0, -1 * moveRange [i] / times);
							} else if (dir == "Z" && v [2] <= 4.5) {
								cm [i].transform.Translate (0, 0, moveRange [i] / times);
							} else if (dir == "X" && v [0] <= 4.5) {
								cm [i].transform.Translate (moveRange [i] / times, 0, 0);
							} else if (dir == "-X" && v [0] >= -4.5) {
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
						total_score += score [j];
						score [j + 1] = 0;
						moveRange [j + 1] += LENGTH;
						movedTo [j + 1] = j;
						for (int k = j + 1 ; k < i+LENGTH-1; k ++) {
							score [k] = score [k + 1];
							score [k + 1] = 0;
							moveRange [k + 1] += LENGTH;
							movedTo [k + 1] = j;
						}
						//break;
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
						total_score += score [j];
						score [j - 1] = 0;
						moveRange [j-1] += LENGTH;
						movedTo [j-1] = j;
						for (int k = j - 1 ; k > i-LENGTH+1; k--) {
							score [k] = score [k - 1];
							score [k - 1] = 0;
							moveRange [k - 1] += LENGTH;
							movedTo [k - 1] = j;
						}
						//break;
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
						total_score += score [j];
						score [j + LENGTH] = 0;
						moveRange [j + LENGTH] += LENGTH;
						movedTo [j + LENGTH] = j;
						for (int k = j + LENGTH ; k <= numOfOBJ - LENGTH; k += LENGTH) {
							score [k] = score [k + LENGTH];
							score [k + LENGTH] = 0;
							moveRange [k + LENGTH] += LENGTH;
							movedTo [k + LENGTH] = j;
						}
						//break;
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
						total_score += score [j];
						score [j - LENGTH] = 0;
						moveRange [j - LENGTH] += LENGTH;
						movedTo [j - LENGTH] = j;
						for (int k = j - LENGTH ; k > LENGTH; k -= LENGTH) {
							score [k] = score [k - LENGTH];
							score [k - LENGTH] = 0;
							moveRange [k - LENGTH] += LENGTH;
							movedTo [k - LENGTH] = j;

						}
						//break;
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

	void ResetAndShow(){   ////!!!BUG原因所在
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
			lightRotateX = 0f;
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
		prevTotalScore = total_score;
		prev_cubeNum = cur_cubeNum;
		for (int i = 1; i <= numOfOBJ; i++) {
			undo_score [i] = score [i];
		}
		UNDO = true;
	}

	void UNDO_move(){
		if (UNDO == true) {
			total_score = prevTotalScore;
			cur_cubeNum = prev_cubeNum;
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
		float x = (3-(cur+3)%4)*6-9;
		x /= 2;
		int y = 3;
		float z = (6*(3-(cur-1)/4)-9);
		z /= 2;
		cm [cur].transform.position = new Vector3(x, y, z);
	}


}
