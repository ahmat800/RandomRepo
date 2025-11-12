using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    private static string path = Path.Combine(Application.persistentDataPath, "levelData.json");


    public static void SaveLevel(LevelData levelData)
    {
        string json = JsonUtility.ToJson(levelData, true);


        File.WriteAllText(path, json);
    }

    public static LevelData LoadLevel()
    {

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);
            return levelData;
        }
        else
        {
            return null;
        }
    }
    public static void ClearLevelData()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static bool IsThereSavedData() 
    {
        return File.Exists(path);
    }
}

[Serializable]
public class LevelData 
{
    public int remainingMovesCount;
    public int score;
    public List<CardData> cards;
}

[Serializable]
public class CardData 
{
    public int cardId;
    public string cardSprite;
    public bool isVisible;
}
