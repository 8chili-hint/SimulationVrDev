using SimulationSystem.V0._1.Manager;

namespace SimulationSystem.V0._1.Warnings
{
    public class DetectWarningBasic : DetectWarning
    {
        private void OnEnable()
        {
            if (!GameManager.Instance.warningDetectsBasic.Contains(this))
            {
                GameManager.Instance.warningDetectsBasic.Add(this);
                GameManager.Instance.UIManager.AddDetectWarning(this);
            }
        }

        private void Start()
        {
            CanCheck = true;
        }

        private void Update()
        {
            if(CanCheck)
                UpdateState();
        } 
        
        public void OnDetect()
        {
            if (WarningText == "")
                hasDetected = false;
            else
                hasDetected = true;
        }

        public void OnUnDetect()
        {
            hasDetected = false;
        }

        public void Detect(bool value)
        {
            hasDetected = value;
        }

        private void OnDisable()
        {
            OnUnDetect();
            CanCheck = false;
            
            if (GameManager.Instance.warningDetectsBasic.Contains(this))
            {
                GameManager.Instance.warningDetectsBasic.Remove(this);
                GameManager.Instance.UIManager.PopDetectWarning(this);
            }
        }
    }
}
