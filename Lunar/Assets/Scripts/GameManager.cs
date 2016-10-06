using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {


	public EnemyRow rowOne;
	public EnemyRow rowTwo;
	public EnemyRow rowThree;
	public PickUpColumn columnOne;
	public PickUpColumn columnTwo;
	public PickUpColumn columnThree;
	public PickUpColumn columnFour;
	public PickUpColumn columnFive;
	public Transform SideSpawnPosition;
	public Transform TopSpawnPosition;
	public Vector3[] SideSpawns = new Vector3[6];
	public Vector3[] TopSpawns = new Vector3[5];
	public float FPS;
    public Text fpsCounter, accelCounter;


	// Use this for initialization
	void Start () {
		rowOne = new EnemyRow(4.0f);
		rowTwo = new EnemyRow(2.0f);
		rowThree = new EnemyRow(1.0f);
		columnOne = new PickUpColumn(5.0f);
		columnTwo = new PickUpColumn(13.0f);
		columnThree = new PickUpColumn(11.0f);
		columnFour = new PickUpColumn(7.0f);
		columnFive = new PickUpColumn(3.0f);
		UpdateSpawnPositions();
	}

	public void InitiateSpawn() {
		rowOne.startSpawn = true;
		rowTwo.startSpawn = true;
		rowThree.startSpawn = true;
		columnOne.startSpawn = true;
		columnTwo.startSpawn = true;
		columnThree.startSpawn = true;
		columnFour.startSpawn = true;
		columnFive.startSpawn = true;
	}
	// Update is called once per frame
	void Update () {
		UpdateSpawnPositions();
		FPS = 1/Time.deltaTime;
		fpsCounter.text = "FPS: " + FPS;
//        accelCounter.text = "A: " + Input.acceleration.x;
		if(Input.GetKeyDown(KeyCode.S)) {
			InitiateSpawn();
		}
		rowOne.Update();
		rowTwo.Update();
		rowThree.Update();
		columnOne.Update();
		columnTwo.Update();
		columnThree.Update();
		columnFour.Update();
		columnFive.Update();
	}

	void UpdateSpawnPositions() {
		for(int i = 0; i < 3; i++) {
			SideSpawns[i] = new Vector3(SideSpawnPosition.position.x+12.0f, SideSpawnPosition.position.y+i*5.0f, 7.0f);
			SideSpawns[i+3] = new Vector3(SideSpawnPosition.position.x-12.0f, SideSpawnPosition.position.y+i*5.0f, 7.0f);
		}
		for(int i = 0; i < 5; i++) {
			TopSpawns[i] = new Vector3(TopSpawnPosition.position.x+(i*2.5f), TopSpawnPosition.position.y, 7.0f);
		}
		rowOne.UpdateRowPositions(SideSpawns[0], SideSpawns[3]);
		rowTwo.UpdateRowPositions(SideSpawns[1], SideSpawns[4]);
		rowThree.UpdateRowPositions(SideSpawns[2], SideSpawns[5]);
		columnOne.UpdateRowPositions(TopSpawns[0]);
		columnTwo.UpdateRowPositions(TopSpawns[1]);
		columnThree.UpdateRowPositions(TopSpawns[2]);
		columnFour.UpdateRowPositions(TopSpawns[3]);
		columnFive.UpdateRowPositions(TopSpawns[4]);

	}



	#region TABLEDEFINITION
	public class TableAttribute {
		public GameObject currentSpawnObject;
		public float rateOfSpawn = 2.0f; //Seconds per spawn
		public float currentRate = 0.0f;
		public bool startSpawn = false;
	}

	public class Row : TableAttribute {
		public Vector3[] spawnPosition = new Vector3[2];
		public string[] objectTypes = new string[3];

		public void UpdateRowPositions(Vector3 spawnPositionOne, Vector3 spawnPositionTwo) {
			spawnPosition[0] = spawnPositionOne;
			spawnPosition[1] = spawnPositionTwo;
		}

		void SpawnObject() {
			if(currentSpawnObject == null) {
				int a = Random.Range(0,2);
				int objectType = Random.Range(0,3);
				if(a==0) {
					currentSpawnObject = (GameObject)Instantiate(Resources.Load<GameObject>(objectTypes[objectType]), spawnPosition[0], new Quaternion(0.5f, -0.5f, -0.5f, -0.5f));
				} else {
					currentSpawnObject = (GameObject)Instantiate(Resources.Load<GameObject>(objectTypes[objectType]), spawnPosition[1], new Quaternion(0.5f, 0.5f, 0.5f, -0.5f));
				}
			}
		}

		public void Update() {
			if(startSpawn == true) {
				currentRate += Time.deltaTime;
				if(currentRate > rateOfSpawn) {
					SpawnObject();
					currentRate = 0.0f;
				}
			}
		}
	};

	public class Column : TableAttribute {
		public Vector3 spawnPosition = new Vector3();
		public string[] objectTypes = new string[3];

		public void UpdateRowPositions(Vector3 spawnPositionOne) {
			spawnPosition = spawnPositionOne;
		}

		void SpawnObject() {
			int count = Random.Range(0,101); //TODO: Make certain pickups rare here
			int objectType = 0;
			if(count%2 == 0) {
				objectType = 0;
			} else if(count%3 == 0) {
				objectType = 1;
			} else if(count%7 == 0) {
				objectType = 2;
			} else {
				objectType = 0;
			}//CONTINUE TO ADD OBJECTS HERE. THE HIGHER THE REMAINDER, THE RARER THE OBJECT
			currentSpawnObject = (GameObject)Instantiate(Resources.Load<GameObject>(objectTypes[objectType]), spawnPosition, new Quaternion(0, 0, 0, 0));
		}

		public void Update() {
			if(startSpawn == true) {
				currentRate += Time.deltaTime;
				if(currentRate > rateOfSpawn) {
					SpawnObject();
					currentRate = 0.0f;
				}
			}
		}

	}

	public class EnemyRow : Row {
		public EnemyRow(float _rateOfSpawn) {
			rateOfSpawn = _rateOfSpawn;
			objectTypes[0] = "Prefabs/Geese";
			objectTypes[1] = "Prefabs/Airplane";
			objectTypes[2] = "Prefabs/UFO";
		}
	}

	public class PickUpColumn : Column {
		public PickUpColumn(float _rateOfSpawn) {
			rateOfSpawn = _rateOfSpawn;
			objectTypes[0] = "Prefabs/Coin1";
			objectTypes[1] = "Prefabs/Coin2";
			objectTypes[2] = "Prefabs/HoopHolder";
		}
	}
	#endregion
}
