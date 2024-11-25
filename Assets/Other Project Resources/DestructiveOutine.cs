using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiveOutine : MonoBehaviour
{
    private Vector3 startingPos;
    AudioSource audioSource;
    private float delta = 0.1f;
    Outline[] childArray;


    private void Start()
    {
        startingPos = gameObject.transform.position;
        EnableOutline();
        InitAudio();
       
    }

    private void Update()
    {
        if (!InRange(gameObject.transform.position, startingPos, delta)) 
        {
            DisableOutline();
            audioSource.Play();
            Destroy(this);


        }
    }


    private void EnableOutline()
    {
        childArray = GetComponentsInChildren<Outline>();

        foreach (Outline outline in childArray)
        {
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = new Color32(240, 0, 0, 255);
            outline.OutlineWidth = 5f;
        }
    }

    private void InitAudio()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load("Sounds/Pop") as AudioClip;
        audioSource.pitch = 0.9f;
        audioSource.spatialBlend = 1f;
    }

    private void DisableOutline()
    {
        foreach (Outline outline in childArray)
        {
            Destroy(outline);
        }
      

    }
    //this checks if it is a certain range of delta
    public bool InRange(Vector3 a, Vector3 b, float delta)
    {
        if (Mathf.Abs(a.x - b.x) <= delta && Mathf.Abs(a.y - b.y) <= delta && Mathf.Abs(a.z - b.z) <= delta)
            return true;
        return false;
    }
}
