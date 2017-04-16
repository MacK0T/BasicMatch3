using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Turn
{
    public Point jewelOne;
    public Point jewelTwo;
}

public struct Point
{
    public int x;
    public int y;

    public Point(int xVal, int yVal)
    {
        x = xVal;
        y = yVal;
    }
}

[RequireComponent(typeof(RandomManager))]
[RequireComponent(typeof(SaveManager))]
public class GameManager : SceneSingleton<GameManager>
{
    public List<Turn> playerTurns { get; private set; }
    public RandomManager randomManager { get; private set; }
    public Field mainField { get; private set; }




    public string[] GetState()
    {

        string[] temp = { randomManager.getSeed.ToString() };

        return temp;
    }

    new private void Awake()
    {
        base.Awake();
        mainField = GetComponentInChildren<Field>();
        randomManager = GetComponent<RandomManager>();
        playerTurns = new List<Turn>();
	}
}