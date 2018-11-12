using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchTree : MonoBehaviour {

	Transform nextWayPoint;
	int nextWayPointIndex=0;
	public Transform pathRoot;
	public int targetRoom = 3;
	public List<Transform> path1;

	Transform from, to;
	public void FindPath(Transform _from, Transform _to){
		this.from = pathRoot = _from;
		this.to = _to;
		targetRoom = System.Int32.Parse(_to.name);
		StartCoroutine( FindPathToTargetRoom() ); 
	}

	private IEnumerator FindPathToTargetRoom(List<Transform> visited = null)
    {
		Queue q = new Queue();
		if(visited==null)
			visited = new List<Transform>();
		List<Transform> steps = new List<Transform>();
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
			//debug--
			ColorNode(current, Color.yellow);
			steps.Add(current);
			yield return new WaitForSeconds(.5f);
			//-------
			if(current.name == targetRoom.ToString()){//found it
				StartCoroutine( SetPath(current) );
				pathRoot = current;
				string _output = "";for (int N = 0; N < steps.Count; N++){_output+=steps[N].name+" | ";}print(" ~path:"+_output);
				StopCoroutine( FindPathToTargetRoom() );
				break;
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
	}

    private void ColorNode(Transform current, Color color)
    {
		if(current == pathRoot) return;
		if(current.name == targetRoom.ToString()) return;
        
		Renderer renderer= current.gameObject.GetComponent<Renderer>();
		renderer.material.SetColor("_Color", color);
    }

    IEnumerator SetPath(Transform current){
		List<Transform> pathFromLeafToNewRoot = new List<Transform>();
		
		pathFromLeafToNewRoot.Add(current);
		
		while(current!=null && current.parent != null && current.parent != from){
			ColorNode(current.parent, Color.blue);//todo, it is better to loop and color the result array rather than color here
			pathFromLeafToNewRoot.Add(current.parent);
			current = current.parent;
			yield return new WaitForSeconds(.5f);
		}
		pathFromLeafToNewRoot.Reverse();
		path1.AddRange(pathFromLeafToNewRoot);
		nextWayPointIndex = 0;
		nextWayPoint = path1[nextWayPointIndex];
		string output = "";for (int X = 0; X < path1.Count; X++){output+=path1[X].name+" | ";}print(" ~path:"+output);
		
	}
}
