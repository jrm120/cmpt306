using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
	public Transform player;
	
	public Vector2
		Margin,
		Smoothing;
	
	public BoxCollider2D Bounds;
	
	private Vector3
		_min,
		_max;
	
	public bool IsFollowing { get; set; }
	
	public void Start()
	{
		_min = Bounds.bounds.min;
		_max = Bounds.bounds.max;
		IsFollowing = true;
	}
	
	public void Update()
	{
		var x = transform.position.x;
		var y = transform.position.y;
		
		if (IsFollowing) 
		{
			if (Mathf.Abs (x - player.position.x) > Margin.x)
				x = Mathf.Lerp (x, player.position.x, Smoothing.x * Time.deltaTime);
			
			if (Mathf.Abs (y - player.position.y) > Margin.y)
				y = Mathf.Lerp (y, player.position.y, Smoothing.y * Time.deltaTime);
		}
		
		var cameraHalfWidth = camera.orthographicSize * ((float)Screen.width / Screen.height);
		
		x = Mathf.Clamp (x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
		y = Mathf.Clamp (y, _min.y + camera.orthographicSize, _max.y - camera.orthographicSize);
		
		transform.position = new Vector3 (x, y, transform.position.z);
	}
}