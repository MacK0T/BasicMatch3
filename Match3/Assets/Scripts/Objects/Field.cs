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
        var xDirection = firstJewel.x - secondJewel.x;
        var yDirection = firstJewel.y - secondJewel.y;
        var znak = -Mathf.Sign(xDirection + yDirection);

        Vector3 speed = new Vector3(Mathf.Abs(xDirection),
            Mathf.Abs(yDirection), 0) / _swapTime * znak;


        Transform first = fieldData[firstJewel.x, firstJewel.y].transform;
        Transform second = fieldData[secondJewel.x, secondJewel.y].transform;

        while (0 <= znak * (secondJewel.x - first.localPosition.x) && 0 <= znak * (secondJewel.y - first.localPosition.y))
        {
            SwapAtSamePlace(first, second, speed);
            yield return null;
        }
        

        second.localPosition = new Vector3(firstJewel.x, firstJewel.y);
        first.localPosition = new Vector3(secondJewel.x, secondJewel.y);


        Jewel firstTempJewel = fieldData[firstJewel.x, firstJewel.y];
        Jewel secondTempJewel = fieldData[secondJewel.x, secondJewel.y];
        Jewel temp = firstTempJewel;
        fieldData[firstJewel.x, firstJewel.y] = secondTempJewel;
        fieldData[secondJewel.x, secondJewel.y] = temp;


        _selectedJewel = new Point(-1, -1);

        //Debug.Log(firstTempJewel)


        //fieldData[firstJewel.x, secondJewel.y].;
        int combo = 0;

        yield return null;
        canPlay = true;
    }

    private void Move(Transform pos, Vector3 speed)
    {
        pos.localPosition += speed * Time.deltaTime;
    }

    private void SwapAtSamePlace(Transform firstObj, Transform secondObj, Vector3 speed)
    {
        Move(firstObj, speed);

        Move(secondObj, -speed);
    }

    private List<Jewel> CountFromPoint(Point position, bool checkRows = true)
    {
        List<Jewel> jewelsDestroy = new List<Jewel>();
        int endFieldFromPoint;

        // becouse we create field from bottom left to top right
        // x pos rows, y pos columns
        if (checkRows)
        {
            endFieldFromPoint = Mathf.Abs(position.x - _fieldRows);
            // top 
            for (int r = position.x + 1; r < endFieldFromPoint; r++)
            {
                if (IsSameType(fieldData[position.x, position.y].type, r, position.y))
                {
                    jewelsDestroy.Add(fieldData[r, position.y]);
                }
                else
                {
                    break;
                }
            }
            // bot 
            for (int r = position.x - 1; r >= 0; r--)
            {
                if (IsSameType(fieldData[position.x, position.y].type, r, position.y))
                {
                    jewelsDestroy.Add(fieldData[r, position.y]);
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            endFieldFromPoint = Mathf.Abs(position.x - _fieldColumns);
            // right
            for (int r = position.y + 1; r < endFieldFromPoint; r++)
            {
                if (IsSameType(fieldData[position.x, position.y].type, position.x, r))
                {
                    jewelsDestroy.Add(fieldData[position.x, r]);
                }
                else
                {
                    break;
                }
            }
            // left
            for (int r = position.y - 1; r >= 0; r--)
            {
                if (IsSameType(fieldData[position.x, position.y].type, position.x, r))
                {
                    jewelsDestroy.Add(fieldData[position.x, r]);
                }
                else
                {
                    break;
                }
            }
        }

        return jewelsDestroy;
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
