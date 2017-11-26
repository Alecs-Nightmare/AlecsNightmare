using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    [System.Serializable]
    struct EntitySpawnData
    {
        [SerializeField] public Color pixelColor;
        [SerializeField] public GameObject prefabToSpawn;
    }

    public Texture2D colorMap;
    public String prefabName = "customLevelPrefab";
    public float ratio = 3;
    [SerializeField]List<EntitySpawnData> entitiesToSpawn = new List<EntitySpawnData>();
    GameObject parent;

    void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        //Create the parent Object
        parent = new GameObject("environment"){ tag = "Environment"};

        for (int x = 0; x < colorMap.width; x++)
        {
            for (int y = 0; y < colorMap.height; y++)
            {
                GenerateEntity(x, y);
            }
        }
    }

    private void GenerateEntity(int x, int y)
    {
        Color pixelColor = colorMap.GetPixel(x, y);

        //Discard the transparent pixels
        if (pixelColor == Color.white || pixelColor.a == 0)
            return;

        for (int i = 0; i < entitiesToSpawn.Count; i++)
        {
            if (pixelColor == entitiesToSpawn[i].pixelColor)
            {
                Vector2 spawnPos = new Vector2(x/ratio, y/ratio);
                Instantiate(entitiesToSpawn[i].prefabToSpawn, spawnPos, Quaternion.identity,parent.transform);
            }
        }
    }
}
