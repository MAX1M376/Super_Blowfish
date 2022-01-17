using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent (typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour
{
    private float lastJump;
    private float horizontalVelocity;
    private Vector2 velocity;
    private Quaternion rotation;
    private CircleCollider2D cipc;
    private Rigidbody2D rb;

    [Header("Physique :")]
    [SerializeField]
    private float gravityForce = 14f;

    [SerializeField]
    private float airResistence = 0.01f;

    [SerializeField]
    private float gravityForceOnWater = 14f;

    [SerializeField]
    private float airResistenceOnWater = 0.01f;

    [Header("Movement :")]
    [HideInInspector]
    public Vector2 force;

    [SerializeField]
    private float jumpForce = 10f;

    [SerializeField]
    private float timeBetweenJump = 0.5f;

    [SerializeField]
    private float jumpForceOnWater = 10f;

    [SerializeField]
    private float timeBetweenJumpOnWater = 0.5f;

    [SerializeField]
    [Range(0.1f, 3f)]
    private float distancePowerfullJump = 1.5f;

    [SerializeField]
    [Range(1f, 5f)]
    private float jumpMaxForce = 1.2f;

    [SerializeField]
    private float rotationSpeed;

    [Header("Property :")]
    [SerializeField]
    private bool freeze = false;

    [Header("Map :")]
    [SerializeField]
    private string mapName;

    [SerializeField]
    private float offsetGroundDistance = 0.02f;

    public bool isOnWater = false;

    private void Start()
    {
        cipc = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void Update()
    {
        if (freeze)
        {
            horizontalVelocity = 0f;
            velocity = Vector2.zero;
            force = Vector2.zero;
            return;
        }

        Movement(isOnWater ? timeBetweenJumpOnWater : timeBetweenJump, isOnWater ? jumpForceOnWater : jumpForce);

        // Collision avec le sols
        RaycastHit2D[] downHits = Physics2D.RaycastAll(transform.position, Vector2.down, offsetGroundDistance);
        var angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        var orientation = angle >= -90f && angle <= 90f ? 180f : 0f;
        var euler = transform.GetChild(0).transform.localEulerAngles;
        if (!downHits.Any(x => x)) // Si pas de collision alors
        {
            // Appliquation gravité
            force.y -= (isOnWater ? gravityForceOnWater : gravityForce) * Time.deltaTime;

            // Tourne le sprite dans le direction
            rotation = Quaternion.Euler(new Vector3(orientation, 0f, (orientation == 180f ? -angle : angle) + 180f));
            euler.x = rotation.x;
            transform.GetChild(0).transform.localEulerAngles = euler;
        }
        else
        {
            rotation = Quaternion.Euler(new Vector3(orientation, 0f, orientation));
            euler.x = rotation.x;
            transform.GetChild(0).transform.localEulerAngles = euler;
        }

        // Collision avec le plafond
        RaycastHit2D[] upHits = Physics2D.RaycastAll(transform.position, Vector2.up, cipc.radius / 2f + offsetGroundDistance);
        if (upHits.Any(x => x)) // Si collision avec le plafond
        {
            // Annulation des forces vertical
            force.y = -1f;
        }

        if (Mathf.Abs(rb.velocity.x) <= 0.0001 && Mathf.Abs(rb.velocity.y) <= 0.0001)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (freeze) return;

        // Application de la résistance de l'air
        force.x = Mathf.SmoothDamp(force.x, 0f, ref horizontalVelocity, 300f * (1 - (isOnWater ? airResistenceOnWater : airResistence)) * Time.fixedDeltaTime);

        // Application des toutes les forces sur le rigidbody
        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref velocity, Time.fixedDeltaTime);

        // Application de la rotation du sprite en fnction de la direction
        transform.GetChild(0).transform.localRotation = Quaternion.Slerp(transform.GetChild(0).transform.localRotation, rotation, Time.fixedDeltaTime * rotationSpeed);
    }

    private void Movement(float timeJump, float forceJump)
    {
        // Les inputs
        if (Input.touchCount == 1 && Time.time - lastJump >= timeJump)
        {
            // Recupération de la position du touché
            var mousePos2D = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            var pos2D = new Vector2(transform.position.x, transform.position.y);

            // Calcul de la direction et de la force du saut
            var direction = (mousePos2D - pos2D).normalized;
            var jump = Vector2.Distance(mousePos2D, pos2D) >= distancePowerfullJump ? (forceJump * jumpMaxForce) : forceJump;

            // Application des force et reset du jump time
            force = direction * jump;
            lastJump = Time.time;
        }

        // Reduction le temps entre chaque si spam clic
        if (Input.touchCount == 0 && Time.time - lastJump <= timeJump)
        {
            lastJump = 0f;
        }
    }
}
