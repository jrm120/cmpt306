using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {
	//player prefab
	public GameObject player;
	//spawn location
	public Transform playerSpawn;
	//new player
	GameObject spawned_player;
	
	// Use this for initialization
	void Start () {
		spawned_player = Instantiate(player, playerSpawn.position, playerSpawn.rotation) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//if there is no player
		if(spawned_player == null){
			spawned_player = Instantiate(player, playerSpawn.position, playerSpawn.rotation) as GameObject;
		}
	}
}