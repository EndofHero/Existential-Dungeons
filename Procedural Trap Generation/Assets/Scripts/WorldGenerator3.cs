using UnityEngine;
using System.Collections;

public class WorldGenerator3 : MonoBehaviour {

	public Transform GrassTile;
	public Transform DiggableWall;
	static int tilesGenerated = 0;
	Vector3 deployAt = new Vector3(0,0,0);
	
	static int maxX = 6; //normal value is 15
	static int maxY = 6; //same
	
	int[,] worldArray = new int[maxX,maxY];
	
	int grassLineSize = 2;
	int remainingLine = 0;
	int odds = 3;
	
	Room testRoom;
	
	/*
		0 = Diggable Wall
		1 = GrassTile
	*/

	// Use this for initialization
	void Start () {
		prepareLand();
		generateLand(deployAt);
		print("tilesGenerated = " + tilesGenerated);
	}
	
	// Update is called once per frame
	void Update () {
		
		if( Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("l") ) {
			print(testRoom.toString());
			testRoom.saveRoomToFile();
		}
	}
	
	/*
		This method populates an array with values that determine which type of tile will
		be generated upon generation.
	*/
	public void prepareLand() {
		
		for(int k = 0; k < maxX; k++) {
			
			for(int j = 0; j < maxY; j++) {
				
				if(odds == Mathf.RoundToInt(Random.Range(1,odds+1)) || remainingLine > 0) {
					worldArray[k,j] = 1;
					
					if(remainingLine == 0) {
						remainingLine = grassLineSize;
					}
					
					remainingLine--;
				}
			}
		}
		
		testRoom = new Room("test",worldArray);
		
	}
	
	/*
		This method actually generates the land for the game, based on the information in
		the world array.
	*/
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
