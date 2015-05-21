using UnityEngine;
using System.Collections;

public class WorldGenerator2 : MonoBehaviour {

	public Transform GrassTile;
	public Transform DiggableWall;
	static int tilesGenerated = 0;
	Vector3 deployAt = new Vector3(5,0,0);
	
	static int maxX = 15;
	static int maxY = 15;
	int maxIterations = maxX/2;
	
	int[,] worldArray = new int[maxX,maxY];
	
	/*
		0 = Diggable Wall
		1 = GrassTile
	*/

	// Use this for initialization
	void Start () {
		prepareLand(maxIterations, 0, 0);
		generateLand(deployAt);
		print("tilesGenerated = " + tilesGenerated);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void prepareLand(int iterationsLeft, int newStartingX, int newStartingY) {
		//Instantiate (GrassTile, new Vector3(5,0,0), Quaternion.identity);	
		int minRoomX = 2;
		int maxRoomX = 4;
		int minRoomY = 2;
		int maxRoomY = 4;
		
		int x = (int) Mathf.Round(Random.Range(minRoomX,maxRoomX));
		int y = (int) Mathf.Round(Random.Range(minRoomX,maxRoomX));
		
		int randX = (int) Mathf.Abs(Random.Range(0, x-1));
		int randY = (int) Mathf.Abs(Random.Range(0, y-1));
		
		print("newStartingX = " + newStartingX);
		print("x = " + x);
		print("x + newStartingX = " + x + newStartingX);
		print("newStartingY = " + newStartingY);
		print("y = " + y);
		print("y + newStartingY = " + y + newStartingY);
		print("iterationsLeft = " + iterationsLeft);
		print("==============================================================");
		
		iterationsLeft -= 1;
		
		for(int k = 0; k < x && k+newStartingX < maxX; k++) {
			
			for(int j = 0; j < y && j+newStartingY < maxY; j++) {
				
				worldArray[k + newStartingX,j + newStartingY] = 1;
				tilesGenerated++;
				
				if(iterationsLeft > 0 && j == y-1 && k == randX) {
					prepareLand(iterationsLeft,k + newStartingX,j + newStartingY);
				}
			}
			
			if(iterationsLeft > 0 && k == x-1) {
				prepareLand(iterationsLeft,k + newStartingX,y-1 + newStartingY);
			}
		}
	}
	
	public void generateLand(Vector3 deployAt) {
		
		print("First dA = " + deployAt);
		
		for(int k = 0; k < maxX; k++) {
			for(int j = 0; j < maxY; j++) {
				
				Vector3 tempVector = deployAt + new Vector3(k,(j*-1),0);
				
				switch(worldArray[k,j]) {
					case 0:
						Instantiate (DiggableWall, tempVector, Quaternion.identity);
						break;
					case 1:
						Instantiate (GrassTile, tempVector, Quaternion.identity);
						break;
				}
			}
		}
	}
}
