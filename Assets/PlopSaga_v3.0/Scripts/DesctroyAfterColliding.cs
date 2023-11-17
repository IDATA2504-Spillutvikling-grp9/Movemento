using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesctroyAfterColliding : MonoBehaviour {
	 
	[SerializeField]
	public GameObject deathEffect;
	public string collideTag = "Ground";
    public string collideTag2 = "MovePlatform";

    // Use this for initialization
    void Start () {
		 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
	
		if (other.tag == collideTag || other .tag ==collideTag2)
		{
			
			GameObject effect = ObjectPooling.instance.GetDeathEffect();
			effect.SetActive(true); 
			effect.transform.position = transform.position; 
			  
			gameObject.SetActive(false);
			 
		}
	}
 
}
