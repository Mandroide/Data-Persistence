using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string name;
    public int score;

    public SaveData(string name = "", int score = 0)
    {
        this.name = name;
        this.score = score;
    }

    public override string ToString()
    {
        return $"{nameof(name)}={name}\t{nameof(score)}={score}";
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_json)
    {
        JsonUtility.FromJsonOverwrite(a_json, this);
    }
}

public interface ISaveable
{
    void PopulateSaveData(SaveData a_SaveData);
    void LoadFromSaveData(SaveData a_SaveData);
}