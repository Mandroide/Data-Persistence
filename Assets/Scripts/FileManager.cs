using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FileManager
{
    public static bool WriteToFile(string a_FileName, string a_FileContents)
    {
        var fullPath = System.IO.Path.Combine(Application.persistentDataPath, a_FileName);
        bool val;
        try
        {
            System.IO.File.WriteAllText(fullPath, a_FileContents);
            val = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to write to {fullPath} with exception {e}");
            val = false;
        }

        return val;
    }

    public static bool LoadFromFile(string a_FileName, out string result)
    {
        var fullPath = System.IO.Path.Combine(Application.persistentDataPath, a_FileName);
        bool val;
        try
        {
            result = System.IO.File.ReadAllText(fullPath);
            val = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to read from {fullPath} with exception {e}");
            result = "";
            val = false;
        }
        return val;
    }
}
