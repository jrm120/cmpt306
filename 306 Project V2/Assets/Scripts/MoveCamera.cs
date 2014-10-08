using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
	public Transform player;

	void Start () {
		//start position
		transform.position = new Vector3(0, 0, -10);
	}

	void Update () {
		//move camera with player
		transform.position = new Vector3(player.position.x +6.5f, player.position.y +2.25f, -10);
	}
}
