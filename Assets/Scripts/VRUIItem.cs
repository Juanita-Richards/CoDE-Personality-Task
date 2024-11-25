using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class VRUIItem : MonoBehaviour
{
    private BoxCollider boxCollider;
    private RectTransform rectTransform;

    private void OnEnable()
    {
        ValidateCollider();
    }

    private void OnValidate()
    {
        ValidateCollider();
    }

    private void ValidateCollider()
    {
        rectTransform = GetComponent<RectTransform>();

        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }

        boxCollider.size = rectTransform.sizeDelta;
    }

    // called when confirm button is clicked
    public void OnClick()
    {
        if (gameObject.tag == "UI_Icon")
        {
            GameObject.Find("Preview").GetComponent<PreviewWindow>().ChangeImage(gameObject.transform.name);
            GameObject.Find("HeadshotPreview").GetComponent<PreviewWindow>().ChangeImage(gameObject.transform.name + "_headshot");
        }
        else if (gameObject.transform.name == "Confirm")
        {
            Selection selection = GameObject.Find("MenuSelection").GetComponent<Selection>();
            string avatarName = GameObject.Find("Preview").GetComponent<PreviewWindow>().avatarName;
            if (avatarName != "")
            {
                if (selection.selectionNum == 1)
                    selection.participant.ChosenAvatar = avatarName;
                if (selection.selectionNum == 2)
                    selection.participant.mAllAvatar = avatarName;
                if (selection.selectionNum == 3)
                    selection.participant.mEthnicityOnlyAvatar = avatarName;
                if (selection.selectionNum == 4)
                    selection.participant.mGenderOnlyAvatar = avatarName;
                if (selection.selectionNum == 5)
                    selection.participant.mNoneAvatar = avatarName;
                selection.UpdateUI();
            }
        }
    }



}