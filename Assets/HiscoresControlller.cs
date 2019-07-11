using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class HiscoresControlller : MonoBehaviour {

    private string secretKey = "mySecretKey"; // Edit this value and make sure it's the same as the one stored on the server
    public string addScoreURL = "http://localhost/madrollers/addscore.php?"; //be sure to add a ? to your url
    public string highscoreURL = "http://localhost/madrollers/display.php";
 
    void Start()
    {
        StartCoroutine(GetScores());
    }
 
    // remember to use StartCoroutine when calling this function!
    IEnumerator PostScores(string name, int score)
    {
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = Md5Test.Md5Sum(name + score + secretKey);
 
        string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
 
        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done
 
        if (hs_post.error != null)
        {
            print("There was an error posting the high score: " + hs_post.error);
        }
    }
 
    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    IEnumerator GetScores()
    {
        WWW receivedData = new WWW(highscoreURL);
        yield return receivedData;

        if (receivedData.error != null)
        {
            print("There was an error getting the high score: " + receivedData.error);
        }
        else
        {
            string[] allData = Regex.Split(receivedData.text, "</n>");

            for (var i = 0; i < allData.Length-1; i++)
            {
                string[] userData = Regex.Split(allData[i], ":");
                string userName = userData[0];
                string score = userData[1];
                print("Name = " + userName + " score: " + score);
            }
        }

    }
 
}

