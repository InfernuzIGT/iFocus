using Events; // Required
using UnityEngine;

public class ExampleEvent : MonoBehaviour
{
    #region Send

    private GameEvent _event;

    private void Start()
    {
        _event = new GameEvent();
        // _event.Variable

        EventController.TriggerEvent(_event);
    }

    #endregion

    #region Receive

    private void OnEnable()
    {
        EventController.AddListener<GameEvent>(OnTestFunction);
    }
    
    private void OnDisable()
    {
        EventController.RemoveListener<GameEvent>(OnTestFunction);
    }
    
    private void OnTestFunction(GameEvent evt)
    {
        
    }

    #endregion

}