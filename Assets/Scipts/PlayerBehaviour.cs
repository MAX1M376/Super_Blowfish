using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent (typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour
{
    private float lastJump;
    private float horizontalVelocity;
    private Vector2 velocity;
    private Vector2 force;
    private Quaternion rotation;
    private CapsuleCollider2D capc;
    private Rigidbody2D rb;

    [Header("Physique :")]
    [SerializeField]
    private float gravityForce = 14f;

    [SerializeField]
    private float airResistence = 0.01f;

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

    [SerializeField]
    private float rotationSpeed;

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
        // Collision avec le sols
        RaycastHit2D[] downHits = Physics2D.RaycastAll(transform.position, Vector2.down, (capc.size.y / 2f) + offsetGroundDistance);
        
        if (!downHits.Any(x => x.collider.gameObject.name == mapCollider.gameObject.name)) // Si pas de collision alors
        { 
            // Appliquation gravité
            force.y -= gravityForce * Time.deltaTime;
        }
        else
        {
            if (Time.time - lastJump >= timeBetweenJump)
            {
                force.y = 0f;
                lastJump = 0f;
            }
            rotation = Quaternion.Euler(0, 0, 0);
        }

        // Collision avec le plafond
        RaycastHit2D[] upHits = Physics2D.RaycastAll(transform.position, Vector2.up, (capc.size.y / 2f) + offsetGroundDistance);
        if (upHits.Any(x => x.collider.gameObject.name == mapCollider.gameObject.name)) // Si collision avec le plafond
        {
            // Annulation des forces vertical
            force.y = -1f;
        }

        // Les inputs
        if (Input.touchCount == 1 && Time.time - lastJump >= timeBetweenJump)
        {
            // Recupération de la position du touché
            var mousePos2D = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            var pos2D = new Vector2(transform.position.x, transform.position.y);

            // Calcul de la direction et de la force du saut
            var direction = (mousePos2D - pos2D).normalized;
            var jump = Vector2.Distance(mousePos2D, pos2D) >= distancePowerfullJump ? (jumpForce * jumpMaxForce) : jumpForce;
            
            // Application des force et reset du jump time
            force = direction * jump;
            lastJump = Time.time;
        }

        // Reduction le temps entre chaque si spam clic
        if (Input.touchCount == 0 && Time.time - lastJump <= timeBetweenJump)
        {
            lastJump = 0f;
        }

        // Tourne le sprite dans le direction
        var angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        var orientation = angle >= -90f && angle <= 90f ? 0f : 180f;
        rotation = Quaternion.Euler(new Vector3(orientation, 0, orientation == 180f ? -angle : angle));

        var euler = transform.GetChild(0).transform.localEulerAngles;
        euler.y = orientation;
        transform.GetChild(0).transform.localEulerAngles = euler;
    }

    private void FixedUpdate()
    {
        // Application de la résistance de l'air
        force.x = Mathf.SmoothDamp(force.x, 0f, ref horizontalVelocity, 300f * (1 - airResistence) * Time.fixedDeltaTime);

        // Application des toutes les forces sur le rigidbody
        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref velocity, Time.fixedDeltaTime);

        // Application de la rotation du sprite en fnction de la direction
        transform.GetChild(0).transform.localRotation = Quaternion.Slerp(transform.GetChild(0).transform.localRotation, rotation, Time.fixedDeltaTime * rotationSpeed);
    }
}
