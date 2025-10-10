using DG.Tweening;
using SimulationSystem.V0._1.Modules.Detect.ToBeRefactored__Derive_from_Detect_Abstract_;
using SimulationSystem.V0._1.Utility.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace SimulationSystem.V0._1.UI
{
    public class UIHintHandler : MonoBehaviour , IUIDetect
    {
        public bool DestroyAfterSuccess { get; set; } = false;
        public bool isInverted = false;
    
        private bool isFirstTrigger = false;
        private bool isFirstSuccess = true;
        private bool isFirstDetect = true;
        private Color hintMeshColor;
        [SerializeField] private Color _originalColor;
    
        [SerializeField] private bool detectByCollider = false;
        [SerializeField] private Image hintImage;
        [SerializeField] private MeshRenderer hintMesh;
        [SerializeField] private DetectHand eventWrapper;

        private void Awake()
        {
            if (eventWrapper == null && !detectByCollider)
            {
                if (GetComponentInParent<DetectHand>())
                {
                    eventWrapper = GetComponentInParent<DetectHand>();
                    
                }
            }
        
            if(!eventWrapper)
                return;
        
          /*  eventWrapper.OnHoverDetect.AddListener(OnHover);
            eventWrapper.OnUnHoverDetect.AddListener(OnUnHover);
            eventWrapper.OnDetect.AddListener(OnSelect);*/
        }

        //private void Start() => _originalColor = hintMesh.material.color;

        public void OnHover(bool isAssignedHand)
        {
            if (isFirstTrigger)
                return;

            if (isAssignedHand)
            {
                hintImage.DOColor(Color.green, 0.1f).OnComplete(delegate { isFirstTrigger = true; });
            }
            else
            {
                hintImage.DOColor(Color.red, 0.1f).OnComplete(delegate { isFirstTrigger = true; });
            }
        }
        public void OnHover()
        {
            if (!isFirstTrigger)
                hintImage.DOColor(Color.green, 0.1f).OnComplete(delegate { isFirstTrigger = true; });
        }
        public void OnUnHover()
        {
            if (isFirstTrigger)
                hintImage.DOColor(Color.yellow, 0.1f).OnComplete(delegate
                {
                    isFirstTrigger = false;
                    isFirstSuccess = true;
                });;
        }
        public void OnSelect() => hintImage.gameObject.DeactivateThisObject();
        public void OnSuccess()
        {
            if (isFirstSuccess)
            {
                Material newMat = new Material(hintMesh.sharedMaterial);
                hintMesh.sharedMaterial = newMat;

                var hintMeshColor = newMat.color;
                hintMesh.sharedMaterial.DOColor(Color.green, 1f).SetEase(Ease.Linear).OnComplete((() =>
                {
                    if (DestroyAfterSuccess)
                        transform.parent.DeactivateThisObject();

                    //hintMesh.sharedMaterial.color = hintMeshColor;
                }));

                isFirstSuccess = false;
            }
        }

        public void OnDetectOnce() => OnHover();
        public void OnDetecting()
        {
            if (isFirstDetect)
            {
                var newMat = new Material(hintMesh.sharedMaterial);
                hintMesh.sharedMaterial = newMat;
                hintMeshColor = newMat.color;
                isFirstDetect = false;
            }
        
            hintMesh.sharedMaterial.color = Color.Lerp(hintMeshColor, Color.green, Mathf.Lerp(0, 1, Time.deltaTime));
        }
        public void OnDetecting(float value)
        {
            if (isFirstDetect)
            {
                var newMat = new Material(hintMesh.sharedMaterial);
                hintMesh.sharedMaterial = newMat;
                hintMeshColor = newMat.color;
                isFirstDetect = false;
            }

            hintMesh.sharedMaterial.color = !isInverted ? Color.Lerp(hintMeshColor, Color.green, value) : Color.Lerp(hintMeshColor, Color.green, 1- value);
        }
        public void OnDetectingFinished() => OnSuccess();
        public void OnUnDetected() => OnUnHover();

        public void ResetDetect()
        {
            hintMesh.sharedMaterial.color = _originalColor;
        }
    }
}
