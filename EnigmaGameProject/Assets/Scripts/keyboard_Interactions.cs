
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;


public class keyboard_interactions : MonoBehaviour
{
    System.Collections.Generic.Dictionary<string, Transform> _keyObjects;
    System.Collections.Generic.Dictionary<string, Vector3> lightGrid;
    private string buttonPressedName;
    private int keyIsPressed = 0;
    private Transform activePressedKey = null;
    public TMPro.TextMeshPro rotorText1; 
    public TMPro.TextMeshPro rotorText2;
    public TMPro.TextMeshPro rotorText3;
    private int scrollCount = 1;
    private PlugSpawner plugSpawner;
    private float lastClick;
    public Vector3[] plugPositions;
    public bool keyPressed = false;

    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    public CinemachineVirtualCamera cam3;

    [SerializeField] private Transform lightObject;

    [SerializeField] private Transform rotor1;
    
    [SerializeField] private LayerMask interactionLayer;

    [SerializeField] private float maxDistance = 100f;

    [SerializeField] private AudioClip cameraSound;
    [SerializeField] private AudioClip rotorSound;
    [SerializeField] private AudioClip type1Sound;
    [SerializeField] private AudioClip type2Sound;
    [SerializeField] private AudioClip type3Sound;
    [SerializeField] private AudioClip unplugSound;
    [SerializeField] public AudioClip plugSound;
    [SerializeField] public AudioClip electricSound;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceTwo;
    [SerializeField] private AudioSource audioSourceThree;




