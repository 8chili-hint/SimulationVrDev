using System.Collections.Generic;
using System.Linq;
using SimulationSystem.V0._1.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Utility.ToBeRefactored
{
    public class ObstacleDetector : MonoBehaviour
    {
        [SerializeField] private List<Obstacle> obstacles = new List<Obstacle>();
        [SerializeField] private List<SafeArea> safeArea = new List<SafeArea>();

        [SerializeField] private UnityEvent onObstacleHit;
        [SerializeField] private UnityEvent onSafeArea;
        [SerializeField] private UnityEvent onHealthEmpty;

        private bool _check = true;
        private bool _isFirstCall = true;

        private float restTime = 1;
        private float time = 0;

        private void OnEnable()
        {
            _check = true;
            _isFirstCall = true;
            time = 0;
        }

        private void Update()
        {
            if (obstacles.Any(g => g.IsDetected) && GameManager.Instance.PlayerManager.GetHealth() > 0 && _check)
            {
                GameManager.Instance.PlayerManager.ReduceHealth(1);
                onObstacleHit?.Invoke();
                _check = false;
                time = 0;
            }
            else if (safeArea.Any(g => g.IsDetected) && GameManager.Instance.PlayerManager.GetHealth() > 0 && !_check && obstacles.All(g => !g.IsDetected))
            {
                time += Time.deltaTime;
                if (restTime < time)
                {
                    onSafeArea?.Invoke();
                    _check = true;
                    time = 0;
                }
            }

            if (GameManager.Instance.PlayerManager.GetHealth() == 0 && _isFirstCall)
            {
                onHealthEmpty?.Invoke();
                GameManager.Instance.OnHealthZero();
                GameManager.Instance.UIManager.InvokeRestartUI();
                GameManager.Instance.PlayerManager.IncreaseHealth(2);
                _isFirstCall = false;
                // this.enabled = false;
            }
        }
    }
}