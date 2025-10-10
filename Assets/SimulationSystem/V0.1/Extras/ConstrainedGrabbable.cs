using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class ConstrainedGrabbable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    [System.Serializable]
    public class AxisConstraints
    {
        public bool ConstrainAxis = false;
        public float Min = 0f;
        public float Max = 1f;
    }

    [System.Serializable]
    public class PositionConstraints
    {
        public AxisConstraints XAxis = new AxisConstraints();
        public AxisConstraints YAxis = new AxisConstraints();
        public AxisConstraints ZAxis = new AxisConstraints();
    }

    [System.Serializable]
    public class RotationConstraints
    {
        public AxisConstraints XAxis = new AxisConstraints();
        public AxisConstraints YAxis = new AxisConstraints();
        public AxisConstraints ZAxis = new AxisConstraints();
    }

    public PositionConstraints positionConstraints;
    public RotationConstraints rotationConstraints;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        // Optionally do something when selected (e.g., change color)
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // Optionally do something when deselected

    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        //Console.WriteLine("This is the current UpdatePhase" + updatePhase);

        // Apply constraints continuously while the object is being grabbed
         ApplyPositionConstraints();
         ApplyRotationConstraints(); 
    }

    private void ApplyPositionConstraints()
    {
        Vector3 constrainedPosition = transform.position;

        if (positionConstraints.XAxis.ConstrainAxis)
            constrainedPosition.x = Mathf.Clamp(constrainedPosition.x, positionConstraints.XAxis.Min, positionConstraints.XAxis.Max);

        if (positionConstraints.YAxis.ConstrainAxis)
            constrainedPosition.y = Mathf.Clamp(constrainedPosition.y, positionConstraints.YAxis.Min, positionConstraints.YAxis.Max);

        if (positionConstraints.ZAxis.ConstrainAxis)
            constrainedPosition.z = Mathf.Clamp(constrainedPosition.z, positionConstraints.ZAxis.Min, positionConstraints.ZAxis.Max);

        transform.position = constrainedPosition;
    }

    private void ApplyRotationConstraints()
    {
        Vector3 eulerAngles = transform.localRotation.eulerAngles;

        if (rotationConstraints.XAxis.ConstrainAxis)
            eulerAngles.x = ClampAngle(eulerAngles.x, rotationConstraints.XAxis.Min, rotationConstraints.XAxis.Max);

        if (rotationConstraints.YAxis.ConstrainAxis)
            eulerAngles.y = ClampAngle(eulerAngles.y, rotationConstraints.YAxis.Min, rotationConstraints.YAxis.Max);

        if (rotationConstraints.ZAxis.ConstrainAxis)
            eulerAngles.z = ClampAngle(eulerAngles.z, rotationConstraints.ZAxis.Min, rotationConstraints.ZAxis.Max);

        transform.localRotation = Quaternion.Euler(eulerAngles);
        Debug.Log("Quad Value"+Quaternion.Euler(eulerAngles));
    }

    private float ClampAngle(float angle, float min, float max)
    {
/*        while (angle < 0) angle += 360; 
        while (angle > 360) angle -= 360;*/

        return Mathf.Clamp(angle, min, max);
    }

    public void DisableAll()
    {
        positionConstraints.XAxis.ConstrainAxis = false;
        positionConstraints.YAxis.ConstrainAxis = false;
        positionConstraints.ZAxis.ConstrainAxis = false;


        rotationConstraints.XAxis.ConstrainAxis = false;
        rotationConstraints.YAxis.ConstrainAxis = false;
        rotationConstraints.ZAxis.ConstrainAxis = false;
    }
}
