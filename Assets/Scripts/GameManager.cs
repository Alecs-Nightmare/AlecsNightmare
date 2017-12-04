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
    public Object gameOverScene;                            // Reference to the Game Over scene
    public Object creditsScene;                             // Reference to the Credits scene
    public Object returnScene;                              // Reference to the scene to reset the game at
    public GameObject playerPrefab;                         // Reference to the player's prefab
    public int gameState;                                   // 1 --> Running / 0 --> Pause / -1 --> End
    public int lifes = 3;                                   // Chances the player has to respawn before Game Over
    [SerializeField]
    private int level = 0;                                  // Current level number (scene)
    [SerializeField]
    private int checkpoint = 0;                             // Active checkpoint
    private GameObject[] checkpointList;                    // References to all the checkpoints of the active level
    private GameObject[] levelAgentsList;                   // Array with pausable objects
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
        gameState = 1;
    }

    // Update is called every frame
    void Update()
    {
        if (loading)
        {
            if (m_AsyncLoaderCoroutine != null)
            {
                if (m_AsyncLoaderCoroutine.progress >= 0.9f)    // Scene almost loaded! (set allowSceneActivation to true when done to automatically switch scenes)
                {
                    m_AsyncLoaderCoroutine.allowSceneActivation = true;
                }
                if (m_AsyncLoaderCoroutine.isDone)              // Scene completely loaded! Start level routines...
                {
                    if (gameState >= 0) // if this is a level...
                    {
                        // Reset checkpoint references
                        ResetCheckpoints();

                        // Spawn the player
                        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                        player.transform.position = GetRespawnTransform().position;
                        print("Player has been spawned!");
                    }

                    // Rebuild level array
                    levelAgentsList = FindObjectsOfType<GameObject>();

                    // Loading is complete!
                    loading = false;
                    print(SceneManager.GetActiveScene().name + " is ready!");
                }
            }
        }
        else if (Input.GetKeyDown("return"))    // if not loading then handle the input...
        {
            switch (gameState)
            {
                case 1:     // Pause game   --if we have an ingame option menu, we should invoke it from here--
                    PauseGame(true);
                    gameState = 0;
                    print("Game paused!");
                    break;

                case 0:     // Resume game
                    PauseGame(false);
                    gameState = 1;
                    print("Game resumed!");
                    break;

                case -1:    // Reset game
                    // --do stuff--
                    break;
            }
        }
    }

    // Starts the next level loading coroutine
    void StartLevelLoadingRoutine(int lvl)
    {
        level = lvl;    // Thus we can load an arbitrary level from the inspector and start the routine from it
        string scene = string.Concat("Scene", lvl.ToString());
        print("Searching " + scene + "...");

        // Checks if it exists the next scene and starts loading coroutine
        if (scenesInBuild.Contains(scene))
        {
            // Stops everything on current scene
            PauseGame(true);

            // Prepares to load next level
            string current = SceneManager.GetActiveScene().name;
            print("Loading " + scene + "...");
            StartCoroutine(LoadSceneAsync(scene));
        }
        else    // if there's no more levels we finished the game!
        {
            print(scene + " has not been found on the Scene Array.");

            // Finish the game
            PauseGame(true);
#if UNITY_EDITOR
            EditorApplication.isPaused = true;
#endif
            gameState = -1;
            StartCoroutine(LoadSceneAsync(creditsScene.name));
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

    // Build the level list, should be called once!
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

    // Loading coroutine
    IEnumerator LoadSceneAsync(string scene)
    {
        m_AsyncLoaderCoroutine = SceneManager.LoadSceneAsync(scene);
        m_AsyncLoaderCoroutine.allowSceneActivation = false;
        loading = true;
        yield return m_AsyncLoaderCoroutine;
    }

    // Pause the scene
    void PauseGame(bool stop)
    {
        if (levelAgentsList != null)
        {
            foreach (GameObject obj in levelAgentsList)
            {
                if (obj.layer != this.gameObject.layer)     // Pauses everything which is not in the layer of who calls for this function
                {
                    obj.SetActive(!stop);
                }
            }
        }
    }

    // Checks a checkpoint and updates it
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

    // Get a transform.position to respawn at
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
