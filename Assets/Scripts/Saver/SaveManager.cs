using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance => _instance;

    private string savePath;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "DawnFall_save.json");
    }

    public SaveData Load()
    {
        if(!File.Exists(savePath))
        {
            Debug.Log("[SAVE] No existe archivo de guardado. Devolviendo SaveData en blanco.");
            return new SaveData();
        }
        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("[Save] Partida Guardada en: " + savePath);
    }

    public bool SaveExists()
    {
        return File.Exists(savePath);   
    }

}