    void Start()
    {
        _keyObjects = new System.Collections.Generic.Dictionary<string, Transform>(); //creates a dictionary called keyobjects which will take the name of the key object and output its tranform
        lightGrid = new System.Collections.Generic.Dictionary<string, Vector3>(); //initializes a dictionary which will output the position of the light name it got
        plugSpawner = GetComponent<PlugSpawner>(); //gets the plugSpawner script
        audioSource = GetComponent<AudioSource>();
        plugPositions = new Vector3[] //initializes all the positions that plugs can be
        {
            new Vector3(-21.2485f,1.477f,4.7861f),
            new Vector3(-21.2485f,1.477f,4.5647f),
            new Vector3(-21.2485f,1.477f,4.3685f),
            new Vector3(-21.2485f,1.477f,4.1967f),
            new Vector3(-21.2485f,1.477f,4.0037f),
            new Vector3(-21.2485f,1.477f,3.8028f),
            new Vector3(-21.2485f,1.477f,3.5997f),
            new Vector3(-21.2485f,1.477f,3.4153f),
            new Vector3(-21.2485f,1.477f,3.238f),
            new Vector3(-21.2485f,1.2696f,4.6926f),
            new Vector3(-21.2485f,1.2696f,4.4815f),
            new Vector3(-21.2485f,1.2696f,4.2824f),
            new Vector3(-21.2485f,1.2696f,4.109f),
            new Vector3(-21.2485f,1.2696f,3.9075f),
            new Vector3(-21.2485f,1.2696f,3.7089f),
            new Vector3(-21.2485f,1.2696f,3.521f),
            new Vector3(-21.2485f,1.2696f,3.3215f),
            new Vector3(-21.2485f,1.0136f,4.7935f),
            new Vector3(-21.2485f,1.0136f,4.5867f),
            new Vector3(-21.2485f,1.0136f,4.3852f),
            new Vector3(-21.2485f,1.0136f,4.2035f),
            new Vector3(-21.2485f,1.0136f,4.0076f),
            new Vector3(-21.2485f,1.0136f,3.8135f),
            new Vector3(-21.2485f,1.0136f,3.6112f),
            new Vector3(-21.2485f,1.0136f,3.4115f),
            new Vector3(-21.2485f,1.0136f,3.2336f)
        };
        //for each looks through every object or child in the parent the script is attached to and adds the name and position to the key objects dictionary and the light objects dictionary
        foreach(Transform child in transform)
        {
            if (child.name.StartsWith("Key"))
            {
                _keyObjects.Add(child.name, child);

                lightGrid.Add(child.name, new Vector3(child.localPosition.x + 0.6845964f,1.523f,child.localPosition.z));
            }
        }
    }

    
    void Update()
    {   
        
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if ((Physics.Raycast(ray, out hit, maxDistance, interactionLayer) && keyIsPressed != 1) || (!(Physics.Raycast(ray, out hit, maxDistance, interactionLayer)) && keyIsPressed == 1))
        {
            Vector2 scrollDelta = Mouse.current.scroll.ReadValue();
            TMPro.TextMeshPro rotorTextObject = null;
            if (keyIsPressed == 1)
            {
                rotorTextObject = rotorText3.GetComponent<TMPro.TextMeshPro>();
                scrollCount = int.Parse(rotorTextObject.text);
                scrollDelta.y = 1;

            }
            else
            {
                string rotorTextName = hit.collider.gameObject.name + "Text";

                TMPro.TextMeshPro textObject = GameObject.Find(rotorTextName).GetComponent<TMPro.TextMeshPro>();

                rotorTextObject = textObject.GetComponent<TMPro.TextMeshPro>();
                scrollCount = int.Parse(rotorTextObject.text);
            }
            if (scrollDelta.y > 0 ) //if you are scrolling down
            {   
                if (keyIsPressed != 1)
                {
                    audioSource.clip = rotorSound;
                    audioSource.Play();
                }
                if (scrollCount == 26)
                {
                    if (rotorTextObject.text == rotorText3.text)
                    {
                        if (rotorText1.text == "26" && rotorText2.text == "26")
                        {
                            rotorText1.text = "1";
                            rotorText2.text = "1";
                        }
                        else if (rotorText2.text == "26")
                        {
                            int textNum = int.Parse(rotorText1.text);
                            rotorText1.text = (textNum += 1).ToString();
                            rotorText2.text = "1";
                        }
                        else if(rotorText3.text == "26")
                        {
                            int textNum = int.Parse(rotorText2.text);
                            rotorText2.text = (textNum += 1).ToString();
                        }
                    }
                    else if (rotorTextObject.text == rotorText2.text)
                    {
                        int textNum = int.Parse(rotorText1.text);
                        rotorText1.text = (textNum += 1).ToString();
                        if (rotorText1.text == "27" && rotorText2.text == "26")
                        {
                            rotorText1.text = "1";
                            rotorText2.text = "1";
                        }
                    }
                }
                scrollCount++;
                if (scrollCount > 26)
                {
                    scrollCount = 1;
                }
                rotorTextObject.text = scrollCount.ToString();
            } 
            else if (scrollDelta.y < 0) //if you are scrolling up
            {
                if (keyIsPressed != 1)
                {
                    audioSource.clip = rotorSound;
                    audioSource.Play();
                }
                scrollCount--;
                if (scrollCount < 1)
                {
                    scrollCount = 26;
                }
                rotorTextObject.text = scrollCount.ToString();
            }
            if(keyIsPressed != 2 && scrollDelta.y != 0) {
                keyIsPressed = 2;
            } else if (scrollDelta.y == 0){
                keyIsPressed = 0;
            }
        }
        if(_keyObjects == null) return;

        if(Keyboard.current == null) return;

        if (Keyboard.current.anyKey.wasPressedThisFrame && keyIsPressed == 0) //if a key was pressed
        {
            foreach (var keyControl in Keyboard.current.allKeys) // goes through each key to see which was pressed
            {
                if (activePressedKey == null && keyControl.wasPressedThisFrame) //if that key was pressed
                {

                    
                    string buttonPressed = keyControl.name.ToUpper();
                    buttonPressedName = "Key_" + buttonPressed;

                    if(_keyObjects.TryGetValue(buttonPressedName, out Transform keyTransform)) // tries to get the transform of that key if it is a letter
                    {

                        if (keyTransform != null && Mouse.current.scroll.ReadValue().y == 0 && !(Physics.Raycast(ray, out hit, maxDistance, interactionLayer)))
                        {
                            animateKeyPress(keyTransform,GetComponent<EnigmaInternals>().Convert(char.Parse(buttonPressed), int.Parse(rotorText1.text), int.Parse(rotorText2.text), int.Parse(rotorText3.text)));

                            if (keyIsPressed == 0)
                            {
                                keyIsPressed = 1;
                            }
                            activePressedKey = keyTransform;
                        }
                    } else if (Keyboard.current.spaceKey.wasPressedThisFrame) // if space is pressed
                    {
                        swapCam();
                    } else if (int.TryParse(keyControl.name, out int result) && result < 10 && GetComponent<EnigmaInternals>().selectingPlugs == false) // if it is and number 0-9
                    {
                        if (Time.time - lastClick <= 0.25f  && GameObject.Find("Plug"+keyControl.name).transform.childCount >= 2) // if it was a double click
                        {
                            addVector3(GameObject.Find("Plug"+int.Parse(keyControl.name)));
                            char char1 = GameObject.Find("Plug"+int.Parse(keyControl.name)).GetComponent<PlugSpawner>().char1;
                            GameObject.Find("enigma machine v1").GetComponent<EnigmaInternals>().plugChars.Remove(char1); // get the component and get char1 from plugs and use it to remove the char1 and char2 from the dictionar in engima internals with this GameObject.Find("Plug"+int.Parse(keyControl.name)).GetComponent<EnigmaInternals>().plugChars.Remove(char1);
                            Destroy(GameObject.Find("Plug"+int.Parse(keyControl.name)));
                        }
                        else if (!GameObject.Find("Plug"+keyControl.name)) // if it was not a double click
                        {
                            GetComponent<EnigmaInternals>().selectingPlugs = true;
                            GameObject parentObject = new GameObject("Plug"+keyControl.name);
                            parentObject.AddComponent<PlugSpawner>();
                            parentObject.AddComponent<LineRenderer>();
                            parentObject.AddComponent<RopeVisualizer>();
                            parentObject.AddComponent<AudioSource>();
                            parentObject.GetComponent<PlugSpawner>().plugSound = plugSound;
                            parentObject.GetComponent<PlugSpawner>().electricSound = electricSound;
                            parentObject.AddComponent<AudioSource>().volume = 0.5f;
                        }
                        lastClick = Time.time;
                    }
                    
                }
            }
        }
        if (keyPressed && Keyboard.current.anyKey.wasReleasedThisFrame && (keyIsPressed == 1 || keyIsPressed == 2 || (Mouse.current.scroll.ReadValue().y == 0 && activePressedKey != null))) // if a key was released
        {
            if(_keyObjects.TryGetValue(buttonPressedName, out Transform keyTransform))
            {
                animateKeyRelease(keyTransform);
                keyIsPressed = 0;
                activePressedKey = null; 
            }
        }
    }
    
