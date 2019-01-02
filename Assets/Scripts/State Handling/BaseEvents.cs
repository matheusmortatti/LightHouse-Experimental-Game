using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEvents {

    UnityEvent CallbackOnTrigger;

    public void Invoke(GameObject obj)
    {
        CallbackOnTrigger.Invoke();
    }
	
}

[System.Serializable]
public class ObjectEvent : UnityEvent<GameObject> { };
