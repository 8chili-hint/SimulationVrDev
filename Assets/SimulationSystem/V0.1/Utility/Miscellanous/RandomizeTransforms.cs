using System.Collections.Generic;
using SimulationSystem.V0._1.Utility.Extensions;
using UnityEngine;

namespace SimulationSystem.V0._1.Utility.Miscellanous
{
    public class RandomizeTransforms : MonoBehaviour
    {
        public bool ShouldAffectObjectRoation;
        [Header("Add Transforms to be randomized below (One of the random transform form list below will be assigned to a random GameObject)")]
        [SerializeField] private List<Transform> _transformPositionsToRandomize = new List<Transform>();//1,2,3 => 2,1,3

        [Header("Add Gameobjects' transform below (One of the Gameobject will be assigned a RandomTransform form the list of transforms above)")]
        [SerializeField] private List<Transform> _transformObjectsToRandomize = new List<Transform>();//2,1,3


       /*Randomized Position List*/ private List<Vector3> ObjectTransformPos = new List<Vector3>();
        private List<Quaternion> ObjectTransformRot = new List<Quaternion>();


        private void Awake() 
        {
       


            _transformPositionsToRandomize.Shuffle();

            foreach (var a in _transformPositionsToRandomize)
            {
                ObjectTransformPos.Add(a.transform.position);
                ObjectTransformRot.Add(a.transform.rotation);

            }

        }

        public void Randomize()
        {
            if (ShouldAffectObjectRoation)
            {
                for (int i = 0; i < _transformObjectsToRandomize.Count; i++)
                {   
                    _transformObjectsToRandomize[i].gameObject.transform.position = ObjectTransformPos[i];

                    _transformObjectsToRandomize[i].gameObject.transform.rotation = ObjectTransformRot[i];
                }
            }
            else
            {
                for (int i = 0; i < _transformObjectsToRandomize.Count; i++)
                {
                    _transformObjectsToRandomize[i].gameObject.transform.position = ObjectTransformPos[i];
                }
            }

            
        }
    }
}
