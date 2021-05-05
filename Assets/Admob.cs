using UnityEngine;
using GoogleMobileAds.Api;

public class Admob : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitial;
    public RewardedAd rewardedAd;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        loadInterstitial();
        loadReward();
    }

    public void loadReward() {

        
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";

        this.rewardedAd = new RewardedAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
        //this.rewardedAd.Show();
    }

    public void Requestreward()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }

        loadReward();
    }

    public void loadInterstitial()
    {
        //MobileAds.Initialize(initStatus => { });
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void RequestInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }

        loadInterstitial();
    }

    public void destroyBanner()
    {
        this.bannerView.Hide();
    }

    public void RequestBanner()
    {    
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        
        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
