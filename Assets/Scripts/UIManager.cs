using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIPlayer
{
    public Text status;
    public Text choppingBoardStatus;
    public Text plateStatus;
    public Text score;
    public Text time;
}

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private UIPlayer player1;
    [SerializeField] private UIPlayer player2;

    public void UpdatePlayerStatus(string text, bool isPlayer1)
    {
        if (isPlayer1)
            player1.status.text = text;
        else
            player2.status.text = text;
    }

    public void UpdateChoppingBoardStatus(string text, bool isPlayer1)
    {
        if (isPlayer1)
            player1.choppingBoardStatus.text = text;
        else
            player2.choppingBoardStatus.text = text;
    }

    public void UpdatePlateStatus(string text, bool isPlayer1)
    {
        if (isPlayer1)
            player1.plateStatus.text = text;
        else
            player2.plateStatus.text = text;
    }

    public void UpdateScore(string text, bool isPlayer1)
    {
        if (isPlayer1)
            player1.score.text = "Score: " + text;
        else
            player2.score.text = "Score: " + text;
    }

    public void UpdateTime(string text, bool isPlayer1)
    {
        if (isPlayer1)
            player1.time.text = "Time: " + text + "s";
        else
            player2.time.text = "Time: " + text + "s";
    }
}
