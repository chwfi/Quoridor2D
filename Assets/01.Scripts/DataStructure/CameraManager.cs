using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _maxZoom = 10f, _minZoom = 5f, _zoomAmount = 2f, _zoomSpeed = 5f;

    private float _camSize, _targetCamSize;
    
    private void Start()
    {
        _mainCam = Camera.main;
        _camSize = _targetCamSize = _mainCam.orthographicSize;
    }

    
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(x, y).normalized;

        Vector3 moveValue = _mainCam.transform.position + dir.normalized * _moveSpeed * Time.deltaTime;

        _mainCam.transform.position = moveValue;

        _targetCamSize -= Input.mouseScrollDelta.y * _zoomAmount;
        _targetCamSize = Mathf.Clamp(_targetCamSize, _minZoom, _maxZoom);

        _camSize = Mathf.Lerp(_camSize, _targetCamSize, Time.deltaTime * _zoomSpeed);
        _mainCam.orthographicSize = _camSize;
    }
}
