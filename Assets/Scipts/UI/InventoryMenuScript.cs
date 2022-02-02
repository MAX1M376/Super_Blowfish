using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenuScript : MonoBehaviour
{
    [Header("Property :")]
    [SerializeField] private float spaceBetweenItem;
    [SerializeField] private Vector2 pivotLeft;
    [SerializeField] private Vector2 pivotRight;

    [Header("Content :")]
    [SerializeField] private RectTransform rect;

    [Header("Inventory Item :")]
    [SerializeField] private GameObject prefabItemButton;

    private void OnEnable()
    {
        Draw(InventoryScript.Inventory);
    }

    public void Draw(List<Prize> prizes)
    {
        for (int i = 0; i < prizes.Count; i++)
        {
            ShowButtonItem(prizes[i], i);
        }
        var size = rect.sizeDelta;
        size.y = spaceBetweenItem * Mathf.CeilToInt(InventoryScript.Inventory.Count / 2f);
        rect.sizeDelta = size;
    }

    private void ShowButtonItem(Prize prize, int i)
    {
        GameObject instantiatedItem = Instantiate(prefabItemButton, rect.transform);
        var script = instantiatedItem.GetComponent<InventoryItem>();
        script.Prize = prize;
        script.ChangePivotPos((i % 2) == 0 ? pivotLeft : pivotRight, Mathf.FloorToInt(i / 2f) * -spaceBetweenItem);
    }
}
