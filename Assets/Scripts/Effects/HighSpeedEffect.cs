using UnityEngine;

public class HighSpeedEffect : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private ParticleSystem[] _windEffects;
    [Header("Options")]
    public Mover CarMover;
    [SerializeField] private float _emittingSpeed = 20f;

    private void Update()
    {
        if(CarMover.GetCurrentSpeed() > _emittingSpeed)
        {
            for (int i = 0; i < _windEffects.Length; i++)
            {
                _windEffects[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < _windEffects.Length; i++)
            {
                _windEffects[i].gameObject.SetActive(false);
            }
        }
    }
}
