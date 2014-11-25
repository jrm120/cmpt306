using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
	// The player's transform, which will guide the camera
	public Transform player;

	// The margin and smoothing vectors of the camera
	public Vector2
		Margin,
		Smoothing;

	// Determines the camera bounds
	public BoxCollider2D Bounds;

	// The min and max variables for the camera bounds
	private Vector3
		_min,
		_max;

	// Determines if the camera is following the player
	public bool IsFollowing { get; set; }

	// The min and max of the camera bounds
	public void Start()
	{
		_min = Bounds.bounds.min;
		_max = Bounds.bounds.max;
		IsFollowing = true;
	}
	
	public void Update()
	{
		if ( GameObject.FindGameObjectsWithTag("Player").Length != 0){
			if(player == null){
				player = GameObject.FindGameObjectWithTag("Player").transform;
			}
			// The x and y values for the camera's position
			var x = transform.position.x;
			var y = transform.position.y;
			
			if (IsFollowing) 
			{
				// If players position has passed the cameras x margin, soft follow
				if (Mathf.Abs (x - player.position.x) > Margin.x)
					x = Mathf.Lerp (x, player.position.x, Smoothing.x * Time.deltaTime);

				// If players position has passed the cameras y margin, soft follow
				if (Mathf.Abs (y - player.position.y) > Margin.y)
					y = Mathf.Lerp (y, player.position.y, Smoothing.y * Time.deltaTime);
			}

			// Gives the camera half width in units
			var cameraHalfWidth = camera.orthographicSize * ((float)Screen.width / Screen.height);

			// Camera clamping constraint on the X axis
			x = Mathf.Clamp (x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);

			// Camera clamping constraint on the Y axis
			y = Mathf.Clamp (y, _min.y + camera.orthographicSize, _max.y - camera.orthographicSize);

			// The position of the camera is the calculated x and y values
			transform.position = new Vector3 (x, y, transform.position.z);
		}
	}
}