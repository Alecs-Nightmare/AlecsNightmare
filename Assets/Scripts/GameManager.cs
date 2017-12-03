using System.Collections;
using System.Collections.Generic;   // Much lists very fun wow -Doge
using UnityEngine;
using UnityEngine.SceneManagement;  // Allows to manage scences
#if UNITY_EDITOR
using UnityEditor;                  // Allows to instanciate an asset directly from Prefabs
#endif

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;              // Static instance of GameManager which allows it to be accessed by any other script
    [SerializeField]
    private int level = 0;                                  // Current level number (scene)
    [SerializeField]
    private int checkpoint = 0;                             // Active checkpoint
    private GameObject[] checkpointList;                    // References to all the checkpoints of the active level
    private List<string> scenesInBuild = new List<string>();
    private AsyncOperation m_AsyncLoaderCoroutine;
    private bool loading;

    // Awake is always called before any Start functions
    void Awake()
    {
        // Check if instance already exists and instances it if it doesn't
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        // Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        BuildSceneList();
        StartLevelLoadingRoutine(level);
    }

    // Update is called every frame
    void Update()
    {
        if (loading && m_AsyncLoaderCoroutine != null)
        {
            if (m_AsyncLoaderCoroutine.progress >= 0.9f)    // Scene almost loaded! (set allowSceneActivation to true when done to automatically switch scenes)
            {
                m_AsyncLoaderCoroutine.allowSceneActivation = true;
            }
            if (m_AsyncLoaderCoroutine.isDone)              // Scene completely loaded! Start level routines...
            {
                // Reset checkpoint references
                ResetCheckpoints();

                // Spawn the player
#if UNITY_EDITOR
                Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/CharacterPrefabs/Player.prefab", typeof(GameObject));
                GameObject player = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                player.transform.position = GetRespawnTransform().position;
#endif
                print("Player has been spawned!");

                loading = false;
                print(SceneManager.GetActiveScene().name + " is ready!");
            }
        }
    }

    // Starts the next level loading coroutine
    void StartLevelLoadingRoutine(int lvl)
    {
        level = lvl;    // Thus we can load an arbitrary level and start the routine from it
        string scene = string.Concat("Scene", lvl.ToString());
        print("Searching " + scene + "...");

        // Checks if it exists the next scene and starts loading coroutine
        if (scenesInBuild.Contains(scene))
        {
            // Prepares to load next level
            string current = SceneManager.GetActiveScene().name;
            print("Loading " + scene + "...");
            StartCoroutine(LoadSceneAsync(scene));
            loading = true;
        }
        else
        {
            print(scene + " has not been found on the Scene Array.");

            // Finish the game
#if UNITY_EDITOR
            EditorApplication.isPaused = true;
#endif
            print("Thanks for playing!");
        }
    }

    // Rebuilds the checkpoint array and sets the initial one as active
    void ResetCheckpoints()
    {
        checkpointList = GameObject.FindGameObjectsWithTag("Checkpoint");
        checkpoint = checkpointList[0].GetComponent<CheckpointStats>().GetNumber();
        foreach (GameObject checkpt in checkpointList)
        {
            if (checkpoint > checkpt.GetComponent<CheckpointStats>().GetNumber())
            {
                checkpoint = checkpt.GetComponent<CheckpointStats>().GetNumber();
            }
        }
    }

    void BuildSceneList()
    {
        scenesInBuild = new List<string>();

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int lastSlash = scenePath.LastIndexOf("/");
            scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
            print("Added: " + scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
        }
    }

    IEnumerator LoadSceneAsync(string scene)
    {
        m_AsyncLoaderCoroutine = SceneManager.LoadSceneAsync(scene);
        m_AsyncLoaderCoroutine.allowSceneActivation = false;
        yield return m_AsyncLoaderCoroutine;
    }

    public void UpdateCurrentCheckNum(int num)
    {
        print("Checking... Last: " + checkpoint + ", New: " + num + ", Array.Length: " + checkpointList.Length);
        if (num >= checkpoint)
        {
            checkpoint = num;

            // If this is the last checkpoint, calls for loading the next level
            if (checkpoint + 1 >= checkpointList.Length)
            {
                level++;
                StartLevelLoadingRoutine(level);
            }
        }
    }

    public Transform GetRespawnTransform()  // --still not sure who calls for this funcion upon respawning on death--
    {
        GameObject active = checkpointList[0];
        foreach (GameObject checkpt in checkpointList)
        {
            CheckpointStats checkpointStats = checkpt.GetComponent<CheckpointStats>();
            if (checkpointStats.GetNumber() == checkpoint)
            {
                active = checkpt;
            }
        }
        return active.transform;
    }
}
