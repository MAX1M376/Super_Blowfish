using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent (typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour
{
    private float lastJump;
    private Vector2 velocity;
    private Vector2 force;
    private CapsuleCollider2D capc;
    private Rigidbody2D rb;

    [Header("Physique :")]
    [SerializeField]
    private float gravityForce = 14f;

    [Header("Movement :")]
    [SerializeField]
    private float jumpForce = 10f;

    [SerializeField]
    private float timeBetweenJump = 0.5f;

    [SerializeField]
    [Range(0.1f, 3f)]
    private float distancePowerfullJump = 1.5f;

    [SerializeField]
    [Range(1f, 5f)]
    private float jumpMaxForce = 1.2f;

    [Header("Map :")]
    [SerializeField]
    private Collider2D mapCollider;

    [SerializeField]
    private float offsetGroundDistance = 0.02f;

    private void Start()
    {
        capc = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -Vector3.up, (capc.size.y / 2f) + offsetGroundDistance);

        if (hits.Length == 2 && hits[1].collider == mapCollider)
        {
            //force.y = 0f;
            force.x = Mathf.Lerp(force.x, 0f, 0.005f);
        }
        else
        {
            force.y -= gravityForce * Time.deltaTime;
        }

        if (Input.touchCount == 1 && Time.time - lastJump >= timeBetweenJump)
        {
            var mousePos2D = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            var pos2D = new Vector2(transform.position.x, transform.position.y);

            var direction = (pos2D - mousePos2D).normalized;
            var jump = Vector2.Distance(mousePos2D, pos2D) >= distancePowerfullJump ? (jumpForce * jumpMaxForce) : jumpForce;
            
            force = direction * jump;
            lastJump = Time.time;
        }

        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref velocity, 0.05f);
    }
}
