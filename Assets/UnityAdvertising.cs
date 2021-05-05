
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class UnityAdvertising : MonoBehaviour, IUnityAdsListener
{
    public static string gameId = "4099329";
    public string bannerSurfacingId = "Banner_Android";
    string rewardSurfacingId = "Rewarded_Android";
    public static bool testMode = true;
    public static gamescript game;

    void Start()
    {
        Advertisement.Initialize(gameId, testMode);

        game = gameObject.GetComponent<gamescript>();
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        StartCoroutine(ShowBannerWhenInitialized());

        Advertisement.AddListener(this);
    }

    public UnityAdvertising()
    {
     
    }

   public void ShowRewardedVideo()
    {  
        //Check if UnityAds ready before calling Show method:
       if (Advertisement.IsReady(rewardSurfacingId))
       {   
            Advertisement.Show(rewardSurfacingId);
       }

        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    public IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        if(SceneManager.GetActiveScene().buildIndex == 0)
            Advertisement.Banner.Show(bannerSurfacingId);
    }

    public void ShowInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }

        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId.Equals(rewardSurfacingId))
        {
            game.giveReward();
        }
    }

    public void OnUnityAdsReady(string placementId) { }

    public void OnUnityAdsDidError(string message){}

    public void OnUnityAdsDidStart(string placementId) { }
  
}
