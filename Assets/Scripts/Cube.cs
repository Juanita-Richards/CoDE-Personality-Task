using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Cube : MonoBehaviour
{
    public GameObject cube;
    //public GameObject floor;
    private GameObject playerAvatar;
    private IndexInput rightHand;
    private IndexInput leftHand;
    private MirrorRoth controller;
    private Vector3 startPosition;
    private bool destroying = false;
    // Start is called before the first frame update
    void Start()
    {
        //controller = GameObject.Find("NarratorController").GetComponent<MirrorRoth>();
        startPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // checks if the cube is too far from origin
        // origin is hardcoded lol because I have exactly 1 day before I move

        //Will need to undo if any errors
        //if (!destroying && (gameObject.transform.position.x < -0.7 || gameObject.transform.position.x > 0.7 || gameObject.transform.position.z < 0.1 || gameObject.transform.position.z > 1.2) && (!leftHand.CurrentlyGrasping && !rightHand.CurrentlyGrasping))
        //{
        //    StartCoroutine(WaitAndDestroy());
        //}
    }

    // change hand pose to grasp
    public void OnPickUpRight()
    {
        Assign();
        rightHand.CurrentlyGrasping = true;
        for (int i = 0; i < 5; i++)
            rightHand.handCode[i] = 4;
    }

    public void OnPickUpLeft()
    {
        Assign();
        leftHand.CurrentlyGrasping = true;
        for (int i = 0; i < 5; i++)
            leftHand.handCode[i] = 4;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!leftHand || !rightHand)
            return;
        if (collision.transform.name == "Ground" && !destroying && !leftHand.CurrentlyGrasping && !rightHand.CurrentlyGrasping)
        {
            if (controller)
                controller.CubeNotHeld();
        }
    }

    public void OnDetachRight()
    {
        rightHand.CurrentlyGrasping = false;
    }

    public void OnDetachLeft()
    {
        leftHand.CurrentlyGrasping = false;
    }

    private void Assign()
    {
        // assigns hands if not yet assigned. This has to be done after Start because the avatar isn't loaded until slightly after the scene starts based on player choices
        if (playerAvatar == null)
        {
            playerAvatar = GameObject.FindGameObjectsWithTag("PlayerAvatar")[0];
            IndexInput[] hands = playerAvatar.GetComponents<IndexInput>();
            foreach (IndexInput hand in hands)
            {
                if (hand.RightHand)
                    rightHand = hand;
                else if (hand.LeftHand)
                    leftHand = hand;
            }

        }
    }
    IEnumerator WaitAndDestroy()
    {
        destroying = true;
        yield return new WaitForSeconds(0.5f);
        controller.CubeNotHeld();
        Instantiate(cube, startPosition, Quaternion.identity);
        Destroy(this.gameObject);
        destroying = false;
    }
}
