using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour
{
    private float verticalForce;
    private CapsuleCollider2D capc;
    private Rigidbody2D rigidb;

    [Header("Physique :")]
    [SerializeField]
    private float gravityForce = 14f;

    [Header("Movement :")]
    [SerializeField]
    private float jumpForce = 10f;

    [Header("Map :")]
    [SerializeField]
    private Collider2D mapCollider;

    [Header("Map :")]
    [SerializeField]
    private float offsetGroundDistance = 0.02f;

    private void Start()
    {
        rigidb = GetComponent<Rigidbody2D>();
        capc = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -Vector3.up, (capc.size.y / 2f) + offsetGroundDistance);

        if (hits.Length == 2 && hits[1].collider == mapCollider)
        {
            Debug.Log("hit ground");
            verticalForce = 0;
        }
        else
        {
            verticalForce -= gravityForce * Time.deltaTime;
        }

        if (Input.touchCount >= 1)
        {
            verticalForce = jumpForce;
        }

        rigidb.velocity = Vector2.up * verticalForce; 
    }
}
