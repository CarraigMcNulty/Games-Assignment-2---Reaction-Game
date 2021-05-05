using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class gamescript : MonoBehaviour
{
    public double timeRemaining;
    public bool timerIsRunning = false;
    public bool objectSpawn = false; 
    bool showUnityInterstitial;
    bool showUnityRewarded;
    bool showAd = false;
    public Button pausebtn;
    gameState state;
    int cubeCount;
    public Admob adobj;
    public GameObject cube; 
    private static int playerLives;
    double objTimeRemaining;
    GameObject savedObject;
    public UnityAdvertising unityAd;
    public GPgameServices gpServices;
    public TextMeshProUGUI textmesh;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject GameOverBG;
    [SerializeField] private Text playerScoreAsString;
    public TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI playerLivesString;


    void Start()
    {
        gpServices = gameObject.GetComponent<GPgameServices>();
        state = gameState.COUNTDOWN;
        playerLives = 3;
        cubeCount = 0;
        objTimeRemaining = .5;
        showUnityRewarded = true;
        showUnityInterstitial = false;

        gpServices.updateTenScore();


    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case gameState.COUNTDOWN:

                playerLivesString.SetText(playerLives.ToString());

                textmesh.enabled = true;
                GameOverBG.SetActive(false);
                Time.timeScale = 1;
                startTimer();
                
            break;

            case gameState.PLAY:

                scoreText.enabled = true;

                playerLivesString.SetText(playerLives.ToString());

                Time.timeScale = 1;

                if (savedObject == null)
                {
                    savedObject = Instantiate(cube, generateCoords(), Quaternion.identity);
                }

                objectTimer();

                if (Input.touchCount == 1 && playerLives > 0)
                {
                    Ray ourRay = Camera.main.ScreenPointToRay(Input.touches[0].position);
                    RaycastHit info;

                    if (Physics.Raycast(ourRay, out info))
                    {
                        GameObject object_hit = info.transform.GetComponent<GameObject>();

                        if (Input.touches[0].phase == TouchPhase.Ended && state == gameState.PLAY)
                        {  
                            savedObject.transform.position = generateCoords();
                            cubeCount = cubeCount + 1;
                            scoreText.text = cubeCount.ToString();

                        }
                    }      

                    else if (!EventSystem.current.IsPointerOverGameObject(0))
                    {
                        if ( Input.touches[0].phase == TouchPhase.Ended)
                        {
                            playerLives = subtractLife(playerLives);
                        }
                    } 
                }

                else if (playerLives == 0)
                {
                    showAd = true;
                    gpServices.unlockFirstDeath();
                    state = gameState.END;
                }

            break;

            case gameState.PAUSED:

                StartCoroutine(unityAd.ShowBannerWhenInitialized());
                        
                timeRemaining = 2;
                Time.timeScale = 0;
                
                pauseMenuUI.SetActive(true);

                if (savedObject != null)
                {
                    savedObject.SetActive(false);
                }

                break;

            case gameState.END:

                if (showAd == true )
                {
                    if (showUnityInterstitial)
                    {
                        unityAd.ShowInterstitialAd();
                        showUnityInterstitial = false;
                    }

                    else
                    {
                       adobj.RequestInterstitial();
                       showUnityInterstitial = true;
                    }    
                   
                   showAd = false;
                }

                PlayerPrefs.SetInt("Score", cubeCount);
                gpServices.updateLeaderboard();
                
                playerLivesString.SetText(playerLives.ToString());

                Time.timeScale = 0;
                playerScoreAsString.text = "Score: " + cubeCount.ToString();
                scoreText.enabled = false;
                GameOverBG.SetActive(true);
                savedObject.SetActive(false);
                break;
        }
    }
   
    public void continueGame()
    {
        if (showUnityRewarded)
        {   
            unityAd.ShowRewardedVideo();
            showUnityRewarded = false;
        }

        else
        {
            adobj.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            adobj.Requestreward();
            showUnityRewarded = true;
        }

        playerLivesString.SetText(playerLives.ToString());
    }

    public void pauseGame()
    { 
        if (state == gameState.PAUSED)
        {
            Advertisement.Banner.Hide();
            scoreText.enabled = false;
            pauseMenuUI.SetActive(false);
            savedObject.SetActive(true);
            timerIsRunning = true;
            state = gameState.COUNTDOWN; 
        }

        else
        {   
            scoreText.enabled = false;
            state = gameState.PAUSED;
        }
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quit()
    {
        SceneManager.LoadScene("Menu");   
    }

    private enum gameState { PLAY, PAUSED, END, COUNTDOWN };
    private System.Random random = new System.Random();

    void startTimer()
    {
        if (timerIsRunning)
        {
            if ((int)timeRemaining >= 1)
            {
                textmesh.SetText(((int)timeRemaining).ToString());
                timeRemaining -= Time.deltaTime;

            }
            else if (timeRemaining < 1 && timeRemaining > 0)
            {
                textmesh.SetText("GO!");

                timeRemaining -= Time.deltaTime;

            }
            else
            {
                timerIsRunning = false;
                state = gameState.PLAY;
                textmesh.enabled = false;
            }
        }   
    }

   void objectTimer()
    {
        if ((int)objTimeRemaining >= 1)
        {
            objTimeRemaining -= Time.deltaTime;
        }

        else if (objTimeRemaining < 1 && objTimeRemaining > 0)
        {
            objTimeRemaining -= Time.deltaTime;
        }

        else
        {
            savedObject.transform.position = generateCoords();
            playerLives = subtractLife(playerLives);   
        }
    }

    Vector3 generateCoords()
    {
        //Generate random x/y coords
        double y = random.NextDouble();
        double x = random.NextDouble();

        //Apply random x/y coords to witin the camer viewport
        Vector3 randomCoord = Camera.main.ViewportToWorldPoint(new Vector3((float)x, (float)y, 1));
        randomCoord.z = 1;

        //set object time to live
        objTimeRemaining = 1;

        return randomCoord;
    }

    public void giveReward()
    {
        savedObject.SetActive(true);
        if(playerLives == 0)
        {
            playerLives = addLife(playerLives);
        }
      
        timerIsRunning = true;
        timeRemaining = 4;
        state = gameState.COUNTDOWN;
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        giveReward();
    }

    public int subtractLife(int playerlives)
    {
        playerlives = playerlives - 1;
        playerLivesString.SetText(playerLives.ToString());

        return playerlives;
    }

    public int addLife(int playerlives)
    {
        playerlives = playerlives + 1;
        playerLivesString.SetText(playerLives.ToString());

        return playerlives;
    }
}
