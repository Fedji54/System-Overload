using UnityEngine;

namespace WinterUniverse
{
    public class PlayerCameraManager : MonoBehaviour
    {
        [SerializeField] private Transform _verticalRoot;
        [SerializeField] private float _followSpeed = 4f;
        [SerializeField] private float _horizontalRotationSpeed = 100f;
        [SerializeField] private float _verticalRotationSpeed = 50f;
        [SerializeField] private float _minLookAngle = -15f;
        [SerializeField] private float _maxLookAngle = 45f;
        [SerializeField] private float _collisionRadius = 0.25f;
        [SerializeField] private float _collisionAvoidanceSpeed = 8f;

        private Camera _camera;
        private Vector2 _lookInput;
        private Vector3 _camLocalPosition;
        private float _lookAngle;
        private float _cameraDefaultOffset;
        private float _cameraTargetOffset;

        public Camera Camera => _camera;

        public void Initialize()
        {
            _camera = GetComponentInChildren<Camera>();
            _cameraDefaultOffset = _camera.transform.localPosition.z;
        }

        public void HandleUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, GameManager.StaticInstance.Player.transform.position, _followSpeed * Time.deltaTime);
            GetInput();
            HandleRotation();
            HandleCollision();
        }

        private void GetInput()
        {
            _lookInput = GameManager.StaticInstance.PlayerInput.LookInput;
        }

        private void HandleRotation()
        {
            if (_lookInput.x != 0f)
            {
                transform.Rotate(Vector3.up * _lookInput.x * _horizontalRotationSpeed * Time.deltaTime);
            }
            if (_lookInput.y != 0f)
            {
                _lookAngle = Mathf.Clamp(_lookAngle - (_lookInput.y * _verticalRotationSpeed * Time.deltaTime), _minLookAngle, _maxLookAngle);
                _verticalRoot.localRotation = Quaternion.Euler(Vector3.right * _lookAngle);
            }
        }

        private void HandleCollision()
        {
            _cameraTargetOffset = _cameraDefaultOffset;
            Vector3 direction = (_camera.transform.position - _verticalRoot.position).normalized;
            if (Physics.SphereCast(_verticalRoot.position, _collisionRadius, direction, out RaycastHit hit, Mathf.Abs(_cameraTargetOffset), GameManager.StaticInstance.WorldLayer.ObstacleMask))
            {
                _cameraTargetOffset = -(Vector3.Distance(_verticalRoot.position, hit.point) - _collisionRadius);
            }
            if (Mathf.Abs(_cameraTargetOffset) < _collisionRadius)
            {
                _cameraTargetOffset = -_collisionRadius;
            }
            _camLocalPosition.z = Mathf.Lerp(_camera.transform.localPosition.z, _cameraTargetOffset, _collisionAvoidanceSpeed * Time.deltaTime);
            _camera.transform.localPosition = _camLocalPosition;
        }
    }
}