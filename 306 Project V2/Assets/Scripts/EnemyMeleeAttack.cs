using UnityEngine;
using System.Collections;

public class EnemyMeleeAttack : MonoBehaviour {
	public int damage = 20;
	
	// Deal damage if arrow hits player
	public void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Player")
		{
			//Destroy (gameObject);
			other.gameObject.SendMessage("ApplyDamage", damage);
		}
	}
}