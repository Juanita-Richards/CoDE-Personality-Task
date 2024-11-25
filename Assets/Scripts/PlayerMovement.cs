using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardspeed = 2f;
    public float turnspeed;
    public float forwardInput, RotationInput;
    public GameObject cam;
    private Vector3 offset = new Vector3(0, 1.5f, 0);
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game started");
    }

    // Update is called once per frame
    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        RotationInput = Input.GetAxis("Horizontal");
        //move the player forward and backward: up and down keys
        transform.Translate(Vector3.forward * Time.deltaTime * forwardspeed * forwardInput);


        //rotate the player to look around: left and right keys

        transform.Rotate(Vector3.up, turnspeed * RotationInput * Time.deltaTime); //Vector3.up is for the y-axis

        //camera follow the player
        cam.transform.position = transform.position + offset;
        cam.transform.rotation = transform.rotation;
    }
}
