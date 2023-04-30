using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterController : MonoBehaviour
{

    public Letter letter;

    public bool viewing;

    // Start is called before the first frame update
    void Awake()
    {
        letter = new Letter();
        viewing = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, gameObject.layer);
    }
    
}
