using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SimulationSystem.V0._1.UI
{
    public class UIPatientDetailHandler : MonoBehaviour
    {
        public PatientDetailContainer patientDetailContainer;
        public Image patientImage;
        public TMP_Text patientDetailText;

        private void Start()
        {
            if(patientDetailContainer == null)
                return;

            patientImage.sprite = patientDetailContainer.patientPicture;
            patientDetailText.text = String.Format("* <indent=6%>Name - {0}</indent> <line-height=130%>\n"
                                                   + "* <indent=6%>Age - {1}</indent><line-height=130%>\n"
                                                   + "* <indent=6%>Weight - {2}</indent><line-height=130%>\n"
                                                   + "* <indent=6%>Location - {3}</indent><line-height=130%>\n"
                                                   + "* <indent=6%>Ethnicity - {4}</indent><line-height=130%>\n"
                                                   + "* <indent=6%>Symptoms - {5}</indent><line-height=130%>\n"
                                                   + "* <indent=6%>Diagnosis - {6}</indent><line-height=130%>\n"
                                                   + "* <indent=6%>{7}</indent><line-height=130%>\n"
                , patientDetailContainer.patientName, patientDetailContainer.ageAndGender, patientDetailContainer.weight,
                patientDetailContainer.location, patientDetailContainer.ethnicity, patientDetailContainer.symptoms,
                patientDetailContainer.diagnosis, patientDetailContainer.results);
        }
    }
}