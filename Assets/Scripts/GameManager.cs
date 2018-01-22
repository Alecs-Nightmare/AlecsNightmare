using System.Collections;
using System.Collections.Generic;   // Much lists very fun wow -Doge
using UnityEngine;
using UnityEngine.SceneManagement;  // Allows to manage scences
using UnityEngine.UI;               // Allows to manage canvas objects
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
    [SerializeField]
    private GameObject lifeCounter;
    [SerializeField]
    private GameObject chipCounter;
    [SerializeField]
    private GameObject calmedIcon;
    [SerializeField]
    private GameObject warningIcon;
    [SerializeField]
    private GameObject warningFace;
    [SerializeField]
    private int lifes;                                      // Chances the player has to respawn before Game Over
    private int initLifes = 3;
    [SerializeField]
    private int currentSanity;                              // We separate current and maxim values because it can be increased during the game
    private int MaxSanity = 100;                            // This is the main resource  
    [SerializeField]
    private int chips = 0;
    [SerializeField]
    private int level = 0;                                  // Current level number (scene)
    [SerializeField]
    private int checkpoint = 0;                             // Active checkpoint
    private GameObject[] checkpointList;                    // References to all the checkpoints of the active level
    private GameObject[] levelAgentsList;                   // Array with pausable objects
    private List<string> scenesInBuild = new List<string>();
    private AsyncOperation m_AsyncLoaderCoroutine;
    private bool loading;
    private int gameState;                                  // 1 --> Running / 0 --> Pause / -1 --> End / -2 --> Resetting...
    public Fade fade;
    [SerializeField]
    private float fadeDuration = 1.0f;

    private GameObject chipSoundObj;
    private AudioSource chipSFX;

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

        // Set up references
        fade = GameObject.FindGameObjectWithTag("Fade").GetComponentInChildren<Fade>();
        chipSoundObj = GameObject.Find("ChipSFX");
        chipSFX = chipSoundObj.GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        BuildSceneList();
        ResetManager();
        StartLevelLoadingRoutine(level);
    }

    // Update is called every frame
    void Update()
    {
        print(chipSFX);
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
                    // if this is a level...
                    if (gameState >= 0)
                    {
                        // Reset the game
                        ResetCheckpoints();
                        currentSanity = MaxSanity;

                        // Spawn the player
                        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                        player.transform.position = ResetPlayer().position;
                        print("Player has been spawned!");
                    }

                    // if game has finished...
                    else if (gameState == -3)
                    {
                        Object.Destroy(this.gameObject);
                    }

                    // Rebuild level array
                    levelAgentsList = FindObjectsOfType<GameObject>();

                    // Loading is complete!
                    loading = false;
                    PauseGame(false);
                    fade.FadeToBlack(false, fadeDuration);  // NO FUNCIONA! (pero necesario para salir de negro)
                    print(SceneManager.GetActiveScene().name + " is ready!");
                }
            }
        }
        else if (!loading && gameState == -3)
        {
            ResetManager();
            StartLevelLoadingRoutine(level);
        }
        /*else if (Input.GetKeyDown("return"))    // if not loading then handle the input...    CHANGE "RETURN" FOR THE DESIRED CONDITION
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

                case -1:    // Resets the game
                    gameState = -3;
                    LoadSpecificScene(returnScene.name);
                    print("Resetting the game...");
                    break;
            }
        }*/
    }

    // Resets the Game Manager attributes   --CHANGE THEM HERE TO MAKE IT PERMANENT--
    void ResetManager()
    {
        lifes = initLifes;
        lifeCounter.GetComponent<UnityEngine.UI.Text>().text = "x" + lifes.ToString();
        chips = 0;
        chipCounter.GetComponent<UnityEngine.UI.Text>().text = "x" + chips.ToString();
        level = 0;
        gameState = 1;
        warningFace.SetActive(false);
        warningIcon.SetActive(false);
        calmedIcon.SetActive(true);
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
            LoadSpecificScene(creditsScene.name);
            gameState = -1;
            print("Thanks for playing!");
        }
    }

    // Loads a specifid scene out of the level loop
    void LoadSpecificScene(string name)
    {
        // Pauses the current scene
        PauseGame(true);

        // Pauses the emulation on the editor
#if UNITY_EDITOR
        //EditorApplication.isPaused = true;
#endif

        // Fades the canvas
        if (fade == null)
        {
            fade = GameObject.FindGameObjectWithTag("Fade").GetComponentInChildren<Fade>();
        }
        //fade.FadeToBlack(false, fadeDuration);    //  NO FUNCIONA!

        // Start scene loading coroutine
        StartCoroutine(LoadSceneAsync(name));
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
        if (stop)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        /*if (levelAgentsList != null)
        {
            foreach (GameObject obj in levelAgentsList)
            {
                if (obj.layer != this.gameObject.layer)     // Pauses everything which is not in the layer of who calls for this function
                {
                    obj.SetActive(!stop);
                }
            }
        }*/
    }

    // Adds (true) or substracts (false) one life
    private void AddSubsLife(bool adds)
    {
        if (adds)
        {
            lifes++;
            print("Lifes: " + lifes);
        }
        else
        {
            lifes--;
            print("Lifes: " + lifes);
            if (lifes < 1)
            {
                // --play GAME OVER SFX--

                // (IMPROVE GAME OVER ROUTINE HERE)
                //LoadSpecificScene(gameOverScene.name);
                gameState = -1;
                print("GAME OVER");
            }
            else
            {
                // --play death SFX--
            }
        }
        lifeCounter.GetComponent<UnityEngine.UI.Text>().text = "x" + lifes.ToString();
    }

    public int GetLifes()
    {
        return lifes;
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
    public Transform ResetPlayer()  // --still not sure who calls for this funcion upon respawning on death--
    {
        currentSanity = MaxSanity;
        warningFace.SetActive(false);
        //fade.FadeToBlack(false, fadeDuration);  // NO FUNCIONA!

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

    public int GetCurrentSanity()
    {
        return currentSanity;
    }

    public void SetCurrentSanity(int sanity)
    {
        currentSanity = sanity;
    }

    // Increases chip counter and adds lifes
    public void AddChips()
    {
        chips++;
        if (chips > 99)
        {
            while (chips > 99)
            {
                chips -= 100;
                AddSubsLife(true);
                // --INSERT LIVE UP SFX HERE--
            }
        }
        else
        {
            // --INSERT COLLECT CHIP SFX HERE--
            chipSFX.Play();
        }
        chipCounter.GetComponent<UnityEngine.UI.Text>().text = "x" + chips.ToString();
        print("Chips: " + chips);
    }

    // Call to recover sanity
    // (negative values recovers 50% by default)
    public void RecoverSanity(int recover)
    {
        if (recover >= 0)
        {
            currentSanity += recover;
            if (currentSanity > MaxSanity) { currentSanity = MaxSanity; }
        }
        else
        {
            currentSanity += MaxSanity / 2;
        }

        if (currentSanity > MaxSanity)
        {
            currentSanity = MaxSanity;
        }

        /*if (currentSanity >= MaxSanity/2)
        {
            warningFace.SetActive(false);
        }*/
    }

    // Call to substract sanity
    public void SubstractSanity(int substract)
    {
        currentSanity -= substract;

        /*if (currentSanity < MaxSanity / 2)
        {
            warningFace.SetActive(true);
        }*/

        if (currentSanity <= 0)
        {
            currentSanity = 0;
            AddSubsLife(false);
        }
        print("Sanity: " + currentSanity);
    }

    public bool CheckSanity(int substract, int threshold)
    {
        bool lives = true;
        if (currentSanity - substract <= threshold)
        {
            lives = false;
        }
        return lives;
    }

    public void DamageRoutine(bool hit)
    {
        warningFace.SetActive(hit);
    }

    public void DeathRoutine(bool death)
    {
        warningFace.SetActive(death);
        warningIcon.SetActive(death);
        calmedIcon.SetActive(!death);
        // --INSERT DEATH SFX HERE--
        //fade.FadeToBlack(true, fadeDuration); // NO FUNCIONA!
    }

    public void GameOver()
    {
        LoadSpecificScene(gameOverScene.name);
    }

}
