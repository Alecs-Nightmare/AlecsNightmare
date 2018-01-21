using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChildren : MonoBehaviour {

    public GameObject child1;
    public GameObject child2;
    public GameObject child3;

    private GameObject[] children;
    private Vector3[] spawnsList;

    public int spawnsNumber = 10;
    private float offsetBetweenSpawns;
    public float spawnLength = 20f;
    public float spawnHeight = 8f;
    public float frequency = 0.5f;
    public float offsetX; //cutre, lo sé.
    // Use this for initialization
    void Start() {

        offsetX = this.transform.position.x - spawnLength / 2;

        offsetBetweenSpawns = spawnLength / spawnsNumber;
        children = new GameObject[3];
        spawnsList = new Vector3[spawnsNumber];

        createSpawnsList();
        createChildrenList();
        SpawnAChild();

        Debug.Log(children.Length);
        Debug.Log(spawnsList.Length);

        InvokeRepeating("SpawnAChild", 2f, frequency);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void createChildrenList()
    {
        children[0] = child1;
        children[1] = child2;
        children[2] = child3;
    }
    public void createSpawnsList()
    {
        float spawnXPosition = -offsetBetweenSpawns;
        for (int i = 0; i<spawnsNumber; i++)
        {

            spawnXPosition += offsetBetweenSpawns;
            spawnsList[i] = new Vector3(spawnXPosition + offsetX, 8f, 0f);
            
        }
        

    }
    public void SpawnAChild()
    {

        GameObject child = children[Random.Range(0, children.Length)];
        Vector3 spawnPosition = spawnsList[Random.Range(0, spawnsList.Length)];
        Instantiate(child, spawnPosition, Quaternion.identity);
        print("Instanciado un crio");

    }
}
