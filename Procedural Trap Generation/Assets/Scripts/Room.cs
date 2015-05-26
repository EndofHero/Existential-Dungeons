using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
	
	string roomName;
	int maxX;
	int maxY;
	int[,] roomArray;
	
	public Room(string name, int x, int y) {
		roomName = name;
		maxX = x;
		maxY = y;
		roomArray = new int[x,y];
	}
	
	public Room(string name, int[,] copyArray) {
		roomName = name;
		roomArray = copyArray;
		maxX = copyArray.GetLength(0);
		maxY = copyArray.GetLength(1);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void saveRoomToFile() {
		System.IO.File.WriteAllText("D:\\Existential-Dungeons\\Procedural Trap Generation\\Assets\\Data\\Resources\\" + roomName + ".txt", this.toString());
	}
	
	public string toString() {
		string temp = (maxX + "," + maxY);
		for(int k = 0; k < maxX; k++) {
			for(int j = 0; j < maxY; j++) {
				temp += "," + roomArray[k,j];
			}
		}
		
		return temp;
	}
}
