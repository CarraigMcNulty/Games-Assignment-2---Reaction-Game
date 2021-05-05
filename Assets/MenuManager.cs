using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class MenuManager : MonoBehaviour
{
    UnityAdvertising unityAdObject;
    public Admob admobObject;
    

    // Start is called before the first frame update
    void Start()
    {
        unityAdObject = gameObject.GetComponent<UnityAdvertising>();
        admobObject = gameObject.GetComponent<Admob>(); 

        Advertisement.Initialize(UnityAdvertising.gameId, UnityAdvertising.testMode);

        admobObject.RequestBanner();  
    }
    public void LoadScene(int level)
    {
        Advertisement.Banner.Hide();

        admobObject.destroyBanner();
      
        SceneManager.LoadScene("gamescene");  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
