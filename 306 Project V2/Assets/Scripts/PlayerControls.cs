using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	
	//skills to upgrade
	public int jumpSkill;
	public int teleportSkill;
	public int attackSkill;
	
	protected Animator anim;
	
	//variables based on skill level
	float jumpForce;
	float damageAmount;
	float teleportDistance;
	float moveSpeed = 0.1f;
	
	bool grounded = false;
	
	bool doubleJump = true;
	
	//Check for ground
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask Ground;
	
	void Start () {
		//start position
		transform.position = new Vector3(-6.5f, -2.25f, 0);
		transform.rotation = new Quaternion(0, 0, 0, 0);
		anim = GetComponent<Animator>();
	}
	
	// Skills are updated as they increase
	void Update()
	{
		jumpForce = 100 * jumpSkill + 750;
		damageAmount = 10 + 2 * attackSkill;
	}
	
	void FixedUpdate () {
		//set vertical speed
		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);
		//check for ground. Set grounded
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, Ground);
		anim.SetBool ("Ground", grounded);
		//check if moving
		float move = Input.GetAxis ("Horizontal");
		if((move>0)&&(transform.position.x<70)){
			//walk right
			anim.SetBool("Walk", true);
			transform.position = new Vector2(transform.position.x + moveSpeed, transform.position.y);
			transform.rotation = new Quaternion(0,0,0,0);
		}else if((move<0)&&(transform.position.x>-7)){
			//walk left
			anim.SetBool("Walk", true);
			transform.position = new Vector2(transform.position.x - moveSpeed, transform.position.y);
			transform.rotation = new Quaternion(0,180,0,0);
		}else{
			//stop walking
			anim.SetBool("Walk", false);
		}
		
		//Jump
		if((Input.GetButtonDown("Jump"))&&(grounded)){
			anim.SetBool("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
			doubleJump = true;
		}
		
		//Double Jump
		if ((Input.GetButtonDown ("Jump")) && (!grounded) && (doubleJump)) {
			rigidbody2D.AddForce (new Vector2(0, jumpForce/1.5f));
			doubleJump = false;
		}
		
		//Attack
		if((Input.GetButtonDown("Fire1"))&&(grounded)){
			anim.SetTrigger("Attack");
		}
		
		ignoreCollisions ();
		
		//Bottom of screen
		if(transform.position.y < -7){
			//restart
			transform.position = new Vector3(-6.5f, -2.25f, 0);
			transform.rotation = new Quaternion(0, 0, 0, 0);
		}
	}
	
	void ignoreCollisions(){
		//ignore collisions if player is jumping
		Physics2D.IgnoreLayerCollision( LayerMask.NameToLayer("Player"), 
		                               LayerMask.NameToLayer("Ground"), rigidbody2D.velocity.y > 0 );
		//ignore attack collision with ground
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Attack"), LayerMask.NameToLayer ("Ground"), true);
		//Ignore attack collision with enemy unless attacking
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Attack"), LayerMask.NameToLayer ("Enemy"), anim.GetBool("Attack") == false);
	}
	
	//return damageAmount
	public float getDamageAmount(){
		return damageAmount;
	}
}