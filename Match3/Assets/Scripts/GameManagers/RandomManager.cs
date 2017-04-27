using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomManager : MonoBehaviour
{
    [SerializeField]
    private int seed = 0;

    public int getSeed
    {
        get
        {
            return seed;
        }
    }
    
    private int GenerateNewSeed(int maxSeed)
    {
        Random.InitState(Random.Range(0, maxSeed));
        return Random.seed;
    }

    public int GetRandowValue(int maxLenght)
    {
        return Random.Range(0, maxLenght);
    }


    private void Awake()
    {
        if (seed == 0)
        {
            seed = GenerateNewSeed(1000000);
        }
    }
}
