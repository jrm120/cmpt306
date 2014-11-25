using UnityEngine;
using System.Collections;

public class RangedEnemy: MonoBehaviour {
	public SpawnBullet spawnBullet;

	// The archer's health
	public int Health = 50;
	
	//How much experience the archer is worth
	public int expWorth = 50;
	
	// The distance between the archer and the player
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
	bool canShoot;

	void Start () {
		//get animator
		anim = GetComponent<Animator>();
		//start off walking
		anim.SetBool ("Walk", true);
		//update health
		anim.SetInteger ("Health", Health);
		//Start position
		transform.position = new Vector3(startX, startY, 0);
		transform.rotation = new Quaternion (0, 0, 0, 0);
		//Set initial bools
		isFacingRight = true;
		isIdle = false;
		canIdle = true;
		canShoot = true;
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
			Destroy(gameObject, 0.6f);
		}
	}
	
	//Enemy decision tree
	delegate void MyDelegate();
	MyDelegate enemyAction;
	
	void Update(){
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
		if(distance < 18)
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
		if(distance < 15)
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
			if (transform.position.x < minX)
			{
				isFacingRight = true;
				transform.position = new Vector2 (minX, transform.position.y);
				transform.rotation = new Quaternion (0, 0, 0, 0);
				xSpeed *= -1;
			}
			else if (maxX < transform.position.x)
			{
				isFacingRight = false;
				transform.position = new Vector2 (maxX, transform.position.y);
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
		if (canShoot)
		{
			Shoot();
		}
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
		else
		{
			//If player is to the left, advance left
			transform.rotation = new Quaternion(0, 180, 0, 0);
			isFacingRight = false;
			if(!attacking){
				xSpeed = -3.5f;
			}
		}
		enemyAction = TargetVisible;
	}
	//SHOOT
	private void Shoot()
	{
		spawnBullet.Shoot (isFacingRight);
		canShoot = false;
		StartCoroutine (ShootCooldown ());
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

	//Unable to shoot again until cooldown finishes
	IEnumerator ShootCooldown()
	{
		yield return new WaitForSeconds(0.95f);
		canShoot = true;
	}
	
}