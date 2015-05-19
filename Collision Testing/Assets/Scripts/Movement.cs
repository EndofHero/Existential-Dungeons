using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = Vector3.zero;
		
		if(Input.GetKeyDown("space")) {
			print("Hey, listen!");
		}
		else if(Input.GetKeyDown("a")) {
			direction = new Vector3(-1,0,0);
		}
		else if(Input.GetKeyDown("s")) {
			direction = new Vector3(0,-1,0);
		}
		else if(Input.GetKeyDown("d")) {
			direction = new Vector3(1,0,0);
		}
		else if(Input.GetKeyDown("w")) {
			direction = new Vector3(0,1,0);
		}
		
		if(direction != Vector3.zero) {
			tryMove(direction);
		}	
	}
	
	void tryMove(Vector3 direction) {
		
		//Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(transform.position + direction);
		Ray ray = new Ray(transform.position + direction + new Vector3(0,0,-1), new Vector3(0,0,1));
		RaycastHit tileHit;
		Transform hitTile = null;
		
		print(ray.ToString());
		
		if(Physics.Raycast(ray, out tileHit)) {
			print("Casting the ray");
			hitTile = tileHit.transform;
		}
		
		if(hitTile != null) {
			print("hitTile is not null");
			print(hitTile.ToString());
			if(hitTile.GetComponent<TileProperties>().isTraversable) {
				print("hitTile is traversable");
				transform.position = transform.position += direction;
			}
		}
	}
}
