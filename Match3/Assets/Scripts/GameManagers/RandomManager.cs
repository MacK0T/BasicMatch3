using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomManager : MonoBehaviour
{
    [SerializeField]
    private string seed = "";

    public string getSeed
    {
        get
        {
            return seed;
        }
    }

    private static System.Random random = new System.Random();
    private string GenerateNewSeed(int lenght)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, lenght)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public void GetRandowJewel()
    {

    }


    private void Awake()
    {
        if (seed == "")
        {
            seed = GenerateNewSeed(8);
        }
    }
}
