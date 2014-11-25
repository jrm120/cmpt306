using UnityEngine;
using System.Collections;

// Interface for enemies.
// GuardLogic implements interface
public interface IEnemy{
	void ApplyDamage(int d);
	void Patrol();
	void Behaviors();
	void Raycasting();
	void OnCollisionEnter2D(Collision2D col);

}
