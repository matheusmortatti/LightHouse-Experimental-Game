using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float WalkSpeedNormal = 5.0f;
    [SerializeField]
    private float LookSensitivity = 5.0f;
    
    [Space]
    [SerializeField]
    private float CrouchHeight = -0.5f;
    [SerializeField]
    private float WalkSpeedCrouch = 3.0f;
    [SerializeField]
    private float crouchVelocity = 0.5f;

    [Space]
    [SerializeField]
    private Text interactionText;
    [SerializeField]
    private InputStateEvents[] baseInputStateEvents;

    private float WalkSpeed;

    private string interactionMessage = "";

    private InputStateEvents currentInputEvent;

    private UnityEvent InteractionEvents;

    private PlayerMotor motor;

    private float _xMov, _zMov, _xRot, _yRot;
    private InteractionStateHandler interactionStateHandler;

    private InputHandler inputHandler;

    private float crouchTimer = 0;
    private bool crouching = false;

    void Start()
    {
        WalkSpeed = WalkSpeedNormal;
        motor = GetComponent<PlayerMotor>();

        interactionStateHandler = FindObjectOfType<InteractionStateHandler>();

        inputHandler = GetComponentInChildren<InputHandler>();

        currentInputEvent = new InputStateEvents();
        UpdateEvents();
    }

    void Update()
    {
        UpdateEvents();
        UpdateMovement();
        //UpdateCrouch();

        //interactionText.text = interactionMessage;
        interactionMessage = "";
    }

    private void UpdateMovement()
    {
        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        Vector3 move_direction = (_movHorizontal + _movVertical);
        Vector3 _velocity = move_direction * WalkSpeed;

        Vector3 _rotation = new Vector3(-_xRot, _yRot, 0) * LookSensitivity;

        motor.ApplyVelocity(_velocity);
        motor.ApplyRotation(_rotation);

        _xMov = 0;
        _zMov = 0;
        _xRot = 0;
        _yRot = 0;
    }

    private void UpdateCrouch()
    {

        if(crouching)
        {
            Vector3 cameraPos = motor.camera.transform.localPosition;
            cameraPos = Vector3.Lerp(cameraPos, new Vector3(cameraPos.x, CrouchHeight, cameraPos.z), crouchTimer);

            motor.camera.transform.localPosition = cameraPos;

            crouchTimer += Time.deltaTime * crouchVelocity;

            WalkSpeed = WalkSpeedCrouch;
        }
        else
        {
            Vector3 cameraPos = motor.camera.transform.localPosition;
            cameraPos = Vector3.Lerp(cameraPos, new Vector3(cameraPos.x, 0, cameraPos.z), crouchTimer);

            motor.camera.transform.localPosition = cameraPos;

            crouchTimer += Time.deltaTime * crouchVelocity;

            WalkSpeed = WalkSpeedNormal;
        }
    }

    #region InputActions

    public void Crouch(bool buttonVal)
    {
        if (buttonVal)
        {
            crouching = !crouching;
            crouchTimer = 0;
        }
    }

    private float lastFocusButtonVal = 0;
    private GameObject handObject;
    public void Focus(float buttonVal)
    {
        
        
    }

    public void InteractionMessage(string message)
    {
        interactionMessage = message;
    }

    private void UpdateEvents()
    {
        currentInputEvent = FindCurrentEvent();
    }

    public void ApplyVerticalMovement(float val)
    {
        _zMov = val;
    }

    public void ApplyHorizontalMovement(float val)
    {
        _xMov = val;
    }

    public void ApplyLookX(float val)
    {
        _yRot = val;
    }

    public void ApplyLookY(float val)
    {
        _xRot = val;
    }

    #endregion

    #region ProcessInput

    public void ProcessZoomInAxis(float val)
    {
        if (currentInputEvent.AltZoomInEvent.eventCounter == 0)
        {
            currentInputEvent.ZoomInEvent.Invoke(val);
        }
        else
        {
            currentInputEvent.AltZoomInEvent.axisEvent.Invoke(val);
            RemoveAllAltListenersZoomIn();
        }
    }

    public void ProcessFocusAxis(float val)
    {
        if (currentInputEvent.AltFocusEvent.eventCounter == 0)
        {
            currentInputEvent.FocusEvent.Invoke(val);
        }
        else
        {
            currentInputEvent.AltFocusEvent.axisEvent.Invoke(val);
            RemoveAllAltListenersFocus();
        }
    }

    public void ProcessVerticalAxis(float val)
    {
        if (currentInputEvent.AltVerticalEvent.eventCounter == 0)
        {
            currentInputEvent.VerticalEvent.Invoke(val);
        }
        else
        {
            currentInputEvent.AltVerticalEvent.axisEvent.Invoke(val);
            RemoveAllAltListenersVertical();
        }
    }

    public void ProcessHorizontalAxis(float val)
    {
        if (currentInputEvent.AltHorizontalEvent.eventCounter == 0)
        {
            currentInputEvent.HorizontalEvent.Invoke(val);
        }
        else
        {
            currentInputEvent.AltHorizontalEvent.axisEvent.Invoke(val);
            RemoveAllAltListenersHorizontal();
        }
    }

    public void ProcessLookX(float val)
    {
        if (currentInputEvent.AltLookXEvent.eventCounter == 0)
        {
            currentInputEvent.LookXEvent.Invoke(val);
        }
        else
        {
            currentInputEvent.AltLookXEvent.axisEvent.Invoke(val);
            RemoveAllAltListenersLookX();
        }
    }

    public void ProcessLookY(float val)
    {
        if (currentInputEvent.AltLookYEvent.eventCounter == 0)
        {
            currentInputEvent.LookYEvent.Invoke(val);
        }
        else
        {
            currentInputEvent.AltLookYEvent.axisEvent.Invoke(val);
            RemoveAllAltListenersLookY();
        }
    }

    public void ProcessInteract(bool buttonValue)
    {
        // Interact with objects
        if (buttonValue)
        {
            if (currentInputEvent.AltInteractionEvent.eventCounter == 0)
            {
                currentInputEvent.InteractionEvent.Invoke();
            }
            else
            {
                currentInputEvent.AltInteractionEvent.unityEvent.Invoke();
            }
        }

        RemoveAllAltListenersInteract();
    }

    public void ProcessCrouch(bool buttonValue)
    {
        if (currentInputEvent.AltCrouchEvent.eventCounter == 0)
        {
            currentInputEvent.CrouchEvent.Invoke(buttonValue);
        }
        else
        {
            currentInputEvent.AltCrouchEvent.buttonEvent.Invoke(buttonValue);
        }

        RemoveAllAltListenersCrouch();
    }

    #endregion

    #region AltListeners

    public void SetAltListenerInteract(UnityAction call)
    {
        currentInputEvent.AltInteractionEvent.unityEvent.AddListener(call);
        currentInputEvent.AltInteractionEvent.eventCounter++;
    }

    public void RemoveAllAltListenersInteract()
    {
        currentInputEvent.AltInteractionEvent.unityEvent.RemoveAllListeners();
        currentInputEvent.AltInteractionEvent.eventCounter = 0;
    }

    public void SetAltListenerHorizontal(UnityAction<float> call)
    {
        currentInputEvent.AltHorizontalEvent.axisEvent.AddListener(call);
        currentInputEvent.AltHorizontalEvent.eventCounter++;
    }

    public void RemoveAllAltListenersHorizontal()
    {
        currentInputEvent.AltHorizontalEvent.axisEvent.RemoveAllListeners();
        currentInputEvent.AltHorizontalEvent.eventCounter = 0;
    }

    public void SetAltListenerVertical(UnityAction<float> call)
    {
        currentInputEvent.AltVerticalEvent.axisEvent.AddListener(call);
        currentInputEvent.AltVerticalEvent.eventCounter++;
    }

    public void RemoveAllAltListenersVertical()
    {
        currentInputEvent.AltVerticalEvent.axisEvent.RemoveAllListeners();
        currentInputEvent.AltVerticalEvent.eventCounter = 0;
    }

    public void SetAltListenerLookX(UnityAction<float> call)
    {
        currentInputEvent.AltLookXEvent.axisEvent.AddListener(call);
        currentInputEvent.AltLookXEvent.eventCounter++;
    }

    public void RemoveAllAltListenersLookX()
    {
        currentInputEvent.AltLookXEvent.axisEvent.RemoveAllListeners();
        currentInputEvent.AltLookXEvent.eventCounter = 0;
    }

    public void SetAltListenerLookY(UnityAction<float> call)
    {
        currentInputEvent.AltLookYEvent.axisEvent.AddListener(call);
    }

    public void RemoveAllAltListenersLookY()
    {
        currentInputEvent.AltLookYEvent.axisEvent.RemoveAllListeners();
    }

    public void SetAltListenerZoomIn(UnityAction<float> call)
    {
        currentInputEvent.AltZoomInEvent.axisEvent.AddListener(call);
    }

    public void RemoveAllAltListenersZoomIn()
    {
        currentInputEvent.AltZoomInEvent.axisEvent.RemoveAllListeners();
    }

    public void SetAltListenerFocus(UnityAction<float> call)
    {
        currentInputEvent.AltFocusEvent.axisEvent.AddListener(call);
    }

    public void RemoveAllAltListenersFocus()
    {
        currentInputEvent.AltFocusEvent.axisEvent.RemoveAllListeners();
    }

    public void SetAltListenerCrouch(UnityAction<bool> call)
    {
        currentInputEvent.AltCrouchEvent.buttonEvent.AddListener(call);
        currentInputEvent.AltCrouchEvent.eventCounter++;
    }

    public void RemoveAllAltListenersCrouch()
    {
        currentInputEvent.AltCrouchEvent.buttonEvent.RemoveAllListeners();
        currentInputEvent.AltCrouchEvent.eventCounter = 0;
    }

    #endregion

    private InputStateEvents FindCurrentEvent()
    {
        foreach(InputStateEvents e in baseInputStateEvents)
        {
            if(e.state == interactionStateHandler.GetCurrentState())
            {
                return e;
            }
        }

        return new InputStateEvents();
    }
}

