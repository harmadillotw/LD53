using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour
{
    private float splashTimer;
    private float splashPeriod = 05f;
    // Start is called before the first frame update
    void Start()
    {
        splashTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.anyKeyDown) || (Input.GetMouseButtonDown(0)) || (Input.GetMouseButtonDown(1)) || (Input.GetMouseButtonDown(2)))
        {
            SceneManager.LoadScene("MainMenu");
        }
        splashTimer += Time.deltaTime;
        if (splashTimer > splashPeriod)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
