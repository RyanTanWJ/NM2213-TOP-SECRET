using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawPool : MonoBehaviour
{
  [SerializeField]
  private GameObject ClawPrefab;
  private List<GameObject> clawPool = new List<GameObject>();
  public List<GameObject> clawsInUse = new List<GameObject>();

  private void OnEnable()
  {
    InvisibleWall.DeactivateClawEvent += ReturnClaw;
    Player.DebuffEvent += ReturnClaw;
  }

  private void OnDisable()
  {
    InvisibleWall.DeactivateClawEvent -= ReturnClaw;
    Player.DebuffEvent -= ReturnClaw;
  }

  private void ReturnClaw(GameObject claw)
  {
    claw.GetComponent<Claw>().ShouldMove = false;
    claw.transform.position = transform.position;
    clawsInUse.Remove(claw);
    clawPool.Add(claw);
    claw.gameObject.SetActive(false);
  }

  public void ReturnAllClaws()
  {
    foreach (GameObject claw in clawsInUse)
    {
      claw.transform.position = transform.position;
      clawsInUse.Add(claw.gameObject);
      claw.gameObject.SetActive(false);
    }
    clawsInUse.Clear();
  }

  public GameObject RetrieveClaw()
  {
    GameObject claw;
    if (clawPool.Count == 0)
    {
      claw = Instantiate(ClawPrefab, transform);
      clawsInUse.Add(claw);
      return claw;
    }
    claw = clawPool[0];
    clawPool.Remove(claw);
    clawsInUse.Add(claw);
    claw.SetActive(true);
    return claw;
  }

  public void ReturnClaw(Claw claw)
  {
    ReturnClaw(claw.gameObject);
  }
}
