using UnityEngine;
using System.Collections;

	
public class DestroyFinishedParticles : MonoBehaviour {

	private ParticleSystem thisParticleSystem;

	
	void Start () {
		thisParticleSystem = GetComponent<ParticleSystem> ();
	}
	// Update is called once per frame
	void Update () {
	if (thisParticleSystem.isPlaying)
		return;
		Destroy (gameObject);
	}

	void OnBecameInvisible()
	{

		Destroy (gameObject);
	}
}

