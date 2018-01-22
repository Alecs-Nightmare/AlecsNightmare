using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float smoothing = 1.0f;

    [SerializeField]
    private Transform[] backgrounds;
    [SerializeField]
    private float[] speedFactor;
    //[SerializeField]
    private Transform cameraTransform;
    //[SerializeField]
    private Vector3 previousCamPos;
    private GameObject cam;
    private float initY;
    private float newY;
    [SerializeField]
    private Vector3 initDisplacement;

    /*
    void Awake()
    {
        //cameraTransform = Camera.main.transform;
    }
    */

    /*
    void Start()
    {
        camera = GameObject.Find("Main Camera");
        cameraTransform = camera.transform;
        previousCamPos = cameraTransform.position;
    }
    */

    private void Update()
    {
        if (cam == null)
        {
            ReferenceCamera();
        }
        else
        {
            newY = initY + (initY - cameraTransform.position.y) * -0.66f;
        }
    }

    void LateUpdate ()
    {
        if (cam != null)
        {
            for (int i = 0; i < backgrounds.Length; i++)
            {
                //the movement to do this frame in opposite direction from the camera
                float deltaMovementX = (previousCamPos.x - cameraTransform.position.x) * speedFactor[i];

                Vector3 targetPos = new Vector3(deltaMovementX + backgrounds[i].transform.position.x, backgrounds[i].transform.position.y, backgrounds[i].transform.position.z);

                //backgrounds[i].position = targetPos;
                backgrounds[i].position = new Vector3(targetPos.x, newY, targetPos.z) + initDisplacement;
            }
            previousCamPos = cameraTransform.position;
        }
	}

    private void ReferenceCamera()
    {
        print("Locating camera...");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        if (cam != null)
        {
            cameraTransform = cam.transform;
            previousCamPos = cameraTransform.position;
            initY = cameraTransform.position.y;
        }
    }
}
