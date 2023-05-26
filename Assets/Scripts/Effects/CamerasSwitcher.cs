using Cinemachine;
using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;

public class CamerasSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _projectorFollowAndLookAtCar;
    [SerializeField] private CinemachineVirtualCamera _projectorFollowAndLookAtProjector;
    [SerializeField] private float _offsetMistake = 0.5f;
    [SerializeField] private float _actionDuration = 2f;
    [SerializeField] private float _rebindDelay = 2f;
    
    public Mover CarMover;
    public SplineProjectorObserver ProjectorObserver;

    private Respawner _carRespawner;
    private MMFeedbacks _accelerationFeedBacks; 

    private void Awake()
    {
        _carRespawner = CarMover.GetComponent<Respawner>();
        _accelerationFeedBacks = GetComponent<MMFeedbacks>();
    }

    private void OnEnable()
    {
        _carRespawner.Proceed += OnChangeCameraBodyBindingMode;
        CarMover.Boosted += OnPlayAccelerationEffect;
    }

    private void Update()
    {
        Transit();
    }

    private void OnDisable()
    {
        _carRespawner.Proceed -= OnChangeCameraBodyBindingMode;
        CarMover.Boosted -= OnPlayAccelerationEffect;
    }

    private void Transit()
    {
        float criticalOffset = CarMover.GetCriticalOffset() + _offsetMistake;
        bool isSwitched = ProjectorObserver.IsGoesBeyondCriticalDistance(criticalOffset);
        _projectorFollowAndLookAtProjector.gameObject.SetActive(isSwitched);
    }

    private void OnChangeCameraBodyBindingMode()
    {
        StartCoroutine(RebindCameras(_rebindDelay));
    }

    private IEnumerator RebindCameras(float delay)
    {
        Transform previouseTargetCar = _projectorFollowAndLookAtCar.LookAt;
        Transform previouseTargetProjector = _projectorFollowAndLookAtProjector.LookAt;
        Transform previouseTargetProjectorFollow = _projectorFollowAndLookAtProjector.Follow;
        Transform previouseTargetCarFollow = _projectorFollowAndLookAtCar.Follow;

        _projectorFollowAndLookAtCar.LookAt = CarMover.transform;
        _projectorFollowAndLookAtProjector.LookAt = CarMover.transform;
        _projectorFollowAndLookAtCar.Follow = CarMover.transform;
        _projectorFollowAndLookAtProjector.Follow = CarMover.transform;
        _projectorFollowAndLookAtCar.GetCinemachineComponent<CinemachineTransposer>().m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp; 
        _projectorFollowAndLookAtProjector.GetCinemachineComponent<CinemachineTransposer>().m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;
        yield return new WaitForSeconds(delay);
        _projectorFollowAndLookAtCar.LookAt = previouseTargetCar;
        _projectorFollowAndLookAtProjector.LookAt = previouseTargetProjector;
        _projectorFollowAndLookAtProjector.Follow = previouseTargetProjectorFollow;
        _projectorFollowAndLookAtCar.Follow = previouseTargetCarFollow;
        _projectorFollowAndLookAtCar.GetCinemachineComponent<CinemachineTransposer>().m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
        _projectorFollowAndLookAtProjector.GetCinemachineComponent<CinemachineTransposer>().m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
    }

    private void OnPlayAccelerationEffect()
    {
        _accelerationFeedBacks?.PlayFeedbacks();
    }
}
