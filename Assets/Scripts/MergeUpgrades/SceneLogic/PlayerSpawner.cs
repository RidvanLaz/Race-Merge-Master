using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

[DefaultExecutionOrder(int.MinValue)]
public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Data _data;
    [SerializeField] private List<CustomizableCar> _cars = new List<CustomizableCar>();

    [Header("Level")] 
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _carSpawnPoint;
    [SerializeField] private CamerasSwitcher _camerasSwitcher;
    [SerializeField] private Projector _projector;
    [SerializeField] private CameraHeightZooming _cameraHeightZooming;
    [SerializeField] private HighSpeedEffect[] _highSpeedEffect;
    [SerializeField] private SplineProjector[] _splineProjector;
    [SerializeField] private CameraPointFollover _cameraPointFollover;
    [SerializeField] private PlaceShower _placeShower;


    [Header("For Car")] 
    [SerializeField] private VariableJoystick _joystick;
    [SerializeField] private CarsObserver _carObserver;
    [SerializeField] private Transform _startRespawnPoint;
    [SerializeField] private SplineComputer _splineComputer;
    [SerializeField] private Transform _instancePlace;
    [SerializeField] private Path _path;
    [SerializeField] private ScenePointsPool _scenePointsPool;

    private CustomizableCar _currentCar;

    private void Awake()
    {
        _data.Load();
        _currentCar = Instantiate(_cars[_data.Options.KuzovUpgrade - 1], transform);
        _currentCar.ActivateWheels(_data.Options.WheelsUpgrade - 1);
        _currentCar.transform.position = _carSpawnPoint.position;
        _currentCar.transform.rotation = _carSpawnPoint.rotation;

        _cameraPointFollover._anchor = _currentCar.transform;
        _camerasSwitcher.CarMover = _currentCar.GetComponent<Mover>();
        _camerasSwitcher.ProjectorObserver = _currentCar.GetComponent<SplineProjectorObserver>();

        _projector.Car = _currentCar.GetComponent<Car>();
        _cameraHeightZooming.Car = _currentCar.GetComponent<Car>();

        foreach (var highSpeedEffect in _highSpeedEffect)
        {
            highSpeedEffect.CarMover = _currentCar.GetComponent<Mover>();
        }

        foreach (var splineProjector in _splineProjector)
        {
            splineProjector.projectTarget = _currentCar.transform;
        }

        _currentCar.GetComponent<PlayerInput>()._joystick = _joystick;
        _currentCar.GetComponent<SplineProjectorObserver>()._splineProjector = _splineProjector[0];
        _currentCar.GetComponent<Respawner>()._carsObserver = _carObserver;
        _currentCar.GetComponent<Respawner>()._respawnPoint = _startRespawnPoint;
        _currentCar.GetComponent<PathController>()._spline= _splineComputer;
        _currentCar.GetComponent<PathController>()._instancePlace= _instancePlace;
        _currentCar.GetComponent<PathController>()._path = _path;

        _currentCar.FloatingTextHandler.Init(_scenePointsPool);
        _currentCar.NameTextRotater._lookTarget = _camera;
        _placeShower._determinedCar = _currentCar.GetComponent<Car>();
    }
}
