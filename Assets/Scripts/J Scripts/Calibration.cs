using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Demos;

public class Calibration : MonoBehaviour
{

    public bool turnedOff;

    //List version
    public List<GameObject> Avatars;
   
    // Start is called before the first frame update
    void Start()
    {
        //blackMirror = GameObject.Find("MirrorCover");
        //Debug.Log(blackMirror.name);
        turnedOff = false;
        
    }

    // Update is called once per frame

    //Turn off script VR IK Calibration Script when spacebar is pressed. 
    //NB: Trigger is the beginning of the calibration step
    //It uses the Grab Pinch action -> which is the binding for the Trigger Button
    void Update()
    {

        foreach (GameObject avatar in Avatars)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (avatar.gameObject.activeInHierarchy)
                {
                    //Debug.Log(avatar.name);
                    avatar.GetComponentInChildren<VRIKAvatarScaleCalibrationSteamVR>().enabled = false;
                    avatar.GetComponentInChildren<Animator>().enabled = true;

                    turnedOff = true;
                }
            }
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    this.gameObject.GetComponent<VRIKAvatarScaleCalibrationSteamVR>().enabled = false;
        //    this.gameObject.GetComponent<Animator>().enabled = true;
        //}
        //Mirror();
    }

    //public void Mirror()
    //{
    //    foreach (GameObject avatar in Avatars)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            if (avatar.activeSelf)
    //            {
    //                //Debug.Log(avatar.name);
    //                avatar.GetComponent<VRIKAvatarScaleCalibrationSteamVR>().enabled = false;
    //                avatar.GetComponent<Animator>().enabled = true;
    //                blackMirror.GetComponentInChildren<TransitionFade>().canFade = true;
    //                turnedOff = true;
    //            }
    //        }
    //    }
    //}

    //public void MirrorTwo()
    //{
    //    //BACKUP - Just because 
    //    //This is using tags

    //    for (int i = 0; i < Avatars.Count; i++)
    //    {
    //        if (Avatars[i].gameObject.activeInHierarchy && Avatars[i].tag == "Player")
    //        {
    //            if (Input.GetKeyDown(KeyCode.Space) && turnedOff != true)
    //            {

    //                Debug.Log(Avatars[i].name);
    //                Avatars[i].GetComponentInChildren<VRIKAvatarScaleCalibrationSteamVR>().enabled = false;
    //                Avatars[i].GetComponentInChildren<Animator>().enabled = true;
    //                turnedOff = true;

    //            }
    //        }
    //    }

    //}
}
