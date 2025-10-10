using SimulationSystem.V0._1.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.UI
{
    public class CountdownTimer : MonoBehaviour
    {
        public float timeLeft = 3.0f;
        public UITextHandler _textHandler; // used for showing countdown
        public UIImageHandler _imageHandler; // used for showing countdown

        public UnityEvent onEnable;
        public UnityEvent onDisable;

        private bool isFirst = true;

        private void OnEnable()
        {
            onEnable?.Invoke();
        }

        void Update()
        {
            timeLeft -= Time.deltaTime;
        
            _textHandler.UpdateText((timeLeft).ToString("0"));
            _imageHandler.OnDetecting(Mathf.InverseLerp(3,0,timeLeft));
            
            if (timeLeft <= 0)
            {
                _textHandler.UpdateText("Next Step");
                if (isFirst)
                {
                    Invoke(nameof(DisableTheObject),0.5f);
                    isFirst = false;
                }
            }
        }

        public void DisableTheObject()
        {
            transform.DeactivateThisObject();
        }

        private void OnDisable()
        {
            isFirst = true;
            onDisable?.Invoke();
        }
    }
}
