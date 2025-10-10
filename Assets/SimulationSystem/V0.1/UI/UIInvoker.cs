using System.Collections.Generic;
using System.Linq;
using SimulationSystem.V0._1.Utility.Extensions;
using UnityEngine;

namespace SimulationSystem.V0._1.UI
{
    public interface IUIDetect
    {
        public void OnDetectOnce();
        public void OnDetecting();
        public void OnDetecting(float value);
        public void OnDetectingFinished();
        public void OnUnDetected();
    }
    
    public class UIInvoker : MonoBehaviour , IUIDetect
    {
        [SerializeField] private UIAnimationHandler animHandler;
        [SerializeField] private UIImageHandler imageHandler;
        [SerializeField] private UIHintHandler hintHandler;
        [SerializeField] private UITextHandler textHandler;

        private IUIDetect _animHandler;
        private IUIDetect _imageHandler;
        private IUIDetect _hintHandler;
        private IUIDetect _textHandler;

        private List<IUIDetect> _handlers = new List<IUIDetect>();
        private readonly List<MonoBehaviour> _monoList = new List<MonoBehaviour>();
    
        private void Start()
        {
            _handlers = transform.GetChild(0).GetComponents<IUIDetect>().Except(Utility<IUIDetect>.ToList(this)).ToList();
            _handlers.ForEach(g => _monoList.Add((MonoBehaviour)g));
        
            animHandler = Utility<UIAnimationHandler>.GetComponentFromList(_monoList);
            imageHandler = Utility<UIImageHandler>.GetComponentFromList(_monoList);
            hintHandler = Utility<UIHintHandler>.GetComponentFromList(_monoList);
            textHandler = Utility<UITextHandler>.GetComponentFromList(_monoList);

            _animHandler = (IUIDetect)animHandler;
            _imageHandler = (IUIDetect)imageHandler;
            _hintHandler = (IUIDetect)hintHandler;
            _textHandler = (IUIDetect)textHandler;
        }
        
        public void OnDetectOnce()
        {
            _animHandler?.OnDetectOnce();
            _imageHandler?.OnDetectOnce();
            _hintHandler?.OnDetectOnce();
            _textHandler?.OnDetectOnce();
        }

        public void OnDetecting()
        {
            _animHandler?.OnDetecting();
            _imageHandler?.OnDetecting();
            _hintHandler?.OnDetecting();
            _textHandler?.OnDetecting();
        }

        public void OnDetecting(float value)
        {
            _animHandler?.OnDetecting(value);
            _imageHandler?.OnDetecting(value);
            _hintHandler?.OnDetecting(value);
            _textHandler?.OnDetecting(value);
        }

        public void OnDetectingFinished()
        {
            _animHandler?.OnDetectingFinished();
            _imageHandler?.OnDetectingFinished();
            _hintHandler?.OnDetectingFinished();
            _textHandler?.OnDetectingFinished();
        }

        public void OnUnDetected()
        {
            _animHandler?.OnUnDetected();
            _imageHandler?.OnUnDetected();
            _hintHandler?.OnUnDetected();
            _textHandler?.OnUnDetected();
        }
    }
}