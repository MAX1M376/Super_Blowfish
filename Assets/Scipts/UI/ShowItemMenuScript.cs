using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowItemMenuScript : MonoBehaviour
{
    private bool isShow = false;
    private float size = 0.0f;
    private int prizeId;
    private string _name;
    private string _description;
    private Sprite _image;
    private GameplayScript gameplay;

    public string Name 
    { 
        get { return _name; }
        set
        {
            _name = value;
            nameText.text = _name;
        } 
    }
    public string Description
    {
        get { return _description; }
        set
        {
            _description = value;
            descriptionText.text = _description;
        }
    }
    public Sprite Image
    {
        get { return _image; }
        set
        {
            _image = value;
            prizeImage.sprite = _image;
        }
    }

    [Header("Property :")]
    [SerializeField] private float growSpeed;
    [SerializeField] private float timeToClose;

    [Header("Images :")]
    [SerializeField] private Image prizeImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

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

    private void OnEnable()
    {
        size = 0.0f;
        isShow = true;
        gameObject.transform.localScale = new Vector3(size, size, 1);

        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            gameplay.Paused();
        }
    }

    public void ShowPrize(Prize prize)
    {
        prizeId = prize.Id;
        Name = prize.Name;
        Description = prize.Description;
        Image = prize.Image;
    }

    private async void OnDisable()
    {
        isShow = false;
        await InventoryScript.PostPrize(prizeId);

        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            if (!gameplay.InventoryScript.gameObject.activeSelf)
            {
                gameplay.Gameplay();
            }
        }
    }

    public void Update()
    {
        if (isShow)
        {
            size += Time.deltaTime * growSpeed;
            size = Mathf.Clamp(size, 0.0f, 1.0f);
        }
        gameObject.transform.localScale = new Vector3(size, size, 1);
    }
}
