using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int fieldColumns;
    [SerializeField]
    private int fieldRows;
    [SerializeField]
    private GameObject jewelPrefab;
    [SerializeField]
    private Sprite[] basicJewels;


    private GameObject _field;

    private void GenerateField()
    {
        for(int i = 0; i < fieldColumns; i++)
            for(int j = 0; j < fieldRows; j++)
            {
                GameObject tempJewel = GameObject.Instantiate(jewelPrefab) as GameObject;
                tempJewel.transform.SetParent(_field.transform, false);
                tempJewel.transform.localPosition = new Vector3(i, j, 0);
                tempJewel.GetComponent<SpriteRenderer>().sprite = basicJewels[Random.Range(0, basicJewels.Length)];
            }
    }

    private void Awake()
    {
        _field = transform.FindChild("Field").gameObject;
        GenerateField();
	}


	
}
