using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public LayerMask draggableMask;
    public LayerMask destinationMask;
    public LayerMask homeMask;
    public LayerMask inTray;
    public LayerMask booksMask;

    public LayerMask stamperMask;

    public GameObject stampFirst;

    public GameObject endDayPanel;

    public GameObject instructionsPanel;
    public GameObject supplementaryPanel;
    public GameObject directoryPanel;

    public GameObject endMonthPanel;

    public GameObject officeStamp;


    public List<GameObject> letterPrefabs;
    public List<GameObject> letterBackPrefabs;

    public List<GameObject> postcardPrefabs;
    public List<GameObject> postcardBackPrefabs;

    public GameObject airmailPrefab;
    public GameObject specialDeliveryPrefab;

    public GameObject privatePrefab;
    public GameObject overduePrefab;
    public GameObject FinalnoticePrefab;
    public GameObject evictionPrefab;

    public List<TMP_FontAsset> typedLetterFonts;
    public List<TMP_FontAsset> handLetterFonts;

    public Image postitImage;

    public AudioSource audioSource;
    public AudioSource audioSourceShort;

    public AudioClip menuClip;
    public AudioClip pickLetterClip;
    public AudioClip postLetterClip;
    public AudioClip openManualClip;
    public AudioClip stampClip;
    //public AudioClip postLetterClip;

    private int day;
    private int numletters;
    private int lettersPosted;

    private int totalLetterCount;
    private int totalCorrectDestinations;
    private int totalIncorrectDestinations;
    private int totalUndelivered;

    GameObject highlightedGO;

    GameObject selectedObject;
    GameObject draggingObject;

    private Vector3 selectedObjectStartPos;
    bool isDragging;
    bool isDraggingStamper;
    private Vector3 screenPoint;
    private Vector3 offset;

    private Vector3 letter_main = new Vector3(6f, 3f, 1f);
    private Vector3 large_letter = new Vector3(12f, 6f, 1f);
    private Vector3 back_letter = new Vector3(2f, 2f, 1f);

    private Vector3 postcard_main = new Vector3(0.5f, 0.5f, 1f);
    private Vector3 large_postcard = new Vector3(1f, 1f, 1f);

    private bool activeLetter;
    private bool examineMode;

    private int spawnLetterCount;
    private bool specialLeterSpawned;
    // Start is called before the first frame update

    private void Awake()
    {
        endDayPanel.SetActive(false);
        endMonthPanel.SetActive(false);
    }
    void Start()
    {
        activeLetter = false;
        examineMode = false;
        isDragging = false;
        day = 1;
        numletters = 5;
        lettersPosted = 0;

        totalLetterCount = 0;
        totalCorrectDestinations = 0;
        totalIncorrectDestinations = 0;
        totalUndelivered = 0;

        spawnLetterCount = 0;
        specialLeterSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        
        RaycastHit2D highlightHit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, destinationMask | inTray);

        if (highlightHit.collider != null)
        {
            if (highlightHit.collider.gameObject.GetComponent<MouseOverController>() != null)
            {
                if (highlightedGO == null)
                {
                    highlightedGO = highlightHit.collider.gameObject;
                    highlightedGO.GetComponent<MouseOverController>().MouseEnter();
                }
                else if (highlightedGO != highlightHit.collider.gameObject)
                {
                    highlightedGO.GetComponent<MouseOverController>().MouseExit();
                    highlightedGO = highlightHit.collider.gameObject;
                    highlightedGO.GetComponent<MouseOverController>().MouseEnter();
                }

            }

        }
        else if (highlightedGO != null)
        {
            highlightedGO.GetComponent<MouseOverController>().MouseExit();
            highlightedGO = null;
        }

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, (inTray | draggableMask));
        //if (hit.collider != null)
        //{
        //    Debug.Log(hit.collider.gameObject.name);
        //    selectedObject = hit.collider.gameObject;
        //    selectedObject.GetComponent<SpriteRenderer>().color = Color.yellow;

        //}

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D booksHit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, booksMask);
            if (booksHit.collider != null)
            {
                if (booksHit.collider.gameObject.tag == "Instructions")
                {
                    playAudio(openManualClip, audioSourceShort, false);
                    instructionsPanel.SetActive(true);
                }
                else if (booksHit.collider.gameObject.tag == "Supplementary")
                {
                    playAudio(openManualClip, audioSourceShort, false);
                    supplementaryPanel.SetActive(true);
                }
                else if (booksHit.collider.gameObject.tag == "Directory")
                {
                    playAudio(openManualClip, audioSourceShort, false);
                    directoryPanel.SetActive(true);
                }
            }



            // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, draggableMask | inTray);
            if (hit.collider != null)
            {
                selectedObject = hit.collider.gameObject;
                if (selectedObject.tag == "Letter")
                {
                    //if (!selectedObject.GetComponent<LetterController>().viewing)
                    if (!examineMode)
                    {
                        draggingObject = hit.collider.gameObject;
                        selectedObjectStartPos = hit.collider.gameObject.transform.position;
                        isDragging = true;
                        Color itemColor = draggingObject.GetComponent<SpriteRenderer>().color;
                        draggingObject.GetComponent<SpriteRenderer>().color = new Color(itemColor.r, itemColor.g, itemColor.b, 0.5f);
                        draggingObject.transform.localScale = new Vector3(2f, 1f, 1f);
                        draggingObject.transform.Rotate(0, 0, 90);
                        offset = hit.collider.gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
                    }
                    else
                    {
                        //selectedObject.transform.position = new Vector3(0, 0, 0);
                        Letter letter = selectedObject.GetComponent<LetterController>().letter;
                        //if (letter.backside != null)
                        //{
                            selectedObject.transform.position = new Vector3(30, 0, 0);
                            letter.backside.transform.position = new Vector3(0, 0, 0);
                       // }
                    }
                }
                if (selectedObject.tag == "Postcard")
                {
                    //if (!selectedObject.GetComponent<LetterController>().viewing)
                    if (!examineMode)
                    {
                        draggingObject = hit.collider.gameObject;
                        selectedObjectStartPos = hit.collider.gameObject.transform.position;
                        isDragging = true;
                        Color itemColor = draggingObject.GetComponent<SpriteRenderer>().color;
                        draggingObject.GetComponent<SpriteRenderer>().color = new Color(itemColor.r, itemColor.g, itemColor.b, 0.5f);
                        draggingObject.transform.localScale = new Vector3(0.25f, 0.25f, 1f);
                        draggingObject.transform.Rotate(0, 0, 90);
                        offset = hit.collider.gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
                    }
                    else
                    {
                        //selectedObject.transform.position = new Vector3(0, 0, 0);
                        Letter letter = selectedObject.GetComponent<LetterController>().letter;
                        //if (letter.backside != null)
                        //{
                        selectedObject.transform.position = new Vector3(30, 0, 0);
                        letter.backside.transform.position = new Vector3(0, 0, 0);
                        // }
                    }
                }
                if ((selectedObject.tag == "InTray") && (!activeLetter))
                {
                    activeLetter = true;
                    playAudio(pickLetterClip, audioSourceShort, false);
                    SpawnLetter();
                }
                if (selectedObject.tag == "Stamper")
                {
                    draggingObject = hit.collider.gameObject;
                    selectedObjectStartPos = hit.collider.gameObject.transform.position;
                    isDragging = true;
                    isDraggingStamper = true;
                    //Color itemColor = draggingObject.GetComponent<SpriteRenderer>().color;
                    //draggingObject.GetComponent<SpriteRenderer>().color = new Color(itemColor.r, itemColor.g, itemColor.b, 0.5f);
                    //draggingObject.transform.localScale = new Vector3(0.25f, 0.25f, 1f);
                    //draggingObject.transform.Rotate(0, 0, 90);
                    offset = hit.collider.gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
                }


            }

        }

        if (isDragging)
        {
            //Debug.Log("Dragging");
            draggingObject.transform.position = mousePos() + offset;
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, homeMask);
            //if (hit.collider != null)
            //{

            //    draggingObject.transform.localScale = letter_main;
            //    draggingObject.transform.Rotate(0, 0, 90); 

            //}
            //else
            //{
            //    draggingObject.transform.localScale = new Vector3(2f, 1f, 1f);
            //    draggingObject.transform.Rotate(0, 0, 0);
            //}
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                if (draggingObject.tag != "Stamper")
                {
                    isDragging = false;
                    Color itemColor = draggingObject.GetComponent<SpriteRenderer>().color;
                    draggingObject.GetComponent<SpriteRenderer>().color = new Color(itemColor.r, itemColor.g, itemColor.b, 1f);
                    draggingObject.transform.localScale = letter_main;
                    draggingObject.transform.Rotate(0, 0, -90);
                    Debug.Log("GetMouseButtonUp");
                    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, destinationMask);
                    if (hit.collider != null)
                    {
                        totalLetterCount++;
                        int chosenDestination = hit.collider.gameObject.GetComponent<DestinationController>().destination;
                        Letter letter = draggingObject.GetComponent<LetterController>().letter;
                        if (letter.destination == chosenDestination)
                        {
                            totalCorrectDestinations++;
                        }
                        else
                        {
                            if (chosenDestination == 32)
                            {
                                totalUndelivered++;
                            }
                            else
                            {
                                totalIncorrectDestinations++;
                            }
                        }

                        Debug.Log("Delivered: " + letter.printAddress());
                        activeLetter = false;
                        playAudio(postLetterClip, audioSourceShort, false);
                        Destroy(letter.backside);
                        Destroy(draggingObject);
                        lettersPosted++;
                        if (lettersPosted >= numletters)
                        {
                            doEndDay();
                        }


                    }
                    else
                    {
                        if (draggingObject != null)
                        {
                            if (draggingObject.tag == "Postcard")
                            {
                                draggingObject.transform.localScale = postcard_main;
                                draggingObject.transform.position = selectedObjectStartPos;
                            }
                            else
                            {
                                draggingObject.transform.localScale = letter_main;
                                draggingObject.transform.position = selectedObjectStartPos;
                            }
                        }
                    }
                }
                else
                {
                    isDraggingStamper = false;
                    isDragging = false;
                    draggingObject.transform.position = selectedObjectStartPos;
                }
            }




        }

        if (Input.GetMouseButtonDown(1))
        {
            selectedObject = hit.collider.gameObject;
            if (selectedObject.tag == "Letter")
            {
                if (isDraggingStamper)
                {
                    playAudio(stampClip, audioSourceShort, false);
                    Vector3 stampLocation = selectedObject.transform.Find("StampLocation").position;
                    float xvary = Random.Range(-0.2f, 0.2f);
                    float yvary = Random.Range(-0.2f, 0.2f);
                    GameObject sGO = Instantiate(officeStamp, new Vector3(stampLocation.x + xvary, stampLocation.y+ yvary, 0), Quaternion.identity, selectedObject.transform.Find("StampLocation"));
                }
                else
                {


                    //if (selectedObject.GetComponent<LetterController>().viewing)
                    if (examineMode)
                    {
                        //examineMode = false;
                        Letter letter = selectedObject.GetComponent<LetterController>().letter;
                        if (letter.isBack)
                        {
                            selectedObject.transform.position = new Vector3(30, -3f, 0);
                            letter.backside.transform.position = new Vector3(0, -3f, 0);
                            letter.backside.transform.localScale = letter_main;
                        }
                        else
                        {
                            selectedObject.transform.position = new Vector3(0, -3f, 0);
                            selectedObject.transform.localScale = letter_main;
                        }


                    }
                    else
                    {
                        //examineMode = true;
                        selectedObject.transform.position = new Vector3(0, 0, 0);
                        selectedObject.transform.localScale = large_letter;
                        //selectedObject.transform.localScale = new Vector3(12f, 6f, 1f);
                    }
                    selectedObject.GetComponent<LetterController>().viewing = !selectedObject.GetComponent<LetterController>().viewing;
                    examineMode = !examineMode;
                }
            }
            if (selectedObject.tag == "Postcard")
            {
                //if (selectedObject.GetComponent<LetterController>().viewing)
                if (examineMode)
                {
                    //examineMode = false;
                    Letter letter = selectedObject.GetComponent<LetterController>().letter;
                    if (letter.isBack)
                    {
                        selectedObject.transform.position = new Vector3(30, -2f, 0);
                        letter.backside.transform.position = new Vector3(0, -2f, 0);
                        letter.backside.transform.localScale = postcard_main;
                    }
                    else
                    {
                        selectedObject.transform.position = new Vector3(0, -2f, 0);
                        selectedObject.transform.localScale = postcard_main;
                    }


                }
                else
                {
                    //examineMode = true;
                    selectedObject.transform.position = new Vector3(0, 0, 0);
                    selectedObject.transform.localScale = large_postcard;
                    //selectedObject.transform.localScale = new Vector3(12f, 6f, 1f);
                }
                selectedObject.GetComponent<LetterController>().viewing = !selectedObject.GetComponent<LetterController>().viewing;
                examineMode = !examineMode;
            }
        }

    }

    Vector3 mousePos()
    {
        Vector3 mouseScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPoint);
        return mousePosition;
    }

    void SpawnLetter()
    {
        if (day == 1)
        {
            //SpawnSpecialPostcard(2);
            SpawnLocalLetter(false, false);
        }

        else if (day == 2)
        {
            //Local 
            //Typed and handwritten
            SpawnLocalLetter(true, false);

        }
        else if (day == 3)
        {
            //Local and national 
            //Typed and handwritten
            if (Random.Range(0, 2) > 0)
            {
                SpawnNationalLetter(true, false, false);
            }
            else
            {
                SpawnLocalLetter(true, false);
            }
        }
        else if (day == 4)
        {
            //Local and national 
            //Typed and handwritten
            if (Random.Range(0, 2) > 0)
            {
                SpawnInternationalLetter(true, false, false);
            }
            else
            {
                SpawnLocalLetter(true, false);
            }
        }
        else if (day == 5)
        {
            //Local,national and international 
            //Typed and handwritten
            int letterType = Random.Range(0, 3);
            switch (letterType)
            {
                case 0:
                    SpawnLocalLetter(true, false);
                    break;
                case 1:
                    SpawnInternationalLetter(true, false, false);
                    break;
                case 2:
                    SpawnNationalLetter(true, false, false);
                    break;
                default:
                    SpawnLocalLetter(true, false);
                    break;
            }

        }
        else if (day == 6)
        {
            //Local,national and international 
            //Airmail
            //Typed and handwritten
            if ((spawnLetterCount > 6) && (!specialLeterSpawned))
            {
                SpawnSpecial(privatePrefab);
                specialLeterSpawned = true;
            }
            else
            {
                int letterType = Random.Range(0, 4);
                switch (letterType)
                {
                    case 0:
                        SpawnLocalLetter(true, false);
                        break;
                    case 1:
                        SpawnInternationalLetter(true, true, false);
                        break;
                    case 2:
                        SpawnNationalLetter(true, false, false);
                        break;
                    case 3:
                        if (!specialLeterSpawned)
                        {
                            SpawnSpecial(privatePrefab);
                            specialLeterSpawned = true;
                        }
                        else
                        {
                            SpawnLocalLetter(true, false);
                        }

                        break;
                    default:
                        SpawnLocal(true, false);
                        break;
                }
            }
            spawnLetterCount++;
        }

        else if (day == 11)
        {
            //Specila Letter 2
            if ((spawnLetterCount > 6) && (!specialLeterSpawned))
            {
                SpawnSpecial(overduePrefab);
                specialLeterSpawned = true;
            }
            else
            {
                int letterType = Random.Range(0, 3);
                switch (letterType)
                {
                    case 0:
                        SpawnLocal(true, true);
                        break;
                    case 1:
                        SpawnInternationalLetter(true, true, true);
                        break;
                    case 2:
                        SpawnNationalLetter(true, true, true);
                        break;
                    case 3:
                        if (!specialLeterSpawned)
                        {
                            SpawnSpecial(overduePrefab);
                            specialLeterSpawned = true;
                        }
                        else
                        {
                            SpawnLocal(true, true);
                        }

                        break;
                    default:
                        SpawnLocal(true, true);
                        break;
                }
            }
            spawnLetterCount++;
        }
        else if (day == 11)
        {
            //Specila Letter 2
            if ((spawnLetterCount > 6) && (!specialLeterSpawned))
            {
                SpawnSpecialPostcard(0);
                specialLeterSpawned = true;
            }
            else
            {
                int letterType = Random.Range(0, 3);
                switch (letterType)
                {
                    case 0:
                        SpawnLocal(true, true);
                        break;
                    case 1:
                        SpawnInternationalLetter(true, true, true);
                        break;
                    case 2:
                        SpawnNationalLetter(true, true, true);
                        break;
                    case 3:
                        if (!specialLeterSpawned)
                        {
                            SpawnSpecialPostcard(0);
                            specialLeterSpawned = true;
                        }
                        else
                        {
                            SpawnLocal(true, true);
                        }

                        break;
                    default:
                        SpawnLocal(true, true);
                        break;
                }
            }
            spawnLetterCount++;
        }
        
        else if (day == 13)
        {
            //Specila Letter 2
            if ((spawnLetterCount > 6) && (!specialLeterSpawned))
            {
                SpawnSpecial(FinalnoticePrefab);
                specialLeterSpawned = true;
            }
            else
            {
                int letterType = Random.Range(0, 3);
                switch (letterType)
                {
                    case 0:
                        SpawnLocal(true, true);
                        break;
                    case 1:
                        SpawnInternationalLetter(true, true, true);
                        break;
                    case 2:
                        SpawnNationalLetter(true, true, true);
                        break;
                    case 3:
                        if (!specialLeterSpawned)
                        {
                            SpawnSpecial(FinalnoticePrefab);
                            specialLeterSpawned = true;
                        }
                        else
                        {
                            SpawnLocal(true, true);
                        }

                        break;
                    default:
                        SpawnLocal(true, true);
                        break;
                }
            }
            spawnLetterCount++;
        }
        else if (day == 14)
        {
            //Specila Letter 2
            if ((spawnLetterCount > 6) && (!specialLeterSpawned))
            {
                SpawnSpecialPostcard(1);
                specialLeterSpawned = true;
            }
            else
            {
                int letterType = Random.Range(0, 3);
                switch (letterType)
                {
                    case 0:
                        SpawnLocal(true, true);
                        break;
                    case 1:
                        SpawnInternationalLetter(true, true, true);
                        break;
                    case 2:
                        SpawnNationalLetter(true, true, true);
                        break;
                    case 3:
                        if (!specialLeterSpawned)
                        {
                            SpawnSpecialPostcard(1);
                            specialLeterSpawned = true;
                        }
                        else
                        {
                            SpawnLocal(true, true);
                        }

                        break;
                    default:
                        SpawnLocal(true, true);
                        break;
                }
            }
            spawnLetterCount++;
        }
        else if (day == 17)
        {
            //Specila Letter 2
            if ((spawnLetterCount > 6) && (!specialLeterSpawned))
            {
                SpawnSpecial(evictionPrefab);
                specialLeterSpawned = true;
            }
            else
            {
                int letterType = Random.Range(0, 3);
                switch (letterType)
                {
                    case 0:
                        SpawnLocal(true, true);
                        break;
                    case 1:
                        SpawnInternationalLetter(true, true, true);
                        break;
                    case 2:
                        SpawnNationalLetter(true, true, true);
                        break;
                    case 3:
                        if (!specialLeterSpawned)
                        {
                            SpawnSpecial(evictionPrefab);
                        }
                        else
                        {
                            SpawnLocal(true, true);
                        }

                        break;
                    default:
                        SpawnLocal(true, true);
                        break;
                }
            }
            spawnLetterCount++;
        }
        else if (day == 25)
        {
            //Specila Letter 2
            if ((spawnLetterCount > 6) && (!specialLeterSpawned))
            {
                SpawnSpecialPostcard(2);
                specialLeterSpawned = true;
            }
            else
            {
                int letterType = Random.Range(0, 3);
                switch (letterType)
                {
                    case 0:
                        SpawnLocal(true, true);
                        break;
                    case 1:
                        SpawnInternationalLetter(true, true, true);
                        break;
                    case 2:
                        SpawnNationalLetter(true, true, true);
                        break;
                    case 3:
                        if (!specialLeterSpawned)
                        {
                            SpawnSpecialPostcard(2);
                        }
                        else
                        {
                            SpawnLocal(true, true);
                        }

                        break;
                    default:
                        SpawnLocal(true, true);
                        break;
                }
            }
            spawnLetterCount++;
        }
        else
        {
            //Local,national and international 
            //Airmail and special delivery
            //Typed and handwritten

            int letterType = Random.Range(0, 3);
            switch (letterType)
            {
                case 0:
                    SpawnLocal(true, true);
                    break;
                case 1:
                    SpawnInternationalLetter(true, true, true);
                    break;
                case 2:
                    SpawnNationalLetter(true, true, true);
                    break;
                default:
                    SpawnLocal(true, true);
                    break;
            }
        }
    }
    void SpawnLocal(bool allowHandwritten, bool allowNoAddress)
    {
        float doLetter = Random.Range(0, 10);
        if (doLetter > 2.5)
        {
            SpawnLocalLetter(allowHandwritten, allowNoAddress);

            }
        else
        {
            SpawnLocalPostcard();
        }
    }
    void SpawnLocalPostcard()
    {
        int postcard = Random.Range(0, 3);
        GameObject lGO = Instantiate(postcardPrefabs[postcard], new Vector3(0, -2f, 0), Quaternion.identity);
        lGO.transform.localScale = postcard_main;
        Letter letter = lGO.GetComponent<LetterController>().letter;
        GameObject bGO = Instantiate(postcardBackPrefabs[0], new Vector3(30f, -3f, 0), Quaternion.identity);
        //bGO.transform.localScale = back_letter;
        Letter backLetter = bGO.GetComponent<LetterController>().letter;
        backLetter.isBack = true;
        backLetter.backside = lGO;
        letter.backside = bGO;

        int localAddress = Random.Range(0, 50);
        int foreaname = Random.Range(0, 6);

        if (foreaname == 0)
        {
            letter.address.name = Constants.localDirectory[localAddress, 2];
        }
        else
        {
            letter.address.name = Constants.localDirectory[localAddress, 3];
        }
        letter.address.name += " " + Constants.localDirectory[localAddress, 4];

        letter.address.road = Constants.localDirectory[localAddress, 0] + " " + Constants.localDirectory[localAddress, 1];
        letter.address.postCode = Constants.localDirectory[localAddress, 5];
        int letterDestination;
        if (int.TryParse(Constants.localDirectory[localAddress, 6], out letterDestination))
        {
            letter.destination = letterDestination;
        }
        int selecFont = Random.Range(0, handLetterFonts.Count);
        TextMeshPro[] textBoxes;


        textBoxes = bGO.GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro textBox in textBoxes)
        {
            if (textBox.name == "AddressText")
            {

                textBox.font = handLetterFonts[selecFont];

                textBox.text = letter.address.name + "\r\n" + letter.address.road + "\r\n" + letter.address.postCode;
            }
            else if (textBox.name == "ContentsText")
            {
                int postcardMessage = Random.Range(0, 4);

                textBox.font = handLetterFonts[selecFont];

                textBox.text = Constants.postcardFlavour[postcardMessage];
            }
        }

   

        Vector3 stampLocation = bGO.transform.Find("StampLocation").position;
        GameObject sGO = Instantiate(stampFirst, new Vector3(stampLocation.x, stampLocation.y, 0), Quaternion.identity, bGO.transform.Find("StampLocation"));
        sGO.transform.localScale = new Vector3(2f, 2f, 1f);
    }

        void SpawnLocalLetter(bool allowHandwritten, bool allowNoAddress)
    {
        int envelope = Random.Range(0, 4);
        GameObject lGO = Instantiate(letterPrefabs[envelope], new Vector3(0, -3f, 0), Quaternion.identity);
        lGO.transform.localScale = letter_main;
        Letter letter = lGO.GetComponent<LetterController>().letter;
        GameObject bGO = Instantiate(letterBackPrefabs[envelope], new Vector3(30f, -3f, 0), Quaternion.identity);
        bGO.transform.localScale = back_letter;
        Letter backLetter = bGO.GetComponent<LetterController>().letter;
        backLetter.isBack = true;
        backLetter.backside = lGO;
        letter.backside = bGO;

        int localAddress = Random.Range(0, 50);
        int foreaname = Random.Range(0, 6);

        if (foreaname == 0)
        {
            letter.address.name = Constants.localDirectory[localAddress, 2];
        }
        else
        {
            letter.address.name = Constants.localDirectory[localAddress, 3];
        }
        letter.address.name += " " + Constants.localDirectory[localAddress, 4];

        letter.address.road = Constants.localDirectory[localAddress, 0] + " " + Constants.localDirectory[localAddress, 1];
        letter.address.postCode = Constants.localDirectory[localAddress, 5];
        int letterDestination;
        if (int.TryParse(Constants.localDirectory[localAddress, 6], out letterDestination))
        {
            letter.destination = letterDestination;
        }

        int selecFont = Random.Range(0, typedLetterFonts.Count);
        lGO.GetComponentInChildren<TextMeshPro>().font = typedLetterFonts[selecFont];
        if (allowHandwritten && (Random.Range(0, 2) > 0))
        {
            selecFont = Random.Range(0, handLetterFonts.Count);
            lGO.GetComponentInChildren<TextMeshPro>().font = handLetterFonts[selecFont];
            int penColor = Random.Range(0, 6);
            switch(penColor)
            {
                case 0:
                case 1:
                case 2:
                    lGO.GetComponentInChildren<TextMeshPro>().color = Color.black;
                        break;
                case 3:
                case 4:
                    lGO.GetComponentInChildren<TextMeshPro>().color = Color.blue;
                    break;
                case 5:
                    float penStrength = Random.Range(35f, 100f);
                    lGO.GetComponentInChildren<TextMeshPro>().color = new Color(255f,255f,255f, penStrength);
                    break;
            }
        }
        int noAddress = Random.Range(0, 20);
        if((allowNoAddress) && (noAddress == 0))
        {
            lGO.GetComponentInChildren<TextMeshPro>().text = letter.address.name + "\r\n" ;
        }
        else
        {
            lGO.GetComponentInChildren<TextMeshPro>().text = letter.address.name + "\r\n" + letter.address.road + "\r\n" + letter.address.postCode;
        }
        Vector3 stampLocation = lGO.transform.Find("StampLocation").position;
        GameObject sGO = Instantiate(stampFirst, new Vector3(stampLocation.x, stampLocation.y, 0), Quaternion.identity, lGO.transform.Find("StampLocation"));
        //sGO.transform.localScale = new Vector3(0.15f, 0.28f, 1f);
        //sGO.transform.position = new Vector3(0,0,0);
    }

    void SpawnNationalLetter(bool allowHandwritten, bool allowSpecial, bool noCityAllowed)
    {
        int envelope = Random.Range(0, 4);
        GameObject lGO = Instantiate(letterPrefabs[envelope], new Vector3(0, -3f, 0), Quaternion.identity);
        lGO.transform.localScale = letter_main;
        Letter letter = lGO.GetComponent<LetterController>().letter;
        GameObject bGO = Instantiate(letterBackPrefabs[envelope], new Vector3(30f, -3f, 0), Quaternion.identity);
        bGO.transform.localScale = back_letter;
        Letter backLetter = bGO.GetComponent<LetterController>().letter;
        backLetter.isBack = true;
        backLetter.backside = lGO;
        letter.backside = bGO;

        int name = Random.Range(0, Constants.names.Length);
        letter.address.name = Constants.names[name];
        
        int special = 0;
        if ((allowSpecial) && (Random.Range(0, 2) > 0))
        {
            special = 5;
        }

        int city = Random.Range(0, 5);
        int postcode = Random.Range(0, 12);
        int houseNum = Random.Range(0, 40);
        int streetName = Random.Range(0, Constants.streetNames.Length);
        int noCity = Random.Range(0, 8);
        if ((noCityAllowed) && (noCity == 0))
        {
            letter.address.road = "" + houseNum + " " + Constants.streetNames[streetName] ;
        }
        else
        {
            letter.address.road = "" + houseNum + " " + Constants.streetNames[streetName] + "\r\n" + Constants.nationalDirectory[city, 0];
        }
        letter.address.postCode = Constants.nationalDirectory[city, 1] + postcode;

        int letterDestination;
        if (int.TryParse(Constants.nationalDirectory[city, 2], out letterDestination))
        {
            letter.destination = letterDestination + special;
        }


        int selecFont = Random.Range(0, typedLetterFonts.Count);
        lGO.GetComponentInChildren<TextMeshPro>().font = typedLetterFonts[selecFont];
        if (allowHandwritten && (Random.Range(0,2) > 0))
        {
            selecFont = Random.Range(0, handLetterFonts.Count);
            lGO.GetComponentInChildren<TextMeshPro>().font = handLetterFonts[selecFont];
            int penColor = Random.Range(0, 6);
            switch (penColor)
            {
                case 0:
                case 1:
                case 2:
                    lGO.GetComponentInChildren<TextMeshPro>().color = Color.black;
                    break;
                case 3:
                case 4:
                    lGO.GetComponentInChildren<TextMeshPro>().color = Color.blue;
                    break;
                case 5:
                    float penStrength = Random.Range(35f, 100f);
                    lGO.GetComponentInChildren<TextMeshPro>().color = new Color(255f, 255f, 255f, penStrength);
                    break;
            }
        }

        lGO.GetComponentInChildren<TextMeshPro>().text = letter.address.name + "\r\n" + letter.address.road + "\r\n" + letter.address.postCode;
        //lGO.GetComponentInChildren<TextMeshPro>().font = typedLetterFonts[selecFont];
        Vector3 stampLocation = lGO.transform.Find("StampLocation").position;
        GameObject sGO = Instantiate(stampFirst, new Vector3(stampLocation.x, stampLocation.y, 0), Quaternion.identity, lGO.transform.Find("StampLocation"));
        if (special > 0)
        {
            Vector3 specialMailLocation = lGO.transform.Find("SpecialLocation1").position;
            GameObject aGO = Instantiate(specialDeliveryPrefab, new Vector3(specialMailLocation.x, specialMailLocation.y - 0.1f, 0), Quaternion.identity, lGO.transform.Find("SpecialLocation1"));

        }
    }

    void SpawnInternationalLetter(bool allowHandwritten, bool allowAirmail, bool noCouutryEnabled)
    {
        int envelope = Random.Range(0, 4);
        GameObject lGO = Instantiate(letterPrefabs[envelope], new Vector3(0, -3f, 0), Quaternion.identity);
        lGO.transform.localScale = letter_main;
        Letter letter = lGO.GetComponent<LetterController>().letter;
        GameObject bGO = Instantiate(letterBackPrefabs[envelope], new Vector3(30f, -3f, 0), Quaternion.identity);
        bGO.transform.localScale = back_letter;
        Letter backLetter = bGO.GetComponent<LetterController>().letter;
        backLetter.isBack = true;
        backLetter.backside = lGO;
        letter.backside = bGO;

        int name = Random.Range(0, Constants.names.Length);
        letter.address.name = Constants.names[name];
        int airmail = 0;
        if ((allowAirmail) && (Random.Range(0, 2) > 0))
        {
            airmail = 5;
        }
        int country = Random.Range(0, 45);
        int postcode = Random.Range(0, 10);
        int houseNum = Random.Range(0, 40);
        int streetName = Random.Range(0, Constants.streetNames.Length);
        int noCountry = Random.Range(0, 8);
        if ((noCouutryEnabled) && (noCountry == 0))
        {
            letter.address.road = "" + houseNum + " " + Constants.streetNames[streetName];
        }
        else 
        { 
            letter.address.road = "" + houseNum + " " + Constants.streetNames[streetName] + "\r\n" + Constants.internationalDirectory[country, 0];
        }
        letter.address.postCode = Constants.internationalDirectory[country, 1] + postcode;
        
        int letterDestination;
        if (int.TryParse(Constants.internationalDirectory[country, 3], out letterDestination))
        {
            letter.destination = letterDestination + airmail;
        }


        int selecFont = Random.Range(0, typedLetterFonts.Count);
        lGO.GetComponentInChildren<TextMeshPro>().font = typedLetterFonts[selecFont];
        if (allowHandwritten && (Random.Range(0, 2) > 0))
        {
            selecFont = Random.Range(0, handLetterFonts.Count);
            lGO.GetComponentInChildren<TextMeshPro>().font = handLetterFonts[selecFont];
            int penColor = Random.Range(0, 6);
            switch (penColor)
            {
                case 0:
                case 1:
                case 2:
                    lGO.GetComponentInChildren<TextMeshPro>().color = Color.black;
                    break;
                case 3:
                case 4:
                    lGO.GetComponentInChildren<TextMeshPro>().color = Color.blue;
                    break;
                case 5:
                    float penStrength = Random.Range(25f, 100f);
                    lGO.GetComponentInChildren<TextMeshPro>().color = new Color(255f, 255f, 255f, penStrength);
                    break;
            }
        }

        lGO.GetComponentInChildren<TextMeshPro>().text = letter.address.name + "\r\n" + letter.address.road + "\r\n" + letter.address.postCode;
        //lGO.GetComponentInChildren<TextMeshPro>().font = typedLetterFonts[selecFont];
        Vector3 stampLocation = lGO.transform.Find("StampLocation").position;
        GameObject sGO = Instantiate(stampFirst, new Vector3(stampLocation.x, stampLocation.y, 0), Quaternion.identity, lGO.transform.Find("StampLocation"));
        if (airmail > 0)
        {
            Vector3 airMailLocation = lGO.transform.Find("SpecialLocation1").position;
            GameObject aGO = Instantiate(airmailPrefab, new Vector3(airMailLocation.x, airMailLocation.y, 0), Quaternion.identity, lGO.transform.Find("SpecialLocation1"));

        }
    }

        private void SpawnSpecial(GameObject extraPrefab)
        {
        int envelope = Random.Range(0, 4);
        GameObject lGO = Instantiate(letterPrefabs[3], new Vector3(0, -3f, 0), Quaternion.identity);
        lGO.transform.localScale = letter_main;
        Letter letter = lGO.GetComponent<LetterController>().letter;
        GameObject bGO = Instantiate(letterBackPrefabs[3], new Vector3(30f, -3f, 0), Quaternion.identity);
        bGO.transform.localScale = back_letter;
        Letter backLetter = bGO.GetComponent<LetterController>().letter;
        backLetter.isBack = true;
        backLetter.backside = lGO;
        letter.backside = bGO;

        lGO.GetComponentInChildren<TextMeshPro>().font = typedLetterFonts[1];
        //"4","Leatherhead Close" , "Mrs", "Jennifer", "Mundy", "BW7", "27" },
        lGO.GetComponentInChildren<TextMeshPro>().text ="Mrs Jennifer Mundy" + "\r\n" +"4 Leatherhead Close" + "\r\n" + "BW7";

        letter.destination = 27;

        //lGO.GetComponentInChildren<TextMeshPro>().font = typedLetterFonts[selecFont];
        Vector3 stampLocation = lGO.transform.Find("StampLocation").position;
        GameObject sGO = Instantiate(stampFirst, new Vector3(stampLocation.x, stampLocation.y, 0), Quaternion.identity, lGO.transform.Find("StampLocation"));

        Vector3 extrasLocation = lGO.transform.Find("ExtrasLocation").position;
        GameObject aGO = Instantiate(extraPrefab, new Vector3(extrasLocation.x, extrasLocation.y, 0), Quaternion.identity, lGO.transform.Find("ExtrasLocation"));
    }

    private void SpawnSpecialPostcard( int postcard)
    {
        // { "4","Brick Road" , "Mrs", "Pamela", "Blair", "BW2", "22" },
        GameObject lGO = Instantiate(postcardPrefabs[postcard], new Vector3(0, -2f, 0), Quaternion.identity);
        lGO.transform.localScale = postcard_main;
        Letter letter = lGO.GetComponent<LetterController>().letter;
        GameObject bGO = Instantiate(postcardBackPrefabs[0], new Vector3(30f, -3f, 0), Quaternion.identity);
        //bGO.transform.localScale = back_letter;
        Letter backLetter = bGO.GetComponent<LetterController>().letter;
        backLetter.isBack = true;
        backLetter.backside = lGO;
        letter.backside = bGO;

        TextMeshPro[] textBoxes;
        textBoxes = bGO.GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro textBox in textBoxes)
        {
            if (textBox.name == "AddressText")
            {

                textBox.font = handLetterFonts[0];

                textBox.text = "Pam  Blair\r\n4 Brick Road\r\nBW2";
            }
            else if (textBox.name == "ContentsText")
            {
                int postcardMessage = Random.Range(0, 4);

                textBox.font = handLetterFonts[0];
                if (postcard == 0)
                {
                    textBox.text = Constants.spost1;
                }
                else if(postcard == 1)
                {
                    textBox.text = Constants.spost2;
                }
                else
                {
                    textBox.text = Constants.spost3;
                }
                
            }
        }



        Vector3 stampLocation = bGO.transform.Find("StampLocation").position;
        GameObject sGO = Instantiate(stampFirst, new Vector3(stampLocation.x, stampLocation.y, 0), Quaternion.identity, bGO.transform.Find("StampLocation"));
        sGO.transform.localScale = new Vector3(2f, 2f, 1f);
    }
    private void doEndDay()
        {
        if (day == 30)
        {
            endMonthPanel.SetActive(true);
        }
        else
        {

            endDayPanel.SetActive(true);
            string endText = "Day " + day + "\r\n";
            endText += "Letters sorted: " + totalLetterCount + "\r\n";
            endText += "Letters correctly sorted: " + totalCorrectDestinations + "\r\n";
            endText += "Letters incorrectly sorted: " + totalIncorrectDestinations + "\r\n";
            endText += "Letters designated undeliverable: " + totalUndelivered + "\r\n";

            endDayPanel.GetComponentInChildren<TextMeshProUGUI>().text = endText;
            foreach (var textBox in endDayPanel.GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                if (textBox.name == "ReportText")
                {
                    textBox.text = endText;
                }
                if (textBox.name == "RefText")
                {
                    int refNum = Random.Range(1, 2000);
                    textBox.text = "REF:" + refNum;
                }
            }

            setPostitNote();
        }

    }
    public void nextDay()
    {
        day++;
        if (day > 2)
        {
            numletters = 10;
        }
        else
        {
            numletters = 5;
        }
        lettersPosted = 0;
        totalLetterCount = 0;
        totalCorrectDestinations = 0;
        totalIncorrectDestinations = 0; 
        totalUndelivered =0;
        specialLeterSpawned = false;
        spawnLetterCount = 0;
        endDayPanel.SetActive(false);
    }

    private void setPostitNote()
    {      
        if (day == 1)
        {
            if (totalCorrectDestinations == totalLetterCount)
            {
                postitImage.GetComponentInChildren<TextMeshProUGUI>().text = Constants.goodJob[0];
            }
            else if (totalIncorrectDestinations >= 2)
            {
                postitImage.GetComponentInChildren<TextMeshProUGUI>().text = Constants.refManuals[0];
                
            }
            else
            {
                postitImage.GetComponentInChildren<TextMeshProUGUI>().text = Constants.endDayFlavour[1];
            }
            postitImage.gameObject.SetActive(true);
        }
        else
        {
            float isPostit = Random.Range(0, 10);
            if (isPostit > 4.5f)
            {
                float isPostit2 = Random.Range(0, 10);
                if (totalCorrectDestinations == totalLetterCount)
                {
                    if (isPostit2 > 3.0f)
                    {
                        postitImage.GetComponentInChildren<TextMeshProUGUI>().text = Constants.endDayFlavour[Random.Range(0,8)];
                    }
                    else
                    {
                        postitImage.GetComponentInChildren<TextMeshProUGUI>().text = Constants.goodJob[Random.Range(0, 4)];
                    }
                }
                else if ( totalIncorrectDestinations > 5)
                {
                    if (isPostit2 > 3.0f)
                    {
                        postitImage.GetComponentInChildren<TextMeshProUGUI>().text = Constants.endDayFlavour[Random.Range(0, 8)];
                    }
                    else
                    {
                        postitImage.GetComponentInChildren<TextMeshProUGUI>().text = Constants.refManuals[Random.Range(0, 3)];
                    }
                }
                else
                {
                    postitImage.GetComponentInChildren<TextMeshProUGUI>().text = Constants.endDayFlavour[Random.Range(0, 8)];
                }

                postitImage.gameObject.SetActive(true);
            }
            else
            {
                postitImage.gameObject.SetActive(false);
            }

        }

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
