using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using SimulationSystem.V0._1.Simulation;
using SimulationSystem.V0._1.Utility.Extensions;
using SimulationSystem.V0._1.Utility.ToBeRefactored.Switchable;
using SimulationSystem.V0._1.Warnings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SimulationSystem.V0._1.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
    

        [field: SerializeField, Header("Managers")] public SimulationManager SimulationManager { get; private set; } //Outside of GameManager Scope so need to give ref of Simulation
        [field: SerializeField] public PlayerManager PlayerManager { get; private set; }
        [field: SerializeField] public AudioManager AudioManager { get; private set; }
        [field: SerializeField] public UIManager UIManager { get; private set; }

        [Header("Warnings")]
        public List<DetectWarning> warningDetects = new List<DetectWarning>();
        public List<DetectWarningGrab> warningDetectsGrab { get; private set; }
        public List<DetectWarningBasic> warningDetectsBasic { get; private set; }
        public List<DetectWarningTrigger> warningDetectsTriggers { get; private set; }
        public List<DetectWarningTriggerPlusGrab> warningDetectsTriggerPlusGrab { get; private set; }

        [Header("Switchables")]
        private List<SwitchableAbs> _switchables;

        [Header("Patient Info")]
        public Transform patient;
        public Vector3[] patientPathLeft;
        public Vector3[] patientPathRight;
        public Vector3[] patientPathCenter;

        [Header("Attributes")] [Space(2)]
        [SerializedDictionary("key", "value")]
        public SerializedDictionary<string, string> attributes = new SerializedDictionary<string, string>();
        
        public UnityEvent onReturn;
        public UnityEvent onRestart;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        
            AudioManager = GetComponentInChildren<AudioManager>();
            PlayerManager = GetComponentInChildren<PlayerManager>();
            UIManager = GetComponentInChildren<UIManager>();
        }

        private void Start()
        {
            warningDetects = Resources.FindObjectsOfTypeAll<DetectWarning>().ToList();
            warningDetectsGrab = Utility<DetectWarningGrab>.GetComponentsFromList(warningDetects.ToList<MonoBehaviour>());
            warningDetectsBasic = Utility<DetectWarningBasic>.GetComponentsFromList(warningDetects.ToList<MonoBehaviour>());
            warningDetectsTriggers = Utility<DetectWarningTrigger>.GetComponentsFromList(warningDetects.ToList<MonoBehaviour>());
            warningDetectsTriggerPlusGrab = Utility<DetectWarningTriggerPlusGrab>.GetComponentsFromList(warningDetects.ToList<MonoBehaviour>());

            _switchables = Resources.FindObjectsOfTypeAll<SwitchableAbs>().ToList();
        }

        [ContextMenu("Move Patient To Left")]
        public void MoveAnatomyToLeft() 
        {
            //patient.DOPath(patientPathLeft, 1); //Refactor this to switchable
            _switchables.ForEach(g => StartCoroutine(g.GetSwitchIEnumerator()));
            attributes.Add("/dh", "left hand");
            attributes.Add("/ndh", "right hand");
        }
    
        [ContextMenu("Move Patient To Right")]
        public void MoveAnatomyToRight()
        {
            //patient.DOPath(patientPathRight, 1);
            _switchables.ForEach(g => StartCoroutine(g.GetOriginalIEnumerator()));
            attributes.Add("/dh", "right hand");
            attributes.Add("/ndh", "left hand");
        }
    
        [ContextMenu("Move Patient Back To Center")]
        public void MoveAnatomyToCenter()
        {
            patient.DOPath(patientPathCenter, 1);
            _switchables.ForEach(g => StartCoroutine(g.GetOriginalIEnumerator()));
        }

        public void OnHealthZero()
        {
            SimulationManager.enabled = false; //
        }

        public void Restart()
        {
            Debug.Log("Restart simulation");
            onRestart.Invoke();
        }

        public void Return()
        {
            Debug.Log("Return to main scene");
            onReturn.Invoke();
        }

        public UnityEvent onBackToMainMenu = new UnityEvent();

        public void RestartScene()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }

        public void BackToMainMenu()
        {
            onBackToMainMenu.Invoke();
        }
    }
}


