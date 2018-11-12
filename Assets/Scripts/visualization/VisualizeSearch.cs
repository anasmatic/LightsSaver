using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeSearch : MonoBehaviour {

	public GameObject root;
	public GameObject end;
	void OnEnable () {
		/* for (int i = 0; i < root.transform.childCount; i++)
		{
			Renderer renderer= root.transform.GetChild(i).gameObject.GetComponent<Renderer>();
			//renderer.material.shader = Shader.Find("_Color");
			renderer.material.SetColor("_Color", Color.green);
		}*/
		Renderer renderer= root.transform.gameObject.GetComponent<Renderer>();
		renderer.material.SetColor("_Color", Color.green);
		renderer = end.transform.gameObject.GetComponent<Renderer>();
		renderer.material.SetColor("_Color", Color.green);
		GetComponent<SearchTree>().FindPath(root.transform,end.transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
