using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

  public enum Hazard { Boulder };
  [SerializeField]
  private int startingNumberOfWaves;
  private Queue<Wave> waves;

	// Use this for initialization
	void Start ()
  {
    waves = new Queue<Wave>();
    GenerateWaves(startingNumberOfWaves);
  }

  private void GenerateWaves(int newWaves)
  {
    for (int i = 0; i < startingNumberOfWaves; i++)
    {
      waves.Enqueue(new Wave(Hazard.Boulder, Random.Range(1, 3)));
    }
  }

  public Wave NextWave()
  {
    return waves.Dequeue();
  }


}
