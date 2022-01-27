using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowItem : MonoBehaviour
{
    private bool isShow = false;

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Sprite Image { get; private set; }

    [Header("Property :")]
    [SerializeField, Range(0.0f, 1.0f)] private float size = 0.0f;
    [SerializeField] private float growSpeed;

    [Header("Images :")]
    [SerializeField] private Image prizeImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void SetName(string name)
    {
        Name = name;
        nameText.text = name;
    }

    public void SetDescription(string description)
    {
        Description = description;
        descriptionText.text = description;
    }

    public void SetImage(Sprite image)
    {
        Image = image;
        prizeImage.sprite = image;
    }

    public void ShowPrize(Prize prize)
    {
        SetName(prize.Name);
        SetDescription(prize.Description);
        SetImage(prize.Image);

        gameObject.transform.localScale = Vector3.zero; 
        gameObject.SetActive(true);
        isShow = true;
    }

    public void ClosePrize()
    {
        gameObject.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
        isShow = false;
    }

    public void Update()
    {
        if (isShow)
        {
            size = Mathf.Lerp(size, 1, Time.deltaTime * growSpeed * 300.0f);
        }
        gameObject.transform.localScale = new Vector3(size, size, 1);
    }
}
