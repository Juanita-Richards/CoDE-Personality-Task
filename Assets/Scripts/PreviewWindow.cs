using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewWindow : MonoBehaviour
{
    public string avatarName = null;

    public void ChangeImage(string imageName)
    {
        Sprite preview = Resources.Load<Sprite>(imageName);
        gameObject.GetComponent<Image>().sprite = preview;
        this.avatarName = imageName;
    }
}
