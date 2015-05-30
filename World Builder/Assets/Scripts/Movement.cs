using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	public Transform grass;
	public Transform wall;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		bool freeMove = false;
		
		Vector3 direction = Vector3.zero;
		
		//Get Input
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
		else if(Input.GetKeyDown("q")) {
			tryToPlace(wall);
		}
		else if(Input.GetKeyDown("e")) {
			tryToPlace(grass);
		}
		else if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("h")) {
			save();
		}
		else if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("l")) {
			loadRoom();
		}
		
		//Try to move
		if(direction != Vector3.zero && !freeMove) {
			move(direction);
		}
		else {
			//print("Trying to freemove");
			transform.position = transform.position += direction;
		}
	}
	
	void move(Vector3 direction) {
		transform.position = transform.position += direction;
	}
	
	public void delete(GameObject hitTile) {
		if(hitTile != null) {
			Destroy(hitTile);
			print("Deleted: " + hitTile);
		}
	}
	
	public void tryToPlace(Transform tile) {
		//Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(transform.position + direction);
		Ray ray = new Ray(transform.position + new Vector3(0,0,-1), new Vector3(0,0,1));
		RaycastHit tileHit;
		GameObject hitTile = null;
		
		//print(ray.ToString());
		
		if(Physics.Raycast(ray, out tileHit)) {
			//print("Casting the ray");
			hitTile = tileHit.transform.gameObject;
		}
		
		delete(hitTile);
		
		Instantiate (tile, transform.position + new Vector3(0f,0f,.01f), Quaternion.identity);
	}
	
	public void save() {
		
		string roomData = "";
		int x = 0;
		int y = 0;
		
		Ray ray;
		RaycastHit tileHit;
		Transform hitTile = null;
		
		Vector3 position = transform.position;
		
		do {
			
			do {
				hitTile = null;
				
				ray = new Ray(position + new Vector3(0,0,-1), new Vector3(0,0,1));
				
				if(Physics.Raycast(ray, out tileHit)) {
					hitTile = tileHit.transform;
					
					position.y--; //Minus because we usually build from left to right, top to bottom,
								  //Which puts us into negative y values
					
					if(x == 0) {
						y++;
					}
					
					roomData += "," + hitTile.GetComponent<TileProperties>().tileNumber;
				}
			} while(hitTile != null);
			
			position.y = transform.position.y;
			
			ray = new Ray(transform.position + new Vector3(x,0,0) + new Vector3(0,0,-1), new Vector3(0,0,1));
				
			if(Physics.Raycast(ray, out tileHit)) {
				hitTile = tileHit.transform;
				
				position.x++;
				x++;
			}
			
			
		} while(hitTile != null);//iterating through x
		
		roomData = x + "," + y + roomData;
		
		print("x = " + x);
		print("y = " + y);
		print("roomData = " + roomData);
		print("=========================================");
		
		//Save the room to a file
		System.IO.File.WriteAllText("D:\\Existential-Dungeons\\Procedural Trap Generation\\Assets\\Data\\Resources\\test2" + ".txt", roomData);
	}
	
	public void loadRoom() {
		TextAsset roomFile = Resources.Load<TextAsset>("test");
		Room room;
		string[] values = roomFile.text.Split(',');
		
		int maxX = int.Parse(values[0]);
		int maxY = int.Parse(values[1]);
		
		int tileIndex = 2;
		
		room = new Room("test", maxX, maxY);
		
		for(int k = 0; k < room.getMaxX(); k++) {
			for(int j = 0; j < room.getMaxY(); j++) {
				room.setArrayValue(int.Parse(values[tileIndex]),k,j);
				tileIndex++;
			}
		}
		
		print(room.toString());
		generateLand(room);
	}
	
	public void generateLand(Room room) {
		
		Vector3 deployAt = new Vector3(0,0,0);
		
		for(int k = 0; k < room.getMaxX(); k++) {
			for(int j = 0; j < room.getMaxY(); j++) {
				
				Vector3 tempVector = deployAt + new Vector3(k,(j*-1),0);
				
				switch(room.getArrayValue(k,j)) {
					case 0:
						Instantiate (wall, tempVector, Quaternion.identity);
						break;
					case 1:
						Instantiate (grass, tempVector, Quaternion.identity);
						break;
				}
			}
		}
	}
}
