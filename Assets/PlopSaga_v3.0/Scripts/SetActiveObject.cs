using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		 
			StartCoroutine ("SetGameObject");
		 
		
	}

	IEnumerator SetGameObject(){

		yield return new WaitForSeconds (2);

		gameObject.SetActive (false);
	}
}
