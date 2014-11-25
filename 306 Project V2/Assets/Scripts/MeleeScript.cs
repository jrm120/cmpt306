using UnityEngine;
using System.Collections;

public class MeleeScript : MonoBehaviour {

	GameObject player;
	PlayerControls playerScript;

	// The damage dealt by the melee attack. Affected by player's melee damage skill
	float damage;

	// Get's player's current damage amount on Start
	void Start(){
		if ( GameObject.FindGameObjectsWithTag("Player").Length != 0){
			if(player == null){
				player = GameObject.FindGameObjectWithTag("Player");
			}
		}
		playerScript = player.GetComponent<PlayerControls>();
		damage = playerScript.getMeleeDmg();
	}
	
	// Destroy the melee attack after .5 seconds
	void Update(){
		Destroy (gameObject, 0.5f);
	}

	// Deal damage if melee attack hits an enemy
	// Destroy melee attack if hits ground
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