using UnityEngine;

public class ScoreController : Singleton<ScoreController>
{
    private float _player1Score = 0;
    private float _player2Score = 0;

    public void CalculateScore(bool isPlayer1, Salad salad, Customer customer)
    {
        float currentWaitTime = customer.currentWaitTime;
        float totalWaitTime = customer.totalWaitTime;

        if (customer.OrderVerification(salad, isPlayer1))
        {
            

            if (isPlayer1)
                _player1Score += 60;
            else
                _player2Score += 60;
        }
        else
        {
            if (isPlayer1)
                _player1Score -= 20;
            else
                _player2Score -= 20;
        }

        UIManager.Instance.UpdateScore(_player1Score.ToString(), isPlayer1);
    }

   
    public void OnCustomerUnfullfilledRequest(bool isAngry, bool isPlayer1)
    {
        if (isAngry)
        {
            if (isPlayer1)
            {
                _player1Score -= 10;
                UIManager.Instance.UpdateScore(_player1Score.ToString(), true);
            }
            else
            {
                _player2Score -= 10;
                UIManager.Instance.UpdateScore(_player2Score.ToString(), false);
            }
        }
        else
        {
            _player1Score = 10;
            _player2Score = 10;
            UIManager.Instance.UpdateScore(_player1Score.ToString(), true);
            UIManager.Instance.UpdateScore(_player2Score.ToString(), false);
        }
    }

    public int GetWinner()
    {
        int winner = 0;

        if (_player1Score > _player2Score)
        {
            winner = 1;
            HighScore.Add(Mathf.RoundToInt(_player1Score), winner);
        }
        else if (_player1Score < _player2Score)
        {
            winner = 2;
            HighScore.Add(Mathf.RoundToInt(_player2Score), winner);
        }
        else
        {
            winner = 0;
            HighScore.Add(Mathf.RoundToInt(_player1Score), winner);
        }


        return winner;
    }

    public void AddPoints(bool isPlayer1, int points)
    {
        if(isPlayer1)
        {
            _player1Score += points;
            UIManager.Instance.UpdateScore(_player1Score.ToString(), true);
        }
        else
        {
            _player2Score += points;
            UIManager.Instance.UpdateScore(_player2Score.ToString(), true);
        }
    }
}
