using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum FieldTakenBy { None, Player, Computer };
public class SingleField : MonoBehaviour
{
    [SerializeField] Sprite playerPickSprite;
    [SerializeField] Sprite computerPickSprite;
    [SerializeField] GameManager gameManager;
    int indexOnField;
    public int IndexOnField { set { indexOnField = value; } }
    public FieldTakenBy fieldTakenBy = FieldTakenBy.None;

    //player is always X

    Image image;
    bool changable = true;
    public bool Changeable { get { return changable; } set { changable = value; } }
    //bool 

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        fieldTakenBy = FieldTakenBy.None;
        changable = false;
        image = transform.GetChild(0).GetComponent<Image>();
        image.enabled = false;
        ChangeColor(Color.white);
    }

    /// <summary>
    /// player clicks this, so if it's changeable it becomes an X or nothing.
    /// It also updates  fieldTakenBy
    /// </summary>
    public void ClickedByPlayer()
    {
        if (!changable)
            return;
        if (image.enabled)
        {
            image.enabled = false;
            fieldTakenBy = FieldTakenBy.None;
        }else
        {
            image.enabled = true;
            image.sprite = playerPickSprite;
            fieldTakenBy = FieldTakenBy.Player;
        }
        gameManager.PlayerSet(indexOnField);
    }

    public void ComputerChoice()
    {
        image.enabled = true;
        image.sprite = computerPickSprite;
        changable = false;
        fieldTakenBy = FieldTakenBy.Computer;
    }

    public void UpdatePlayerChoice()
    {
        if (changable && fieldTakenBy == FieldTakenBy.Player)
        {
            image.enabled = false;
            fieldTakenBy = FieldTakenBy.None;
        }  
    }
    public void FinalizePlayerChoice()
    {
        if (fieldTakenBy != FieldTakenBy.None)
            changable = false;
    }
    public void ChangeColor(Color color)
    {
        GetComponent<Image>().color = color;
    }
}
