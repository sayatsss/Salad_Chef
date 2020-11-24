using UnityEngine;

public class Customer : MonoBehaviour
{
    public class Emotions
    {
        public bool CustAngry;
        public bool isPlayer1;
    }

    [SerializeField] private UICustomerState m_State;

    public float currentWaitTime { get; private set; }
    public float totalWaitTime { get; private set; }

    public Emotions emotions { get; private set; } = new Emotions();

    
    private Salad _order;

   
    private float _waitTimeFactor = 1f;

    private void Start()
    {
        OrderGeneration();
    }

    private void Update()
    {
        m_State.SetTime(currentWaitTime, totalWaitTime);
        currentWaitTime -= Time.deltaTime * _waitTimeFactor;

        if (Mathf.RoundToInt(currentWaitTime) == 0)
            Moveaway();
    }

   
    private void Moveaway()
    {
        ScoreController.Instance.OnCustomerUnfullfilledRequest(emotions.CustAngry, emotions.isPlayer1);
        OrderGeneration();
    }

    
    
    public bool OrderVerification(Salad order, bool isPlayer1)
    {
        if (_order == order)
        {
            OrderGeneration();
            return true;
        }
        else
        {
            emotions.isPlayer1 = isPlayer1;
            emotions.CustAngry = true;
            _waitTimeFactor = 2f;
            return false;
        }
    }

  
    public void OrderGeneration()
    {
        _order = Salad.GenerateRandom();
        m_State.SetOrder(_order.ToString());
        currentWaitTime = Random.Range(Const.CUSTOMER_WAIT_TIME,
                                       _order.vegetabeCount * (Const.CUSTOMER_WAIT_TIME + 1));
        totalWaitTime = currentWaitTime;
        _waitTimeFactor = 1f;
    }

}
