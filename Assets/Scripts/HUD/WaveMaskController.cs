using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMaskController : MonoBehaviour {

    GameManager manager;
    RectTransform transform;
    GameObject cameraObject;
    Camera camera;
    private Vector3 basePosition;
    private Vector3 newPosition;
    private Vector3 displacement;
    private Vector3 height;
    private float targetHeight;
    private float currentHeight = 0f;
    private float timer = 0f;
    [SerializeField]
    private float waveScope = 10f;
    [SerializeField]
    private float waveVelocity = 2f;
    [SerializeField]
    private float fillingScope = 2.4f;
    [SerializeField]
    private float fillingVelocity = 100f;
    [SerializeField]
    private float normalizingFactor = 1.33f;

    // Set up references
    void Awake ()
    {
        manager = GetComponentInParent<GameManager>();
        transform = GetComponent<RectTransform>();
    }

    // Use this for initialization
    void Start ()
    {
        basePosition = transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        timer += Time.smoothDeltaTime;
        while (timer >= 360) { timer -= 360; }
        displacement = new Vector3(-Mathf.Sin(timer), Mathf.Sin(timer), 0f);

        targetHeight = manager.GetCurrentSanity();
        if (currentHeight < targetHeight)
        {
            currentHeight += fillingVelocity*Time.smoothDeltaTime;
            if (currentHeight > targetHeight) { currentHeight = targetHeight; }
        }
        else if (currentHeight > targetHeight)
        {
            manager.DamageRoutine(true);
            currentHeight -= fillingVelocity * Time.smoothDeltaTime;
            if (currentHeight < targetHeight)
            {
                currentHeight = targetHeight;
                if (currentHeight > 0) { manager.DamageRoutine(false); }
            }
        }

        /*
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        camera = cameraObject.GetComponent<Camera>();
        if (camera != null)
        {
            camera.ResetAspect();
            fillingScope = camera.aspect * normalizingFactor;
        }
        */

        height = new Vector3(0f, fillingScope*currentHeight, 0f);
        newPosition = basePosition + waveScope * waveVelocity * displacement + height;
    }

    // LateUpdate is called once before rendering
    private void LateUpdate ()
    {
        transform.position = newPosition;
    }

    public Vector3 GetHeight()
    {
        return height;
    }
}
