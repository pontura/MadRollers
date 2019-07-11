 ﻿using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 
 public static class Utils {
 
     public static void RemoveAllChildsIn(Transform container)
     {
         int num = container.transform.childCount;
         for (int i = 0; i < num; i++) UnityEngine.Object.DestroyImmediate(container.transform.GetChild(0).gameObject);
     }
     public static void ShuffleListTexts(List<string> texts)
     {
         if (texts.Count < 2) return;
         for (int a = 0; a < 100; a++)
         {
             int id = Random.Range(1, texts.Count);
             string value1 = texts[0];
             string value2 = texts[id];
             texts[0] = value2;
             texts[id] = value1;
         }
     }
	public static class CoroutineUtil
	{
		public static IEnumerator WaitForRealSeconds(float time)
		{
			float start = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < start + time)
			{
				yield return null;
			}
		}
	}
	public static string FormatNumbers(int num)
	{
		return string.Format ("{0:#,#}",  num);
	}
    public static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
}
