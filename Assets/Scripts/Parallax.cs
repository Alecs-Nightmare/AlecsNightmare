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

	void Update ()
    {
        if (cam == null)
        {
            ReferenceCamera();
        }
        else
        {
            for (int i = 0; i < backgrounds.Length; i++)
            {
                //the movement to do this frame in opposite direction from the camera
                float deltaMovement = (previousCamPos.x - cameraTransform.position.x) * speedFactor[i];

                Vector3 targetPos = new Vector3(deltaMovement + backgrounds[i].transform.position.x, backgrounds[i].transform.position.y, backgrounds[i].transform.position.z);

                backgrounds[i].position = targetPos;
            }
            previousCamPos = cameraTransform.position;
        }
	}

    private void ReferenceCamera()
    {
        print("Locating camera...");
        cam = GameObject.Find("Main Camera");
        cameraTransform = cam.transform;
        previousCamPos = cameraTransform.position;
    }
}
