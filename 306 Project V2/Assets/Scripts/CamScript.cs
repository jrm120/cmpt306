using UnityEngine;
using System.Collections;

public class CamScript : MonoBehaviour {

	// The player's transform, which will guide the camera
		public Transform player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( GameObject.FindGameObjectsWithTag("Player").Length != 0){
			if(player == null){
				player = GameObject.FindGameObjectWithTag("Player").transform;
			}

			float x = player.position.x + 10;
			float y = player.position.y + 1;
			float z = player.position.z - 10;
			if(20.5f> x){
				x = 20.5f;
			}else if(390.5 < x){
				x = 390.5f;
			}else if (y < - 2){
				y = -2;
			}
			transform.position = new Vector3(x,y,z);
		}
	}
}
