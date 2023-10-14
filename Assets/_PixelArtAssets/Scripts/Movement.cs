using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
	onlyLeft,
	onlyRight,
	onlyUp,
	onlyDown
}

public class Movement : MonoBehaviour {

	public MovementType movement;

	[HideInInspector]
	public float currentValue = 0; // or wherever you want to start
	public float speed;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		MovementSelect();
	}

	void MovementSelect()
	{
		switch (movement)
		{ 
		case MovementType.onlyLeft:
			MoveOnlyLeft();
			break;

		case MovementType.onlyRight:
			MoveOnlyRight();
			break;
		case MovementType.onlyUp:
			MoveOnlyUp();
			break;

		case MovementType.onlyDown:
			MoveOnlyDown();
			break;
		}
	}

	void MoveOnlyLeft()
	{
		currentValue += Time.deltaTime * -1 * speed;
		transform.position = new Vector3(currentValue, transform.position.y, transform.position.z);
	}

	void MoveOnlyRight()
	{
		currentValue += Time.deltaTime * 1 * speed;
		transform.position = new Vector3(currentValue, transform.position.y, transform.position.z);
	}

	void MoveOnlyUp()
	{
		currentValue += Time.deltaTime * 1 * speed;
		transform.position = new Vector3(transform.position.x, currentValue, transform.position.z);
	}

	void MoveOnlyDown()
	{
		currentValue += Time.deltaTime * -1 * speed;
		transform.position = new Vector3(transform.position.x, currentValue, transform.position.z);
	}

}
