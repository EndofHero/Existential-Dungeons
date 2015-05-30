using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour {
	
	//Transforms
	public Transform grass;
	public Transform wall;
	//Tile Information
	int numberOfTiles = 2;
	int selectedTile = 0;
	Transform[] tiles;
	List<Transform> world = new List<Transform>();
	public Material[] materials;
	Renderer sampleTile;
	string[] tileNames;
	string fileName = "";
	
	// Use this for initialization
	void Start () {
		tileNames = new string[numberOfTiles];
		tileNames[0] = "Wall";
		tileNames[1] = "Grass";
		tiles = new Transform[numberOfTiles];
		tiles[0] = wall;
		tiles[1] = grass;
		
		sampleTile = transform.Find("Sample Tile").GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		bool freeMove = false;
		
		Vector3 direction = Vector3.zero;
		
		//Get Input
		if(Input.GetKeyDown("space")) {
			tryToPlace(tiles[selectedTile]);
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
			previousTile();
		}
		else if(Input.GetKeyDown("e")) {
			nextTile();
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
			transform.position = transform.position += direction;
		}
	}
	
	void OnGUI() {
		Rect brush = new Rect(0, 0, 200, 25);
	   
		GUI.Label(brush, "File Name: ");
	   
		Event e = Event.current;
		bool unfocus = false;
		if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Return) {
				e.Use();
				unfocus = true;
		}
	   
		brush.x += 200;
		GUI.SetNextControlName("fileName");
		fileName = GUI.TextField(brush, fileName);
	   
		brush.y -= 200;
		GUI.SetNextControlName("nofocus");
		GUI.TextField(brush, "");	   
	   
		if (unfocus) {
				GUI.FocusControl("nofocus");
		}
		
		Rect coords = new Rect(0,0,Screen.width,Screen.height);
		GUI.skin.label.alignment = TextAnchor.UpperRight;
		GUI.Label(coords, "(" + transform.position.x + "," + transform.position.y + ")");
	}
	public void create(Transform creation, Vector3 position, Quaternion id) {
		Transform newTile = Instantiate(creation,position,Quaternion.identity) as Transform;
		world.Add(newTile);
	}
	public void nextTile() {
		if(selectedTile + 1 == numberOfTiles) {
			selectedTile = 0;
		}
		else {
			selectedTile++;
		}
		
		sampleTile.sharedMaterial = materials[selectedTile];
	}
	
	public void previousTile() {
		if(selectedTile - 1 == -1) {
			selectedTile = numberOfTiles-1;
		}
		else {
			selectedTile--;
		}
		
		sampleTile.sharedMaterial = materials[selectedTile];
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
		
		create (tile, transform.position + new Vector3(0f,0f,.01f), Quaternion.identity);
	}
	
	public void save() {
		print(fileName);
		//Move cursor to (0,0,0)
		transform.position = new Vector3(0,0,0);
		
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
		
		System.IO.File.WriteAllText("D:\\Existential-Dungeons\\World Builder\\Assets\\Data\\Resources\\" + fileName + ".txt", roomData);
		fileName = "";
	}
	
	public void loadRoom() {
		
		destroyLand();
		
		transform.position = Vector3.zero;
				
		TextAsset roomFile = Resources.Load<TextAsset>(fileName);
		fileName = "";
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
	
	public void destroyLand() {
		foreach (Transform tile in world) {
			Destroy(tile.gameObject);
		}
		world.Clear();
	}
	
	public void generateLand(Room room) {
		
		Vector3 deployAt = new Vector3(0,0,0);
		
		for(int k = 0; k < room.getMaxX(); k++) {
			for(int j = 0; j < room.getMaxY(); j++) {
				
				Vector3 tempVector = deployAt + new Vector3(k,(j*-1),0);
				
				switch(room.getArrayValue(k,j)) {
					case 0:
						create (wall, tempVector, Quaternion.identity);
						break;
					case 1:
						create (grass, tempVector, Quaternion.identity);
						break;
				}
			}
		}
	}
}
