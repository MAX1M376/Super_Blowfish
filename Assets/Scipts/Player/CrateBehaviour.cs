using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrateBehaviour : MonoBehaviour
{
    private bool active = true;
    private float lastHit;
    private bool transparent = false;
    private SpriteRenderer crrd;
    private GameplayScript gameplay;

    [Header("Property :")]
    [SerializeField] private int totalLives = 2;
    [SerializeField] private int actualLives = 2;
    [SerializeField] private float recoveryTime = 0.3f;
    [SerializeField] private float timeToDestroy = 2f;

    [Header("Prizes :")]
    [SerializeField, Range(0f, 1f)] private float probabilityWin;
    [SerializeField] private string namePrize;

    [Header("Crates sprites :")]
    [SerializeField] private Sprite normalCrate;
    [SerializeField] private Sprite semiBrokenCrate;
    [SerializeField] private Sprite brokenCrate;

    private void Awake()
    {
        var gameplayGO = GameObject.FindGameObjectWithTag("Gameplay");
        if (gameplayGO != null)
        {
            gameplay = gameplayGO.GetComponent<GameplayScript>();
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex > 0)
            {
                Debug.LogError("Gameplay components not load");
            }
        }
    }

    private void Start()
    {
        crrd = transform.GetChild(0).GetComponent<SpriteRenderer>(); 
        GameStateManager.Instance.OnGameStateChange += Instance_OnGameStateChange;
        InventoryScript.PrizeEarnDuringLevel = 0;
    }

    public void Hit(int damage)
    {
        if (!active)
        {
            return;
        }

        var timeBeforceHit = Time.time - lastHit;
        if (timeBeforceHit >= recoveryTime)
        {
            actualLives -= damage;
            lastHit = Time.time;
        }

        if (actualLives == totalLives)
        {
            crrd.sprite = normalCrate;
        }

        if (actualLives < totalLives && actualLives > 0)
        {
            crrd.sprite = semiBrokenCrate;
        }

        if (actualLives <= 0)
        {
            gameObject.tag = "Broken crates";
            crrd.sprite = brokenCrate;
            active = false;
            transparent = true;
            Destroy(gameObject, timeToDestroy);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.7f, 0.2f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.0f, 0.4f);

            float rnd = Random.Range(0f, 1f);
            if (rnd <= probabilityWin)
            {
                gameplay.ShowItemScript.gameObject.SetActive(true);

                try
                {
                    InventoryScript.Inventory.Add(GetPrize());
                    InventoryScript.PrizeEarnDuringLevel += 1;
                    gameplay.ShowItemScript.ShowPrize(InventoryScript.Inventory.Last());
                }
                catch (System.Exception)
                {
                    Debug.LogWarning("Inventory Script isn't load, it's load at start of menu scene");
                }
            }
        }
    }

    private Prize GetPrize()
    {
        Prize prize;
        if (InventoryScript.AllPrizes.Any(x => x.Name == namePrize))
        {
            prize = InventoryScript.AllPrizes.First(x => x.Name == namePrize);
        }
        else
        {
            prize = InventoryScript.AllPrizes[Random.Range(0, InventoryScript.AllPrizes.Count - 1)];
        }
        return prize;
    }

    private void Instance_OnGameStateChange(GameState state)
    {
        enabled = state == GameState.GamePlay;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChange -= Instance_OnGameStateChange;
    }

    private void Update()
    {
        if (transparent)
        {
            float transparency = Mathf.Lerp(crrd.color.a, 0, Time.deltaTime * 1 / timeToDestroy);
            crrd.color = new Color(crrd.color.r, crrd.color.g, crrd.color.b, transparency);
        }
    }
}
