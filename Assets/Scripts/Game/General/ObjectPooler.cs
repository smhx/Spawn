//the next 78 lines of code were taken from this website: https://www.raywenderlich.com/136091/object-pooling-unity

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Random = System.Random;

[System.Serializable]
public class ObjectPoolItem {
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand; //if true, it will instantiate a new object when all pooled items are active
	public Transform parent = null;
}

public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler SharedInstance;
    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjects;

    void OnEnable() {
		if (SharedInstance == null){
			SharedInstance = this;
		} else {
			Destroy (this.gameObject);
		}
    }
    
    void Start() {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool) {
            for (int i = 0; i < item.amountToPool; i++) {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
				if (item.parent!=null) {
					obj.transform.parent = item.parent;
				}
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

	public void DeactivateAll(string tag) {
		for (int i = 0; i < pooledObjects.Count; i++) {
			if (pooledObjects [i].activeInHierarchy && pooledObjects [i].tag == tag)
				pooledObjects [i].SetActive (false);
		}
	}

    public GameObject GetPooledObject(string tag) {
        for (int i = 0; i < pooledObjects.Count; i++) {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
                return pooledObjects[i];
        }
        foreach (ObjectPoolItem item in itemsToPool) {
            if (item.objectToPool.tag == tag) {
                if (item.shouldExpand) {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    if (item.parent != null) {
                        obj.transform.parent = item.parent;
                    }
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

    Random rnd = new Random();

	public GameObject GetRandomGameObject(string tag) {
        List<GameObject> all = GetAllActiveGameObjects(tag);
        if (all.Count == 0)
            return null; //no active bricks
        return all[rnd.Next(all.Count)];
	}

	public int NumberPooled(string tag) {
		int cnt = 0;
		foreach (var obj in pooledObjects)
			if (obj.tag == tag && obj.activeInHierarchy)
				++cnt;
		return cnt;
	}

	public List<GameObject> GetAllActiveGameObjects(string tag) {
		
		List<GameObject> g = new List<GameObject>();
		for (int i = 0; i < pooledObjects.Count; i++) {
			if (pooledObjects[i].activeInHierarchy && pooledObjects[i].tag==tag) {
				g.Add (pooledObjects [i]);
			}
		}
		return g;
	}

	public void Unique(string tag) {
		for (int i = 0; i < pooledObjects.Count; i++) 
			if (pooledObjects[i].activeInHierarchy && pooledObjects[i].tag==tag) 
				pooledObjects [i].SetActive (false);
	}
}

//the previous 78 lines of code were taken from this website: https://www.raywenderlich.com/136091/object-pooling-unity