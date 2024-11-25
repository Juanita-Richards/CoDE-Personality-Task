
using UnityEngine;
using RootMotion.FinalIK;
using Valve.VR;

namespace RootMotion.Demos
{
    // Simple avatar scale calibration.
    public class VRIKAvatarScaleCalibrationSteamVR : MonoBehaviour
    {
        public VRIK ik;
        public float scaleMlp = 1f;
        public SteamVR_Action_Boolean calibrationAction = SteamVR_Input.GetBooleanAction("InteractUI");
        public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;


        private bool calibrateFlag;
        private AudioClip audioClip;
        private AudioSource audioSource;
        private Animator animator; 

        private void Start()
        {
            ik = gameObject.GetComponent<VRIK>();
            audioSource = gameObject.AddComponent<AudioSource>();
            animator = gameObject.GetComponent<Animator>();
            animator.enabled = false;
        }
        private void Update()
        {
            if (calibrationAction.GetStateUp(inputSource) && calibrateFlag == false)
            {
                calibrateFlag = true;
            }
        }

        private void LateUpdate()
        {
            // Making sure calibration is done in LateUpdate
            if (!calibrateFlag) return;
            calibrateFlag = false;

            // Compare the height of the head target to the height of the head bone, multiply scale by that value.
            float sizeF = (ik.solver.spine.headTarget.position.y - ik.references.root.position.y) / (ik.references.head.position.y - ik.references.root.position.y);
            ik.references.root.localScale *= sizeF * scaleMlp;
            audioClip = (AudioClip)Resources.Load("Sounds/exercise-loaded");
            audioSource.PlayOneShot(audioClip);
        }
    }
}
