using UnityEngine;
using System.Collections;

// Interface for the player characters
public interface IHero{

	void ignoreCollisions();
	// returns damage amount based on skill level
	float getDamageAmount();
	// Applys damage if hit
	void ApplyDamage(int d);
}
