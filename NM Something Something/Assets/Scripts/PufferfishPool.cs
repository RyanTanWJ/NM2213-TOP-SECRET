using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferfishPool : MonoBehaviour
{
  [SerializeField]
  private GameObject PufferfishPrefab;
  private List<GameObject> pufferfishPool = new List<GameObject>();
  public List<GameObject> pufferfishesInUse = new List<GameObject>();

  private void OnEnable()
  {
    Pufferfish.DeactivatePufferfishEvent += ReturnPufferfish;
  }

  private void OnDisable()
  {
    Pufferfish.DeactivatePufferfishEvent -= ReturnPufferfish;
  }

  private void ReturnPufferfish(GameObject pufferfish)
  {
    pufferfish.transform.position = transform.position;
    pufferfishesInUse.Remove(pufferfish);
    pufferfishPool.Add(pufferfish);
    pufferfish.gameObject.SetActive(false);
  }

  public void ReturnAllPufferfishes()
  {
    foreach (GameObject pufferfish in pufferfishesInUse)
    {
      pufferfish.transform.position = transform.position;
      pufferfishesInUse.Add(pufferfish.gameObject);
      pufferfish.gameObject.SetActive(false);
    }
    pufferfishesInUse.Clear();
  }

  public GameObject RetrievePufferfish()
  {
    GameObject pufferfish;
    if (pufferfishPool.Count == 0)
    {
      pufferfish = Instantiate(PufferfishPrefab, transform);
      pufferfishesInUse.Add(pufferfish);
      return pufferfish;
    }
    pufferfish = pufferfishPool[0];
    pufferfishPool.Remove(pufferfish);
    pufferfishesInUse.Add(pufferfish);
    pufferfish.SetActive(true);
    return pufferfish;
  }

  public void ReturnPufferfish(Pufferfish pufferfish)
  {
    ReturnPufferfish(pufferfish.gameObject);
  }
}
