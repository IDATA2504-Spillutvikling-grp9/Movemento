using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour {

	public static ObjectPooling instance;

	public GameObject deathEffect;
	public GameObject spawnedSaw;
    public GameObject portalEffect;
    public GameObject fireBlueEffect;
  

	public int count;

	List<GameObject> DeathEffect = new List<GameObject>();
	List<GameObject> SpawnedSaw = new List<GameObject>();
    List<GameObject> PortalEffect = new List<GameObject>();
    List<GameObject> FireBlueEffect = new List<GameObject>();
   
    void Awake()
	{
		MakeInstance();
	}

	void MakeInstance()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {

		//deathEffect
		for (int i = 0; i < count; i++)
		{
			GameObject obj = Instantiate(deathEffect);
			obj.transform.parent = gameObject.transform;
			obj.SetActive(false);
			DeathEffect.Add(obj);
		}

		//saw
		for (int i = 0; i < count; i++)
		{
			GameObject obj = Instantiate(spawnedSaw);
			obj.transform.parent = gameObject.transform;
			obj.SetActive(false);
			SpawnedSaw.Add(obj);
		}

        //portalEffect
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(portalEffect);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            PortalEffect.Add(obj);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

	//deathEffect
	public GameObject GetDeathEffect()
	{
		for (int i = 0; i < DeathEffect.Count; i++)
		{
			if (!DeathEffect[i].activeInHierarchy)
			{
				return DeathEffect[i];
			}
		}
		GameObject obj = (GameObject)Instantiate(deathEffect);
		obj.transform.parent = gameObject.transform;
		obj.SetActive(false);
		DeathEffect.Add(obj);
		return obj;
	}

	//saw
	public GameObject GetSpawnedSaw()
	{
		for (int i = 0; i < SpawnedSaw.Count; i++)
		{
			if (!SpawnedSaw[i].activeInHierarchy)
			{
				return SpawnedSaw[i];
			}
		}
		GameObject obj = (GameObject)Instantiate(spawnedSaw);
		obj.transform.parent = gameObject.transform;
		obj.SetActive(false);
		SpawnedSaw.Add(obj);
		return obj;
	}

    //portalEffect
    public GameObject GetPortalEffect()
    {
        for (int i = 0; i < PortalEffect.Count; i++)
        {
            if (!PortalEffect[i].activeInHierarchy)
            {
                return PortalEffect[i];
            }
        }
        GameObject obj = (GameObject)Instantiate(portalEffect);
        obj.transform.parent = gameObject.transform;
        obj.SetActive(false);
        PortalEffect.Add(obj);
        return obj;
    }

    //fireBlueEffect
    public GameObject GetFireBlueEffect()
    {
        for (int i = 0; i < FireBlueEffect.Count; i++)
        {
            if (!FireBlueEffect[i].activeInHierarchy)
            {
                return FireBlueEffect[i];
            }
        }
        GameObject obj = (GameObject)Instantiate(fireBlueEffect);
        obj.transform.parent = gameObject.transform;
        obj.SetActive(false);
        FireBlueEffect.Add(obj);
        return obj;
    } 

}
