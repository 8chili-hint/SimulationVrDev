using UnityEngine;

namespace SimulationSystem.V0._1.UI
{
    [CreateAssetMenu(fileName = "PatientData", menuName = "ScriptableObjects/Patient/PatientData", order = 1)]
    public class PatientDetailContainer : ScriptableObject
    {
        public Sprite patientPicture;
        public string patientName;
        public string ageAndGender;
        public string weight;
        public string location;
        public string ethnicity;
        [TextArea]
        public string symptoms;
        [TextArea]
        public string diagnosis;
        [TextArea]
        public string results;
    }
}