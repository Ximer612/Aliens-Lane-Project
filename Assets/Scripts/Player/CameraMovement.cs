using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _playerRef;
    [SerializeField] Transform _playerCameraRef;
    [SerializeField] Transform _playerOrientation;
    [SerializeField] InputManager _inputManager;

    [Header("Variables")]
    [SerializeField] Vector2 _sensitivity;
    [SerializeField] Vector2 _inputCameraDirection;
    [SerializeField] Vector2 _rotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        transform.position = _playerRef.transform.position;

        _inputCameraDirection = new Vector2(-_inputManager.GetInput.CameraInput.y, _inputManager.GetInput.CameraInput.x) * Time.fixedDeltaTime * _sensitivity;

        _rotation += new Vector2(_inputCameraDirection.x, _inputCameraDirection.y);
        _rotation.x = Mathf.Clamp(_rotation.x, -90f, 90f);

        _playerCameraRef.transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
        _playerOrientation.rotation = Quaternion.Euler(0, _rotation.y, 0);
    }
}
