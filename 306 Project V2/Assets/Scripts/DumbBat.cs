using UnityEngine;
using System.Collections;

public class DumbBat : MonoBehaviour {
	
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
	
	//enemy health
	public int Health;
	
	public int expWorth = 25;
	
	void Start () {
		//get animator
		anim = GetComponent<Animator>();
		//update health
		anim.SetInteger ("Health", Health);
		//Start position
		transform.position = new Vector3(startX, startY, 0);
		transform.rotation = new Quaternion (0, 0, 0, 0);
	}
	
	void Update () {
		//if position is invalid, turn around and move in other direction
		if (transform.position.x < minX){
			transform.position = new Vector2(minX, transform.position.y);
			transform.rotation = new Quaternion(0, 0, 0, 0);
			xSpeed*= -1;
		}else if (maxX < transform.position.x){
			transform.position = new Vector2(maxX, transform.position.y);
			transform.rotation = new Quaternion(0, 180, 0, 0);
			xSpeed*= -1;
		}
		
		//update velocity
		rigidbody2D.velocity = new Vector2(xSpeed, 0);
	}
	
	//when damaged
	void ApplyDamage(int d){
		Health -= d;
		//update health
		anim.SetInteger ("Health", Health);
		//die
		if(Health <1){
			xSpeed = 0;
			rigidbody2D.velocity = new Vector2(0, 0);
			//destroy enemy after die animation
			Destroy(gameObject, .6f);
			//gameManager.curEXP = gameManager.curEXP + expWorth;
		}
	}
}

