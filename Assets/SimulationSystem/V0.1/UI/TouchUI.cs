using System.Collections;
using SimulationSystem.V0._1.Utility.Event;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SimulationSystem.V0._1.UI
{
    public class TouchUI : MonoBehaviour
    {
        [Header("Colors")] 
        [SerializeField] private Color normal;
        [SerializeField] private Color hover;
        [SerializeField] private Color select;

        [Header("Events")]
        [SerializeField] private UnityEvent onClick;

        private bool _isSelectable = true;
        private Image _image;
        private TouchUI[] _touchUIs;
    
        #region Monobehaviour

        private void Awake()
        {
            _image = GetComponent<Image>();
        
            TriggerUnityEventWrapper hoverTrigger = transform.GetChild(0).GetComponent<TriggerUnityEventWrapper>();
            hoverTrigger.onTriggerEnterEvent.AddListener(arg0 =>
            {
                if (arg0.CompareTag("Hand") && _isSelectable)
                {
                    ColorChange(hover);
                }
            });
            hoverTrigger.onTriggerExitEvent.AddListener(arg0 =>
            {
                if (arg0.CompareTag("Hand") && _isSelectable)
                {
                    ColorChange(normal);
                }
            });
            _touchUIs = Resources.FindObjectsOfTypeAll<TouchUI>();
        }



        private void OnDisable()
        {
            ColorChange(normal);
            _isSelectable = true;
        }

        #endregion
    
        #region Trigger Events
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hand") && _isSelectable)
            {
                //HapticManager.Instance.Haptic(0.2f, other.transform.GetComponent<FreeGrabBehavior>().handSide);
                StartCoroutine(IsSelectableSwitch());
            }
        }
    
        private IEnumerator IsSelectableSwitch()
        {
            ColorChange(select);
        
            foreach (TouchUI ui in _touchUIs)
            {
                ui._isSelectable = false;
            }
        
            onClick.Invoke();
            yield return new WaitForSeconds(2f);
        
            ColorChange(normal);
        
            foreach (TouchUI ui in _touchUIs)
            {
                ui._isSelectable = true;
            }    
        }
    
        #endregion

        #region Color Change

        private void ColorChange(Color currentColor)
        {
            _image.color = currentColor;
        }

        #endregion

    }
}