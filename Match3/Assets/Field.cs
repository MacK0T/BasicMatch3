using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    private int _fieldColumns;
    [SerializeField]
    private int _fieldRows;
    [SerializeField]
    private GameObject _jewelPrefab;
    [SerializeField]
    private Sprite[] _basicJewels;
    [SerializeField]
    private float _swapTime;

    private Point _selectedJewel = new Point(-1, -1);

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

    public Jewel[,] fieldData { get; private set; }
    public float spaceBetween { get; private set; }
    

    private void GenerateField()
    {
        for (int x = 0; x < _fieldColumns; x++)
            for (int y = 0; y < _fieldRows; y++)
            {
                GameObject tempJewel = GameObject.Instantiate(_jewelPrefab) as GameObject;
                tempJewel.transform.SetParent(transform, false);
                tempJewel.transform.localPosition = new Vector3(x, y, 0);
                bool notSameColorX = true;
                bool notSameColorY = true;
                int jewelColor;
                do
                {
                    jewelColor = GameManager.Instance.randomManager.GetRandowJewel(_basicJewels.Length);
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
                tempJewel.GetComponent<SpriteRenderer>().sprite = _basicJewels[jewelColor];
                fieldData[x, y] = tempJewel.GetComponent<Jewel>();
                fieldData[x, y].ChangeType(jewelColor);
                fieldData[x, y].Init();
            }
        spaceBetween = fieldData[1, 0].transform.position.x - fieldData[0, 0].transform.position.x;
    }

    private IEnumerator SwapJewels(Point firstJewel, Point secondJewel)
    {
        float speedX = Mathf.Abs(firstJewel.x - secondJewel.x) / _swapTime;
        float speedY = Mathf.Abs(firstJewel.y - secondJewel.y) / _swapTime;

        bool vertical = false;
        if (firstJewel.y == secondJewel.y)
            vertical = true;
        if (vertical)
        {
            if (firstJewel.x < secondJewel.x)
            {
                while (fieldData[firstJewel.x, firstJewel.y].transform.localPosition.x <= secondJewel.x)
                {
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition = new Vector3(
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition.x + speedX * Time.deltaTime,
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition.y,
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition.z);

                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition = new Vector3(
                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition.x - speedX * Time.deltaTime,
                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition.y,
                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition.z);
                    yield return null;
                }
            }
            else
            {
                while (fieldData[firstJewel.x, firstJewel.y].transform.localPosition.x >= secondJewel.x)
                {
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition = new Vector3(
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition.x - speedX * Time.deltaTime,
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition.y,
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition.z);

                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition = new Vector3(
                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition.x + speedX * Time.deltaTime,
                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition.y,
                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition.z);
                    yield return null;
                }
            }
        }
        else
        {
            if (firstJewel.y < secondJewel.y)
            {
                while (fieldData[firstJewel.x, firstJewel.y].transform.localPosition.y <= secondJewel.y)
                {
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition = new Vector3(
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition.x,
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition.y + speedY * Time.deltaTime,
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition.z);

                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition = new Vector3(
                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition.x,
                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition.y - speedY * Time.deltaTime,
                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition.z);
                    yield return null;
                }
            }
            else
            {
                while (fieldData[firstJewel.x, firstJewel.y].transform.localPosition.y >= secondJewel.y)
                {
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition = new Vector3(
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition.x,
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition.y - speedY * Time.deltaTime,
                    fieldData[firstJewel.x, firstJewel.y].transform.localPosition.z);

                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition = new Vector3(
                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition.x,
                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition.y + speedY * Time.deltaTime,
                    fieldData[secondJewel.x, secondJewel.y].transform.localPosition.z);
                    yield return null;
                }
            }
        }
        


        Jewel temp = fieldData[firstJewel.x, firstJewel.y];
        fieldData[firstJewel.x, firstJewel.y] = fieldData[secondJewel.x, secondJewel.y];
        fieldData[secondJewel.x, secondJewel.y] = temp;
        _selectedJewel = new Point(-1, -1);

        fieldData[firstJewel.x, firstJewel.y].transform.localPosition = new Vector3(firstJewel.x, firstJewel.y);
        fieldData[secondJewel.x, secondJewel.y].transform.localPosition = new Vector3(secondJewel.x, secondJewel.y);

        //fieldData[firstJewel.x, secondJewel.y].;
        //for(int i = 0; i < )

        yield return null;
        canPlay = true;
    }

    /*
    private int DestroyCombinationJewels()
    {
        int points = 0;
        for (int x = 0; x < _fieldColumns; x++)
            for (int y = 0; y < _fieldRows; y++)
            {

            }
        return points;
    }
    */

    public bool IsSameType(int type, int posX, int posY)
    {
        if (posX >= 0 && posX < _fieldColumns && posY >= 0 && posY < _fieldRows)
        {
            return fieldData[posX, posY].type == type;
        }
        else
        {
            return false;
        }
    }

    public void ChooseJewel(Point selected)
    {
        if(_selectedJewel.Equals(selected))
        {
            fieldData[_selectedJewel.x, _selectedJewel.y].transform.localScale /= 1.1f;
            _selectedJewel = new Point(-1, -1);
        }
        else if(_selectedJewel.Equals(new Point(-1, -1)))
        {
            _selectedJewel = selected;
            fieldData[_selectedJewel.x, _selectedJewel.y].transform.localScale *= 1.1f;
        }
        else if((Mathf.Abs(_selectedJewel.x - selected.x) > 1 || Mathf.Abs(_selectedJewel.y - selected.y) > 1) ||
            (Mathf.Abs(_selectedJewel.x - selected.x) == 1 && Mathf.Abs(_selectedJewel.y - selected.y) == 1))
        {

        }
        else
        {
            canPlay = false;
            fieldData[_selectedJewel.x, _selectedJewel.y].transform.localScale /= 1.1f;
            StartCoroutine(SwapJewels(_selectedJewel, selected));
        }
    }
    
    private void Awake()
    {
        fieldData = new Jewel[_fieldColumns, _fieldRows];
        GenerateField();
    }
}
