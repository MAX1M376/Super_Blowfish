using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteStretch : MonoBehaviour
{
    private Vector3 offset;

    public bool KeepAspectRatio;

    void Start()
    {
        offset = transform.position - Camera.main.transform.position;
        
        var screenWorldWidth = Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x - Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        var screenWorldHeight = Camera.main.ScreenToWorldPoint(Vector3.down * Screen.height).y - Camera.main.ScreenToWorldPoint(Vector3.zero).y;
       
        var spriteSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;

        var scaleFactorX = Mathf.Abs(screenWorldWidth / spriteSize.x);
        var scaleFactorY = Mathf.Abs(screenWorldHeight / spriteSize.y);

        if (KeepAspectRatio)
        {
            if (scaleFactorX > scaleFactorY)
            {
                scaleFactorY = scaleFactorX;
            }
            else
            {
                scaleFactorX = scaleFactorY;
            }
        }

        gameObject.transform.localScale = new Vector3(scaleFactorX, scaleFactorY, 1);
    }

    private void LateUpdate()
    {
        gameObject.transform.position = Camera.main.transform.position + Vector3.forward * offset.z;
    }
}
