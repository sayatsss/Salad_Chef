using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool m_IsPlayer1 = true;
    [SerializeField] private float m_MoveSpeed = 7;

    [SerializeField] private float m_ChopSpeed = 2f;
    [SerializeField] private GameObject m_Status;
    [SerializeField] private int m_MaxVegetableCapacity = 2;

    [SerializeField] private ObservableCollection<Vegetable> m_Vegetables = new ObservableCollection<Vegetable>();

    private Vegetable _currentVegetable { get; set; }
    private Vegetable _vegetableOnPlate { get; set; }
    private Salad _salad = new Salad();
    private Rigidbody _rigidBody = null;
    private Customer _currentCustomer = null;
    private bool _canMove = true;

    enum State
    {
        none,
        pick,   
        chop,   
        plate,  
        serve,  
        trash   
    }

    private State _state;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();

        Physics.IgnoreLayerCollision(9, 9);

        m_Vegetables.CollectionChanged += OnVegetablesChanged;
    }

    private void OnVegetablesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => SetCarriedVegetableText();

    private void Update()
    {
        if (_canMove)
            PlayerMovement();

        if (m_Status != null)
        {
            float x = transform.position.x + 1;
            float y = m_Status.transform.position.y;
            float z = transform.position.z + 1;

            m_Status.transform.position = new Vector3(x, y, z);
        }
        
    }
   
    private void PlayerMovement()
    {
       
        if (m_IsPlayer1)
            Player1Movement();
        else
            Player2Movement();
    }

    
    private void Player1Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //Up movement
            _rigidBody.velocity = Vector3.forward * m_MoveSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            //Left movement
            _rigidBody.velocity = Vector3.left * m_MoveSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            //Down movement
            _rigidBody.velocity = Vector3.back * m_MoveSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            //Right movement
            _rigidBody.velocity = Vector3.right * m_MoveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.L))
            OnActions();
    }

    
    private void Player2Movement()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Up movement
            _rigidBody.velocity = Vector3.forward * m_MoveSpeed;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Left movement
            _rigidBody.velocity = Vector3.left * m_MoveSpeed;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            //Down movement
            _rigidBody.velocity = Vector3.back * m_MoveSpeed;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            //Right movement
            _rigidBody.velocity = Vector3.right * m_MoveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            OnActions();
    }

  

    
    
    private void OnTriggerStay(Collider collider)
    {
       
        switch (collider.tag)
        {
            case Const.VEGETABLE_TAG:
                string vegetableName = collider.name.Split('_')[1];
                _currentVegetable = new Vegetable(vegetableName);
                _state = State.pick;
                break;
            case Const.BOARD_TAG:
                if ((collider.name.Contains("1") && m_IsPlayer1) ||
                 (collider.name.Contains("2") && !m_IsPlayer1))
                    _state = State.chop;
                break;
            case Const.PLATE_TAG:
                _state = State.plate;
                break;
            case Const.TRASHCAN_TAG:
                _state = State.trash;
                break;
            case Const.CUSTOMER_TAG:
                _currentCustomer = collider.GetComponent<Customer>();
                _state = State.serve;
                break;
            
            default:
                break;


        }
    }

    private void OnTriggerExit(Collider collider)
    {
       
        switch (collider.tag)
        {
            case Const.VEGETABLE_TAG:
                _state = State.none;
                _currentVegetable = null;
                break;
            case Const.BOARD_TAG:
            case Const.PLATE_TAG:
            case Const.TRASHCAN_TAG:
            case Const.CUSTOMER_TAG:
                _state = State.none;
                _currentCustomer = null;
                break;
            default:
                break;
        }


    }
   

    

    private void OnActions()
    {
        switch (_state)
        {
            case State.none:
                break;
            case State.pick:
                VegetablePickUp();
                break;
            case State.chop:
                ChopVegetables();
                break;
            case State.plate:
                PutVegetableOnPlate();
                break;
            case State.serve:
                ServeToCustomer();
                break;
            case State.trash:
                DumpVegetables();
                break;
            default:
                break;
        }
    }


    private void AddVegetable()
    {
        Vegetable vegetable = m_Vegetables[1];
        m_Vegetables[1] = _currentVegetable;
        m_Vegetables[0] = vegetable;
    }

    private void UpdatePlayerVegetableStatus()
    {
        string vegetables = "";

        foreach (Vegetable vegetable in m_Vegetables)
            vegetables += vegetable.name + ",";

        if (vegetables.Length == 0)
            vegetables = "...";
        else
            vegetables = vegetables.Remove(vegetables.Length - 1);

        UIManager.Instance.UpdatePlayerStatus(vegetables, m_IsPlayer1);
    }

  
    private void VegetablePickUp()
    {
        if (_currentVegetable == null)
            return;

        if (m_Vegetables.Count == m_MaxVegetableCapacity)
        {
            Vegetable vegetable = m_Vegetables[1];
            m_Vegetables[1] = _currentVegetable;
            m_Vegetables[0] = vegetable;
        }
        else if (m_Vegetables.Count < m_MaxVegetableCapacity)
        {
            m_Vegetables.Add(_currentVegetable);
        }
    }

   
    private void ChopVegetables()
    {
        if (m_Vegetables.Count == 0)
        {
            if (_salad.isEmpty)
                return;

            PickSalad();
            return;
        }

        StartCoroutine(ChopVegtableRoutine());
    }

    private IEnumerator ChopVegtableRoutine()
    {
       
        _canMove = false;
        yield return new WaitForSeconds(m_ChopSpeed);
        Vegetable vegetable = m_Vegetables.First();
        _salad.Add(vegetable);
        m_Vegetables.Remove(vegetable);      
        UIManager.Instance.UpdateChoppingBoardStatus(_salad.ToString(), m_IsPlayer1);
        _canMove = true;
    }

    private void SetCarriedVegetableText()
    {
        if (m_Vegetables.Count == 0)
        {
            UIManager.Instance.UpdatePlayerStatus("...", m_IsPlayer1);
            return;
        }

        UpdatePlayerVegetableStatus();
    }

    private void PutVegetableOnPlate()
    {
       
        if (_vegetableOnPlate == null && m_Vegetables.Count != 0)
        {
            Vegetable vegetable = m_Vegetables.First();
            _vegetableOnPlate = vegetable;
            UIManager.Instance.UpdatePlateStatus(_vegetableOnPlate.name, m_IsPlayer1);
            m_Vegetables.Remove(vegetable);
            UpdatePlayerVegetableStatus();
        }
       
        else if (_vegetableOnPlate != null && m_Vegetables.Count < m_MaxVegetableCapacity)
        {
            m_Vegetables.Add(_vegetableOnPlate);
            _vegetableOnPlate = null;
            UIManager.Instance.UpdatePlateStatus("...", m_IsPlayer1);
            UpdatePlayerVegetableStatus();
        }
    }
    
    private void PickSalad()
    {
        _salad.isPickedUp = true;
        UIManager.Instance.UpdatePlayerStatus(_salad.ToString(), m_IsPlayer1);
        UIManager.Instance.UpdateChoppingBoardStatus("...", m_IsPlayer1);
    }

 
    private void DumpVegetables()
    {
        if (_salad.isEmpty || !_salad.isPickedUp)
            return;

        UIManager.Instance.UpdatePlayerStatus("...", m_IsPlayer1);
        _salad.Clear();
    }


    private void ServeToCustomer()
    {
        if (_salad.isEmpty)
            return;

        ScoreController.Instance.CalculateScore(m_IsPlayer1, _salad, _currentCustomer);
        UIManager.Instance.UpdatePlayerStatus("...", m_IsPlayer1);
        _salad.isPickedUp = false;
        _salad.Clear();
    }

}
