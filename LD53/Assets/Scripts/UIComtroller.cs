using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIComtroller : MonoBehaviour
{
    public GameObject helpPanel;

    public List<Sprite> manualSprites;
    public GameObject manualGO;
    public Image manualImage;
    public TextMeshProUGUI supSubText;
    public TextMeshProUGUI supText;
    public TextMeshProUGUI supCol1Text;
    public TextMeshProUGUI supCol2Text;

    public TextMeshProUGUI directoryText;
    public TextMeshProUGUI directoryText2;
    public TextMeshProUGUI directoryType;

    public Button instNextButton;
    public Button instPrevButton;

    public Button supNextButton;
    public Button supPrevButton;

    public Button dirNextButton;
    public Button dirPrevButton;

    public GameObject instructionsPanel;
    public GameObject supplementaryPanel;
    public GameObject directoryPanel;

    public AudioSource audioSource;
    public AudioSource audioSourceShort;

    public AudioClip menuClip;
    public AudioClip turnPageClip;
    public AudioClip openManualClip;


    int supPage = 1;
    int dirPage = 1;

    private int pageNumber;
    // Start is called before the first frame update
    void Start()
    {
        if (manualGO != null)
        {
            manualImage = manualGO.GetComponent<Image>();
            if ((manualSprites != null) && (manualSprites.Count > 0))
            {
                manualImage.sprite = manualSprites[0];
                pageNumber = 0;
            }
        }
        supPrevButton.gameObject.SetActive(false);
        instPrevButton.gameObject.SetActive(false);
        dirPrevButton.gameObject.SetActive(false);
        instPrevButton.enabled = false;

        setSupContents();
        setDirContents();

    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown(KeyCode.Tab)))
        {
            helpPanel.SetActive(!helpPanel.activeSelf);
        }
    }

    public void closeManual()
    {
        playAudio(openManualClip, audioSource, false);
        instructionsPanel.SetActive(false);
    }

    public void closeSupplementary()
    {
        playAudio(openManualClip, audioSource, false);
        supplementaryPanel.SetActive(false);      
    }

    public void closeDirectory()
    {
        playAudio(openManualClip, audioSource, false);
        directoryPanel.SetActive(false);
    }
    public void nextManualPage()
    {
        playAudio(turnPageClip, audioSource, false);
        pageNumber++;
        instPrevButton.enabled = true;
        instPrevButton.gameObject.SetActive(true);
        if (pageNumber >= manualSprites.Count)
        {
            pageNumber = manualSprites.Count - 1;
            instNextButton.enabled = false;
            instNextButton.gameObject.SetActive(false);
        }
        manualImage.sprite = manualSprites[pageNumber];
    }

    public void previousManualPage()
    {
        playAudio(turnPageClip, audioSource, false);
        pageNumber--;
        instNextButton.gameObject.SetActive(true);
        instNextButton.enabled = true;
        if (pageNumber <= 0)
        {
            instPrevButton.enabled = false;
            instPrevButton.gameObject.SetActive(false);
            pageNumber = 0;
        }
        manualImage.sprite = manualSprites[pageNumber];
    }

    public void nextSuppPage()
    {
        playAudio(turnPageClip, audioSource, false);
        supPage++;
        supPrevButton.gameObject.SetActive(true);
        if (supPage >= 6)
        {
            supNextButton.gameObject.SetActive(false);
            supPage = manualSprites.Count - 1;
        }
        setSupContents();
    }
    public void previousSupPage()
    {
        playAudio(turnPageClip, audioSource, false);
        supPage--;
        supNextButton.gameObject.SetActive(true);
        if (supPage <= 1)
        {
            supPrevButton.gameObject.SetActive(false);
            supPage = 1;
        }
        setSupContents();
    }

    public void setSupContents()
    {
        string formattedSupText = "";
        string formattedCol1Text = "";
        string formattedCol2Text = "";
        switch (supPage)
        {
            case 1:
                supSubText.text = "Contents";
                supText.text = "";
                formattedCol1Text = "National Postcodes\r\nInternationalPostcodes";
                formattedCol2Text = "1\r\n2";
                supCol1Text.text = formattedCol1Text;
                supCol2Text.text = formattedCol2Text;
                break;
            case 2:
                supSubText.text = "National Postcodes";
                formattedSupText = "National postcodes consist of a letter, based on the destination city and a number for the destination subregion.\r\n";
                //formattedSupText += "City \t\t\t Postcode\r\n";
                formattedCol1Text = "City\r\n\r\n";
                formattedCol2Text = "PostCode\r\n\r\n";
                for (int i = 0; i < Constants.nationalDirectoryCount; i++)
                {
                    formattedCol1Text += Constants.nationalDirectory[i, 0] + "\r\n";
                    formattedCol2Text += Constants.nationalDirectory[i, 1] + "\r\n";
                }
                supText.text = formattedSupText;
                supCol1Text.text = formattedCol1Text;
                supCol2Text.text = formattedCol2Text;
                break;
            case 3:
                supSubText.text = "Interational Postcodes";
                formattedSupText = "International postcodes consist of a 4 digit number. The first 3 digits represtent the destination country while the 4th is the subregion.\r\n";
                //formattedSupText += "Country \t\t\t Postcode\r\n";
                formattedCol1Text = "Country\r\n\r\n";
                formattedCol2Text = "PostCode\r\n\r\n";
                for (int i = 0; i < 12; i++)
                {
                    formattedCol1Text += Constants.internationalDirectory[i, 0] + "\r\n";
                    formattedCol2Text += Constants.internationalDirectory[i, 1] + "\r\n";
                }
                supText.text = formattedSupText;
                supCol1Text.text = formattedCol1Text;
                supCol2Text.text = formattedCol2Text;
                break;
            case 4:
                supSubText.text = "Interational Postcodes";
                formattedSupText = "";
                //formattedSupText += "Country \t\t\t Postcode\r\n";
                formattedCol1Text = "Country\r\n\r\n";
                formattedCol2Text = "PostCode\r\n\r\n";
                for (int i = 12; i < 24; i++)
                {
                    formattedCol1Text += Constants.internationalDirectory[i, 0] + "\r\n";
                    formattedCol2Text += Constants.internationalDirectory[i, 1] + "\r\n";
                }
                supText.text = formattedSupText;
                supCol1Text.text = formattedCol1Text;
                supCol2Text.text = formattedCol2Text;
                break;
            case 5:
                supSubText.text = "Interational Postcodes";
                formattedSupText = "";
                //formattedSupText += "Country \t\t\t Postcode\r\n";
                formattedCol1Text = "Country\r\n\r\n";
                formattedCol2Text = "PostCode\r\n\r\n";
                for (int i = 24; i < 36; i++)
                {
                    formattedCol1Text += Constants.internationalDirectory[i, 0] + "\r\n";
                    formattedCol2Text += Constants.internationalDirectory[i, 1] + "\r\n";
                }
                supText.text = formattedSupText;
                supCol1Text.text = formattedCol1Text;
                supCol2Text.text = formattedCol2Text;
                break;
            case 6:
                supSubText.text = "Interational Postcodes";
                formattedSupText = "";
                //formattedSupText += "Country \t\t\t Postcode\r\n";
                formattedCol1Text = "Country\r\n\r\n";
                formattedCol2Text = "PostCode\r\n\r\n";
                for (int i = 36; i < Constants.internationalDirectoryCount; i++)
                {
                    formattedCol1Text += Constants.internationalDirectory[i, 0] + "\r\n";
                    formattedCol2Text += Constants.internationalDirectory[i, 1] + "\r\n";
                }
                supText.text = formattedSupText;
                supCol1Text.text = formattedCol1Text;
                supCol2Text.text = formattedCol2Text;
                break;
            


        }

    }

    public void nextDirectoryPage()
    {
        playAudio(turnPageClip, audioSource, false);
        dirPage++;
        dirPrevButton.gameObject.SetActive(true);
        if (dirPage >= 3)
        {
            dirNextButton.gameObject.SetActive(false);
            dirPage = 3;
        }
        setDirContents();
    }
    public void previousDirectoryPage()
    {
        playAudio(turnPageClip, audioSource, false);
        dirPage--;
        dirNextButton.gameObject.SetActive(true);
        if (dirPage <= 1)
        {
            dirPrevButton.gameObject.SetActive(false);
            dirPage = 1;
        }
        setDirContents();
    }

    private void setDirContents()
    {
        string formattedDirText = "";
        string formattedDirText2 = "";
        int start = 0 + ((dirPage -1) * 20);
        int end = dirPage * 20;
        if (end > 46)
        {
            end = 46;
        }
        for (int i = start; i < end; i++)
        {
            formattedDirText2 += Constants.localDirectory[i, 0] + " " + Constants.localDirectory[i, 1] +  "\r\n";
            formattedDirText += Constants.localDirectory[i, 2] + " " + Constants.localDirectory[i, 3] + " " + Constants.localDirectory[i, 4] + "\r\n";

        }
        directoryText.text = formattedDirText2;
        directoryText2.text = formattedDirText;
    }

    public void startGame()
    {
        playAudio(menuClip, audioSource, false);
        SceneManager.LoadScene("MainScene" );
    }

    public void mainMenu()
    {
        playAudio(menuClip, audioSource, false);
        SceneManager.LoadScene("MainMenu");
    }

    public void goOptions()
    {
        playAudio(menuClip, audioSource, false);
        SceneManager.LoadScene("OptionsScene");
    }

    public void goCredits()
    {
        playAudio(menuClip, audioSource, false);
        SceneManager.LoadScene("CreditsScene");
    }
    public void goInstroduction()
    {
        playAudio(menuClip, audioSource, false);
        SceneManager.LoadScene("IntroductionScene");
    }

    public void goJobAddn()
    {
        playAudio(menuClip, audioSource, false);
        SceneManager.LoadScene("JobAddScene");
    }
    public void Exit()
    {
        playAudio(menuClip, audioSource, false);
        Application.Quit();
    }

    private void playAudio(AudioClip clip, AudioSource audioSource, bool contPlay)
    {
        if ((contPlay) && (audioSource.isPlaying))
        {
            return;
        }
        int volumeSet = PlayerPrefs.GetInt("FXvolumeSet");
        float vol = 1f;
        if (volumeSet > 0)
        {
            int volume = PlayerPrefs.GetInt("FXVolume");
            vol = 1f;
            vol = (float)volume / 100f;
        }

        audioSource.PlayOneShot(clip, vol);
    }
}
