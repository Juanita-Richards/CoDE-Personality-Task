using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLerpAnim : MonoBehaviour
{
    private GameObject door;
    private float time = 0f;
    private readonly float duration = 4f;
    private readonly float delta = 5f;
    private Quaternion startRot;
    [HideInInspector] public Quaternion endRot;
    private float yAngle;
    private bool flip = true;
    private bool lerp = false;
    private bool init = false;
    private MeshRenderer[] childArray;

    private void Start()
    {
        //get start and end rotations for lerping 
        startRot = gameObject.transform.rotation;
        if (gameObject.transform.rotation.eulerAngles.y < 100f && gameObject.transform.rotation.eulerAngles.y > 80f)
        {
            yAngle = 0f;
        }
        else
        {
            yAngle = 90f;
        }
        endRot = Quaternion.Euler(transform.rotation.x, yAngle, transform.rotation.z);

        EnableOutline();

        //reference to physical door in scene with SAME NAME
        door = GameObject.Find(gameObject.name);

        //list of all children mesh components in tree
        childArray = GetComponentsInChildren<MeshRenderer>(true);

        //initial mesh state
        ToggleMeshes(false);
    }

    private void Update()
    {
        if (lerp)
        {
            LerpAnim(); //play animation
        }
        if (InRange(door.transform.rotation, endRot, delta))
        {
            ToggleMeshes(false);
            lerp = false;
        }
        else if (init)
        {
            ToggleMeshes(true);
            lerp = true;
        }
    }

    public void LerpAnim()
    {
        float t = time / duration;
        t = t * t * (3f - 2f * t);

        if (flip)
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            if (transform.rotation == endRot)
            {
                flip = false;
                time = 0;
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(endRot, startRot, t);
            if (transform.rotation == startRot)
            {
                flip = true;
                time = 0;
            }
        }

        time += Time.deltaTime;
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

    private bool InRange(Quaternion a, Quaternion b, float delta)
    {
        if (Mathf.Abs(a.eulerAngles.y - b.eulerAngles.y) <= delta)
            return true;
        return false;
    }

    public void ToggleMeshes(bool flag)
    {
        foreach (MeshRenderer mesh in childArray)
            mesh.enabled = flag;
    }

    public void StartLerpAnim()
    {
        lerp = true;
        init = true;
    }

    public bool GetLerpVal()
    {
        return lerp;
    }
}
