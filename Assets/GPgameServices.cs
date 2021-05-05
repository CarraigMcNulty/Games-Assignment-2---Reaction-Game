using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPgameServices : MonoBehaviour
{
    // Start is called before the first frame update
    public void openLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }
     public void openAchievements()
    {
        Social.ShowAchievementsUI();
    }
    
    public void updateLeaderboard()
    {
      if (PlayerPrefs.GetInt("Score") > 0)
      {
            Social.ReportScore(PlayerPrefs.GetInt("Score"),GPGSIds.leaderboard_high_score, null);
      }
    }

    public void updateTenScore()
    {
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_so_it_begins, 1 , null);
    }

    public void unlockFirstDeath()
    {

        Social.ReportProgress(GPGSIds.achievement_first_of_many, 100f, null);

    }


}
