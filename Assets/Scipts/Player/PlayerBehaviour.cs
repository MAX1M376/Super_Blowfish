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
    private float lookDirection;
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

    [Header("Water :")]
    public bool isOnWater = false;

    [SerializeField]
    private float inflatePerSecond = 10.0f;

    [SerializeField]
    private float deflatePerSecond = 2.0f;

    [Header("Body :")]
    [SerializeField]
    private InflateDeflate body;

    [SerializeField]
    private float downRadius;

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
            rb.velocity = force = velocity = Vector2.zero;
            return;
        }

        if (Mathf.Abs(rb.velocity.x) <= 0.01) rb.velocity = new Vector2(0, rb.velocity.y);
        if (Mathf.Abs(rb.velocity.y) <= 0.01) rb.velocity = new Vector2(rb.velocity.x, 0);

        // Collision avec le sols
        var downHits = Physics2D.OverlapCircleAll(transform.position - Vector3.up * offsetGroundDistance, cipc.radius / 2f);
        
        // Rotation
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x);
        int orientation = Mathf.Cos(angle) >= 0 ? 0 : 180;

        var euler = transform.localEulerAngles;
        var scale = transform.localScale;

        scale.x = orientation == 180f ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        //transform.localScale = scale;

        if (!downHits.Any(x => x)) // Si pas de collision alors
        {
            // Appliquation gravit�
            force.y -= (isOnWater ? gravityForceOnWater : gravityForce) * Time.deltaTime;

            // Tourne le sprite dans le direction
            rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + orientation);
            lookDirection = orientation;
        }
        else
        {
            rotation = Quaternion.Euler(0, lookDirection, 0);
        }

        // Collision avec le plafond
        var upHits = Physics2D.RaycastAll(transform.position, Vector2.up, cipc.radius / 2f + offsetGroundDistance);
        if (upHits.Any(x => x))
        {
            force.y = velocity.y = 0f;
        }

        // Collision a gauche
        var leftHits = Physics2D.RaycastAll(transform.position, Vector2.left, cipc.radius / 2f + offsetGroundDistance);
        if (leftHits.Any(x => x))
        {
            force.x = velocity.x = horizontalVelocity = 0f;
        }

        // Collision a droite
        var rightHits = Physics2D.RaycastAll(transform.position, Vector2.right, cipc.radius / 2f + offsetGroundDistance);
        if (rightHits.Any(x => x))
        {
            horizontalVelocity = force.x = velocity.x = 0f;
        }

        // Mouvement du joueur
        Movement(isOnWater ? timeBetweenJumpOnWater : timeBetweenJump, isOnWater ? jumpForceOnWater : jumpForce);


        // Degonflement ou regonflement
        Inflate(isOnWater);
    }

    private void FixedUpdate()
    {
        if (freeze) return;

        // Application de la r�sistance de l'air
        force.x = Mathf.SmoothDamp(force.x, 0f, ref horizontalVelocity, 300f * (1 - (isOnWater ? airResistenceOnWater : airResistence)) * Time.fixedDeltaTime);

        // Application des toutes les forces sur le rigidbody
        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref velocity, Time.fixedDeltaTime);

        // Application de la rotation du sprite en fnction de la direction
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.fixedDeltaTime * rotationSpeed);
    }

    private void Movement(float timeJump, float forceJump)
    {
        // Les inputs
        if (Input.touchCount == 1 && Time.time - lastJump >= timeJump)
        {
            // Recup�ration de la position du touch�
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

    private void Inflate(bool water)
    {
        float inflate;
        if (water)
        {
            inflate = body.InflateLevel + Time.deltaTime * (inflatePerSecond / 100);
        }
        else
        {
            inflate = body.InflateLevel - Time.deltaTime * (deflatePerSecond / 100);
        }
        body.InflateLevel = Mathf.Clamp(inflate, 0f, 1f);
    }
}