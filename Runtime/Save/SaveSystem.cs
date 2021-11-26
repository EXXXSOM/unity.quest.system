using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public const string SAVE_NAME = "SaveData";
    public const string SAVE_EXTENSION = "sd";

    private static bool _saveLoaded = false;
    private static SaveData _currentSaveData = new SaveData(true);

    public static string GetSavePath => Application.persistentDataPath + "/";
    public static SaveData SaveData => _currentSaveData;
    public static bool SaveLoaded => _saveLoaded;

    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + SAVE_NAME + "." + SAVE_EXTENSION;
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(QuestSystem.Instance.GetSaveDataAllQuests());

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Game saved! Save path: " + path);
    }

    public static void LoadData(string nameSaveData = SAVE_NAME)
    {
        string path = Application.persistentDataPath + "/" + nameSaveData + "." + SAVE_EXTENSION;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            _currentSaveData = formatter.Deserialize(stream) as SaveData;
            if (_currentSaveData == null)
            {
                _saveLoaded = false;
            }
            else
            {
                _saveLoaded = true;
            }

            stream.Close();

            Debug.LogWarning("Game loaded!");
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
        }
    }
}
