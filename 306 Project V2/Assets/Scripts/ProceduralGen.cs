using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralGen : MonoBehaviour {
	//all rooms
	public GameObject[] prefabs;
	//available rooms (for next)
	List <GameObject> available;
	//start point of the last room
	//has the gem room appeared this level?
	bool gemRoom;
	public float startPos;
	//placement point for new room
	float curPos;
	//length of each room
	public float length;
	//how many rooms have been generated
	int roomNum = 0;
	//which room is current (array position)
	int roomID = -5;
	//maximum number of rooms
	public int maxRooms;
	//has the next room been generated?
	bool nextRoom = false;
	//The first room (where player spawns)
	GameObject firstRoom;
	//The complete point of the level
	Transform completePoint;
	//time between enemy spawns
	float spawnTime;
	//last enemy spawn time
	float lastSpawn = -5;
	//enemy spawn point
	Transform enemySpawn;
	//enemy prefab arrays
	public GameObject[] meleeEnemies;
	public GameObject[] rangeEnemies;
	//available enemy list
	List<GameObject> availEnemies;
	//player prefab
	public GameObject playerPrefab;
	//spawn location
	Transform playerSpawn;

	//player position
	public Transform player;

	GameObject guiDisplay;
	GUIDisplay gui;

	float Level;

	bool levelComplete = false;

	private static readonly System.Random getrandom = new System.Random();
	private static readonly object syncLock = new object();
	
	
	public static int GetRandomNumber(int min, int max)
	{
		lock(syncLock) { // synchronize
			return getrandom.Next(min, max);
		}
	}

	void Awake(){
		//generate first room
		Generate();
	}

	// Use this for initialization
	void Start () {
		guiDisplay = GameObject.FindGameObjectWithTag ("GUI");
		gui = guiDisplay.GetComponent<GUIDisplay>();
	}

	void FixedUpdate () {
		Level = gui.Level;
		if(!levelComplete){
			//set the spawn time
			spawnTime = 8.5f - (Level/2.0f);
			//spawnTime = 3.0f;
			//if there is no player, spawn and find the player
			if (player == null) {
				Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
				player = GameObject.FindGameObjectWithTag("Player").transform;
			}
			//player is 1/4 way through room, generate next room
			if ((player.position.x > startPos + length / 4)&&(!nextRoom)){
				nextRoom = true;
				Generate();
			}
			//player enters new room, update start position to start of new room
			if ((player.position.x > startPos + length)&&(nextRoom)){
				nextRoom = false;
				startPos += length;
			}
			//set enemy spawn point
			GameObject[] enSpawnArr = GameObject.FindGameObjectsWithTag("EnemySpawn");
			for(int i = 0; i<enSpawnArr.Length; i++){
				if(enSpawnArr[i].transform.position.x > startPos){
					enemySpawn = enSpawnArr[i].transform;
				}
			}
			//Spawn enemies
			if(Time.time - lastSpawn > spawnTime){
				spawnEnemy();
			}
			//find game complete point if its the last room
			if((roomNum == maxRooms) &&(completePoint == null)){
				GameObject[] endGameArr = GameObject.FindGameObjectsWithTag("EndGame");
				for(int i = 0; i<endGameArr.Length; i++){
					if(endGameArr[i].transform.position.x > startPos){
						completePoint = endGameArr[i].transform;
					}
				}
			}
			//player reaches end of last room
			if(completePoint){
				if(player.position.x > completePoint.position.x){
					gui.SendMessage("CompleteLevel");
					levelComplete = true;
					EndLevel();
				}
			}
		}
	}

	void Generate(){
		//if max number of rooms has not been generated
		if(roomNum < maxRooms){
			//reset available rooms list
			available = new List<GameObject>();
			//loop through all rooms, check acceptability
			for(int i = 0; i < prefabs.Length; i++){
				//if room option is not the same as the current room
				if(roomID != i){
					//get script
					PrefabScript s = prefabs[i].GetComponent<PrefabScript>();
					//get room difficulty
					int diff = s.difficulty;
					//if gem room has already been generated, don't add it to available
					if(!((gemRoom)&&(s.ID == 6))){
						//level 1-3
						if(Level < 4){
							//first 3 rooms must be difficulty 1 or 2
							if((roomNum < 3)&&(diff<3)){
								//add room to available list
								available.Add(prefabs[i]);
							//rooms 4-7 must have difficulty < 4
							}else if((2<roomNum)&&(roomNum<7)&&(diff<4)){
								//add room to available list
								available.Add(prefabs[i]);
							//last 3 rooms must be difficulty 2 or 3
							}else if((6<roomNum)&&(1<diff)){
								//add room to available list
								available.Add(prefabs[i]);
							}
						//level 4-7
						}else if(Level < 8){
							//first 3 rooms must be difficulty 1-3
							if((roomNum < 3)&&(diff<4)){
								//add room to available list
								available.Add(prefabs[i]);
								//can have any dificulty
							}else if((2<roomNum)&&(roomNum<7)){
								//add room to available list
								available.Add(prefabs[i]);
								//last 3 rooms must be difficulty > 1
							}else if((6<roomNum)&&(1<diff)){
								//add room to available list
								available.Add(prefabs[i]);
							}
						//level > 7
						}else{
							//first 5 rooms can have any difficulty
							if(roomNum < 6){
								//add room to available list
								available.Add(prefabs[i]);
							//last 5 rooms must have difficulty > 1
							}else if(1<diff){
								//add room to available list
								available.Add(prefabs[i]);
							}
						}
					}
				}
			}
			//choose random room from list of available, instantiate it
			int index = GetRandomNumber(0, available.Count);
			if(roomNum == 0){
				//reset gem room to false (new level)
				gemRoom = false;
				Instantiate(available[index], new Vector2(curPos, 0), new Quaternion(0,0,0,0));
				//find player spawn position
				GameObject[] spawns = GameObject.FindGameObjectsWithTag ("PlayerSpawn");
				for(int i = 0; i< spawns.Length; i++){
					if(spawns[i]!= null){
						playerSpawn = spawns[i].transform;
					}
				}
				levelComplete = false;
			}else{
				Instantiate(available[index], new Vector2(curPos, 0), new Quaternion(0,0,0,0));
			}
			//update id of last room
			PrefabScript script = available[index].GetComponent<PrefabScript>();
			roomID = script.ID;
			//gem room
			if(roomID == 6){
				gemRoom = true;
			}
			//update current position to be the end of the new room
			curPos += length;
			//increase existing number of rooms
			roomNum++;
		}
	}

	void spawnEnemy(){
		//reset available enemies list
		availEnemies = new List<GameObject>();
		//first 3 levels
		if(Level < 4){
			//add melee enemies (except ice golem) to available
			for(int i = 0; i<meleeEnemies.Length; i++){
				if(i<3){
					availEnemies.Add(meleeEnemies[i]);
				}
			}
			//second half of level, add rock throw golem to available
			if(roomNum > 5){
				availEnemies.Add(rangeEnemies[0]);
			}
		}else if(Level < 7){
			//add melee enemies (except ice golem) to available
			for(int i = 0; i<meleeEnemies.Length; i++){
				if(i<3){
					availEnemies.Add(meleeEnemies[i]);
				}
			}
			//add rock throw golem to available
			availEnemies.Add(rangeEnemies[0]);
			//second half of level, add ice golem and troll archer to available
			if(roomNum > 5){
				availEnemies.Add(meleeEnemies[3]);
				availEnemies.Add(rangeEnemies[1]);
			}
		}else{
			//add axe enemy
			availEnemies.Add(meleeEnemies[1]);
			//add range enemies (except skeleton archer)
			for(int i = 0; i<rangeEnemies.Length; i++){
				if(i<2){
					availEnemies.Add(rangeEnemies[i]);
				}
			}
			//second half of level, and skeleton archer and ice golem
			if(roomNum > 5){
				availEnemies.Add(meleeEnemies[3]);
				availEnemies.Add(rangeEnemies[2]);
			}
		}

		//choose random room from list of available, instantiate it
		int index = GetRandomNumber(0, availEnemies.Count);
		GameObject ob = Instantiate(availEnemies[index], enemySpawn.position, enemySpawn.rotation) as GameObject; 
		ob.transform.parent = enemySpawn.gameObject.transform;
		//reset last spawn time
		lastSpawn = Time.time;
	}

	void InitLevel(){
		roomNum = 0;
		curPos = 0;
		startPos = 0;
		roomID = -5;
		nextRoom = false;

		Generate ();
	}

	void EndLevel(){
		//Destroy player
		Destroy(GameObject.FindGameObjectWithTag ("Player"));
		//destroy all rooms
		GameObject[] rooms = GameObject.FindGameObjectsWithTag ("Room");
		foreach (GameObject r in rooms) {
			Destroy(r);
		}
		Invoke("InitLevel", 1.5f);
	}
}
