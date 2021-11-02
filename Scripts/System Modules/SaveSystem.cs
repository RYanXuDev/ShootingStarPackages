using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(string saveFileName, object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            File.WriteAllText(path, json);

            #if UNITY_EDITOR
            Debug.Log($"Successfully saved data to {path}.");
            #endif
        }
        catch (System.Exception exception)
        {
            #if UNITY_EDITOR
            Debug.LogError($"Failed to saved data to {path}. \n{exception}");
            #endif
        }
    }

    public static T Load<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);

            return data;
        }
        catch (System.Exception exception)
        {
            #if UNITY_EDITOR
            Debug.LogError($"Failed to load data from {path}. \n{exception}");
            #endif

            return default;
        }
    }

    public static void DeleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            File.Delete(path);
        }
        catch (System.Exception exception)
        {
            #if UNITY_EDITOR
            Debug.LogError($"Failed to delete {path}. \n{exception}");
            #endif
        }
    }

    public static bool SaveFileExists(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        return File.Exists(path);
    }
}