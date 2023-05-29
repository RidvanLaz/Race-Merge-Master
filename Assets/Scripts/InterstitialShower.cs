using Dreamteck;
using UnityEngine;

public class InterstitialShower : Singleton<InterstitialShower>
{
    [Min(1)][SerializeField] private int _levelsToShowAd = 1;

    private int _currentLevels;

    public void TryShow()
    {
        _currentLevels++;

        if (_currentLevels >= _levelsToShowAd)
        {
            Ads.ShowInterstitial("");
            _currentLevels = 0;
        }
    }
}