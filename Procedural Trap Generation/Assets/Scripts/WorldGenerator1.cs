using UnityEngine;
using System.Collections;

public class WorldGenerator1 : MonoBehaviour {

	public Transform GrassTile;
	static int tilesGenerated = 0;
	int maxIterations = 3;
	Vector3 deployAt = new Vector3(5,0,0);
	

	// Use this for initialization
	void Start () {
		generateLand(maxIterations, deployAt);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void generateLand(int iterationsLeft, Vector3 deployAt) {
		//Instantiate (GrassTile, new Vector3(5,0,0), Quaternion.identity);	
		int x = Mathf.Abs(Random.Range(2,8));
		int y = Mathf.Abs(Random.Range(2,8));
		
		print("x = " + x);
		print("y = " + y);
		print("==============================================================");
		
		iterationsLeft -= 1;
		
		for(int k = 0; k < x; k++) {
			deployAt.x += 1;
			//print("deployAt.x = " + deployAt.x);
			
			for(int j = 0; j < y; j++) {
				deployAt.y -= 1;
				//print("deployAt.y = " + deployAt.y);
				
				if( isEmpty(deployAt) ) {
					Instantiate (GrassTile, deployAt, Quaternion.identity);
					tilesGenerated++;
				}
				
				if(iterationsLeft > 0 && k == x-1) {
					generateLand( (iterationsLeft-1) , deployAt);
				}
			}
					
			if(iterationsLeft > 0 && k == x-1) {
				generateLand( (iterationsLeft-1) , deployAt);
			}
			
			deployAt.y += y;
		}
		
		print("Tiles Generated = " + tilesGenerated);
		
	}
	
	public bool isEmpty(Vector3 deployAt) {
		Ray ray = new Ray(deployAt + new Vector3(0,0,-1), new Vector3(0,0,1));
		RaycastHit tileHit;
		Transform hitTile = null;
		
		//print(ray.ToString());
		
		if(Physics.Raycast(ray, out tileHit)) {
			//print("Casting the ray");
			hitTile = tileHit.transform;
		}
		
		if(hitTile == null) {
			return true;
		}
		else {
			return false;
		}
	}
}