[System.Serializable]
public class InputStateEvents
{
    public InputStateEvents()
    {
        VerticalEvent = new AxisUpdate();
        HorizontalEvent = new AxisUpdate();
        LookXEvent = new AxisUpdate();
        LookYEvent = new AxisUpdate();
        ZoomInEvent = new AxisUpdate();
        FocusEvent = new AxisUpdate();
        CrouchEvent = new ButtonUpdate();
        InteractionEvent = new UnityEvent();

        AltHorizontalEvent = new AxisEventParams();
        AltVerticalEvent = new AxisEventParams();
        AltLookXEvent = new AxisEventParams();
        AltLookYEvent = new AxisEventParams();
        AltZoomInEvent = new AxisEventParams();
        AltFocusEvent= new AxisEventParams();
        AltCrouchEvent = new ButtonEventParams();
        AltInteractionEvent = new UnityEventParams();
    }

    public InteractionStateNames state;

    public AxisUpdate VerticalEvent, HorizontalEvent, LookXEvent, LookYEvent, ZoomInEvent, FocusEvent;
    public ButtonUpdate CrouchEvent;
    public UnityEvent InteractionEvent;

    [HideInInspector]
    public AxisEventParams AltVerticalEvent, AltHorizontalEvent, AltLookXEvent, AltLookYEvent, AltZoomInEvent, AltFocusEvent;
    [HideInInspector]
    public ButtonEventParams AltCrouchEvent;
    [HideInInspector]
    public UnityEventParams AltInteractionEvent;

    public class UnityEventParams
    {
        public UnityEvent unityEvent;
        public int eventCounter;

        public UnityEventParams()
        {
            unityEvent = new UnityEvent();
            eventCounter = 0;
        }
    }

    public class AxisEventParams
    {
        public AxisUpdate axisEvent;
        public int eventCounter;

        public AxisEventParams()
        {
            axisEvent = new AxisUpdate();
            eventCounter = 0;
        }
    }

    public class ButtonEventParams
    {
        public ButtonUpdate buttonEvent;
        public int eventCounter;

        public ButtonEventParams()
        {
            buttonEvent = new ButtonUpdate();
            eventCounter = 0;
        }
    }
}
