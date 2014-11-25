using UnityEngine;
using System.Collections;

public class EndTutScript : MonoBehaviour {
	public GUIDisplay display;
	public Transform player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//if there is no player, spawn and find the player
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player").transform;
		}
		if(player.position.x > 292){
			display.SendMessage("CompleteLevel");
			Invoke("BackToStart", 2); 
		}
	}

	void BackToStart(){
		Application.LoadLevel(0);
	}
}
