using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ObjectStates : MonoBehaviour {

    public ObjectStateEvents[] Events;

    [SerializeField]
    private ObjectStateNames currentState = 0;

    void Start()
    {
        InvokeEvents();
    }
	
    public void ChangeState(ObjectStateNames state)
    {
        currentState = state;

        InvokeEvents();
    }

    public void ChangeState(int state)
    {
        currentState = (ObjectStateNames)state;

        InvokeEvents();
    }

    private void InvokeEvents()
    {
        foreach(ObjectStateEvents e in Events)
        {
            if(e.eventTrigger == currentState)
            {
                e.Invoke();
            }
        }
    }
}

[System.Serializable]
public enum ObjectStateNames
{
    ONGROUND = 0, HOLDING = 1
};

[System.Serializable]
public class ObjectStateEvents
{
    public ObjectStateNames eventTrigger;
    //public GameObject ObjectParam;
    //public ObjectEvent objectEvents;
    public UnityEvent standardEvents;

    public void Invoke()
    {
        //objectEvents.Invoke(ObjectParam);
        standardEvents.Invoke();
    }
}

