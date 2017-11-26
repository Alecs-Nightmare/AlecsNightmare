using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSizeController : MonoBehaviour
{

    private float xdis, ydis;
    private float sizePlatformX;
    public float minSize;
    private int numRays;
    private Vector3 sizePlayer;
    private Vector3 temp;
    private Renderer rend;
    private RaycastHit2D[] arrayRayos;
    private bool playerInPlatform;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        sizePlayer = GameObject.Find("Player").GetComponent<BoxCollider2D>().bounds.size;
        sizePlatformX = GetComponent<Transform>().localScale.x;

        xdis = rend.bounds.size.x;
        ydis = rend.bounds.size.y;

        numRays = (int)Mathf.Ceil(xdis / sizePlayer.x) + 2; //calcula el numero de rayos dependiendo del tamaño de la plataforma
        arrayRayos = new RaycastHit2D[numRays];
    }

    void Update()
    {
        xdis = rend.bounds.size.x;
        ydis = rend.bounds.size.y;

        Vector2 topLeftCorner = new Vector2(transform.position.x - xdis / 2, transform.position.y + ydis / 2);

        for (int i = 0; i < numRays; i++)
        {
            Debug.DrawLine(topLeftCorner + new Vector2((xdis / (numRays - 1)) * i, 0), topLeftCorner + new Vector2((xdis / (numRays - 1)) * i, +0.05f), Color.green); //Origen, fin, color
            arrayRayos[i] = Physics2D.Raycast(topLeftCorner + new Vector2((xdis / (numRays - 1)) * i, 0), new Vector2(0, 1), 0.05f); //origen, direccion, longitud
        }

        playerInPlatform = false;

        foreach (RaycastHit2D rayo in arrayRayos) {
            if (rayo.collider != null) {
                if (rayo.collider.tag == "Player")
                    playerInPlatform = true;
            }
            
        }

        if (playerInPlatform)
            shrinkPlatform();
        else
            enlargePlatform();

    }

    private void shrinkPlatform() {
        if (transform.localScale.x > minSize)
        {
            temp = transform.localScale;
            temp.x -= Time.deltaTime;
            transform.localScale = temp;
        }
    }

    private void enlargePlatform() {
        if (transform.localScale.x < sizePlatformX)
        {
            temp = transform.localScale;
            temp.x += Time.deltaTime;
            transform.localScale = temp;
        }
    }
        
    
}

