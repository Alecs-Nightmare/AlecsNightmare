using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientController : MonoBehaviour {

    WaveMaskController dynamicMask;
    RectTransform transform;
    private Vector3 basePosition;

    // Set up references
    void Awake ()
    {
        dynamicMask = GetComponentInParent<WaveMaskController>();
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
		
	}

    // LateUpdate is called once before rendering
    private void LateUpdate ()
    {
        transform.position = basePosition + dynamicMask.GetHeight();
    }
}
