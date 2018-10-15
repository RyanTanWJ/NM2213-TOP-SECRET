using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderPool : MonoBehaviour {
  [SerializeField]
  private GameObject BoulderPrefab;
  private List<GameObject> boulderPool = new List<GameObject>();
  public List<GameObject> bouldersInUse = new List<GameObject>();

  private int boulders = 8;

  private void OnEnable()
  {
    InvisibleWall.DeactivateBoulderEvent += ReturnBoulder;
  }

  private void OnDisable()
  {
    InvisibleWall.DeactivateBoulderEvent -= ReturnBoulder;
  }

  // Use this for initialization
  void Start ()
  {
    for (int i = 0; i < boulders; i++)
    {
      GameObject boulder = Instantiate(BoulderPrefab, transform);
      boulderPool.Add(boulder);
      boulder.SetActive(false);
    }
	}
	
  public GameObject RetrieveBoulder()
  {
    GameObject boulder;
    if (boulderPool.Count == 0)
    {
      boulder = Instantiate(BoulderPrefab, transform);
      bouldersInUse.Add(boulder);
      return boulder;
    }
    boulder = boulderPool[0];
    boulderPool.Remove(boulder);
    bouldersInUse.Add(boulder);
    boulder.SetActive(true);
    return boulder;
  }

  public void ReturnBoulder(Boulder boulder)
  {
    ReturnBoulder(boulder.gameObject);
  }

  private void ReturnBoulder(GameObject boulder)
  {
    boulder.GetComponent<Boulder>().ShouldMove = false;
    boulder.transform.position = transform.position;
    bouldersInUse.Remove(boulder);
    boulderPool.Add(boulder);
    boulder.gameObject.SetActive(false);
  }

  public void ReturnAllBoulders()
  {
    foreach (GameObject boulder in bouldersInUse)
    {
      boulder.transform.position = transform.position;
      boulderPool.Add(boulder.gameObject);
      boulder.gameObject.SetActive(false);
    }
    bouldersInUse.Clear();
  }
}
