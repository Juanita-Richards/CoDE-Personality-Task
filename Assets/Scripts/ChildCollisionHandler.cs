using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollisionHandler : MonoBehaviour
{
    private BreakObject parentScript;
    //private bool debounce = true;

    private void Start()
    {
        parentScript = transform.parent.GetComponent<BreakObject>();
    }
    //parent script doesn't have the rigidbody, so call the child scripts
    private void OnCollisionEnter(Collision collision)
    {
        parentScript.OnCollide();
    }
}
