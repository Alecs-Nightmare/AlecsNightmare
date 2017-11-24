using System.Collections;
using System.Collections.Generic;   // Much lists very fun wow -Doge
using UnityEngine;
using UnityEngine.SceneManagement;  // Allows to manage scences
//using UnityEditor;                  // Allows to instanciate an asset directly from Prefabs

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;              // Static instance of GameManager which allows it to be accessed by any other script
    //public GameObject player;                               // Player reference
    [SerializeField]
    private int level = 0;                                  // Current level number (scene)
    [SerializeField]
    private int checkpoint = 0;                             // Last checkpoint
    private GameObject[] checkpointList;                    // References all the checkpoints of a level
    private bool switchActive = false;                      // Scene load control bool

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
        // Call the InitGame function to initialize the first level
        // If we have a stating menu, we should stop right here!
        InitLevel(level);
    }

    // Update is called every frame
    void Update()
    {
        // Changes the active scene if previously full loaded
        if (switchActive)
        {
            string scene = string.Concat("Scene", level.ToString());
            try
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
            }
            catch
            {
                print("Scene not loaded yet!");
            }
            if (SceneManager.GetActiveScene().name == scene)
            {
                checkpointList = GameObject.FindGameObjectsWithTag("Checkpoint");
                print(SceneManager.GetActiveScene().name + " is active!");
                switchActive = false;
            }
        }
    }

    // Initializes the scene
    void InitLevel(int lvl)
    {
        level = lvl;    // Thus we can load an arbitrary level and start the routine from it
        string scene = string.Concat("Scene", lvl.ToString());
        print("Searching " + scene + "...");

        // Checks if it exists the next scene and loads it
        List<string> scenesInBuild = new List<string>();

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int lastSlash = scenePath.LastIndexOf("/");
            scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
            print("Added: " + scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
        }

        if (scenesInBuild.Contains(scene))
        {
            // Loads the next level
            string current = SceneManager.GetActiveScene().name;
            if (current != "Loader")
            {
                // Destroy all non-manager objects
                foreach (GameObject obj in Object.FindObjectsOfType<GameObject>())
                {
                    if (obj.tag != "Manager")
                    {
                        Destroy(obj);
                        //print("Destroyed: " + obj);
                    }
                }

                // Unloads current scene
                print("Unloading " + current + "...");
                SceneManager.UnloadSceneAsync(current);
            }
            //DontDestroyOnLoad(this.gameObject);
            print("Loading " + scene + "...");
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            switchActive = true;
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));  // this won't be ready on this frame

            // Reset checkpoints
            //checkpointList = GameObject.FindGameObjectsWithTag("Checkpoint");
            checkpoint = 0;

            // Spawn the player
            //Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Player.prefab", typeof(GameObject));
            //GameObject player = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            //player.transform.position = Vector2.one;
        }
        else
        {
            print("Thanks for playing!");
        }
    }

    public void UpdateCurrentCheckNum(int num)
    {
        print("Checking... Last: " + checkpoint + ", New: " + num + ", Array.Length: " + checkpointList.Length);
        if (num >= checkpoint)
        {
            checkpoint = num;

            // If this is the last checkpoint, loads the next level
            if (checkpoint + 1 >= checkpointList.Length)
            {
                level++;
                InitLevel(level);
            }
        }
    }

    public Transform Respawn()
    {
        GameObject active = checkpointList[0];
        foreach (GameObject cp in checkpointList)
        {
            CheckpointStats checkpointStats = cp.GetComponent<CheckpointStats>();
            if (checkpointStats.number == checkpoint)
            {
                active = cp;
            }
        }
        return active.transform;
    }
}
