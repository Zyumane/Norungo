using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    private static GameProgressManager _instance;
    public static GameProgressManager Instance => _instance;

    [Header("Estado Del Juego")]
    public bool[] spareParts = new bool[5];
    public bool[] foundDocCollectable = new bool[15];

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveFileGame()
    {
        PlayerManager player = FindObjectOfType<PlayerManager>();
        if(player == null)
        {
            Debug.LogWarning("[PROGRESS] No se encontró PlayerManager al guardar.");
            return;
        }
        SaveData data = new SaveData();
        data.FoundSpareParts = spareParts;
        data.foundDocumentsCollectable = foundDocCollectable;
        data.posX = player.transform.position.x;
        data.posY = player.transform.position.y;
        data.posZ = player.transform.position.z;

        SaveManager.Instance.SaveGame(data);
    }

    public void LoadFileGame()
    {
        if (!SaveManager.Instance.SaveExists())
        {
            Debug.LogWarning("[PROGRESS] No existe partida guardada.");
            return;
        }

        SaveData data = SaveManager.Instance.Load();

        spareParts = data.FoundSpareParts;
        foundDocCollectable = data.foundDocumentsCollectable;

        PlayerManager player = FindObjectOfType<PlayerManager>();
        if (player != null)
        {
            player.transform.position = new Vector3(data.posX, data.posY, data.posZ); 
        }
    }


}
