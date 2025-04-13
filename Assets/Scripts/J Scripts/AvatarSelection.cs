using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSelection : MonoBehaviour
{
    public Calibration avatarActivate;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void HeadshotActivate()
    {
        
        //go through the list in Calibration
        foreach (GameObject avatar in avatarActivate.Avatars)
        {
            //if the name matches what I clicked on, then activate that avatar
            if (image.sprite.name == avatar.name)
            {
                //avatar.SetActive(true);
                Instantiate(avatar);
            }
        }
    }

    
}
