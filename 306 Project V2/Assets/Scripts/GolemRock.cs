using UnityEngine;
using System.Collections;

public class GolemRock : MonoBehaviour {
	public int damage = 20;
	
	// Destroy the rock after .8 seconds
	void Update(){
		Destroy (gameObject, 0.8f);
	}
	
	// Deal damage if rock hits player
	public void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Player")
		{
			Destroy (gameObject);
			other.gameObject.SendMessage("ApplyDamage", damage);
		}
	}
}
