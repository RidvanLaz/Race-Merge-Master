using MoreMountains.Feedbacks;
using UnityEngine;

public class FloatinTextHandler : MonoBehaviour
{
    [SerializeField] private MMFeedbacks _floatingTextFeedback;
    public ScenePointsPool _scenePointsPool;

    public void Init(ScenePointsPool scenePointsPool)
    {
        _scenePointsPool = scenePointsPool;
        _scenePointsPool.PointsWthidrawed += OnPlayFeedbacks;
    }

    private void OnDestroy()
    {
        _scenePointsPool.PointsWthidrawed -= OnPlayFeedbacks;
    }

    private void OnPlayFeedbacks(int textValue)
    {
        _floatingTextFeedback?.PlayFeedbacks(transform.position, textValue);
    }
}
