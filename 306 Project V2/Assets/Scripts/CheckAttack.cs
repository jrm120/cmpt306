using UnityEngine;
using System.Collections;

public class CheckAttack : MonoBehaviour {

	GameObject player;
	PlayerControls playerScript;
	float damage;

	void Start(){
		player = GameObject.Find("Player");
		playerScript = player.GetComponent<PlayerControls>();
	}

	void OnCollisionEnter2D(Collision2D col) {
		//damage enemy
		if (col.gameObject.tag == "Enemy"){
			//get amount of damage player attack does
			damage = playerScript.getDamageAmount ();
			//send damage message to enemy
			col.gameObject.SendMessage("ApplyDamage", damage);
		}
	}
	
}
