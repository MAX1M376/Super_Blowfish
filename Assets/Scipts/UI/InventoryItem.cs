using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
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
    private GameplayScript gameplay;

    [Header("Components :")]
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image image;

    public void Start()
    {
        rectTrans = gameObject.GetComponent<RectTransform>();
        var gameplayGO = GameObject.FindGameObjectWithTag("Gameplay");
        if (gameplayGO != null)
        {
            gameplay = gameplayGO.GetComponent<GameplayScript>();
        }
        else
        {
            Debug.LogError("Gameplay components not load");
        }
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

        gameplay.ShowItemScript.gameObject.SetActive(true);
        gameplay.ShowItemScript.ShowPrize(Prize);
    }
}
