using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cues : MonoBehaviour
{
    private Vector3 startingPos;
    AudioSource audioSource;
    private float delta = 0.1f;
    public GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = gameObject.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!InRange(gameObject.transform.position, startingPos, delta))
        {
            arrow.SetActive(false);

        }
    }


    public bool InRange(Vector3 a, Vector3 b, float delta)
    {
        if (Mathf.Abs(a.x - b.x) <= delta && Mathf.Abs(a.y - b.y) <= delta && Mathf.Abs(a.z - b.z) <= delta)
            return true;
        return false;
    }
}
