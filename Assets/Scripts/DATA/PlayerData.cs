using System.Collections.Generic;
using System;

[Serializable]
public class LevelData
{
    public List<int> levelComplete = new List<int>();
    public List<int> levelSkip = new List<int>();
    public int currentLevel = 0;
}

[Serializable]
public class PlayerSetting
{
    public float volume = 1;
    public bool vibrateOn = true;
    public bool notifiOn = true;
}

public static class PlayerData
{
    static LevelData levelData = new LevelData();
    static PlayerSetting playerSetting = new PlayerSetting();

    static PlayerData()
    {
        LoadPlayerLevel();
    }

    #region Level
    public static void AddLevelComplete(int levelName)
    {
        if (levelData.levelComplete.Contains(levelName)) return;
        if(checkIfContainsSkipLevel(levelName)) //neu' choi lai, man` skip
        {
            levelData.levelComplete.Add(levelName);
            levelData.levelSkip.Remove(levelName);
        }
        else
        {
            levelData.levelComplete.Add(levelName);
            levelData.currentLevel++;
        }
        SavePlayerLevel();
    }

    public static void AddLevelSkip(int levelName)
    {
        if (levelData.levelSkip.Contains(levelName) || levelData.levelComplete.Contains(levelName)) return;

        levelData.levelSkip.Add(levelName);
        levelData.currentLevel++;
        SavePlayerLevel();
    }

    public static int GetCurrentLevelPlay()
    {
        return levelData.currentLevel;
    }

    public static bool checkIfContainsCompleteLevel(int numlevel)
    {
        if (levelData.levelComplete.Contains(numlevel))
            return true;
        return false;
    }

    public static bool checkIfContainsSkipLevel(int numlevel)
    {
        if (levelData.levelSkip.Contains(numlevel))
            return true;
        return false;
    }

    #endregion

    #region Setting
    public static void SetVolumeValue(float value)
    {
        playerSetting.volume = value;
        SavePlayerLevel();
    }
    public static void TurnOnVibration(bool t)
    {
        playerSetting.vibrateOn = t;
        SavePlayerLevel();
    }
    public static void TurnOnNotifi(bool t)
    {
        playerSetting.notifiOn = t;
        SavePlayerLevel();
    }
    public static float GetVolumeValue()
    {
        return playerSetting.volume;
    }
    public static bool GetVibrationState()
    {
        return playerSetting.vibrateOn;
    }
    public static bool GetNotifiState()
    {
        return playerSetting.notifiOn;
    }

    #endregion
    public static void LoadPlayerLevel()
    {
        levelData = BinarySerializer.Load<LevelData>("LevelData.txt");
        playerSetting = BinarySerializer.Load<PlayerSetting>("PlayerSetting.txt");
    }
    public static void SavePlayerLevel()
    {
        BinarySerializer.Save(levelData, "LevelData.txt");
        BinarySerializer.Save(playerSetting, "PlayerSetting.txt");
    }
}
