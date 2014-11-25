using UnityEngine;
using System.Collections;

public class DaggerScript : MonoBehaviour {

	GameObject player;
	PlayerControls playerScript;

	// The damage dealt by the dagger. Affected by player's ranged damage skill
	float damage;

	// Get's player's current damage amount on Start
	void Start(){
		if ( GameObject.FindGameObjectsWithTag("Player").Length != 0){
			if(player == null){
				player = GameObject.FindGameObjectWithTag("Player");
			}
		}
		playerScript = player.GetComponent<PlayerControls>();
		damage = playerScript.getRangedDmg();
	}


	// Destroy the dagger after .8 seconds
	void Update(){
		Destroy (gameObject, 0.8f);
	}

	// Deal damage if dagger hits an enemy
	// Destroy dagger if hits ground
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Enemy")
		{
			Destroy (gameObject);
			col.gameObject.SendMessage ("ApplyDamage", damage);
		}
		else if (col.gameObject.tag == "Ground")
		{
			Destroy (gameObject);
		}
	}
}
