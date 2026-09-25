
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;


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
    private int scrollCount;
    private PlugSpawner plugSpawner;
    private float lastClick;
    public Vector3[] plugPositions;

    [SerializeField] private Transform lightObject;

    [SerializeField] private Transform rotor1;
    
    [SerializeField] private LayerMask interactionLayer;

    [SerializeField] private float maxDistance = 100f;



    void Start()
    {
        _keyObjects = new System.Collections.Generic.Dictionary<string, Transform>();
        plugSpawner = GetComponent<PlugSpawner>();
        plugPositions = new Vector3[]
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
        lightGrid = new System.Collections.Generic.Dictionary<string, Vector3>();
        //for each looks through every object or child in the parent the script is attached to. The capital Transform is the data type the for each looks for and saves to child
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

        if (Physics.Raycast(ray, out hit, maxDistance, interactionLayer) || keyIsPressed == 1)
        {
            Vector2 scrollDelta = Mouse.current.scroll.ReadValue();
            TMPro.TextMeshPro rotorTextObject = null;
            if (keyIsPressed == 1)
            {
                rotorTextObject = rotorText3.GetComponent<TMPro.TextMeshPro>();
                scrollDelta.y = 1;
            }
            else
            {
                string rotorTextName = hit.collider.gameObject.name + "Text";

                TMPro.TextMeshPro textObject = GameObject.Find(rotorTextName).GetComponent<TMPro.TextMeshPro>();

                rotorTextObject = textObject.GetComponent<TMPro.TextMeshPro>();
            }
            if (scrollDelta.y > 0 )
            {
                if (scrollCount == 0)
                {
                    if (rotorTextObject.text == rotorText3.text)
                    {

                        if (rotorText2.text == "26")
                        {
                            int textNum = int.Parse(rotorText1.text);
                            rotorText1.text = (textNum += 1).ToString();
                            rotorText2.text = "0";
                        }
                        else
                        {
                            int textNum = int.Parse(rotorText2.text);
                            rotorText2.text = (textNum += 1).ToString();
                        }
                    }
                    else if (rotorTextObject.text == rotorText2.text)
                    {
                        int textNum = int.Parse(rotorText1.text);
                        rotorText1.text = (textNum += 1).ToString();
                    }
                }
                scrollCount++;
                rotorTextObject.text = scrollCount.ToString();
                if (scrollCount > 25)
                {
                    scrollCount = 0;
                }
                keyIsPressed = 2;
            } 
            else if (scrollDelta.y < 0)
            {
                rotorTextObject.text = scrollCount.ToString();
                scrollCount--;
                if (scrollCount < 1)
                {
                    scrollCount = 26;
                }
            }
        }
        if(_keyObjects == null) return;
        if(Keyboard.current == null) return;
        if (Keyboard.current.anyKey.wasPressedThisFrame && keyIsPressed == 0)
        {
            foreach (var keyControl in Keyboard.current.allKeys)
            {
                if (activePressedKey == null && keyControl.wasPressedThisFrame)
                {
                    
                    string buttonPressed = keyControl.name.ToUpper();
                    buttonPressedName = "Key_" + buttonPressed;

                    if(_keyObjects.TryGetValue(buttonPressedName, out Transform keyTransform))
                    {
                        if (keyTransform != null)
                        {
                            animateKeyPress(keyTransform,GetComponent<EnigmaInternals>().Convert(char.Parse(buttonPressed), int.Parse(rotorText1.text), int.Parse(rotorText2.text), int.Parse(rotorText3.text)));
                            if (keyIsPressed == 0)
                            {
                                keyIsPressed = 1;
                            }
                            activePressedKey = keyTransform;
                        }
                    }
                    else if (int.Parse(keyControl.name) < 10)
                    {
                        if (Time.time - lastClick <= 0.25f)
                        {
                            addVector3(GameObject.Find("Plug"+int.Parse(keyControl.name)));
                            char char1 = GameObject.Find("Plug"+int.Parse(keyControl.name)).GetComponent<PlugSpawner>().char1;
                            GameObject.Find("enigma machine v1").GetComponent<EnigmaInternals>().plugChars.Remove(char1); // get the component and get char1 from plugs and use it to remove the char1 and char2 from the dictionar in engima internals with this GameObject.Find("Plug"+int.Parse(keyControl.name)).GetComponent<EnigmaInternals>().plugChars.Remove(char1);
                            Destroy(GameObject.Find("Plug"+int.Parse(keyControl.name)));
                        }
                        else if (!GameObject.Find("Plug"+keyControl.name))
                        {
                            GameObject parentObject = new GameObject("Plug"+keyControl.name);
                            parentObject.AddComponent<PlugSpawner>();
                            parentObject.AddComponent<LineRenderer>();
                            parentObject.AddComponent<RopeVisualizer>();
                        }
                        lastClick = Time.time;
                    }
                    
                }
            }
        }
        if (Keyboard.current.anyKey.wasReleasedThisFrame && (keyIsPressed == 1 || keyIsPressed == 2))
        {
            if(_keyObjects.TryGetValue(buttonPressedName, out Transform keyTransform))
            {
                animateKeyRelease(keyTransform);
                keyIsPressed = 0;
                activePressedKey = null; 
            }
        }
    }
    
    void animateKeyPress(Transform keyTransform,string lightPosName)
    {
        Vector3 initialPos = keyTransform.localPosition;

        Vector3 pressedPos = initialPos + new Vector3(0,-0.05f,0);
        _keyObjects.TryGetValue("Key_"+lightPosName, out Transform lightTransform);
        lightObject.localPosition = new Vector3(lightTransform.localPosition.x + 0.6845964f,1.523f,lightTransform.localPosition.z);

        keyTransform.localPosition = pressedPos;
    }
    void animateKeyRelease(Transform keyTransform)
    {
        Vector3 initialPos = keyTransform.localPosition;

        Vector3 releasedPos = initialPos + new Vector3(0,0.05f,0);

        lightObject.localPosition = new Vector3(0,0,0);

        keyTransform.localPosition = releasedPos;
    }
    void addVector3(GameObject plug)
    {
        int firstArrayPos = plug.GetComponent<PlugSpawner>().firstPlugScroll;
        int secondArrayPos = plug.GetComponent<PlugSpawner>().secondPlugScroll + 1;
        Debug.Log(firstArrayPos);
        Debug.Log(secondArrayPos);
        Vector3 firstVector = plug.transform.GetChild(0).position;
        Vector3 secondVector = plug.transform.GetChild(1).position;
        List<Vector3> list = plugPositions.ToList();
        list.Insert(firstArrayPos, firstVector);
        list.Insert(secondArrayPos + 1, secondVector);
        plugPositions = list.ToArray();
    }
}