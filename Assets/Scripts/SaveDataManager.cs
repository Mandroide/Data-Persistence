using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager
{
    private static readonly string filename = "highestscore.json";
    public static void SaveJsonData(IEnumerable<ISaveable> a_saveables)
    {
        SaveData saveData = new SaveData();
        foreach (var saveable in a_saveables)
        {
            saveable.PopulateSaveData(saveData);
        }

        if (FileManager.WriteToFile(filename, saveData.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public static void LoadJsonData(IEnumerable<ISaveable> a_saveables)
    {
        if (FileManager.LoadFromFile(filename, out string json))
        {
            SaveData saveData = new SaveData();
            saveData.LoadFromJson(json);
            foreach (var saveable in a_saveables)
            {
                saveable.LoadFromSaveData(saveData);
            }

            Debug.Log("Load complete");
        }
    }
}
