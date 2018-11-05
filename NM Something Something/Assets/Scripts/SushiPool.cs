using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiPool : MonoBehaviour {
  [SerializeField]
  private List<GameObject> SushiPrefabs;
  private List<GameObject> sushiPool = new List<GameObject>();
  private List<GameObject> sushiInUse = new List<GameObject>();

  private void OnEnable()
  {
    InvisibleWall.DeactivateBoulderEvent += ReturnSushi;
  }

  private void OnDisable()
  {
    InvisibleWall.DeactivateBoulderEvent -= ReturnSushi;
  }

  // Use this for initialization
  void Start ()
  {
    for (int i = 0; i < SushiPrefabs.Count; i++)
    {
      GameObject sushi = Instantiate(SushiPrefabs[i], transform);
      sushiPool.Add(sushi);
      sushi.SetActive(false);
    }
	}
	
  public GameObject RetrieveSushi()
  {
    GameObject sushi;
    if (sushiPool.Count == 0)
    {
      sushi = Instantiate(SushiPrefabs[Random.Range(0,SushiPrefabs.Count)], transform);
      sushiInUse.Add(sushi);
      return sushi;
    }
    sushi = sushiPool[0];
    sushiPool.Remove(sushi);
    sushiInUse.Add(sushi);
    sushi.SetActive(true);
    return sushi;
  }

  public void ReturnSushi(Sushi sushi)
  {
    ReturnSushi(sushi.gameObject);
  }

  private void ReturnSushi(GameObject sushi)
  {
    sushi.GetComponent<Sushi>().ShouldMove = false;
    sushi.transform.position = transform.position;
    sushiInUse.Remove(sushi);
    sushiPool.Add(sushi);
    sushi.gameObject.SetActive(false);
  }

  public void ReturnAllSushi()
  {
    foreach (GameObject sushi in sushiInUse)
    {
      sushi.transform.position = transform.position;
      sushiPool.Add(sushi.gameObject);
      sushi.gameObject.SetActive(false);
    }
    sushiInUse.Clear();
  }
}
