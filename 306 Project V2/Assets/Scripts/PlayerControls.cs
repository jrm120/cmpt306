using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public SpawnBullet spawnBullet;
	public MeleeAttack meleeAttack;

	//skills to upgrade
	public static int jumpSkill;
	public static int teleportSkill;
	public static int meleeSkill;
	public static int rangedSkill;
	public static int energySkill;
	public static int energyRegenSkill;
	public static int healthRegen;
	public static int healthSkill;
	public static int shieldSkill;
	public static float moveSpeed = .15f;

	// Amount of EXP the player currently has
	public int curEXP = 0;
	
	// Amount of EXP needed to level up
	public int maxEXP = 50;
	
	// Players level
	public int level = 1;
	
	// Players skill points
	public int skillPoints = 0;

	protected Animator anim;
	
	//variables based on skill level
	float jumpForce;
	float meleeDmg;
	float rangedDmg;
	float teleportDistance;

	public float Health;
	public float healthCap;
	public float Energy;
	public float energyRegen;
	public float energyCap;
	public float shield;
	public float shieldCap;

	public int Coins;
	public int Gems;

	static int GEM_COUNT = 0;
	static int COIN_COUNT = 0;

	// boolean checks
	bool dead = false;
	bool grounded = false;
	bool doubleJump = true;
	bool isFacingRight = true;
	bool canMelee = true;

	//Check for ground
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask Ground;

	public BoxCollider2D faceLeftTrigBox;
	public BoxCollider2D faceRightTrigBox;
	public CircleCollider2D faceLeftGround;
	public CircleCollider2D faceRightGround;

	void Awake()
	{
		Gems = GEM_COUNT;
		Coins = COIN_COUNT;
	}
	
	void Start () 
	{
		//start position
		//transform.position = new Vector3(-6.5f, -2.25f, 0);
		//transform.rotation = new Quaternion(0, 0, 0, 0);
		anim = GetComponent<Animator>();
		// Regenerate energy quickly
		InvokeRepeating("regenerateEnergy", 0, 0.25f);
		// Regenerate health very slowly
		InvokeRepeating("regenerateHealth", 0, 2.5f);
	}
	
	// Skills are updated as they increase
	void Update()
	{
		GEM_COUNT = Gems;
		COIN_COUNT = Coins;
		jumpForce = 100 * jumpSkill + 750;
		meleeDmg = 10 + 2 * meleeSkill;
		rangedDmg = 10 + 2 * rangedSkill;
		energyRegen = 1 + energyRegenSkill;
		healthRegen = 1;
		energyCap = 100 + energySkill;
		Debug.Log (healthSkill);
		healthCap = 100 + healthSkill;
		shieldCap = 100 + shieldSkill;
		teleportDistance = 5 + teleportSkill;

		if(isFacingRight)
		{
			faceRightGround.enabled = true;
			faceRightTrigBox.enabled = true;
			faceLeftGround.enabled = false;
			faceLeftTrigBox.enabled = false;
		}
		else
		{
			faceRightGround.enabled = false;
			faceRightTrigBox.enabled = false;
			faceLeftGround.enabled = true;
			faceLeftTrigBox.enabled = true;
		}

		// Ensure the shield does not exceed the capacity
		if (shield > shieldCap)
		{
			shield = shieldCap;
		}
		// Ensure the shield does not fall below zero
		if (shield < 0)
		{
			shield = 0;
		}

		if (curEXP >= maxEXP)
		{
			Debug.Log ("Level up!");
			LevelUp ();
		}

		// Shoot a bullet if player has enough energy
		if ((Input.GetKeyDown (KeyCode.F) || Input.GetButtonDown("RangedAttack")) && Energy >= 15)
		{
			spawnBullet.Shoot(isFacingRight);
			Energy = Energy - 15;
		}

		// Attack with Melee
		if (Input.GetButtonDown("Fire1")&&(grounded)&&(canMelee))
		{
			canMelee = false;
			meleeAttack.Melee(isFacingRight);
			StartCoroutine(Cooldown());
		}

		// Teleport if the player has enough energy
		if ((Input.GetKeyDown (KeyCode.B) || Input.GetButtonDown ("Teleport")) && Energy >= 20)
		{
			if(isFacingRight)
			{
				transform.position = new Vector3(transform.position.x + teleportDistance, transform.position.y, 0);
			}
			else
			{
				transform.position = new Vector3(transform.position.x - teleportDistance, transform.position.y, 0);
			}
			Energy -= 20;
		}
	}

	// Regenerate energy if it is less than the energy capacity
	// Will not go over the capacity
	void regenerateEnergy()
	{
		if (Energy < energyCap)
		{
			if((Energy + energyRegen) > energyCap)
			{
				Energy = energyCap;
			}
			else
			{
				Energy = Energy + energyRegen;
			}
		}
	}

	// Regenerate health if it is less than the health capacity
	// Will not go over the capacity

	void regenerateHealth()
	{
		if (Health < healthCap)
		{
			if((Health + healthRegen) > healthCap)
			{
				Health = healthCap;
			}
			else
			{
				Health = Health + healthRegen;
			}
		}
	}
	
	void FixedUpdate () {
		//set vertical speed
		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);
		//check for ground. Set grounded
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, Ground);
		anim.SetBool ("Ground", grounded);
		//set health
		anim.SetFloat("Health", Health);
		//check if moving
		float move = Input.GetAxis ("Horizontal");
		// If the player is grounded, the double jump ability is available
		if (grounded)
		{
			doubleJump = true;
		}
		if(move>0){
			//walk right
			isFacingRight = true;
			anim.SetBool("Walk", true);
			transform.position = new Vector2(transform.position.x + moveSpeed, transform.position.y);
			transform.rotation = new Quaternion(0,0,0,0);
		}else if(move<0){
			//walk left
			isFacingRight = false;
			anim.SetBool("Walk", true);
			transform.position = new Vector2(transform.position.x - moveSpeed, transform.position.y);
			transform.rotation = new Quaternion(0,180,0,0);
		}else{
			//stop walking
			anim.SetBool("Walk", false);
		}
		
		//Jump
		if((Input.GetButtonDown("Jump") &&(grounded))){
			anim.SetBool("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
		}
		
		//Double Jump
		if ((Input.GetButtonDown ("Jump")) && (!grounded) && (doubleJump) && Energy >= 10) {
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce/50f);
			Energy-=10;
			doubleJump = false;
		}
		
		//Attack
		if((Input.GetButtonDown("Fire1"))&&(grounded)&&(canMelee)){
			anim.SetTrigger("Attack");
		}

		//Shoot projectile
		if ((Input.GetKeyDown (KeyCode.F) || Input.GetButtonDown("RangedAttack")) && Energy >= 15)
		{
			anim.SetTrigger ("RangedAttack");
		}

		ignoreCollisions ();
		
		//Bottom of screen
		if(transform.position.y < -7){
			//Die
			Health = 0;
		}

		//TESTING ONLY
		//Lower Health
		if((Input.GetKeyDown(";"))&&(Health>0)){
			ApplyDamage(10);
		}
		//Lower Energy
		if((Input.GetKeyDown("'"))&&(Energy>0)){
			Energy -= 10;
		}
		//Add Coins
		if(Input.GetKeyDown(".")){
			Coins += 10;
		}
		//Add Gems
		if(Input.GetKeyDown("/")){
			Gems += 10;
		}
		//Increase shield
		if (Input.GetKeyDown ("1")) {
			shield += 10;
		}

		//Lower Shield
		if (Input.GetKeyDown("2")){
			shield -= 10;
		}
		//END TESTING

		//die
		if((Health <1) && (!dead)){
			dead = true;
			//anim.SetBool ("Walk", false);
			//rigidbody2D.velocity = new Vector2(0, 0);
			//destroy after die animation
			Destroy(gameObject, .4f);
		}
	}
	
	void ignoreCollisions(){

		//ignore collisions with ground if player is jumping

		Physics2D.IgnoreLayerCollision( LayerMask.NameToLayer("Player"), 
		                               LayerMask.NameToLayer("Ground"), rigidbody2D.velocity.y > 0 );

		//ignore attack collision with ground
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Combat"), LayerMask.NameToLayer ("Ground"), true);

	}

	// The cooldown before being able to melee attack again
	IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(0.5f);
		canMelee = true;
	}

	//return meleeDmg
	public float getMeleeDmg(){
		return meleeDmg;
	}

	//return meleeDmg
	public float getRangedDmg(){
		return rangedDmg;
	}

	//return health
	public float getHealth(){
		return Health;
	}
	
	//return coins
	public int getCoins(){
		return Coins;
	}
	
	//return gems
	public int getGems(){
		return Gems;
	}
	
	//return energy
	public float getEnergy(){
		return Energy;
	}

	//return shield
	public float getShield(){
		return shield;
	}
	
	//when damaged, update health
	//If shield is available, update shield before health
	void ApplyDamage(float d){
		if (shield > 0)
		{
			if(shield > d)
			{
				shield -= d;
				if (shield < 0)
				{
					shield = 0;
				}
			}
			else
			{
				d -= shield;
				shield = 0;
				Health -= d;
			}
		}
		else
		{
			Health -= d;
		}
		if (Health < 0)
		{
			Health = 0;
		}
	}

	// Levels up the character when a certain amount of EXP is obtained
	void LevelUp()
	{
		Debug.Log ("Level up!");
		curEXP = 0;
		maxEXP = maxEXP + 50;
		level++;
	}

	// Increase health capacity
	public void IncreaseHealth()
	{
		if (Gems > 0)
		{
			healthSkill += 10;
			Gems--;
		}
	}

	// Increase energy capacity
	public void IncreaseEnergy()
	{
		if (Gems > 0)
		{
			energySkill += 10;
			Gems--;
		}
	}

	// Increase shield capacity
	public void IncreaseShield()
	{
		if (Gems > 0)
		{
			shieldSkill += 10;
			Gems--;
		}
	}

	// Increase melee damage
	public void IncreaseMeleeDamage()
	{
		if (Gems > 0)
		{
			meleeSkill += 1;
			Gems--;
		}
	}

	// Increase ranged damage
	public void IncreaseRangedDamage()
	{
		if (Gems > 0)
		{
			rangedSkill += 1;
			Gems--;
		}
	}

	// Increase jump skill
	public void IncreaseJump()
	{
		if (Gems > 0)
		{
			jumpSkill += 1;
			Gems--;
		}
	}

	// Increase teleport skill
	public void IncreaseTeleport()
	{
		if (Gems > 0)
		{
			teleportSkill+= 1;
			Gems--;
		}
	}

	// Add shield (shop)
	public void AddShield()
	{
		if (Coins >= 10)
		{
			shield+=20;
			Coins-=10;
		}
	}

	public void AddHealth ()
	{
		if (Coins >= 10)
		{
			Health += 15;
			Coins -= 10;
		}
	}

	public void AddEnergy ()
	{
		if (Coins >= 20)
		{
			Energy += 10;
			Coins -= 20;
		}
	}



	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Gem")
		{
			Gems += 1;
			other.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			other.gameObject.GetComponent<PolygonCollider2D> ().enabled = false;
			other.gameObject.audio.Play();
			Destroy (other.gameObject,2);
		}
	}
}