using System.Collections;
using UnityEngine;
using RootMotion.Demos;
using System.Linq;

public class MirrorRoth : MonoBehaviour
{
    private GameObject playerAvatar;
    private GameObject mirrorCover;
    private AudioSource audioSource;
    private AudioClip audioClip;
    private bool currentlyGivingInstructions = true;

    public int currentVoiceLine = 31;
    public bool calibrated = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        mirrorCover = GameObject.Find("MirrorCover");
        StartCoroutine(WaitAtStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !currentlyGivingInstructions && currentVoiceLine != -5)
        {
            if (!calibrated)
            {
                calibrated = true;
                playerAvatar.GetComponent<VRIKAvatarScaleCalibrationSteamVR>().enabled = false;
                playerAvatar.GetComponent<Animator>().enabled = true;
                GameObject LogData = GameObject.Find("LogData");
                LogData.GetComponent<LogMotion>().StartLogging();
                LogData.GetComponent<LogJoints>().StartLogging();
                mirrorCover.GetComponent<TransitionFade>().canFade = true;
            }
            currentVoiceLine--;
            StartCoroutine(LoadAndPlay("Voicelines/Roth Embodiment-00"+currentVoiceLine));
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !currentlyGivingInstructions && currentVoiceLine != -5)
        {
            if (!calibrated)
                StartCoroutine(LoadAndPlay("Voicelines/Mirror-1"));
            else
                StartCoroutine(Replay());
        }
    }

    public void CubeNotHeld()
    {
        // in cube pickup stages
        if (currentVoiceLine <= -1 && currentVoiceLine > -4)
        {
            currentVoiceLine = -1;
            StartCoroutine(LoadAndPlay("Voicelines/Roth Embodiment-00" + currentVoiceLine));
        }
    }

    // waits at the start, then asks the user to stand straight to calibrate the avatar 
    IEnumerator WaitAtStart()
    {
        currentlyGivingInstructions = true;

        // wait a bit for the avatar prefab to spawn, then grab it
        yield return new WaitForSeconds(1.0f);
        playerAvatar = GameObject.FindGameObjectsWithTag("PlayerAvatar")[0];

        // ask  player to calibrate the avatar
        audioClip = (AudioClip)Resources.Load("Voicelines/Mirror-1");
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
        currentlyGivingInstructions = false;

        // enable player to calibrate their avatar
        playerAvatar.GetComponent<VRIKAvatarScaleCalibrationSteamVR>().enabled = true;
    }

    // loads and plays a given audio clip without interruption
    IEnumerator LoadAndPlay(string audioString)
    {
        currentlyGivingInstructions = true;
        audioClip = (AudioClip)Resources.Load(audioString);
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
        currentlyGivingInstructions = false;
    }

    IEnumerator Replay()
    {
        int oldCurrentVoiceLine = currentVoiceLine;
        currentVoiceLine = 31;
        yield return new WaitForSeconds(0.02f);
        currentVoiceLine = oldCurrentVoiceLine;
        StartCoroutine(LoadAndPlay("Voicelines/Roth Embodiment-00" + currentVoiceLine));
    }
}
