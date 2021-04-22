using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] SingleField[] fields = new SingleField[9];
    [SerializeField] AI ai;
    [SerializeField] Text text;
    [SerializeField] Button[] aiIntelligenceButtons;
    [Header("Colors")]
    [SerializeField] Color computerWinColor = Color.red;
    [SerializeField] Color playerWinColor = Color.green;
    [SerializeField] Color tieColor = Color.yellow;

    FieldTakenBy[] fieldArray;
    bool gameOver = false;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        gameOver = true;
        text.enabled = true;
        text.text = "To Start Game, press 'Start Game' Button.";
        for (int i = 0; i < fields.Length; i++)
        {
            fields[i].IndexOnField = i;
            fields[i].Init();
        }
        foreach (Button b in aiIntelligenceButtons)
            b.enabled = true;
    }
    public void StartGame()
    {
        if (!aiIntelligenceButtons[0].enabled)
            return;
        text.enabled = false;
        gameOver = false;
        foreach (SingleField field in fields)
        {
            field.Changeable = true;
        }
        foreach (Button b in aiIntelligenceButtons)
            b.enabled = false;
    }

    public void PlayerSet(int index)
    {
        for (int i = 0; i < fields.Length; i++)
        {
            if (i != index)
            {
                fields[i].UpdatePlayerChoice();
            }
        }
    }

    public void FinishTurn()
    {
        if (gameOver)
            return;
        //check that the player made a move
        bool madeMove = false;
        foreach(SingleField field in fields)
        {
            if (field.Changeable && field.fieldTakenBy == FieldTakenBy.Player)
            {
                madeMove = true;
                break;
            }
        }
        if (!madeMove)
            return;
        //player turn and update
        foreach (SingleField field in fields)
            field.FinalizePlayerChoice();
        CheckEndState();
        //computer turn
        if (gameOver)
            return;
        ComputerTurn();
        CheckEndState();
    }

    private void ComputerTurn()
    {
        int computerChoice = ai.ChooseAction(fieldArray);
        fields[computerChoice].ComputerChoice();
    }

    private void UpdateField()
    {
        fieldArray = new FieldTakenBy[9];
        for (int i = 0; i < 9; i++)
        {
            SingleField field = fields[i];
            fieldArray[i] = field.fieldTakenBy;
        }
    }

    #region check End state
    private void CheckEndState()
    {
        UpdateField();
        if (CheckPlayerWin())
        {
            StopInput();
            text.text = "Player Wins";
            text.enabled = true;
        }
        else if (CheckComputerWin())
        {
            StopInput();
            text.text = "Computer Wins";
            text.enabled = true;
        }
        else if (CheckTie())
        {
            foreach (SingleField field in fields)
                field.ChangeColor(tieColor);
            StopInput();
            text.text = "Tie";
            text.enabled = true;
        }
    }
    private void StopInput()
    {
        gameOver = true;
        foreach (SingleField field in fields)
        {
            field.Changeable = false;
        }
    }
    private bool CheckTie()
    {
        foreach (SingleField field in fields)
        {
            if (field.Changeable)
                return false;
        }
        return true;
    }
    private bool CheckPlayerWin()
    {
        return CheckRow(FieldTakenBy.Player, playerWinColor) ||
            CheckColumn(FieldTakenBy.Player, playerWinColor) ||
            CheckDiagonal(FieldTakenBy.Player, playerWinColor);
    }
    private bool CheckComputerWin()
    {
        return CheckRow(FieldTakenBy.Computer, computerWinColor) ||
            CheckColumn(FieldTakenBy.Computer, computerWinColor) ||
            CheckDiagonal(FieldTakenBy.Computer, computerWinColor);
    }

    private bool CheckRow(FieldTakenBy currentPlayer, Color color)
    {
        for (int i = 0; i < 9; i += 3)
        {
            bool rowExist = true;
            for (int j = i; j < i + 3; j++)
            {
                if (fieldArray[j] != currentPlayer)
                    rowExist = false;
            }
            if (rowExist)
            {
                for (int j = i; j < i + 3; j++)
                    fields[j].ChangeColor(color);
                return true; 
            }
        }
        return false;
    }
    private bool CheckColumn(FieldTakenBy currentPlayer, Color color)
    {
        for (int i = 0; i < 3; i++)
        {
            bool rowExist = true;
            for (int j = i; j < i + 9; j += 3)
            {
                if (fieldArray[j] != currentPlayer)
                    rowExist = false;
            }
            if (rowExist)
            {
                for (int j = i; j < i + 9; j += 3)
                    fields[j].ChangeColor(color);
                return true;
            }
        }
        return false;
    }
    private bool CheckDiagonal(FieldTakenBy currentPlayer, Color color)
    {
        if (fieldArray[0] == currentPlayer && fieldArray[4] == currentPlayer && fieldArray[8] == currentPlayer)
        {
            fields[0].ChangeColor(color);
            fields[4].ChangeColor(color);
            fields[8].ChangeColor(color);
            return true;
        }
        if (fieldArray[2] == currentPlayer && fieldArray[4] == currentPlayer && fieldArray[6] == currentPlayer)
        {
            fields[2].ChangeColor(color);
            fields[4].ChangeColor(color);
            fields[6].ChangeColor(color);
            return true;
        }
        return false;
    }

    #endregion


}
