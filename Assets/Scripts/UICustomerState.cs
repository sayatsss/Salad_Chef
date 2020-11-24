using UnityEngine;
using UnityEngine.UI;

public class UICustomerState : MonoBehaviour
{
    [SerializeField] private Text m_Order;
    [SerializeField] private Text m_Time;
    [SerializeField] private Image m_TimeBar;

    public void SetOrder(string text)
    {
        m_Order.text = text;
    }

    public void SetTime(float currentTime, float totalTime)
    {
        m_Time.text = ((int)currentTime).ToString();
        m_TimeBar.fillAmount = currentTime / totalTime;
    }
}
