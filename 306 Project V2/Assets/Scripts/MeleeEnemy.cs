using UnityEngine;

using System.Collections;

public class MeleeEnemy : MonoBehaviour {

	// The health of the enemy
	public int Health;

	// How much experience the enemy is worth
	public int expWorth = 250;

	// The minimum distance for the player to be visible
	// Ice golem: 18
	// Cyclops: 9
	public int distPlayerVisible;

	// The minimum distance from the player to be in melee range
	// Ice golem: 11
	// Cyclops: 4
	public int distPlayerInRange;
	
	// The distance between the golem and the player
	float distance;
	GameObject player;

	//animator
	protected Animator anim;
	//speed
	public float xSpeed;
	
	//min and max positions
	public float minX;
	public float maxX;
	//start position
	public float startX;
	public float startY;

	//Boolean checks
	bool isFacingRight;
	bool attacking;
	bool isIdle;
	bool canIdle;

	public BoxCollider2D faceLeftBox;
	public BoxCollider2D faceRightBox;

	void Start () {
		//get animator
		anim = GetComponent<Animator>();
		//start off walking
		anim.SetBool ("Walk", true);
		//update health
		anim.SetInteger ("Health", Health);
		//Start position
		transform.localPosition = new Vector3(startX, startY, 0);
		transform.rotation = new Quaternion (0, 0, 0, 0);
		//Set initial bools
		isFacingRight = true;
		isIdle = false;
		canIdle = true;
		//Start decision tree
		enemyAction = TargetVisible;
	}
	
	//when damaged
	void ApplyDamage(int d){
		Health -= d;
		//update health
		anim.SetInteger ("Health", Health);
		//die
		if(Health <1){
			//stop moving
			anim.SetBool ("Walk", false);
			xSpeed = 0;
			//Stop attacking
			anim.SetBool ("Attack", false);
			rigidbody2D.velocity = new Vector2(0, 0);
			//destroy enemy after die animation
			Destroy(gameObject, 0.7f);
		}
	}

	//Enemy decision tree
	delegate void MyDelegate();
	MyDelegate enemyAction;
	
	void Update(){

		// Enable/disable appropriate box colliders
		if(isFacingRight)
		{
			faceRightBox.enabled = true;
			faceLeftBox.enabled = false;
		}
		else
		{
			faceRightBox.enabled = false;
			faceLeftBox.enabled = true;
		}

		//Find the player
		if (GameObject.FindGameObjectsWithTag ("Player").Length != 0)
		{
			if (player == null)
			{
				player = GameObject.FindGameObjectWithTag ("Player");
			}
			//Find the distance between the enemy and the player
			distance = Vector3.Distance (transform.position, player.transform.position);
		}
		//Continuously update the enemies speed
		rigidbody2D.velocity = new Vector2 (xSpeed, 0);
		enemyAction ();
	}

	//Target Visible?
	private void TargetVisible()
	{
		if(distance < distPlayerVisible)
		{
			//The target is visible, check if in range of target
			enemyAction = inRange;
		}
		else
		{
			//The target is not visible, do random action
			enemyAction = RandomNum;
		}
	}

	//In range?
	private void inRange()
	{
		if(distance < distPlayerInRange)
		{
			//The target is in melee range
			enemyAction = Attack;
		}
		else
		{
			//The target is close, but not in melee range. Advance.
			attacking = false;
			anim.SetBool ("Attack", false);
			enemyAction = Advance;
		}
	}

	private void RandomNum()
	{
		if(Random.value > 0.5f)
		{
			// Walk randomly
			enemyAction = RandomWalk;
		}
		else
		{
			// Begin idle
			enemyAction = Idle;
		}
	}
	
	// RANDOM WALK
	private void RandomWalk()
	{
		if (!isIdle)
		{
			// Move right
			if (isFacingRight && (!attacking))
			{
				xSpeed = 2;
			}
			// Move left
			else if (!isFacingRight && (!attacking)) 
			{
				xSpeed = -2;
			}
			anim.SetBool ("Walk", true);
			//if position is invalid, turn around and move in other direction
			if (transform.localPosition.x < minX)
			{
				isFacingRight = true;
				transform.localPosition = new Vector2 (minX, transform.localPosition.y);
				transform.rotation = new Quaternion (0, 0, 0, 0);
				xSpeed *= -1;
			}
			else if (maxX < transform.localPosition.x)
			{
				isFacingRight = false;
				transform.localPosition = new Vector2 (maxX, transform.localPosition.y);
				transform.rotation = new Quaternion (0, 180, 0, 0);
				xSpeed *= -1;
			}
		}
		enemyAction = TargetVisible;
	}
	
	//ATTACK
	private void Attack()
	{
		//Face the direction of the player
		if (transform.position.x < player.transform.position.x)
		{
			transform.rotation = new Quaternion(0, 0, 0, 0);
			//gameObject.collider2D.transform.position += vector
			isFacingRight = true;
		}
		else
		{
			transform.rotation = new Quaternion(0, 180, 0, 0);
			isFacingRight = false;
		}
		xSpeed = 0;
		anim.SetBool ("Walk", false);
		attacking = true;
		anim.SetBool ("Attack", true);
		enemyAction = TargetVisible;
	}
	
	//ADVANCE
	private void Advance()
	{
		anim.SetBool ("Walk", true);
		//If player is to the right, advance right
		if (transform.position.x < player.transform.position.x)
		{
			transform.rotation = new Quaternion(0, 0, 0, 0);
			isFacingRight = true;
			if(!attacking){
				xSpeed = 3.5f;
			}
		}
		//If player is to the left, advance left
		else
		{
			transform.rotation = new Quaternion(0, 180, 0, 0);
			isFacingRight = false;
			if(!attacking){
				xSpeed = -3.5f;
			}
		}
		enemyAction = TargetVisible;
	}

	//IDLE
	private void Idle()
	{
		//If the idle action is off cooldown, idle
		if (canIdle)
		{
			xSpeed = 0;
			anim.SetBool ("Walk", false);
			anim.SetBool ("Attack", false);
			if (!isIdle)
			{
				isIdle = true;
				StartCoroutine (stopIdle ());
			}
		}
		enemyAction = TargetVisible;
	}
	
	//Idle for at least 3 seconds before randomly walking again
	IEnumerator stopIdle()
	{
		yield return new WaitForSeconds(3.0f);
		isIdle = false;
		canIdle = false;
		StartCoroutine(IdleCooldown());
	}

	//Unable to idle again until cooldown finishes
	IEnumerator IdleCooldown()
	{
		yield return new WaitForSeconds(9.0f);
		canIdle = true;
	}

}
