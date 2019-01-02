using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour {

    [SerializeField]
    private AxisUpdate Walk_Vertical, Walk_Horizontal, Look_X, Look_Y, ZoomIn, Focus;

    [SerializeField]
    private ButtonUpdate Interact, Crouch;

    private Dictionary<string, float> lastAxisValue;

    void Start()
    {
        lastAxisValue = new Dictionary<string, float>();

        lastAxisValue["Vertical"] = 0;
        lastAxisValue["Horizontal"] = 0;
        lastAxisValue["Look X"] = 0;
        lastAxisValue["Look Y"] = 0;
        //lastAxisValue["Interact"] = 0;
        lastAxisValue["ZoomIn"] = 0;
        //lastAxisValue["Focus"] = 0;
        //lastAxisValue["Crouch"] = 0;
    }

    void LateUpdate()
    {
        HandleInput();
        UpdateKeyValues();
    }

    private void HandleInput()
    {
        Walk_Vertical.Invoke(Input.GetAxisRaw("Vertical"));
        Walk_Horizontal.Invoke(Input.GetAxisRaw("Horizontal"));
        Look_X.Invoke(Input.GetAxisRaw("Look X"));
        Look_Y.Invoke(Input.GetAxisRaw("Look Y"));
        ZoomIn.Invoke(Input.GetAxisRaw("ZoomIn"));
        //Focus.Invoke(Input.GetAxisRaw("Focus"));

        //Crouch.Invoke(GetKeyDown("Crouch"));
        //Interact.Invoke(GetKeyDown("Interact"));
    }

    private void UpdateKeyValues()
    {
        List<string> keyList = new List<string>(lastAxisValue.Keys);
        foreach(string Key in keyList)
        {
            lastAxisValue[Key] = Input.GetAxis(Key);
        }
    }

    private bool GetKeyDown(string key)
    {
        if(lastAxisValue.ContainsKey(key))
        {
            return lastAxisValue[key] == 0 && Input.GetAxis(key) == 1;
        }

        return false;
    }


    public void SetListenerInteract(UnityAction<bool> call)
    {
        Interact.AddListener(call);
    }

    public void RemoveAllListenersInteract()
    {
        Interact.RemoveAllListeners();
    }


    public void SetListenerLookY(UnityAction<float> call)
    {
        Look_Y.AddListener(call);
    }

    public void RemoveAllListenersLookY()
    {
        Look_Y.RemoveAllListeners();
    }

    public void SetListenerLookX(UnityAction<float> call)
    {
        Look_X.AddListener(call);
    }

    public void RemoveAllListenersLookX()
    {
        Look_X.RemoveAllListeners();
    }

    public void SetListenerVertical(UnityAction<float> call)
    {
        Walk_Vertical.AddListener(call);
    }

    public void RemoveAllListenersVertical()
    {
        Walk_Vertical.RemoveAllListeners();
    }

    public void SetListenerHorizontal(UnityAction<float> call)
    {
        Walk_Horizontal.AddListener(call);
    }

    public void RemoveAllListenersHorizontal()
    {
        Walk_Horizontal.RemoveAllListeners();
    }

    public void RemoveAllListeners()
    {
        Walk_Horizontal.RemoveAllListeners();
        Walk_Vertical.RemoveAllListeners();
        Look_Y.RemoveAllListeners();
        Look_X.RemoveAllListeners();
        Interact.RemoveAllListeners();
        Focus.RemoveAllListeners();
    }

}

[System.Serializable]
public class AxisUpdate : UnityEvent<float> { };

[System.Serializable]
public class ButtonUpdate : UnityEvent<bool> { };
