using UnityEngine;


public class CameraController : MonoBehaviour 
{
    #region Private Data
    [SerializeField] private Transform _targetPlayer;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _zoomSpeed = 4f;
    [SerializeField] private float _minZoom = 5f;
    [SerializeField] private float _maxZoom = 15f;
    [SerializeField] private float _pitch = 2f;

    private Transform _transform;
    private float _currentZoom = 10f;
    private float _currentRotation = 0f;
    private float _prevMouseX;
    #endregion


    #region Properties
    public Transform Target { set { _targetPlayer = value; } }
    #endregion


    #region Unity Methods
    private void Start () 
    {
        _transform = transform;
    }
	
	private void Update () 
    {
        if (_targetPlayer != null) 
        {
            _currentZoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);

            if (Input.GetMouseButton(2)) 
            {
                _currentRotation += Input.mousePosition.x - _prevMouseX;
            }
        }
        _prevMouseX = Input.mousePosition.x;
    }

    private void LateUpdate() 
    {
        if (_targetPlayer != null) 
        {
            _transform.position = _targetPlayer.position - _offset * _currentZoom;
            _transform.LookAt(_targetPlayer.position + Vector3.up * _pitch);
            _transform.RotateAround(_targetPlayer.position, Vector3.up, _currentRotation);
        }
    }
    #endregion
}
