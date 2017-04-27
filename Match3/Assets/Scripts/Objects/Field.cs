using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private int _pointsPerJewel;
    [SerializeField]
    private float _swapTime;
    [SerializeField]
    private Text _pointsUI;

    private Point _selectedJewel = new Point(-1, -1);

    private bool _canPlay = true;

    
    private int _points = 0;

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
                    jewelColor = GameManager.Instance.randomManager.GetRandowValue(_basicJewels.Length);
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
        StartCoroutine(MoveJewel(fieldData[firstJewel.x, firstJewel.y], secondJewel));
        yield return StartCoroutine(MoveJewel(fieldData[secondJewel.x, secondJewel.y], firstJewel));
        
        Jewel temp = fieldData[firstJewel.x, firstJewel.y];
        fieldData[firstJewel.x, firstJewel.y] = fieldData[secondJewel.x, secondJewel.y];
        fieldData[secondJewel.x, secondJewel.y] = temp;

        int winPoints = 0;
        var startFromBot = Mathf.Min(CountCombinations(firstJewel, ref winPoints), CountCombinations(secondJewel, ref winPoints));
        if (winPoints > 0)
        {
            do
            {
                UpdatePoints(winPoints);
                winPoints = 0;
                yield return new WaitForSeconds(0.55f);
                List<Point> newJewels = FieldUpdate(startFromBot);
                int[] startPoints = new int[newJewels.Count];
                for (int i = 0; i < newJewels.Count; i++)
                {
                    startPoints[i] = CountCombinations(newJewels[i], ref winPoints);
                }
                startFromBot = Mathf.Min(startPoints);
            } while (winPoints > 0);
        }
        else
        {
            StartCoroutine(MoveJewel(fieldData[firstJewel.x, firstJewel.y], secondJewel));
            yield return StartCoroutine(MoveJewel(fieldData[secondJewel.x, secondJewel.y], firstJewel));

            temp = fieldData[firstJewel.x, firstJewel.y];
            fieldData[firstJewel.x, firstJewel.y] = fieldData[secondJewel.x, secondJewel.y];
            fieldData[secondJewel.x, secondJewel.y] = temp;
        }
        
        _selectedJewel = new Point(-1, -1);
        canPlay = true;
    }

    private IEnumerator MoveJewel(Jewel start, Point end)
    {
        Transform firstTransform = start.transform;
        Vector3 firstPosition = firstTransform.localPosition;
        Vector3 secondPosition = new Vector3(end.x, end.y, 0);

        float inverseSwapTime = 1.0f / _swapTime;
        Vector3 position = firstPosition;
        for (float t = 0.0f; t < _swapTime; t += Time.deltaTime)
        {
            float delta = t * inverseSwapTime;

            position.x = Mathf.Lerp(firstPosition.x, end.x, delta);
            position.y = Mathf.Lerp(firstPosition.y, end.y, delta);
            firstTransform.localPosition = position;
            
            yield return null;
        }
        firstTransform.localPosition = secondPosition;
    }

    private int CountCombinations(Point position, ref int getedPoints)
    {
        var type = fieldData[position.x, position.y].type;

        // top
        int maxY = position.y + 1;
        while (maxY < _fieldColumns && IsSameType(type, position.x, maxY))
        {
            maxY++;
        }
        maxY--;
        // bot
        int minY = position.y - 1;
        while (minY >= 0 && IsSameType(type, position.x, minY))
        {
            minY--;
        }
        minY++;
        // right
        int maxX = position.x + 1;
        while (maxX < _fieldRows && IsSameType(type, maxX, position.y))
        {
            maxX++;
        }
        maxX--;
        // left
        int minX = position.x - 1;
        while (minX >= 0 && IsSameType(type, minX, position.y))
        {
            minX--;
        }
        minX++;

        if ((maxY - minY) >= 2)
        {
            getedPoints += CountPoints((maxY - minY) + 1);
            for(int i = minY; i <= maxY; i++)
                if (fieldData[position.x, i] != null)
                    StartCoroutine(fieldData[position.x, i].DestroyWithAnimation());

        }
        if ((maxX - minX) >= 2)
        {
            getedPoints += CountPoints((maxX - minX) + 1);
            for (int i = minX; i <= maxX; i++)
                if (fieldData[i, position.y] != null)
                    StartCoroutine(fieldData[i, position.y].DestroyWithAnimation());
        }
        return minY;
    }

    private int CountPoints(int countOfJewels)
    {
        int points = 0;
        // количество кристалов на очки на комбо
        points = countOfJewels * _pointsPerJewel * (countOfJewels - 2);
        return points;
    }

    private void UpdatePoints(int plusPoints)
    {
        _points += plusPoints;
        _pointsUI.text = "Points:" + _points;
    }

    private List<Point> FieldUpdate(int startFromBot)
    {
        List<Point> checkNextUpdate = new List<Point>();
        for (int y = startFromBot; y < _fieldRows; y++)
            for (int x = 0; x < _fieldColumns; x++)
            {
                var placeInField = fieldData[x, y];
                if (placeInField == null)
                {
                    List<Jewel> variants = new List<Jewel>();
                    Point currentPoint = new Point(x, y);
                    if (IsInField(x - 1, y + 1) && fieldData[x - 1, y + 1]!=null)
                    {
                        variants.Add(fieldData[x - 1, y + 1]);
                    }

                    if (IsInField(x, y + 1) && fieldData[x, y + 1] != null)
                    {
                        variants.Add(fieldData[x, y + 1]);
                    }

                    if(IsInField(x + 1, y + 1) && fieldData[x + 1, y + 1] != null)
                    {
                        variants.Add(fieldData[x + 1, y + 1]);
                    }
                    if (variants.Count != 0)
                    {
                        Jewel choosen = variants[GameManager.Instance.randomManager.GetRandowValue(variants.Count)];
                        fieldData[(int)choosen.transform.localPosition.x, (int)choosen.transform.localPosition.y] = null;
                        fieldData[x, y] = choosen;
                        StartCoroutine(MoveJewel(choosen, currentPoint));
                    }
                    else
                    {
                        GameObject tempJewel = GameObject.Instantiate(_jewelPrefab) as GameObject;
                        tempJewel.transform.SetParent(transform, false);
                        tempJewel.transform.localPosition = new Vector3(x, y + 1, 0);
                        int jewelColor = GameManager.Instance.randomManager.GetRandowValue(_basicJewels.Length);
                        tempJewel.GetComponent<SpriteRenderer>().sprite = _basicJewels[jewelColor];
                        fieldData[x, y] = tempJewel.GetComponent<Jewel>();
                        fieldData[x, y].ChangeType(jewelColor);
                        fieldData[x, y].Init();
                        StartCoroutine(MoveJewel(fieldData[x, y], currentPoint));
                    }
                    checkNextUpdate.Add(currentPoint);
                }
            }
        return checkNextUpdate;
    }

    public bool IsInField(int x, int y)
    {
        return (x >= 0 && x < _fieldColumns && y >= 0 && y < _fieldRows) ? true : false;
    }
    

    public bool IsSameType(int type, int posX, int posY)
    {
        if (IsInField(posX, posY))
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
