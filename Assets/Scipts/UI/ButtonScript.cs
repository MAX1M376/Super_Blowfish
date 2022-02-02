using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Button images :")]
    [SerializeField] private Sprite upButton;
    [SerializeField] private Sprite downButton;

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().sprite = downButton;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().sprite = upButton;
    }

    private void OnDisable()
    {
        gameObject.GetComponent<Image>().sprite = upButton;
    }
}
