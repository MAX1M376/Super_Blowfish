using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent (typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour
{
    [HideInInspector] public Vector2 force;

    private int keyboardDirection;
    private float lastJump;
    private float lookDirection = 0f;
    private float horizontalVelocity;
    private Vector2 velocity;
    private Quaternion rotation;
    private CircleCollider2D cipc;
    private Rigidbody2D rb;

    [Header("Propety :")]
    [SerializeField] private float gravityForce = 14f;
    [SerializeField] private float airResistence = 0.01f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float timeBetweenJump = 0.5f;
    [SerializeField] private Vector2 lockedDirection = new Vector2(1.0f, 1.6f);

    [Header("Property On Water :")]
    [SerializeField] private float gravityForceOnWater = 14f;
    [SerializeField] private float airResistenceOnWater = 0.01f;
    [SerializeField] private float jumpForceOnWater = 10f;
    [SerializeField] private float timeBetweenJumpOnWater = 0.5f;
    [SerializeField] private Vector2 lockedDirectionOnWater = new Vector2(1.0f, 0.8f);
    [SerializeField] private float inflatePerSecond = 10.0f;
    [SerializeField] private float deflatePerSecond = 2.0f;

    [Header("Other Property :")]
    [SerializeField, Range(0.1f, 3f)] private float distancePowerfullJump = 1.5f;
    [SerializeField, Range(1f, 5f)] private float jumpMaxForce = 1.2f;
    [SerializeField] private float offsetGroundDistance = 0.02f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool newSystem = false;

    [Header("State :")]
    [SerializeField] private bool freeze = false;
    public bool isOnWater = false;

    [Header("Map :")]
    [SerializeField] private string mapName;

    [Header("UI :")]
    [SerializeField] private Slider airBar;
    [SerializeField] private ShowItem itemShow;

    [Header("Body :")]
    [SerializeField] private InflateDeflate body;

    [Header("Crates :")]
    [SerializeField] private string crateTag;

    private void Start()
    {
        cipc = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        itemShow.ShowPrize(new Prize() { Name = "Une caisse", Description = "Toutes les caisse du magazins sont offertes !", Image = Resources.Load<Sprite>(@"Prizes/saucisse") });

    }

    private void Update()
    {
        if (freeze)
        {
            horizontalVelocity = 0f;
            rb.velocity = force = velocity = Vector2.zero;
            return;
        }

        // 0 when arround 0
        if (Mathf.Abs(rb.velocity.x) <= 0.01) rb.velocity = new Vector2(0, rb.velocity.y);
        if (Mathf.Abs(rb.velocity.y) <= 0.01) rb.velocity = new Vector2(rb.velocity.x, 0);
        
        // Rotation
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x);
        int orientation = Mathf.Cos(angle) >= 0 ? 0 : 180;
        var euler = transform.localEulerAngles;
        var eulerChild = transform.GetChild(0).transform.localEulerAngles;

        eulerChild.y = lookDirection;
        transform.GetChild(0).transform.localEulerAngles = eulerChild;

        // Collision avec le sol
        var downHits = Physics2D.OverlapCircleAll(transform.position - Vector3.up * offsetGroundDistance, cipc.radius / 2f);
        if (downHits.Any(x => x))
        {
            if (Input.touchCount == 0)
            {
                rotation.z = force.y = 0f;
            }

            // Si collide avec une caisse
            if (downHits.Any(x => x.gameObject.tag == crateTag))
            {
                force.y = (isOnWater ? jumpForceOnWater : jumpForce) / 1.5f;
                downHits.ToList().Where(x => x.gameObject.tag == crateTag).ToList().ForEach(x => x.gameObject.GetComponent<CrateBehaviour>().Hit(1));
            }
        }
        else
        {
            // Appliquation gravité
            force.y -= (isOnWater ? gravityForceOnWater : gravityForce) * Time.deltaTime;

            // Tourne le sprite dans le direction
            rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + orientation);
        }

        // Collision avec les Raycast
        force.y = RaycastPlayer(force.y, Vector2.up, -0.5f, -3, 1, 0.03f);
        force.x = RaycastPlayer(force.x, Vector2.left, 0.1f, 2, 1, 0.03f);
        force.x = RaycastPlayer(force.x, Vector2.right, -0.1f, -2, 1, 0.03f);

        // Mouvement du joueur
        Movement(isOnWater ? timeBetweenJumpOnWater : timeBetweenJump, isOnWater ? jumpForceOnWater : jumpForce, isOnWater ? lockedDirectionOnWater : lockedDirection);

        // Degonflement ou regonflement
        Inflate(isOnWater);
    }

    private void FixedUpdate()
    {
        if (freeze) return;

        // Application de la résistance de l'air
        force.x = Mathf.SmoothDamp(force.x, 0f, ref horizontalVelocity, 300f * (1 - (isOnWater ? airResistenceOnWater : airResistence)) * Time.fixedDeltaTime);

        // Application des toutes les forces sur le rigidbody
        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref velocity, Time.fixedDeltaTime);

        // Application de la rotation du sprite en fnction de la direction
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.fixedDeltaTime * rotationSpeed);
    }

    private float RaycastPlayer(float originalForce, Vector2 dir, float oppositeBounce, int ratioForce, int hitDamage, float additionOffset)
    {
        float resultat = originalForce;
        var Hits = Physics2D.RaycastAll(transform.position, dir, cipc.radius / 2f + offsetGroundDistance + additionOffset);
        if (Hits.Any(x => x))
        {
            resultat = oppositeBounce;

            // Si touche une caisse 
            if (Hits.Any(x => x.collider.gameObject.tag == crateTag))
            {
                resultat = (isOnWater ? jumpForceOnWater : jumpForce) / ratioForce;
                Hits.ToList().Where(x => x.collider.gameObject.tag == crateTag).ToList().ForEach(x => x.collider.gameObject.GetComponent<CrateBehaviour>().Hit(hitDamage));
            }
        }
        return resultat;
    }

    private void Movement(float timeJump, float forceJump, Vector2 lockDir)
    {
        // Les inputs
        if (Input.touchCount == 1 && Time.time - lastJump >= timeJump)
        {
            Vector2 direction;
            float jump;

            MovementSystem(forceJump, lockDir, out direction, out jump);

            lookDirection = direction.x < 0 ? 0 : 180;

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

    private void MovementSystem(float forceJump, Vector2 lockDir, out Vector2 direction, out float jump)
    {
        if (newSystem)
        {
            // Recupération de la position du touché
            var mousePos2D = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            var pos2D = new Vector2(transform.position.x, transform.position.y);
            var offsetMousePos = mousePos2D - pos2D;
            lockDir.Normalize();

            direction = new Vector2(offsetMousePos.x < 0 ? -Mathf.Abs(lockDir.x) : Mathf.Abs(lockDir.x), lockDir.y);
            jump = Mathf.Abs((mousePos2D - pos2D).x) >= distancePowerfullJump ? (forceJump * jumpMaxForce) : forceJump;
        }
        else
        {
            // Recupération de la position du touché
            var mousePos2D = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            var pos2D = new Vector2(transform.position.x, transform.position.y);

            // Calcul de la direction et de la force du saut
            direction = (mousePos2D - pos2D).normalized;
            jump = Vector2.Distance(mousePos2D, pos2D) >= distancePowerfullJump ? (forceJump * jumpMaxForce) : forceJump;
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
        airBar.value = body.InflateLevel = Mathf.Clamp(inflate, 0f, 1f);
    }
}
