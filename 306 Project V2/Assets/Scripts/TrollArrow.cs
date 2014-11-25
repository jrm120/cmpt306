using UnityEngine;
using System.Collections;

public class TrollArrow : MonoBehaviour {
	public int damage = 20;

	// Destroy the arrow after .8 seconds
	void Update(){
		Destroy (gameObject, 0.8f);
	}
	
	// Deal damage if arrow hits player
	public void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Player")
		{
			Destroy (gameObject);
			other.gameObject.SendMessage("ApplyDamage", damage);
		}
	}
}
