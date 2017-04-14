using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(RandomManager))]
[RequireComponent(typeof(SaveManager))]
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

    private bool _canPlay = true;

    public bool canPlay
    {
        get
        {
            return _canPlay;
        }
        private set
        {
            _canPlay = value;
        }
    }

    public RandomManager _randomManager { get; private set; }
    public Jewel[,] fieldData { get; private set; }
    public float spaceBetween { get; private set; }

    private GameObject _field;

    private void GenerateField()
    {
        for (int x = 0; x < fieldColumns; x++)
            for (int y = 0; y < fieldRows; y++)
            {
                GameObject tempJewel = GameObject.Instantiate(jewelPrefab) as GameObject;
                tempJewel.transform.SetParent(_field.transform, false);
                tempJewel.transform.localPosition = new Vector3(x, y, 0);
                bool notSameColorX = true;
                bool notSameColorY = true;
                int jewelColor;
                do
                {
                    jewelColor = Random.Range(0, basicJewels.Length);
                    if (x >= 2)
                    {
                        notSameColorX = (fieldData[x - 1, y].type != jewelColor)
                            && (fieldData[x - 2, y].type != jewelColor);
                    }

                    if (y >= 2)
                    {
                        notSameColorY = (fieldData[x, y - 1].type != jewelColor)
                            && (fieldData[x, y - 2].type != jewelColor);
                    }

                }
                while (!notSameColorX || !notSameColorY);
                tempJewel.GetComponent<SpriteRenderer>().sprite = basicJewels[jewelColor];
                fieldData[x, y] = tempJewel.GetComponent<Jewel>();
                fieldData[x, y].ChangeType(jewelColor);
                fieldData[x, y].Init();
            }
        spaceBetween = fieldData[1, 0].transform.position.x - fieldData[0, 0].transform.position.x; 
    }
    
    private IEnumerator SwapJewels()
    {
        yield return null;
        canPlay = true;
    }

    public void TrySwap()
    {
        canPlay = false;
        StartCoroutine(SwapJewels());
    }

    public bool IsSameType(int type, int posX, int posY)
    {
        if (posX >= 0 && posX < fieldColumns && posY >= 0 && posY < fieldRows)
        {
            return fieldData[posX, posY].type == type;
        }
        else
        {
            return false;
        }
    }

    public string[] GetState()
    {
        string[] temp = { _randomManager.getSeed };

        return temp;
    }

    private void Awake()
    {
        _randomManager = GetComponent<RandomManager>();
        _field = transform.FindChild("Field").gameObject;
        fieldData = new Jewel[fieldColumns, fieldRows];
        GenerateField();
	}
}