using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDrawItem : MonoBehaviour
{
    [Header("Property :")]
    [SerializeField] private float spaceBetweenItem;
    [SerializeField] private Vector2 pivotLeft;
    [SerializeField] private Vector2 pivotRight;

    [Header("Inventory Item :")]
    [SerializeField] private GameObject itemButton;

    [Header("Item Window :")]
    [SerializeField] private ShowItem showItem;

    private void Start()
    {
        Draw(InventoryScript.Inventory);
    }

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
        var size = gameObject.GetComponent<RectTransform>().sizeDelta;
        size.y = spaceBetweenItem * InventoryScript.AllPrizes.Count / 2;
        gameObject.GetComponent<RectTransform>().sizeDelta = size;
    }

    private void ShowButtonItem(Prize prize, int i)
    {
        GameObject instantiatedItem = Instantiate(itemButton, gameObject.transform);
        var script = instantiatedItem.GetComponent<InventoryItem>();
        script.Prize = prize;
        script.ShowItem = showItem;
        script.ChangePivotPos((i % 2) == 0 ? pivotLeft : pivotRight, Mathf.FloorToInt(i / 2f) * -spaceBetweenItem);
    }
}
