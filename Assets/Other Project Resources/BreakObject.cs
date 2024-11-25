using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BreakObject : MonoBehaviour
{
    [SerializeField] private GameObject nonBroken;
    [SerializeField] private GameObject broken;
    [HideInInspector] public Hand[] handScripts;
    private GameObject child;
    private Rigidbody rb;
    private float[] arrayOfSpeeds;
    private int arrSize = 80;
    private float currSpeed;
    private float delta = 5f;
    private bool wait = true;
    private int tick = 0;
    private float largestSpeed;

    private void Awake() //needs to be Awake for child script to work
    {
        arrayOfSpeeds = new float[arrSize];
        rb = nonBroken.GetComponent<Rigidbody>();
        currSpeed = 0f;
        child = gameObject.transform.GetChild(0).gameObject; //get nonbroken item child
        child.AddComponent<ChildCollisionHandler>(); //add ChildCollisionHandler to child plate
    }

    private void Start()
    {
        handScripts = FindObjectsOfType<Hand>();
    }

    private void Update()
    {
        //wait for items to settle
        if (wait)
        {
            tick++; //it waits 60 frames before calculating speeds
            if (tick == 60)
            {
                wait = false;
                tick = 0;//reset for the next plate
            }
        }
        else
            StartCoroutine(GetSequenceOfSpeeds()); //wait doesn't become true again, so run this
    }

    IEnumerator GetSequenceOfSpeeds()
    {
        if (tick >= arrSize) //we are ticking because it tracks the frame and index
            tick = 0;

        currSpeed = Vector3.Magnitude(rb.velocity); //speed at that frame

        arrayOfSpeeds[tick] = currSpeed; //it stores most recent speeds, and the amound of speed is array size
        //tick++ it loops again
        //e.g if array size is 20, then it resets again

        tick++;

        yield return null;
    }
    //called in another script
    public void OnCollide()
    {
        
        largestSpeed = GetLargestSpeed(arrayOfSpeeds);
        if (CanBreak(largestSpeed, delta))
        {
            Break(); 
        }
    }

    private void Break() //to enable the break 
    {
        Debug.Log(largestSpeed);

        foreach (Hand hand in handScripts)
        {
            hand.ForceDetach(nonBroken); //makes sure to force detacment when the sound pop comes on
        }

        nonBroken.SetActive(false);
        broken.transform.position = nonBroken.transform.position; //puts the broken plate at the place of parent
        broken.SetActive(true);
    }
    //checks if the speed is greater than the change(threshold)
    private static bool CanBreak(float speed, float delta)
    {
        if (speed > delta)
            return true;
        return false;
    }
    //gets thhe largest speed in the array
    private static float GetLargestSpeed(float[] arr)
    {
        float largest = arr[0];
        int length = arr.Length;

        for (int i = 1; i < length; i++)
        {
            if (arr[i] > largest)
                largest = arr[i];
        }

        return largest;
    }
}
