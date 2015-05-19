using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space")) {
			print("Hey, listen!");
		}
		else if(Input.GetKeyDown("a")) {
			transform.position = transform.position + new Vector3(-1,0,0);
		}
		else if(Input.GetKeyDown("s")) {
			transform.position = transform.position + new Vector3(0,-1,0);
		}
		else if(Input.GetKeyDown("d")) {
			transform.position = transform.position + new Vector3(1,0,0);
		}
		else if(Input.GetKeyDown("w")) {
			transform.position = transform.position + new Vector3(0,1,0);
		}
	}
}
