using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using SimulationSystem.V0._1.Modules.Detect.Interface;
using SimulationSystem.V0._1.Simulation;
using UnityEngine;
using UnityEngine.Events;

namespace SimulationSystem.V0._1.Utility.Extensions
{
    public static class ExtensionClass
    {
        public static Coroutine InvokeCondition(this MonoBehaviour mono, Action action, bool condition) =>
            mono.StartCoroutine(InvokeCondition(action, condition));
        /*public static void InvokeCondition(this MonoBehaviour mono, Action action, float time) =>
        mono.StartCoroutine(InvokeCondition(action, time));*/

        public static Coroutine InvokeCondition(this MonoBehaviour mono, Action action, float time) =>
            mono.StartCoroutine(InvokeCondition(action, time));
    

        public static bool DistanceFrom(this Transform objectToDetectFrom, Transform desiredTransform, float precision)
        {
            float dist = Vector3.Distance(desiredTransform.localPosition, objectToDetectFrom.localPosition);
            if(dist > precision)
            {
                return true;
            }

            return false;
        }
        public static bool DistanceFrom(this Transform objectToDetectFrom, Vector3 desiredVector, float precision)
        {
            var dist = Vector3.Distance(desiredVector, objectToDetectFrom.localPosition);
            return dist < precision;
        }
        public static bool DistanceFrom(this Transform objectToDetectFrom, Vector3 desiredVector, float precision, out float distanceLeft)
        {
            var dist = Vector3.Distance(desiredVector, objectToDetectFrom.localPosition);
            distanceLeft = dist;
            return dist < precision;
        }
    
        public static bool DistanceFromAtWorld(this Transform objectToDetectFrom, Vector3 desiredVector, float precision, out float distanceLeft)
        {
            var dist = Vector3.Distance(desiredVector, objectToDetectFrom.position);
            distanceLeft = dist;
            return dist < precision;
        }
        public static bool AngleFrom(this Transform objectToDetectFrom, Transform desiredTransform, float precision)
        {
            float dist = Mathf.Abs(Quaternion.Dot(objectToDetectFrom.rotation, desiredTransform.rotation));
            if(dist > precision)
            {
                return true;
            }
        
            return false;
        }
        public static bool AngleFrom(this Transform objectToDetectFrom, Quaternion desiredAngle, float precision)
        {
            float dist = Mathf.Abs(Quaternion.Dot(objectToDetectFrom.rotation, desiredAngle));
            if(dist > precision)
            {
                return true;
            }
        
            return false;
        }

        public static IEnumerator DelayDetection(this IDetect detect, Action action, bool condition)
        {
            yield return new WaitUntil((() => condition));
            action?.Invoke();
        }
        public static IEnumerator DelayDetection(this IDetect detect, Action action, float time)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }
    
        public static  TweenerCore<Vector3,Vector3,VectorOptions> AnimatePop(this Transform prop, Vector3 newScale, float animateTime)
        {
            Vector3 localScale = prop.localScale;
        
            var tween = prop.DOScale(newScale, animateTime).SetEase(Ease.Linear);

            return tween;
        }
        public static Vector3 AnimatePop(this Transform prop, float animateTime)
        {
            Vector3 localScale = prop.localScale;
            Vector3 newScale = Vector3.one;
        
            if(newScale.magnitude > localScale.magnitude)
                prop.DOScale(newScale, animateTime).SetEase(Ease.InOutBack);
            else if (newScale.magnitude == localScale.magnitude)
                prop.DOScale(new Vector3(0, 0, 0), animateTime).SetEase(Ease.InOutBack).Complete();

            return localScale;
        }

        public static void ActivateThisObject(this GameObject obj) => obj.SetActive(true);
        public static void DeactivateThisObject(this GameObject obj) => obj.SetActive(false);

        /*public static void ActivateAnimation(this UIAnimationHandler anim) {
        anim.OnDetectOnce();
    }*/
        //public static void DeactivateAnimation(this UIAnimationHandler anim) => anim.OnUnDetected();
        private static IEnumerator InvokeCondition(Action action, bool condition)
        {
            yield return new WaitUntil((() => condition));
            action?.Invoke();
        }
    
        private static IEnumerator InvokeCondition(Action action, float time)
        {
            yield return new WaitForSeconds((time));
            action?.Invoke();
        }

