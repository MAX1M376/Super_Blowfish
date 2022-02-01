using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarFixSize : MonoBehaviour
{
    private Scrollbar bar;
    private void Start()
    {
        bar = GetComponent<Scrollbar>();
    }

    void Update()
    {
        bar.size = 0f;
    }

    void LateUpdate()
    {
        bar.size = 0f;
    }
}
