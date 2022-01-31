using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [HideInInspector] public ShowItem ShowItem;
    public Prize Prize 
    { 
        get { return prize; }
        set
        {
            prize = value;
            textMesh.text = value.Name;
            image.sprite = value.Image;
        }
    }

    private RectTransform rectTrans;
    private Prize prize;

    [Header("Components :")]
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image image;

    public void Start()
    {
        rectTrans = gameObject.GetComponent<RectTransform>();
    }

    public void ChangePivotPos(Vector2 pivot, float Y)
    {
        if (rectTrans == null)
        {
            rectTrans = gameObject.GetComponent<RectTransform>();
        }
        rectTrans.pivot = pivot;
        rectTrans.anchorMax = pivot;
        rectTrans.anchorMin = pivot;
        rectTrans.anchoredPosition = new Vector2(0, Y);
    }

    public void OpenItem()
    {
        if (ShowItem != null)
        {
            ShowItem.ShowPrize(Prize);
        }
    }
}
