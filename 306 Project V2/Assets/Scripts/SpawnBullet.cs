using UnityEngine;
using System.Collections;

public class SpawnBullet : MonoBehaviour {
	public Rigidbody2D bulletPrefab;
	/*
	float angle1 = 45.0f;
	float angle2 = 135.0f;
	*/

	// Determine if player is facing right or left
	// then instantiate bullet in that direction
	public void Shoot(bool isFacingRight){
		Rigidbody2D bPrefab = Instantiate (bulletPrefab, transform.position, transform.rotation) as Rigidbody2D;
		if (isFacingRight)
		{
			bPrefab.rigidbody2D.AddForce (Vector2.right * 600);
		}
		else
		{
			bPrefab.rigidbody2D.AddForce (Vector2.right * -600);
		}
	}
	/*
	public void Throw(bool isFacingRight){
		Rigidbody2D bPrefab = Instantiate (bulletPrefab, transform.position, transform.rotation) as Rigidbody2D;
		if (isFacingRight)
		{
			Vector2 dir = Quaternion.AngleAxis (angle1, Vector3.forward) * Vector3.right;
			//bPrefab.rigidbody2D.AddForce (Vector2.right * 600);
			bPrefab.rigidbody2D.AddForce (dir*600);
		}
		else
		{
			Vector2 dir = Quaternion.AngleAxis (angle2, Vector3.forward) * Vector3.right;
			//bPrefab.rigidbody2D.AddForce (Vector2.right * -600);
			bPrefab.rigidbody2D.AddForce (dir*600);
		}
	}*/
}
