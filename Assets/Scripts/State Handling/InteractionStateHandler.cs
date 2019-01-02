using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionStateHandler : MonoBehaviour {

    public InteractionStateEvents[] Events;

    [SerializeField]
    private InteractionStateNames currentState = 0;

    void Start()
    {
        InvokeEvents();
    }

    public void ChangeState(InteractionStateNames state)
    {
        currentState = state;

        InvokeEvents();
    }

    public void ChangeState(int state)
    {
        currentState = (InteractionStateNames)state;

        InvokeEvents();
    }

    public InteractionStateNames GetCurrentState()
    {
        return currentState;
    }

    private void InvokeEvents()
    {
        foreach (InteractionStateEvents e in Events)
        {
            if (e.eventTrigger == currentState)
            {
                e.Invoke();
            }
        }
    }

}

[System.Serializable]
public enum InteractionStateNames
{
    HANDSFREE = 0, HOLDING = 1, FOCUSING = 2
};

[System.Serializable]
public class InteractionStateEvents
{
    public InteractionStateNames eventTrigger;
    //public GameObject ObjectParam;
    //public ObjectEvent objectEvents;
    public UnityEvent events;

    public void Invoke()
    {
        //objectEvents.Invoke(ObjectParam);
        events.Invoke();
    }
}
