using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    public Transform mirror;
    public GameObject NarratorController;
    private MirrorRoth control;
    private Transform playerTarget;

    // Start is called before the first frame update
    void Start()
    {
        control = NarratorController.GetComponent<MirrorRoth>();
        StartCoroutine(WaitForPlayerToSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //transform.position = mirror.TransformPoint(new Vector3(localPlayer.x, 0.0f, -localPlayer.z/3));

        Vector3 lookAtMirror = mirror.TransformPoint(new Vector3(-localPlayer.x, localPlayer.y, localPlayer.z));
        transform.LookAt(lookAtMirror);
        */
        if (control.calibrated)
        {
            // have mirror camera based on player rotations
            Vector3 localPlayer = mirror.InverseTransformPoint(playerTarget.position);
            transform.position = mirror.TransformPoint(new Vector3(localPlayer.x, -localPlayer.y/2, localPlayer.z));
            Vector3 position = mirror.TransformPoint(new Vector3(-localPlayer.x, localPlayer.y, localPlayer.z));
            float distanceToPlane = Vector3.Dot(transform.up, position - transform.position);
            Vector3 pointOnPlane = position - (transform.up * distanceToPlane);
            transform.LookAt(pointOnPlane, transform.up);

        }
    }

    IEnumerator WaitForPlayerToSpawn()
    {
        // wait a bit for the avatar prefab to spawn, then grab it
        yield return new WaitForSeconds(1.0f);
        GameObject playerAvatar = GameObject.FindGameObjectsWithTag("Player")[0];
        playerTarget = playerAvatar.transform.Find("Head").transform;
    }
}
