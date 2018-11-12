using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamePrototype : MonoBehaviour{

	// Use this for initialization
	public GameObject buttonsContainer;
	public GameObject lightsContainer;
	public Transform pathRoot;//public for debug ///////
	public int roomsCount = 9;
	public int targetRoom = 3;//public for debug ////////
	public int howManyLoops = 10;
	[Range(.1f,3)]public float speed = 0.7f;
	public Transform[] buttons;//public for debug
	public Transform[] lights;//public for debug
	public List<Transform> path1;//public for debug  ////////
	public GameObject motherfucker1;//public for debug ////////

	Transform nextWayPoint;
	int nextWayPointIndex=0;


	RaycastHit hit ;
    Ray ray;
    void Awake () {
		buttons = buttonsContainer.GetComponentsInChildren<Transform>(true);
		lights = lightsContainer.GetComponentsInChildren<Transform>(true);
		hit = new RaycastHit();
	}

	void OnEnable() {
		StartCoroutine(StartGame());
	}

    private IEnumerator StartGame()
    {
		//perpare enter flat
		targetRoom = 3;
		FindPathToTargetRoom();
        //TODO:Enter animation
		yield return new WaitForSeconds(2);
		StartCoroutine(GameLoop());
	}
	private IEnumerator GameLoop()
    {
        //choose random room
        int newtargetRoom;
		
		do{newtargetRoom = UnityEngine.Random.Range(1,9);} 
		while (newtargetRoom == targetRoom);
		targetRoom = newtargetRoom;
		print("<color=green>from "+pathRoot.name+" to "+targetRoom+"</color>");
		path1 = new List<Transform>();
		FindPathToTargetRoom();
		
		if(--howManyLoops == 0){
			StopCoroutine(GameLoop());
			print("** we are done**");
			yield return null;
		}else{
			yield return new WaitForSeconds(10);
			StartCoroutine(GameLoop());
		}
    }
//fix  ~path:_rb | 3 | waypoints | 0 | 3 | 
	void SetPath(Transform current){
		List<Transform> pathFromLeafToNewRoot = new List<Transform>();
		
		pathFromLeafToNewRoot.Add(current);
		
		while(current!=null && current.parent != null && current.parent != pathRoot){
			pathFromLeafToNewRoot.Add(current.parent);
			current = current.parent;
		}
		pathFromLeafToNewRoot.Reverse();
		path1.AddRange(pathFromLeafToNewRoot);
		nextWayPointIndex = 0;
		nextWayPoint = path1[nextWayPointIndex];
		string output = "";for (int X = 0; X < path1.Count; X++){output+=path1[X].name+" | ";}print(" ~path:"+output);
	}

    private void FindPathToTargetRoom(List<Transform> visited = null)
    {
		Queue q = new Queue();
		if(visited==null)
			visited = new List<Transform>();
		q.Enqueue(pathRoot);
		Transform current;
		string output = "";
//		string output = "";for (int X = 0; X < q.Count; X++){output+=q.ToArray()[X]+" | ";}print(" ~q:"+output);
		
		while (q.Count > 0)
		{
			current = (Transform)q.Dequeue();
//			print(" w current_1:"+current);
			if(current == null)continue;
			if(visited.Contains(current))continue;
			visited.Add(current);
			
			if(current.name == targetRoom.ToString()){//found it
				SetPath(current);
				pathRoot = current;
				return;
			}
			for (int i = 0; i < current.childCount; i++){
//				print(" wf child "+current.GetChild(i).name);
				if( ! visited.Contains(current.GetChild(i))){
//					print(" wf +child not visited");
					q.Enqueue(current.GetChild(i));
//					output = "";for (int X = 0; X < q.Count; X++){output+=q.ToArray()[X]+" | ";}print(" wf add>"+output);
				}
//				else print(" wf -child visited !!");
			}
		}
		//print("could not find in children");
		if(pathRoot.parent.name != "0"){
			pathRoot = pathRoot.parent;
			path1.Add(pathRoot);
			FindPathToTargetRoom(visited);
		}
//		output = "";for (int X = 0; X < visited.Count; X++){output+=visited[X].name+" | ";}print(" ~visited:"+output);
					
		return;

    }



    void Update () {
		//buttons cast
		if(Input.GetKeyUp(KeyCode.Mouse0)) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        	Debug.Log("Doing ray test");
        	if(Physics.Raycast(ray, out hit)){
				for (int i = 0; i < buttons.Length; i++)
				{
					if(hit.collider.gameObject == buttons[i].gameObject)
					{
						print("found button");
						lights[i].gameObject.SetActive(!lights[i].gameObject.activeInHierarchy);
					}
				}
			}
		}

		//movement
		if(Vector3.Distance(motherfucker1.transform.position, nextWayPoint.position) < .1 ){
			if(nextWayPointIndex < path1.Count)
				nextWayPoint = path1[nextWayPointIndex++];
		}

		float step = speed * Time.deltaTime;
        // Move our position a step closer to the target.
        motherfucker1.transform.position = Vector3.MoveTowards(motherfucker1.transform.position, nextWayPoint.position, step);

	}
}
