using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderPool : MonoBehaviour {
  [SerializeField]
  private GameObject BoulderPrefab;
  private Queue<GameObject> boulderPool = new Queue<GameObject>();

  private int boulders = 8;

	// Use this for initialization
	void Start ()
  {
    for (int i = 0; i < boulders; i++)
    {
      GameObject boulder = Instantiate(BoulderPrefab, transform);
      boulderPool.Enqueue(boulder);
      boulder.SetActive(false);
    }
	}
	
  public GameObject RetrieveBoulder()
  {
    if (boulderPool.Count == 0)
    {
      Debug.LogError("No more Boulders!");
      return null;
    }
    GameObject boulder = boulderPool.Dequeue();
    boulder.SetActive(true);
    return boulder;
  }

  public void ReturnBoulder(Boulder boulder)
  {
    boulder.gameObject.transform.parent = transform;
    boulderPool.Enqueue(boulder.gameObject);
    boulder.gameObject.SetActive(false);
  }
}
