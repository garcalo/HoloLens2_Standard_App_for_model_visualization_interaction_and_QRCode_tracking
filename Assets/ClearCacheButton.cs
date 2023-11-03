// Author: GAA

using System.IO;
using UnityEngine;

public class ClearCacheButton : MonoBehaviour
{
    public void ClearCacheFolder()
    {
        string cacheFolderPath = Path.Combine(Application.persistentDataPath, "cache");

        if (Directory.Exists(cacheFolderPath))
        {
            // Delete all files in the cache folder
            string[] files = Directory.GetFiles(cacheFolderPath);
            foreach (string file in files)
            {
                File.Delete(file);
            }

            // Delete all subdirectories in the cache folder
            string[] subDirectories = Directory.GetDirectories(cacheFolderPath);
            foreach (string subDir in subDirectories)
            {
                Directory.Delete(subDir, true);
            }

            // Clear all player prefs too
            PlayerPrefs.DeleteAll();

            Debug.Log("Cache folder contents cleared successfully.");
        }
        else
        {
            Debug.Log("Cache folder does not exist.");
        }
    }
}
