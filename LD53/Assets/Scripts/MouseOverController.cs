using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverController : MonoBehaviour
{
   // Color originalColor;
    Vector3 originalSize;
    // Start is called before the first frame update
    void Start()
    {
        //originalColor = gameObject.GetComponent<SpriteRenderer>().color;
        originalSize = gameObject.GetComponent<SpriteRenderer>().transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MouseEnter()
    {
        //gameObject.GetComponent<SpriteRenderer>().color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        gameObject.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(originalSize.x + 0.2f, originalSize.y + 0.2f, originalSize.z);

    }

    public void MouseExit()
    {
        //gameObject.GetComponent<SpriteRenderer>().color = originalColor;
        gameObject.GetComponent<SpriteRenderer>().transform.localScale = originalSize;
    }

}
