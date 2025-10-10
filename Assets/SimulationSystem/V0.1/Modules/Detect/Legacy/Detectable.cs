using UnityEngine;

namespace SimulationSystem.V0._1.Modules.Detect.Legacy
{
    public class Detectable : MonoBehaviour
    {
        // [Header("[DETECT]")] [SerializeField]
        // private bool _startOnAwake = false;
        //
        // [Space(10)]
        // [SerializeField, Interface(typeof(IDetect))]
        // private MonoBehaviour detectObject = null;
        //
        // [SerializeField, Interface(typeof(IDetect))]
        // private MonoBehaviour detectAngle = null;
        //
        // [SerializeField, Interface(typeof(IDetect))]
        // private MonoBehaviour detectDistance = null;
        //
        // [SerializeField, Interface(typeof(IDetect))]
        // private MonoBehaviour detectHand = null;
        //
        // private IDetect _detectObject;
        // private IDetect _detectAngle;
        // private IDetect _detectDistance;
        // private IDetect _detectHand;
        //
        // private List<IDetect> _handlers = new List<IDetect>();
        // private readonly List<MonoBehaviour> _monoList = new List<MonoBehaviour>();
        //
        // public void Awake()
        // {
        //     InjectComponent();
        //     
        //     _detectObject = (IDetect)detectObject;
        //     _detectAngle = (IDetect)detectAngle;
        //     _detectDistance = (IDetect)detectDistance;
        //     _detectHand = (IDetect)detectHand;
        //     
        //     StopDetection();
        //     
        //     if(_startOnAwake)
        //         StartDetection();   
        // }
        //
        // private void InjectComponent()
        // {
        //     _handlers = transform.GetComponents<IDetect>().ToList();
        //     _handlers.ForEach(g => _monoList.Add((MonoBehaviour)g));
        //     
        //     if (!detectHand)
        //         detectHand = Utility<DetectHand>.GetComponentFromList(_monoList);
        //     
        //     if (!detectAngle)
        //         detectAngle = Utility<DetectAngle>.GetComponentFromList(_monoList);
        //     
        //     if (!detectDistance)
        //         detectDistance = Utility<DetectDistance>.GetComponentFromList(_monoList);
        //     
        //     if (!detectObject)
        //         detectObject = Utility<DetectObjectAbstract>.GetComponentFromList(_monoList);
        // }
        //
        // private void Start()
        // {
        //     _detectObject?.Initialize();
        //     _detectAngle?.Initialize();
        //     _detectDistance?.Initialize();
        //     _detectHand?.Initialize();
        // }
        //
        // public void Update()
        // {
        //     _detectObject?.UpdateLoop();
        //     _detectAngle?.UpdateLoop();
        //     _detectDistance?.UpdateLoop();
        //     _detectHand?.UpdateLoop();
        // }
        //
        // private void OnTriggerEnter(Collider other)
        // {
        //     _detectObject?.OnObjectEnter(other);
        //     _detectAngle?.OnObjectEnter(other);
        //     _detectDistance?.OnObjectEnter(other);
        //     _detectHand?.OnObjectEnter(other);
        // }
        //
        // private void OnTriggerStay(Collider other)
        // {
        //     _detectObject?.OnObjectStay(other);
        //     _detectAngle?.OnObjectStay(other);
        //     _detectDistance?.OnObjectStay(other);
        //     _detectHand?.OnObjectStay(other);
        // }
        //
        // private void OnTriggerExit(Collider other)
        // {
        //     _detectObject?.OnObjectExit(other);
        //     _detectAngle?.OnObjectExit(other);
        //     _detectDistance?.OnObjectExit(other);
        //     _detectHand?.OnObjectExit(other);
        // }
        //
        // public void StartDetection()
        // {
        //     _detectObject?.StartDetection();
        //     _detectAngle?.StartDetection();
        //     _detectDistance?.StartDetection();
        //     _detectHand?.StartDetection();
        // }
        //
        // public void StopDetection()
        // {
        //     _detectObject?.StopDetection();
        //     _detectAngle?.StopDetection();
        //     _detectDistance?.StopDetection();
        //     _detectHand?.StopDetection();
        // }
    }
}
