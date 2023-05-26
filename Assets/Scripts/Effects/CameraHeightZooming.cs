using Cinemachine;
using Dreamteck.Splines;
using UnityEngine;

[RequireComponent(typeof(SplineProjector))]
public class CameraHeightZooming : MonoBehaviour
{
    public Car Car;
    [SerializeField] private float _moveSpeed = 0.05f;
    [SerializeField] private float _allowOffsetWalue = 1;

    private float _currentGroundSurfacePoint;
    private SplineProjector _projector;

    private void Awake()
    {
        _projector = GetComponent<SplineProjector>();
    }

    private void Update()
    {
        if (Physics.Raycast(Car.transform.position, Vector3.down, out RaycastHit hit))
        {
            _currentGroundSurfacePoint = hit.point.y;
            Debug.DrawRay(Car.transform.position, Vector3.down * 100f, Color.red);
        }

        if (Car.transform.position.y - _currentGroundSurfacePoint >= _allowOffsetWalue)
        {
            _projector.motion.offset = Vector2.Lerp(_projector.motion.offset,
                new Vector2(0, Car.transform.position.y - _currentGroundSurfacePoint), _moveSpeed * Time.deltaTime);

        }
        else
        {
            _projector.motion.offset = Vector2.Lerp(_projector.motion.offset,
                new Vector2(0f, 0f), _moveSpeed * Time.deltaTime);
        }
    }
}
