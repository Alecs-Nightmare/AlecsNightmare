using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float smoothing = 1.0f;

    [SerializeField] private Transform[] backgrounds;
    [SerializeField] private float[] speedFactor;
    [SerializeField] private Transform cam;
    [SerializeField] private Vector3 previousCamPos;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    private void Start()
    {
        previousCamPos = cam.position;
    }

	void Update ()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            //the movement to do this frame in opposite direction from the camera
            float deltaMovement = (previousCamPos.x - cam.position.x) * speedFactor[i];

            Vector3 targetPos = new Vector3(deltaMovement + backgrounds[i].transform.position.x, backgrounds[i].transform.position.y, backgrounds[i].transform.position.z);

            backgrounds[i].position = targetPos;
        }

        previousCamPos = cam.position;
	}
}
