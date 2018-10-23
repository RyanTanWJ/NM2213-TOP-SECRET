using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pufferfish : MonoBehaviour {

  private Vector2Int bombCenter;

  [SerializeField]
  Animator animator;

  [SerializeField]
  private List<GameObject> blastShards;

  private List<Vector2Int> AOE;
  
  public delegate void DeactivatePufferfish(GameObject pufferfish);
  public static event DeactivatePufferfish DeactivatePufferfishEvent;

  // Use this for initialization
  void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

  /// <summary>
  /// Drops the bomb at aoe[0], and tells the bomb where to scatter pieces to.
  /// </summary>
  /// <param name="aoe">A list of tiles the bomb will explode on, with the first </param>
  public void DropBomb(List<Vector2Int> aoe)
  {
    AOE = aoe;
    //TODO: Bomb falling from the sky
    explode();
  }

  private void explode()
  {
    for (int i=0; i<AOE.Count; i++)
    {
      GameObject blastShard = blastShards[i];
      blastShard.SetActive(true);
      //TODO: Use Coroutine to move a blast shard to the AOE
      
    }
  }

  IEnumerator ShardExplosionMove(GameObject shard, Vector3 direction, float moveTime)
  {
    float speed = direction.magnitude / moveTime;
    while (moveTime>0)
    {
      shard.transform.position = shard.transform.position + direction * speed * Time.deltaTime;
      moveTime -= Time.deltaTime;
      yield return FadeOut(1.0f);
    }
  }

  IEnumerator FadeOut(float fadeTime)
  {
    while (fadeTime > 0)
    {
      fadeTime -= Time.deltaTime;
      yield return null;
    }
    //TODO: Return to PufferfishPool
  }
}