        public static void ActivateThisObject(this Transform obj) => obj.gameObject.SetActive(true);
        public static void DeactivateThisObject(this Transform obj) => obj.gameObject.SetActive(false);
    }

    public static class Utility<T>
    {
        //Test it
        public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            Vector3 AB = b - a;
            Vector3 AV = value - a;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }

        /// <summary>
        /// Convert any single object to a list
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToList(T t)
        {
            var _t = new List<T>();
            _t.Add(t);
            return _t;
        }

        /// <summary>
        /// Get T component from the list of MonoBehaviours
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T GetComponentFromList(List<MonoBehaviour> t)
        {
            CastHelper<T> _j = new CastHelper<T>(); 
            foreach (var obj in t)
            {
                if (obj.TryGetComponent(out T _t))
                {
                    _j.t = _t;
                    return _t;
                }
            }
            return _j.t;
        }
    
        /// <summary>
        /// Get T components from the list of MonoBehaviours
        /// </summary>
        /// <param name="t"></param>
        /// <returns>
        /// "List<T>"
        /// </returns>
        public static List<T> GetComponentsFromList(List<MonoBehaviour> t)
        {
            CastHelper<T> _t = new CastHelper<T>();

            _t.tList = new List<T>();
        
            foreach (var obj in t)
            {
                if (obj.TryGetComponent(out T _j))
                {
                    _t.tList.Add(_j);
                }
            }
            return _t.tList;
        }
    
        public static string CheckForAttributes(string sentence, Dictionary<string, string> attribute)
        {
            string newSentence = sentence;
            foreach (var keyValuePair in attribute.Where(keyValuePair => sentence.Contains(keyValuePair.Key)))
            {
                newSentence = sentence.Replace(keyValuePair.Key, keyValuePair.Value);
                sentence = newSentence;
            }
            return newSentence;
        }

        public static void UpdateStateProperty(string id, List<StateUpdater> states)
        {
            states.ForEach(g =>
            {
                if (g.id.Contains(id))
                {
                    g.UpdateStatePrompt();
                }
            });
        }
        #region UnityEventCopy
        /*public static void CopyUnityEvents(object sourceObj, string source_UnityEvent, object dest, bool debug = false)
{
    FieldInfo unityEvent = sourceObj.GetType().GetField(source_UnityEvent, E_Helpers.allBindings);
    if (unityEvent.FieldType != dest.GetType())
    {
        if (debug == true)
        {
            Debug.Log("Source Type: " + unityEvent.FieldType);
            Debug.Log("Dest Type: " + dest.GetType());
            Debug.Log("CopyUnityEvents - Source & Dest types don't match, exiting.");
        }
        return;
    }
    else
    {
        SerializedObject so = new SerializedObject((Object)sourceObj);
        SerializedProperty persistentCalls = so.FindProperty(source_UnityEvent).FindPropertyRelative("m_PersistentCalls.m_Calls");
        for (int i = 0; i < persistentCalls.arraySize; ++i)
        {
            Object target = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Target").objectReferenceValue;
            string methodName = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_MethodName").stringValue;
            MethodInfo method = null;
            try
            {
                method = target.GetType().GetMethod(methodName, E_Helpers.allBindings);
            }
            catch
            {
                foreach (MethodInfo info in target.GetType().GetMethods(E_Helpers.allBindings).Where(x => x.Name == methodName))
                {
                    ParameterInfo[] _params = info.GetParameters();
                    if (_params.Length < 2)
                    {
                        method = info;
                    }
                }
            }
            ParameterInfo[] parameters = method.GetParameters();
            switch(parameters[0].ParameterType.Name)
            {
                case nameof(System.Boolean):
                    bool bool_value = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Arguments.m_BoolArgument").boolValue;
                    var bool_execute = System.Delegate.CreateDelegate(typeof(UnityAction<bool>), target, methodName) as UnityAction<bool>;
                    UnityEventTools.AddBoolPersistentListener(
                        dest as UnityEventBase,
                        bool_execute,
                        bool_value
                    );
                    break;
                case nameof(System.Int32):
                    int int_value = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Arguments.m_IntArgument").intValue;
                    var int_execute = System.Delegate.CreateDelegate(typeof(UnityAction<int>), target, methodName) as UnityAction<int>;
                    UnityEventTools.AddIntPersistentListener(
                        dest as UnityEventBase,
                        int_execute,
                        int_value
                    );
                    break;
                case nameof(System.Single):
                    float float_value = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Arguments.m_FloatArgument").floatValue;
                    var float_execute = System.Delegate.CreateDelegate(typeof(UnityAction<float>), target, methodName) as UnityAction<float>;
                    UnityEventTools.AddFloatPersistentListener(
                        dest as UnityEventBase,
                        float_execute,
                        float_value
                    );
                    break;
                case nameof(System.String):
                    string str_value = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Arguments.m_StringArgument").stringValue;
                    var str_execute = System.Delegate.CreateDelegate(typeof(UnityAction<string>), target, methodName) as UnityAction<string>;
                    UnityEventTools.AddStringPersistentListener(
                        dest as UnityEventBase,
                        str_execute,
                        str_value
                    );
                    break;
                case nameof(System.Object):
                    Object obj_value = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Arguments.m_ObjectArgument").objectReferenceValue;
                    var obj_execute = System.Delegate.CreateDelegate(typeof(UnityAction<Object>), target, methodName) as UnityAction<Object>;
                    UnityEventTools.AddObjectPersistentListener(
                        dest as UnityEventBase,
                        obj_execute,
                        obj_value
                    );
                    break;
                default:
                    var void_execute = System.Delegate.CreateDelegate(typeof(UnityAction), target, methodName) as UnityAction;
                    UnityEventTools.AddPersistentListener(
                        dest as UnityEvent,
                        void_execute
                    );
                    break;
            }
        }
    }
}*/
        #endregion
    }

    struct CastHelper<T>
    {
        public T t;
        public List<T> tList;
        public IntPtr onePointerFurtherThanT;
    }

    [Serializable]
    public class FloatEvent : UnityEvent<float> { }
}