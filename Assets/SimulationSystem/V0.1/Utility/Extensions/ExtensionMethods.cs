using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using SimulationSystem.V0._1.Manager;
using SimulationSystem.V0._1.Simulation;


public static class ExtensionMethods
{
    private static Vector3 currentvelocity = Vector3.zero;
    private static float speed = 0.3f;

    public static async Task MoveToPositionAsync(this Transform originalTransform, TransformContainer destinationTransform, CancellationToken token)
    {
        if (originalTransform == null)
        {
            // Handle the null case, throw an exception or return early
            return;
        }

        var destination = destinationTransform;
        var lerpAmount = 0f;

        while (Vector3.Distance(originalTransform.position, destination.Position) > 0.001f || originalTransform.rotation != destinationTransform.Rotation)
        {
            lerpAmount = Mathf.Clamp01(lerpAmount + Time.deltaTime * 0.1f);

            var newPosition = Vector3.Lerp(originalTransform.position, destination.Position, lerpAmount);
            var newRotation = Quaternion.Lerp(originalTransform.rotation, destination.Rotation, lerpAmount);
            var newScale = Vector3.Lerp(originalTransform.localScale, destination.localScale, lerpAmount);

            // Ensure these modifications are done on the main thread
            await Task.Yield();

            // Check for cancellation after awaiting Task.Yield()
            if (token.IsCancellationRequested)
            {
                originalTransform.SetPositionAndRotation(destinationTransform.Position, destinationTransform.Rotation);
                originalTransform.localScale = destinationTransform.localScale;
                return;
            }

            // Apply the modifications on the main thread
            originalTransform.SetPositionAndRotation(newPosition, newRotation);
            originalTransform.localScale = newScale;
        }
        // Apply the modifications on the main thread
        originalTransform.SetPositionAndRotation(destinationTransform.Position, destinationTransform.Rotation);
        originalTransform.localScale = destinationTransform.localScale;

    }

    public static void ForceUnGrab(this UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable a)
    {
           a.enabled= false;
    }


   /* private static void ForceUnGrabUsingCollider(Grabbable a)
    {
        a.GetComponentInChildren<Collider>().enabled = false;
    }
*/
    
}

public class MyTransform
{
    private TransformContainer ThisTransform = new TransformContainer();

    //For initializing TransfromContainer and setting it
    public MyTransform(Transform tf)
    {
        ThisTransform.Position = tf.position;
        ThisTransform.Rotation = tf.rotation;
        ThisTransform.localScale = tf.localScale;
    }

    public TransformContainer GetTransform()
    {
        return ThisTransform;
    }
}
public class TransformContainer
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 localScale;
}