using UnityEngine;
using System.Collections;

public class TileProperties : MonoBehaviour {

	public bool isTraversable = false;
	public bool isDiggable = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool getIsTraversable() {
		return isTraversable;
	}
}
