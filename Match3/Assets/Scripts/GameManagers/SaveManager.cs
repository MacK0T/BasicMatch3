using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System;

public class SaveManager : MonoBehaviour
{
    private GameManager _gm;

    private void Awake()
    {
        _gm = GetComponent<GameManager>();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
        }
        FileStream stream = new FileStream(Application.persistentDataPath + "/Saves/" + 1.ToString() + ".sav", FileMode.Create);

        bf.Serialize(stream, Encode(String.Join("|", _gm.GetState())));
        stream.Close();
    }

    public void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/Saves/" + 1.ToString() + ".sav", FileMode.Open);

        string tempsav = Decode(bf.Deserialize(stream) as string);
        string[] save = tempsav.Split(new char[] { '|' });

        stream.Close();
    }

    private static string Encode(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        return Convert.ToBase64String(plainTextBytes);
    }

    private static string Decode(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);

        return Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
