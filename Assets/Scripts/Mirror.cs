using System.Collections;
using UnityEngine;
using RootMotion.Demos;
using UnityEngine.UI;
using Random = System.Random;
using System.Linq;

public class Mirror : MonoBehaviour
{
    private GameObject playerAvatar;
    private GameObject mirrorCover;
    private AudioSource audioSource;
    private AudioClip audioClip;
    private bool currentlyGivingInstructions = true;
    private int[] poseOrder = { 1, 2, 3, 4 };

    public Image poseSprite;
    public int stage = 0;
    public int currentExercise = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        mirrorCover = GameObject.Find("MirrorCover");
        StartCoroutine(WaitAtStart());

        // get random order of exercises
        Random random = new();
        poseOrder = poseOrder.OrderBy(x => random.Next()).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !currentlyGivingInstructions)
        {
            if (currentExercise == 0)
                stage++;
            else if (currentExercise < 4)
                GiveExercise(++currentExercise);
            else if (currentExercise == 4)
            {
                audioClip = (AudioClip)Resources.Load("Voicelines/Mirror-End");
                audioSource.PlayOneShot(audioClip);
                poseSprite.GetComponent<Image>().enabled = false;
                currentExercise++;
            }

        }

        switch (stage)
        {
            case 1:
                playerAvatar.GetComponent<VRIKAvatarScaleCalibrationSteamVR>().enabled = false;
                playerAvatar.GetComponent<Animator>().enabled = true;
                GameObject LogData = GameObject.Find("LogData");
                LogData.GetComponent<LogMotion>().StartLogging();
                LogData.GetComponent<LogJoints>().StartLogging();
                mirrorCover.GetComponent<TransitionFade>().canFade = true;
                audioClip = (AudioClip)Resources.Load("Voicelines/Mirror-2");
                audioSource.PlayOneShot(audioClip);
                stage++;
                break;
            case 3:
                stage++;
                StartCoroutine(Instructions());
                break;
        }
    }

    IEnumerator WaitAtStart()
    {
        // asks the user to stand straight to calibrate the avatar 
        currentlyGivingInstructions = true;
        yield return new WaitForSeconds(1.0f);
        playerAvatar = GameObject.FindGameObjectsWithTag("PlayerAvatar")[0];
        audioClip = (AudioClip)Resources.Load("Voicelines/Mirror-1");
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
        currentlyGivingInstructions = false;
        playerAvatar.GetComponent<VRIKAvatarScaleCalibrationSteamVR>().enabled = true;
    }

    IEnumerator Instructions()
    {
        currentlyGivingInstructions = true;
        audioClip = (AudioClip)Resources.Load("Voicelines/Mirror-3");
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
        currentlyGivingInstructions = false;
        GiveExercise(++currentExercise);
    }


    private void GiveExercise(int num)
    {
        audioClip = (AudioClip)Resources.Load("Sounds/exercise-loaded");
        audioSource.PlayOneShot(audioClip);
        poseSprite.GetComponent<Image>().enabled = true;
        poseSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>("Poses/pose" + poseOrder[num-1]);
    }
}
