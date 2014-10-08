using UnityEngine;
using System.Collections;

public class MovePlatform : MonoBehaviour {
	//vertical and horizontal speeds
	public float xSpeed;
	public float ySpeed;
	//min and max positions
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(14, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		//if position is invalid, move in other direction
		if (transform.position.x < minX){
			transform.position = new Vector3(minX, transform.position.y, transform.position.z);
			xSpeed*= -1;
		}else if (maxX < transform.position.x){
			transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
			xSpeed*= -1;
		}

		if (transform.position.y < minY){
			transform.position = new Vector3(transform.position.x, minY, transform.position.z);
			ySpeed*= -1;
		}else if (maxY < transform.position.y){
			transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
			ySpeed*= -1;
		}
		//update velocity
		rigidbody2D.velocity = new Vector2(xSpeed, ySpeed);
	}
}
