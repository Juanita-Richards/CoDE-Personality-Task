
using System.Linq;
using UnityEngine;
using Valve.VR;

public class IndexInput : MonoBehaviour
{
    public SteamVR_Action_Skeleton SkelectionAction = null;
    public SteamVR_Action_Single SqueezeAction = null;
    public bool LeftHand = false;
    public bool RightHand = false;
    public bool CurrentlyGrasping = false;
    public int[] handCode = { 0, 1, 1, 1, 1 };
    private int Code => handCode.Aggregate((result, x) => result* 10 + x);
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Skeleton();
        if (LeftHand)
        {
            animator.SetInteger("fingerPose_L", Code);
        }
        if (RightHand)
        {
            animator.SetInteger("fingerPose_R", Code);
        }
    }

    private void Skeleton()
    {
        if (!CurrentlyGrasping)
        {
            if (SqueezeAction.axis > 0.6)
                handCode[0] = 3;
            else
            {
                handCode[0] = SkelectionAction.thumbCurl > 0.3f ? 1 : 0;
                handCode[1] = SkelectionAction.indexCurl < 0.3f ? 1 : 0;
                handCode[2] = SkelectionAction.middleCurl < 0.3f ? 1 : 0;
                handCode[3] = SkelectionAction.ringCurl < 0.3f ? 1 : 0;
                handCode[4] = SkelectionAction.pinkyCurl < 0.3f ? 1 : 0;
            }
        }

    }

}
