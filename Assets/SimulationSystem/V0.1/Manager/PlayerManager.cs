using System;
using SimulationSystem.V0._1.Modules.Detect;
using SimulationSystem.V0._1.Modules.Detect.ToBeRefactored__Derive_from_Detect_Abstract_;
using SimulationSystem.V0._1.VR_Player;
using UnityEngine;
using UnityEngine.Events;


namespace SimulationSystem.V0._1.Manager
{
    public class PlayerManager : MonoBehaviour
    {
         [field: SerializeField] public UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor RightGrabInteractor { get; private set; }
        [field: SerializeField] public UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor LeftGrabInteractor { get; private set; }

        private static PlayerManager instance;
       public static PlayerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (PlayerManager)FindObjectOfType(typeof(PlayerManager));
                    if (instance == null)
                        instance = (new GameObject("PlayerManager")).AddComponent<PlayerManager>();
                }
                return instance;
            }
        }

       public Transform mainCamera;
       public Transform headAnchor;
       public Transform handAnchor;
        public Transform leftHand;
        public Transform rightHand;
       

        [field: SerializeField] public HandStatus RightHandStatus { get; private set; }
        [field: SerializeField] public HandStatus LeftHandStatus { get; private set; }
    
        [field: SerializeField] public PlayerController controller { get; private set; }

        [field: SerializeField] public GameObject leftHandHoverCollider { get; private set; }
        [field: SerializeField] public GameObject rightHandHoverCollider { get; private set; }

    
        [Space(10)]
        [SerializeField] private UnityEvent onLeftDominantHandSet;
        [SerializeField] private UnityEvent onRightDominantHandSet;

        [SerializeField] private int health = 2; //step property
        public DominantHand DominantHand { get; set; } = DominantHand.RightHand;

        private void Awake()
        {
/*            if (Instance == null)
                Instance = this;
            else
                Destroy(this);*/
            #region HardCode
            // RightGrabInteractor = GameObject.Find("Hands/RightHand/HandInteractorsRight/HandGrabInteractor")
            //     .GetComponent<HandGrabInteractor>();
            // LeftGrabInteractor = GameObject.Find("Hands/LeftHand/HandInteractorsLeft/HandGrabInteractor")
            //     .GetComponent<HandGrabInteractor>();
            #endregion
        }
        //Oculus event does not support parameterised function
        public void SetDominantRightHand()
        {
            DominantHand = DominantHand.RightHand;
            RightHandStatus.handType = HandType.DominantHand;
            LeftHandStatus.handType = HandType.NonDominantHand;
            onLeftDominantHandSet?.Invoke();
        }
        public void SetDominantLeftHand()
        {
            DominantHand = DominantHand.LeftHand;
            LeftHandStatus.handType = HandType.DominantHand;
            RightHandStatus.handType = HandType.NonDominantHand;
            onRightDominantHandSet?.Invoke();
        }

        private bool CheckRightInteractorHasObject()
        {
            return true;
          //  return RightGrabInteractor.Interactable != null && RightGrabInteractor.SelectedInteractable != null;
        }
    
        private bool CheckLeftInteractorHasObject()
        {
            return true;

            //return LeftGrabInteractor.Interactable != null && LeftGrabInteractor.SelectedInteractable != null;
        }

        public void ReduceHealth(int amt)
        {
            health -= amt;
        }

        public int GetHealth()
        {
            return health;
        }

        public void IncreaseHealth(int amt)
        {
            health += amt;
        }
    }

    [Serializable]
    public enum DominantHand
    {
        None,
        LeftHand,
        RightHand,
    }

    [Serializable]
    public enum DetectStates
    {
        Normal,
        Detect,
        UnDetect,
    }

    [Serializable]
    public enum LabelStatus
    {
        None,
        Show,
        Hide,
    }
}