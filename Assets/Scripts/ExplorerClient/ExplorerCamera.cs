using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerCamera : BoltSingletonPrefab<ExplorerCamera>
{
    public Camera myCamera;
    [Header("Rotation Settings")]
    [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

    private Vector2 _mouseAxis;
    [SerializeField] private float moveSensitivity = 0.3f;

    private Vector3 _offset;
    
    //for closing camera
    private float _minSize = 5f;
    private float _maxSize = 15f;
    [SerializeField] private float scrollSensitivity = 2f;

    //for moving camera
    private bool _pressingScroll;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        myCamera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        InputEventManager.inputEvent.mouseWheelMoved += ChangeOrthoCameraSize;
        InputEventManager.inputEvent.onMouseMoved += SetMouseAxis;
        InputEventManager.inputEvent.mouseWheelClick += SetPressingScroll;

        _offset = transform.position;
    }

    private void OnDisable()
    {
        InputEventManager.inputEvent.mouseWheelMoved -= ChangeOrthoCameraSize;
        InputEventManager.inputEvent.onMouseMoved -= SetMouseAxis;
        InputEventManager.inputEvent.mouseWheelClick -= SetPressingScroll;
    }

    private void Update()
    {
        var translation = Vector3.zero;

       // if (_pressingScroll)
        {
         //   m_TargetCameraState.yaw += ChangeCameraRotation();
        }
        
        if (_pressingScroll)
        {
            // Framerate-independent interpolation
            // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / moveSensitivity) * Time.deltaTime);
            //var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / moveSensitivity) * Time.deltaTime);
            
            translation = _mouseAxis * moveSensitivity;
            Vector3 rotatedTranslation = transform.rotation * translation;
            Vector3 targetPos = rotatedTranslation + transform.position;
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSensitivity);
        }

    }

    void ChangeOrthoCameraSize(float scrollWheelAxis)
    {
        foreach (var cam in GetComponentsInChildren<Camera>())
        {
            var size = cam.orthographicSize;
            size -= scrollWheelAxis * scrollSensitivity;
            size = Mathf.Clamp(size, _minSize, _maxSize);
            cam.orthographicSize = size;
        }
    }
    
    void SetMouseAxis(Vector2 axis)
    {
        //TODO
        /*Set axis according to screen size. To avoid slow up-down camera movement*/
        _mouseAxis = axis;//myCamera.ScreenToWorldPoint(axis);
    }

    void SetPressingScroll(bool pressing)
    {
        _pressingScroll = pressing;
    }

    float ChangeCameraRotation()
    {
        var mouseMovement = _mouseAxis * -1f;
                
        var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

        return mouseMovement.x * mouseSensitivityFactor;
    }

    public void BeamCamera(Vector3 toPos)
    {
        toPos = new Vector3(toPos.x,transform.position.y,toPos.z);
        this.transform.position = toPos;
    }
    
}

