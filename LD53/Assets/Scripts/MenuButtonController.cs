using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private TextMeshProUGUI buttonText;
    private float fontsize;
    private void Awake()
    {
        buttonText = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        fontsize = buttonText.fontSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.fontSize = buttonText.fontSize + 5;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.fontSize = fontsize;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //buttonText.color = new Color32(204, 36, 111, 255);
        buttonText.color = new Color32(163, 111, 13, 255);
    }
}
