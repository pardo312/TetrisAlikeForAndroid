using JiufenGames.TetrisAlike.Logic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private string m_inputKey = "";
    private TouchInputListenerInput m_inputsListener;

    public void Start()
    {
        InputsController._instance.m_inputsListener.ForEach((item) =>
        {
            if (item.GetType() == typeof(TouchInputListenerInput))
            {
                m_inputsListener = item as TouchInputListenerInput;
            }
        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_inputsListener.PressedButton(m_inputKey);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_inputsListener.UnPressedButton(m_inputKey);
    }
}