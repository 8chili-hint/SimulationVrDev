using SimulationSystem.V0._1.Manager;
using UnityEngine;

namespace SimulationSystem.V0._1.Legacy
{
    ///Currently not in use
    public class DamageSystem : MonoBehaviour
    {
        [SerializeField] private GameObject damageProvider;
        [SerializeField] private int health = 2;

        private bool _firstCall = true;
        private void Update()
        {
            if (health < 1 && _firstCall)
            {
                GameManager.Instance.UIManager.InvokeRestartUI();
                GameManager.Instance.OnHealthZero();
                _firstCall = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == damageProvider)
            {
                health--;
            }
        }
    }
}
