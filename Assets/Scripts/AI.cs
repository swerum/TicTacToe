using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    const int maxDepth = 6;
    int difficulty = 5;
    const int infinity = 5000;
    [SerializeField] Text text;
    private void Start()
    {
        text.text = "" + difficulty;
    }
    public void IncreaseDepth(int num)
    {
        difficulty += num;
        if (difficulty > maxDepth)
            difficulty = 1;
        else if (difficulty < 1)
            difficulty = maxDepth;
        text.text = ""+difficulty;
    }

    public int ChooseAction(FieldTakenBy[] field)
    {
        return MinMax(field, 0, true).action;
    }


    #region MinMaxa algorithm
    private ActionValue MinMax(FieldTakenBy[] field, int depth, bool min)
    {
        //check if it's an end state
        if (depth > difficulty)
            return new ActionValue(-1, 0);
        if (isWinState(field))
            return new ActionValue(-1, 500);
        if (isLoseState(field))
            return new ActionValue(-1, -500);
        //get the min or max
        ActionValue actionValue;
        if (min)
            actionValue = Min(field, depth);
        else
            actionValue = Max(field, depth + 1);
        //check if there were no options left
        if (Mathf.Abs(actionValue.value) >= infinity-20)
            return new ActionValue(-1, 0);
        return actionValue;
    }


    private ActionValue Min(FieldTakenBy[] field,int depth)
    {
        //get the min of the max actions
        int min = infinity;
        int action = -1;
        for (int i = 0; i < field.Length; i++)
        {
            if (field[i] != FieldTakenBy.None)
                continue;
            FieldTakenBy[] newField = GetSuccessorState(i, field, FieldTakenBy.Computer);
            int value = MinMax(newField, depth, false).value;
            if (value < min)
            {
                min = value;
                action = i;
            }
        }
        return new ActionValue(action, min+depth);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>the max value the player could get from this field</returns>
    private ActionValue Max(FieldTakenBy[] field, int depth)
    {
        //get the max of the min values
        int max = -infinity;
        int action = -1;
        for (int i = 0; i < field.Length; i++)
        {
             if (field[i] != FieldTakenBy.None)
                continue;
            FieldTakenBy[] newField = GetSuccessorState(i, field, FieldTakenBy.Player);
            int value = MinMax(newField, depth+1, true).value;
            if (value > max)
            {
                max = value;
                action = i;
            }
        }
        return new ActionValue(action,max-depth);
    }
    #endregion

    private FieldTakenBy[] GetSuccessorState(int i, FieldTakenBy[] field, FieldTakenBy currentPlayer)
    {
        FieldTakenBy[] newField = new FieldTakenBy[9];
        field.CopyTo(newField, 0);
        newField[i] = currentPlayer;
        return newField;
    }

    #region end states
    /// <summary>
    /// Does the player win in this state
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    private bool isWinState(FieldTakenBy[] field)
    {
        return TripleExists(field, FieldTakenBy.Player);
    }
    /// <summary>
    /// does the player lose in this state
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    private bool isLoseState(FieldTakenBy[] field)
    {
        return TripleExists(field, FieldTakenBy.Computer);
    }
    private bool TripleExists(FieldTakenBy[] field, FieldTakenBy currentPlayer)
    {
        return CheckRow(field, currentPlayer) || CheckColumn(field, currentPlayer) || CheckDiagonal(field, currentPlayer);
    }

    private bool CheckRow(FieldTakenBy[] field, FieldTakenBy currentPlayer)
    {
        for (int i = 0; i < 9; i += 3)
        {
            bool rowExist = true;
            for (int j = i; j < i + 3; j++)
            {
                if (field[j] != currentPlayer)
                    rowExist = false;
            }
            if (rowExist) return true;
        }
        return false;
    }
    private bool CheckColumn(FieldTakenBy[] field, FieldTakenBy currentPlayer)
    {
        for (int i = 0; i < 3; i ++)
        {
            bool rowExist = true;
            for (int j = i; j < i + 9; j+=3)
            {
                if (field[j] != currentPlayer)
                    rowExist = false;
            }
            if (rowExist) return true;
        }
        return false;
    }
    private bool CheckDiagonal(FieldTakenBy[] field, FieldTakenBy currentPlayer)
    {
        if (field[0] == currentPlayer && field[4] == currentPlayer && field[8] == currentPlayer)
            return true;
        if (field[2] == currentPlayer && field[4] == currentPlayer && field[6] == currentPlayer)
            return true;
        return false;
    }

    #endregion
}

public class ActionValue
{
    public int action = -1;
    public int value = -1;
    public ActionValue(int a, int v)
    {
        action = a;
        value = v;
    }
    public void Print() { Debug.Log("Action: " + action + ", Value: " + value); }
}
