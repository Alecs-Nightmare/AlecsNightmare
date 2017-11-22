using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class RaycastController : MonoBehaviour {
    public LayerMask collisionMask;

    public const float skinWidth = .015f;
    public int horizontalRayCount = 4;//greater or equal than 2
    public int verticalRayCount = 4;//greater or equal than 2

    [HideInInspector]
    public float horizontalRaySpacing;

    [HideInInspector]
    public float verticalRaySpacing;


    [HideInInspector]
    public BoxCollider2D m_collider;
    public RaycastOrigins raycastOrigins;

    public virtual void Awake()
    {
        m_collider = GetComponent<BoxCollider2D>();
    }

    // Use this for initialization
    public virtual void Start()
    {
        CalculateRaySpacing();
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void CalculateRaySpacing()
    {
        Bounds bounds = m_collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);


    }
    public void UpdateRaycastOrigins()
    {
        Bounds bounds = m_collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }
    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;



    }
}
