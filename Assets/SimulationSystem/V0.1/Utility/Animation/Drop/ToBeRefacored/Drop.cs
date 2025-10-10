using UnityEngine;

//Extension for drop animation
namespace SimulationSystem.V0._1.Utility.Animation.Drop.ToBeRefacored
{
    public class Drop : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            this.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}
