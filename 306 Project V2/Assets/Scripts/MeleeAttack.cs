using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour {
	public Rigidbody2D meleePrefab;

	// Determine if player is facing right or left
	// then instantiate melee attack in that direction
	public void Melee(bool isFacingRight){
		if (isFacingRight)
		{
			// Attack right
			Rigidbody2D bPrefab = Instantiate (meleePrefab, transform.position, Quaternion.Euler(1,1,90)) as Rigidbody2D;
			bPrefab.rigidbody2D.AddForce (Vector2.right * 10);
		}
		else
		{
			// Attack left
			Rigidbody2D bPrefab = Instantiate (meleePrefab, transform.position, Quaternion.Euler(1,1,270)) as Rigidbody2D;
			bPrefab.rigidbody2D.AddForce (Vector2.right * -10);
		}
	}
}