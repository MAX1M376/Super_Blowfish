using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowItem : MonoBehaviour
{
    private bool isShow = false;
    private float size = 0.0f;
    private string _name;
    private string _description;
    private Sprite _image;

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

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowPrize(Prize prize)
    {
        Name = prize.Name;
        Description = prize.Description;
        Image = prize.Image;

        size = 0.0f;
        isShow = true;
        gameObject.transform.localScale = new Vector3(size, size, 1);
        gameObject.SetActive(true);

        GameObject.Find("Gameplay").GetComponent<GameplayScript>().Paused();
    }

    public void ClosePrize()
    {
        isShow = false;
        gameObject.SetActive(false);

        if (GameObject.Find("InventoryMenu") == null)
        {
            GameObject.Find("Gameplay").GetComponent<GameplayScript>().Gameplay();
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
