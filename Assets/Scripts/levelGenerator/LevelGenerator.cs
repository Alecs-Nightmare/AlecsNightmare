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
    public float ratio = 3;
    [SerializeField]List<EntitySpawnData> entitiesToSpawn = new List<EntitySpawnData>();

    void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
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
        if (pixelColor == Color.white)
            return;

        print(pixelColor);

        for (int i = 0; i < entitiesToSpawn.Count; i++)
        {
            if (pixelColor == entitiesToSpawn[i].pixelColor)
            {
                Vector2 spawnPos = new Vector2(x/ratio, y/ratio);
                Instantiate(entitiesToSpawn[i].prefabToSpawn, spawnPos, Quaternion.identity,transform);
            }
        }
    }
}
