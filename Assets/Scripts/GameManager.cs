using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum State
    {
       inGame,
        pause,
        reset
    }


    [SerializeField] private GameObject ResetPanel;


    private float _player1Time = Const.DEFAULT_TIME;
    private float _player2Time = Const.DEFAULT_TIME;

    private State state { get; set; }

    private void Start()
    {
        HighScore.Load();
    }

    private void Update()
    {
        if (state == State.inGame)
        {
            if (Mathf.RoundToInt(_player1Time) != 0)
                _player1Time -= Time.deltaTime;
            if (Mathf.RoundToInt(_player2Time) != 0)
                _player2Time -= Time.deltaTime;

            UIManager.Instance.UpdateTime(Mathf.RoundToInt(_player1Time).ToString(), true);
            UIManager.Instance.UpdateTime(Mathf.RoundToInt(_player2Time).ToString(), false);

            if (Mathf.RoundToInt(_player1Time) == 0 && Mathf.RoundToInt(_player2Time) == 0)
                EndGame();
        }
    }
    

    private void EndGame()
    {
        int winner = ScoreController.Instance.GetWinner();

        string Feedback = "";

        switch (winner)
        {
            case 0:
                Feedback = "TIE";
                break;
            case 1:
                Feedback = "Player 1 WINS !!!";
                break;
            case 2:
                Feedback = "Player 2 WINS !!!";
                break;
            default:
                break;
        }

        UnityEngine.UI.Text text = ResetPanel.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        UnityEngine.UI.Text highscore = ResetPanel.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>();

        text.text = Feedback;

        foreach (HighScore.Score score in HighScore.GetTop10())
        {
            highscore.text += "\n" + score.value.ToString();

            string winnerName = "";

            if (score.player == 0)
                winnerName = "Tie";
            else
                winnerName = "Player" + score.player;

            highscore.text += "\t\t" + winnerName;

        }

        Time.timeScale = 0;
        state = State.reset;
        ResetPanel.SetActive(true);
    }

    public void AddTime(bool isPlayer1, float time)
    {
        if (isPlayer1)
            _player1Time += time;
        else
            _player2Time += time;
    }

    public void Reset()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
