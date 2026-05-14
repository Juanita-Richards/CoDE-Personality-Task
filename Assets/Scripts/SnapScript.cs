using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SnapScript : MonoBehaviour
{
    [HideInInspector] public Hand[] handScripts;
    AudioSource audioSource;
    Collider boxCollider;
    private Vector3 initialPos;
    private GameObject targetObject;
    
    private void Start() 
    {        
        //init
        boxCollider = GetComponent<Collider>();
        boxCollider.isTrigger = true;

        EnableOutline();

        //handScripts = FindObjectsOfType<Hand>();
        handScripts = FindObjectsByType<Hand>(FindObjectsSortMode.None);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (gameObject.name == other.gameObject.name && other.gameObject.tag == "Snap")
        {
            other.gameObject.tag = "Untagged"; //check for tag

            InitAudio(other);

            //play sound
            audioSource.Play();

            initialPos = gameObject.transform.position; //get initial pos

            //detach from hand
          
            foreach (Hand hand in handScripts)
            {
                //Disabled because there is some sort of error
                //hand.ForceDetach(other.gameObject); //makes sure to force detacment when the sound pop comes on
                hand.DetachObject(other.gameObject);
              
            }

            //zero velocity
            Rigidbody body = other.gameObject.GetComponent<Rigidbody>();
            body.velocity = Vector3.zero; 
            body.angularVelocity = Vector3.zero;

            //set interactable pos & rot to where ghost is
            other.gameObject.transform.position = gameObject.transform.position;
            other.gameObject.transform.rotation = gameObject.transform.rotation;

            //stop physics
            body.constraints = RigidbodyConstraints.FreezeAll;
            body.isKinematic = true;

            //ignore hovering
            other.gameObject.AddComponent<IgnoreHovering>();

            Destroy(gameObject); //destroy the ghost object

        }
    }
    private void EnableOutline()
    {
        Outline[] childArray = GetComponentsInChildren<Outline>();
      

        foreach (Outline outline in childArray)
        {
            outline.enabled = true;
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = new Color32(152, 255, 109, 255);
            outline.OutlineWidth = 5f;
        }
    }

    private void InitAudio(Collider other)
    {
        audioSource = other.gameObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load("Sounds/Pop") as AudioClip;
        audioSource.pitch = 0.9f;
        audioSource.spatialBlend = 1f;
    }

    
}
