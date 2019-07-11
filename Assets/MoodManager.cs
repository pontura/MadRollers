using UnityEngine;
using System.Collections;
using System;

public class MoodManager : MonoBehaviour {
    
    [Serializable]
    public class Mood
    {
       // public int id;
      //  public string floorTexture;
      //  public string backgroundTexture;
     //   public Color fogColor;
      //  public Color cameraColor;
    }

    public Mood[] moods;

	public Mood GetMood (int id) 
    {
        foreach (Mood mood in moods)
        {
           // if (mood.id == id) return mood;
        }
        return null;
	}

    public void Changes(int id)
    {
		return;
    }
}
