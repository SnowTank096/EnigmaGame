using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlugSpawner : MonoBehaviour
{
    public GameObject segmentPrefab; // THis is the rope segment 
    public Rigidbody startAnchor; //This is the anchor so it would most likely be the end of the plug that goes into the socket
    public Transform endTarget; // This will be the other side of the plug that goes into the second socket
    public GameObject Plug;
    private GameObject selectedPlug;
    private Vector3[] plugPositions;
    public Material BlackPlainMaterial;
    private Vector3 plugStartVector;
    private Vector3 spawnPos;
    private GameObject newSegment;
    public keyboard_interactions keyInterScript;
    public int firstPlugScroll;
    public int secondPlugScroll;

    public int segmentCount = 6; //How many segments you want
    private int plugScroll = 0;
    public float segmentLength = 0.4f; // how long you want the segments

    private Transform[] spawnedTransforms; //array that store the trnasform of each rope segment
    private RopeVisualizer ropeVisualizer; //rope visualizer script

    void Start()
    {
        keyInterScript = GameObject.Find("enigma machine v1").GetComponent<keyboard_interactions>();
        plugPositions = keyInterScript.plugPositions;
        segmentPrefab = GameObject.Find("RopeSegment");
        ropeVisualizer = GetComponent<RopeVisualizer>();
        Plug = GameObject.Find("Plug");
        BlackPlainMaterial = Resources.Load<Material>("BlackPlainMaterial");
        SetAnchors();
    }
    void SetAnchors()
    {
        plugStartVector = new Vector3(-21.24183f,1.4771f,4.78512f);
        selectedPlug = Instantiate(Plug, plugStartVector, Quaternion.Euler(0,0,90));
        selectedPlug.transform.parent = transform;
    }
    void SpawnRope()
    {
        Rigidbody previousRigidBody = startAnchor; //sets the first rigid body that the first rope segemnt will connect to to the starting anchor point

        Vector3 spawnDirection = (endTarget.position - startAnchor.position).normalized; // gets the direction the rope will point towards
        for (int i = 1; i < 6; i++)
        {
            if (i != 5) 
            {
                spawnPos = startAnchor.position; // spawns the joint at the distance from the anchor
           
                newSegment = Instantiate(segmentPrefab, spawnPos, Quaternion.LookRotation(spawnDirection)); // creates the object witht the settings

                newSegment.transform.parent = transform;

                spawnedTransforms[i] = newSegment.transform; //sets the transform of the crated object as i# in the array for the visualizer array 
            }
            else
            {
                newSegment = selectedPlug;
                newSegment.AddComponent<ConfigurableJoint>();
            }
            ConfigurableJoint joint = newSegment.GetComponent<ConfigurableJoint>(); // connects the joint configuration
            if (joint != null) // checks if the joint is created
            {
                joint.connectedBody = previousRigidBody; // sets the joint connection to the previous joint or anchor

                joint.autoConfigureConnectedAnchor = false; 
                joint.anchor = new Vector3(0, 0, 0); //sets a hinge on itself
                joint.connectedAnchor = new Vector3(0, 0, 0); //sets a hinge on the connected componet or previous one
                joint.xMotion = ConfigurableJointMotion.Limited;
                joint.yMotion = ConfigurableJointMotion.Limited;
                joint.zMotion = ConfigurableJointMotion.Limited;

                SoftJointLimit limitSettings = joint.linearLimit; //makes a copy of the settings
                limitSettings.limit = segmentLength; // changes the limit to the segments length in the copy
                joint.linearLimit = limitSettings; //makes the original settings change to the copy's
            }

            previousRigidBody = newSegment.GetComponent<Rigidbody>();
        }
        if (ropeVisualizer != null)
        {
            ropeVisualizer.ropeSegments = spawnedTransforms; //puts the rope segments tranforms into the rope visulaizer array
            ropeVisualizer.enabled = true;
        }
    }
    void Update()
    {
        Vector2 mouseDelta = Mouse.current.scroll.ReadValue();
        if (mouseDelta.y > 0)
        {
            plugScroll++;
            Debug.Log(plugScroll);
        }
        else if (mouseDelta.y < 0)
        {
            plugScroll--;
            Debug.Log(plugScroll);
        }
        if (plugScroll < 0)
        {
            plugScroll = plugPositions.Length - 1;
        } 
        if (plugScroll > plugPositions.Length - 1)
        {
            plugScroll = 0;
        } 
        if (endTarget == null)
        {
            selectedPlug.transform.position = plugPositions[plugScroll];
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && endTarget == null)
        {
            if (startAnchor == null)
            {
                spawnedTransforms = new Transform[segmentCount]; // sets array length to the amount of segments there are
                spawnedTransforms[0] = selectedPlug.transform;
                Renderer selectedPlugRenderer = selectedPlug.GetComponent<Renderer>();
                selectedPlugRenderer.material = BlackPlainMaterial;
                startAnchor = selectedPlug.GetComponent<Rigidbody>();
                selectedPlug = Instantiate(Plug, plugStartVector, Quaternion.Euler(0,0,90));
                selectedPlug.transform.parent = transform;
                spawnedTransforms[5] = selectedPlug.transform;
                List<Vector3> list = plugPositions.ToList();
                firstPlugScroll = plugScroll;
                list.RemoveAt(firstPlugScroll);
                plugPositions = list.ToArray();
                keyInterScript.plugPositions = plugPositions;

            }
            else
            {
                Renderer selectedPlugRenderer = selectedPlug.GetComponent<Renderer>();
                selectedPlugRenderer.material = BlackPlainMaterial;
                endTarget = selectedPlug.transform;
                SpawnRope();
                List<Vector3> list = plugPositions.ToList();
                secondPlugScroll = plugScroll - 1;
                list.RemoveAt(secondPlugScroll + 1);
                plugPositions = list.ToArray();
                keyInterScript.plugPositions = plugPositions;
            }
        }
    }
}
