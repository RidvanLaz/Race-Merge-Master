using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;

public enum AdResult
{
	Finished,
	Closed
}

public class Ads : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
	private static Ads _instance;
	private static Action<AdResult> _onRewardedAdResult;
	private static bool _isInitialized;
	private static InterstitialAd _admobInterstitial;
	private static RewardedAd _admobRewarded;
	private static bool _isUnityInterstitialLoaded;
	private static bool _isUnityRewardedLoaded;

	[Header("AdMob")]
	[SerializeField]
	private string admobInterstitialID;

	[SerializeField]
	private string admobRewardedID;

	[Header("Unity Ads")]
	[SerializeField]
	private string gameID;

	[SerializeField]
	private string unityInterstitialID;

	[SerializeField]
	private string unityRewardedID;

	private void Awake()
	{
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
			Destroy(this);
			return;
        }

        MobileAds.Initialize(_ => OnInitializationComplete());
		Advertisement.Initialize(gameID, Application.isEditor, this);
		DontDestroyOnLoad(this);
	}

	public void OnInitializationComplete()
	{
		_isInitialized = true;
		PreloadRewarded();
	}

	public void OnInitializationFailed(UnityAdsInitializationError error, string message)
	{
		Debug.LogError($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
	}

	public static void PreloadInterstitial()
	{
		if (!_isInitialized) return;
		if (_admobInterstitial != null) return;

		AdRequest request = new AdRequest.Builder().Build();
		InterstitialAd.Load(_instance.admobInterstitialID, request, OnInterstitialLoadComplete);
	}

	private static void OnInterstitialLoadComplete(InterstitialAd ad, LoadAdError error)
	{
		if (error == null)
		{
			_admobInterstitial = ad;
		}
		else
		{
			Debug.LogError(error);
			PreloadUnityInterstitial();
		}
	}

	private static void PreloadUnityInterstitial()
	{
		if (_isUnityInterstitialLoaded) return;

		Advertisement.Load(_instance.unityInterstitialID, _instance);
	}

	public static void PreloadRewarded()
	{
		if (!_isInitialized) return;
		if (_admobRewarded != null) return;

		AdRequest request = new AdRequest.Builder().Build();
		RewardedAd.Load(_instance.admobRewardedID, request, OnRewardedLoadComplete);
	}

	public void OnUnityAdsAdLoaded(string placementId)
	{
		if (placementId == unityInterstitialID)
		{
			_isUnityInterstitialLoaded = true;
		}
		else if (placementId == unityRewardedID)
		{
			_isUnityRewardedLoaded = true;
		}
	}

    public static bool IsRewardedAdReady()
    {
        return _isInitialized && (_isUnityRewardedLoaded || (_admobRewarded != null && _admobRewarded.CanShowAd()));
    }

    private static void OnRewardedLoadComplete(RewardedAd ad, LoadAdError error)
	{
		if (error == null)
		{
			_admobRewarded = ad;
			_admobRewarded.OnAdFullScreenContentClosed += OnRewardedVideoClosed;
			_admobRewarded.OnAdFullScreenContentFailed += OnRewardedVideoFailedToLoad;
			_admobRewarded.OnAdFullScreenContentFailed += OnRewardedVideoFailedToLoad;
		}
		else
		{
			Debug.LogError(error);
			PreloadUnityRewarded();
		}
	}

	private static void PreloadUnityRewarded()
	{
		if (_isUnityRewardedLoaded) return;

		Advertisement.Load(_instance.unityRewardedID, _instance);
	}

	public static bool ShowInterstitial(string placement)
	{
		if (!_isInitialized) return true;

		PreloadInterstitial();

		if (_admobInterstitial == null || !_admobInterstitial.CanShowAd())
		{
			PreloadUnityInterstitial();
			Advertisement.Show(_instance.unityInterstitialID, _instance);
			_isUnityInterstitialLoaded = false;
		}
		else
		{
			_admobInterstitial.Show();
			_admobInterstitial = null;
			//Analytics.ReportEvent(new AdEvent {Placement = placement});
		}

		return false;
	}

	public static bool ShowRewarded(string placement, Action<AdResult> onAdResult)
	{
		if (!_isInitialized)
		{
			onAdResult?.Invoke(AdResult.Closed);
			return true;
		}

		PreloadRewarded();
		_onRewardedAdResult = onAdResult;

		if (_admobRewarded == null || !_admobRewarded.CanShowAd())
		{
			PreloadUnityRewarded();
			Advertisement.Show(_instance.unityRewardedID, _instance);
			_isUnityRewardedLoaded = false;
		}
		else
		{
			_admobRewarded.Show(reward =>
            {
                OnRewardedVideoFinished(reward.Amount, reward.Type);
            });
		}

		return false;
	}

	public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
	{
		Debug.LogError($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");
		if (placementId == unityRewardedID)
		{
			OnRewardedVideoFailedToLoad($"{error.ToString()} - {message}");
		}
	}

	private static void OnRewardedVideoFailedToLoad(AdError error) => OnRewardedVideoFailedToLoad(error.ToString());

	private static void OnRewardedVideoFailedToLoad(string error)
	{
		Debug.LogError($"RewardedVideoFailedToLoad: {error}");
		_onRewardedAdResult?.Invoke(AdResult.Closed);
		ResetAdMobRewarded();
	}

	public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
	{
		Debug.LogError($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
	}

	public void OnUnityAdsShowStart(string placementId)
	{
	}

	public void OnUnityAdsShowClick(string placementId)
	{
	}

	public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
	{
		if (placementId == unityRewardedID)
		{
			switch (showCompletionState)
			{
				case UnityAdsShowCompletionState.COMPLETED:
					OnRewardedVideoFinished(0, "");
					break;
				default:
					OnRewardedVideoClosed();
					break;
			}
		}
	}

	private static void OnRewardedVideoFinished(double amount, string currency)
	{
		Debug.Log($"RewardedVideoFinished, Amount: {amount}, Currency: {currency}");
		_onRewardedAdResult?.Invoke(AdResult.Finished);
		ResetAdMobRewarded();
	}

	private static void OnRewardedVideoClosed()
	{
		Debug.Log("RewardedVideoClosed");
		_onRewardedAdResult?.Invoke(AdResult.Closed);
		ResetAdMobRewarded();
	}

	private static void ResetAdMobRewarded()
	{
		if (_admobRewarded != null)
		{
			_admobRewarded.OnAdFullScreenContentClosed -= OnRewardedVideoClosed;
			_admobRewarded.OnAdFullScreenContentFailed -= OnRewardedVideoFailedToLoad;
		}

		_admobRewarded = null;
        PreloadRewarded();
	}

	public static void RemoveAds()
	{
		if (_admobInterstitial != null)
		{
			_admobInterstitial.Destroy();
			_admobInterstitial = null;
		}

		if (_admobRewarded != null)
		{
			_admobRewarded.Destroy();
			_admobRewarded = null;
		}
	}

	private void OnDestroy()
	{
        if (_instance == this)
        {
            RemoveAds();
            _isInitialized = false;
            _admobInterstitial = null;
            ResetAdMobRewarded();
            _isUnityInterstitialLoaded = false;
            _isUnityRewardedLoaded = false;
            _instance = null;
        }
    }
}