    void animateKeyPress(Transform keyTransform,string lightPosName) // animates the key, plays the sound, and displays the light
    {
        Vector3 initialPos = keyTransform.localPosition;
        keyPressed = true;
        Vector3 pressedPos = initialPos + new Vector3(0,-0.05f,0);
        _keyObjects.TryGetValue("Key_"+lightPosName, out Transform lightTransform);
        lightObject.localPosition = new Vector3(lightTransform.localPosition.x + 0.6845964f,1.465f,lightTransform.localPosition.z);

        keyTransform.localPosition = pressedPos;
        int num = Random.Range(0,3);
        if (num == 0)
        {
            audioSource.clip = type1Sound;
            audioSource.Play();
        } else if (num == 1)
        {
            audioSourceTwo.clip = type2Sound;
            audioSourceTwo.Play();
        } else
        {
            audioSourceThree.clip = type3Sound;
            audioSourceThree.Play();
        }
    }
    void animateKeyRelease(Transform keyTransform) // resets everything the animateKeyPress did
    {
        Vector3 initialPos = keyTransform.localPosition;
        keyPressed = false;
        Vector3 releasedPos = initialPos + new Vector3(0,0.05f,0);

        lightObject.localPosition = new Vector3(0,0,0);

        keyTransform.localPosition = releasedPos;
    }
    void addVector3(GameObject plug) // adds a plug 
    {
        audioSource.clip = unplugSound;
        audioSource.Play();
        int firstArrayPos = plug.GetComponent<PlugSpawner>().firstPlugScroll;
        int secondArrayPos = plug.GetComponent<PlugSpawner>().secondPlugScroll;
        int[] plugIndex = GetComponent<EnigmaInternals>().plugIndex;
        Vector3 firstVector = plug.transform.GetChild(0).position;
        Vector3 secondVector = plug.transform.GetChild(1).position;
        int firstInt = plug.GetComponent<PlugSpawner>().firstInt;
        int secondInt = plug.GetComponent<PlugSpawner>().secondInt;
        List<Vector3> list = plugPositions.ToList();
        List<int> list2 = plugIndex.ToList();
        for (int i = 0; i < plugIndex.Length; i++)
        {
            if (firstInt < plugIndex[i])
            {
                list.Insert(i, firstVector);
                list2.Insert(i, firstInt);
                break;
            }
        }
        for (int i = 0; i < plugIndex.Length; i++)
        {
            if (secondInt < plugIndex[i])
            {
                list.Insert(i + 1, secondVector);
                list2.Insert(i + 1, secondInt);
                break;
            }
        }
        plugPositions = list.ToArray();
        GetComponent<EnigmaInternals>().plugIndex = list2.ToArray();
        GetComponent<EnigmaInternals>().plugChars.Remove(plug.GetComponent<PlugSpawner>().char1);
        GetComponent<EnigmaInternals>().plugCharsReverse.Remove(plug.GetComponent<PlugSpawner>().char2);
        for (int i = 0; i < GetComponent<EnigmaInternals>().plugIndex.Length; i++)
        {
            Debug.Log(GetComponent<EnigmaInternals>().plugIndex[i]);
        }
    }
    void swapCam() //swaps the camera position
    {
        audioSource.clip = cameraSound;
        audioSource.Play();
        if (cam1.Priority == 2)
        {
            cam3.Priority = 0;
            cam2.Priority = 2;
            cam1.Priority = 1;
        } else if (cam2.Priority == 2)
        {
            cam3.Priority = 2;
            cam2.Priority = 1;
            cam1.Priority = 0;
        } else
        {
            cam3.Priority = 1;
            cam2.Priority = 0;
            cam1.Priority = 2;
        }
    }
}
