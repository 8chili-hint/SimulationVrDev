using UnityEngine;
using System.Collections.Generic;
using System.Collections; // Required for Coroutines
using UnityEngine.Events;

/// <summary>
/// This script controls a drone autonomously, guiding it along predefined paths
/// by directly manipulating the drone's Transform using Vector3.MoveTowards and Quaternion.RotateTowards.
/// It bypasses the physics-based movement (pitch, roll, yaw, throttle inputs) of the DroneController
/// when in autopilot mode.
///
/// IMPORTANT: For this script to function correctly, you MUST make the following changes
/// to your existing 'DroneController.cs' script:
///
/// 1.  Change the access modifier for the following private fields to public.
///     This allows the AutoPilotController script to directly set these values.
///     Find these lines in DroneController.cs and change 'private' to 'public':
///     public float pitchInput;
///     public float rollInput;
///     public float yawInput;
///     public float throttleInput;
///
/// 2.  Add a public property and a public method to DroneController.cs to manage the autopilot's state
///     and prevent conflicts with manual input. Add these lines within the 'DroneController' class:
///
///     public bool IsAutoPiloting { get; private set; } = false;
///
///     public void SetAutoPilotActive(bool active)
///     {
///         IsAutoPilating = active;
///         if (active)
///         {
///             // When autopilot is active, disable manual input actions
///             if (leftJoystickActionProperty != null && leftJoystickActionProperty.action != null)
///                 leftJoystickActionProperty.action.Disable();
///             if (rightJoystickActionProperty != null && rightJoystickActionProperty.action != null)
///                 rightJoystickActionProperty.action.Disable();
///
///             // Reset inputs to zero for autopilot to take over cleanly
///             pitchInput = 0f;
///             rollInput = 0f;
///             yawInput = 0f;
///             throttleInput = 0f;
///         }
///         else
///         {
///             // When autopilot is deactivated, re-enable manual input actions
///             if (leftJoystickActionProperty != null && leftJoystickActionProperty.action != null)
///                 leftJoystickActionProperty.action.Enable();
///             if (rightJoystickActionProperty != null && rightJoystickActionProperty.action != null)
///                 rightJoystickActionProperty.action.Enable();
///
///             // Reset inputs to zero for manual control to take over cleanly
///             pitchInput = 0f;
///             rollInput = 0f;
///             yawInput = 0f;
///             throttleInput = 0f;
///         }
///     }
///
/// 3.  Modify the input callback methods (OnLeftJoystickPerformed, OnLeftJoystickCanceled,
///     OnRightJoystickPerformed, and OnRightJoystickCanceled) in DroneController.cs
///     to only apply joystick input if 'IsAutoPiloting' is FALSE. For example, change:
///
///     private void OnLeftJoystickPerformed(InputAction.CallbackContext context)
///     {
///         Vector2 leftStickValue = context.ReadValue<Vector2>();
///         rollInput =  -ApplyDeadzoneAndRemap(leftStickValue.y, joystickDeadzone);
///         pitchInput = ApplyDeadzoneAndRemap(leftStickValue.x, joystickDeadzone);
///     }
///
///     TO THIS:
///
///     private void OnLeftJoystickPerformed(InputAction.CallbackContext context)
///     {
///         if (!IsAutoPiloting) // <--- Add this check
///         {
///             Vector2 leftStickValue = context.ReadValue<Vector2>();
///             rollInput =  -ApplyDeadzoneAndRemap(leftStickValue.y, joystickDeadzone);
///             pitchInput = ApplyDeadzoneAndRemap(leftStickValue.x, joystickDeadzone);
///         }
///     }
///     (Apply similar 'if (!IsAutoPiloting)' checks to the beginning of the other three
///     joystick input callback methods: OnLeftJoystickCanceled, OnRightJoystickPerformed,
///     and OnRightJoystickCanceled.)
/// </summary>
[RequireComponent(typeof(DroneController))]
public class AutoPilotController : MonoBehaviour
{
    [Tooltip("Assign a child GameObject here that represents the 'front' of the drone for accurate yaw alignment. If null, the main drone's forward is used.")]
    public Transform FrontOfTheDrone;

    [Tooltip("Offset for the Gizmo indicating the drone's forward direction, applied in local space relative to the drone's center.")]
    public Vector3 forwardGizmoOffset = new Vector3(0, 0.2f, 0); // Default offset to raise the gizmo slightly

    [Tooltip("Rotational offset (Euler angles) applied to the drone's 'forward' vector to define the effective forward axis for yaw alignment. This rotates the blue gizmo.")]
    public Vector3 forwardAxisRotationOffset = Vector3.zero; // Default to no rotation offset

    // A serializable struct to define a single drone path, containing a list of waypoints.
    [System.Serializable] // Essential for Unity to show this struct in the Inspector
    public struct DronePath
    {
        [Tooltip("The ordered list of Transform waypoints for this path.")]
        public List<Transform> Waypoints;
    }

    [Header("Autopilot Settings")]
    [Tooltip("A list of predefined drone paths, each with its own set of waypoints.")]
    public List<DronePath> dronePaths = new List<DronePath>();

    [Tooltip("The distance threshold to consider a waypoint 'reached'.")]
    public float waypointReachedThreshold = 1f;

    [Tooltip("The speed at which the drone moves towards its target waypoint when in autopilot mode.")]
    public float speed = 5f; // Public float for overall speed control

    [Tooltip("The speed at which the drone rotates towards its target orientation when in autopilot mode.")]
    public float rotationSpeed = 100f; // Speed for Quaternion.RotateTowards (degrees per second)

    [Tooltip("The angular threshold (in degrees) within which the drone's forward direction is considered 'aligned' with the target waypoint's horizontal direction.")]
    public float yawAlignmentThreshold = 5f; // degrees

    private DroneController droneController;
    private Coroutine currentFlightCoroutine;
    private bool isAutoPilotActive = false;
    private int currentPathIndex = -1;
    private int currentWaypointIndex = -1;

    public UnityEvent OnAutoPilotStarted;
    public UnityEvent OnAutoPilotKilled;

    public List<DronePath> LandingBaseWaypoint; // Ensure its use aligns with your game logic.

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures the DroneController component is present.
    /// </summary>
    void Awake()
    {
        droneController = GetComponent<DroneController>();
        if (droneController == null)
        {
            Debug.LogError("AutoPilotController requires a DroneController component on the same GameObject.");
            enabled = false; // Disable this script if DroneController is not found
        }
        if (FrontOfTheDrone == null)
        {
            Debug.LogWarning("FrontOfTheDrone Transform is not assigned. Yaw alignment will default to the main drone's forward direction.");
        }
    }

    // A context menu item for quick testing in the editor.
    [ContextMenu("Try Auto Pilot (Path 0 - dronePaths)")]
    public void CheckAutoPilot()
    {
        FlyToDestination(0);
    }

    [ContextMenu("Land Drone (Path 0 - LandingBaseWaypoint)")]
    public void LandDrone(int pathIndex = 0)
    {
        OnAutoPilotStarted?.Invoke();

        // If DroneController is not found, cannot proceed
        if (droneController == null)
        {
            Debug.LogError("DroneController not found. Cannot start landing autopilot.");
            return;
        }

        // Validate the provided path index for LandingBaseWaypoint
        if (pathIndex < 0 || pathIndex >= LandingBaseWaypoint.Count || LandingBaseWaypoint[pathIndex].Waypoints == null || LandingBaseWaypoint[pathIndex].Waypoints.Count == 0)
        {
            Debug.LogWarning($"Invalid landing path index {pathIndex} or empty landing path. Aborting autopilot.");
            KillAutoPilot();
            return;
        }

        KillAutoPilot(); // Stop any currently active autopilot sequence before starting a new one

        currentPathIndex = pathIndex;
        currentWaypointIndex = 0;
        isAutoPilotActive = true;

        droneController.SetAutoPilotActive(true);

        Debug.Log($"Starting landing autopilot on path {pathIndex} with {LandingBaseWaypoint[pathIndex].Waypoints.Count} waypoints.");

        currentFlightCoroutine = StartCoroutine(LandingAutoPilotFlightRoutine());
    }

    /// <summary>
    /// Public method to start autonomous flight along a specified path.
    /// This method can be called from other scripts or from the Unity Inspector.
    /// </summary>
    /// <param name="pathIndex">The index of the path in the 'dronePaths' list to fly along.</param>
    public void FlyToDestination(int pathIndex)
    {
        // If DroneController is not found, cannot proceed
        if (droneController == null)
        {
            Debug.LogError("DroneController not found. Cannot start autopilot.");
            return;
        }

        // Validate the provided path index
        if (pathIndex < 0 || pathIndex >= dronePaths.Count || dronePaths[pathIndex].Waypoints == null || dronePaths[pathIndex].Waypoints.Count == 0)
        {
            Debug.LogWarning($"Invalid path index {pathIndex} or empty path. Aborting autopilot.");
            KillAutoPilot(); // Ensure autopilot is off if input is invalid
            return;
        }

        KillAutoPilot(); // Stop any currently active autopilot sequence before starting a new one

        // Initialize state for the new flight path
        currentPathIndex = pathIndex;
        currentWaypointIndex = 0;
        isAutoPilotActive = true;

        // Inform the DroneController to disable manual input and prepare for autopilot control
        droneController.SetAutoPilotActive(true);

        Debug.Log($"Starting autopilot on path {pathIndex} with {dronePaths[pathIndex].Waypoints.Count} waypoints.");

        // Start the main autopilot coroutine
        currentFlightCoroutine = StartCoroutine(AutoPilotFlightRoutine());
    }

    /// <summary>
    /// Public method to immediately stop any active autonomous flight.
    /// This method can be called from other scripts or from the Unity Inspector.
    /// </summary>
    public void KillAutoPilot()
    {
        // Stop the flight coroutine if it's running
        if (currentFlightCoroutine != null)
        {
            StopCoroutine(currentFlightCoroutine);
            currentFlightCoroutine = null;
        }

        isAutoPilotActive = false; // Set autopilot state to inactive

        // If DroneController exists, reset its inputs and re-enable manual control
        if (droneController != null)
        {
            // Reset inputs to zero as we are bypassing them for autopilot
            droneController.pitchInput = 0f;
            droneController.rollInput = 0f;
            droneController.yawInput = 0f;
            droneController.throttleInput = 0f;
            droneController.SetAutoPilotActive(false);
        }

        // Reset path and waypoint indices
        currentPathIndex = -1;
        currentWaypointIndex = -1;
        OnAutoPilotKilled?.Invoke();
        Debug.Log("Autopilot killed.");
    }

    /// <summary>
    /// The main coroutine that drives the autonomous flight.
    /// It moves the drone directly using MoveTowards and rotates it using RotateTowards.
    /// </summary>
    private IEnumerator AutoPilotFlightRoutine()
    {
        OnAutoPilotStarted?.Invoke();

        // Continue as long as autopilot is active and there are waypoints left in the current path
        while (isAutoPilotActive && currentPathIndex >= 0 && currentWaypointIndex < dronePaths[currentPathIndex].Waypoints.Count)
        {
            // Get the current target waypoint
            Transform targetWaypoint = dronePaths[currentPathIndex].Waypoints[currentWaypointIndex];
            if (targetWaypoint == null)
            {
                Debug.LogError($"Waypoint at path index {currentPathIndex}, waypoint index {currentWaypointIndex} is null. Aborting autopilot.");
                KillAutoPilot(); // Abort if a waypoint is unexpectedly null
                yield break; // Exit the coroutine
            }

            Vector3 currentPosition = droneController.transform.position; // Get current position
            Vector3 directionToTarget = targetWaypoint.position - currentPosition;
            float distanceToTarget = directionToTarget.magnitude;

            // --- Rotation Control (Yaw-First Alignment) ---

            // Get the drone's base forward direction, either from FrontOfTheDrone or the main transform.
            Vector3 baseForward = (FrontOfTheDrone != null) ? FrontOfTheDrone.forward : droneController.transform.forward;

            // Apply the rotational offset to the base forward vector directly.
            // This rotation defines the 'effective' front for yawing from the drone's current orientation.
            Vector3 droneForwardForYaw = Quaternion.Euler(forwardAxisRotationOffset) * baseForward;

            // Project the result onto the horizontal plane for yaw-only comparison.
            droneForwardForYaw.y = 0;
            if (droneForwardForYaw.magnitude < 0.01f) // Fallback if the effective horizontal forward is nearly zero
            {
                droneForwardForYaw = Vector3.forward; // A safe default horizontal direction
            }
            droneForwardForYaw.Normalize(); // Ensure it's a unit vector

            // Calculate target horizontal direction for the drone to look at
            Vector3 horizontalDirectionToTarget = new Vector3(directionToTarget.x, 0, directionToTarget.z);
            if (horizontalDirectionToTarget.magnitude < 0.01f) // If target is directly above/below or very close horizontally
            {
                horizontalDirectionToTarget = droneForwardForYaw; // Maintain current effective forward direction if no clear target
            }
            horizontalDirectionToTarget.Normalize();

            // The target rotation is based on where the drone's *effective* forward should point.
            // We want the drone's actual transform to rotate such that 'droneForwardForYaw' aligns with 'horizontalDirectionToTarget'.
            // This requires calculating the inverse rotation of the offset, applying it to the target direction,
            // and then using Quaternion.LookRotation for the drone's transform.
            Quaternion targetRotationForDroneTransform = Quaternion.LookRotation(horizontalDirectionToTarget, Vector3.up);
            // Now, we need to account for the 'forwardAxisRotationOffset' in reverse to get the actual drone transform rotation.
            targetRotationForDroneTransform *= Quaternion.Inverse(Quaternion.Euler(forwardAxisRotationOffset));

            // Rotate the drone towards the target rotation
            droneController.transform.rotation = Quaternion.RotateTowards(droneController.transform.rotation, targetRotationForDroneTransform, rotationSpeed * Time.fixedDeltaTime);

            // Calculate the signed angle between the drone's current effective front and the desired horizontal direction after rotation
            float currentYawAngleToTarget = Vector3.SignedAngle(droneForwardForYaw, horizontalDirectionToTarget, Vector3.up);
            bool isAlignedHorizontally = Mathf.Abs(currentYawAngleToTarget) < yawAlignmentThreshold;

            // --- Positional Movement Control ---
            // Only move towards the target if aligned, or if very close
            if (isAlignedHorizontally || distanceToTarget <= waypointReachedThreshold)
            {
                droneController.transform.position = Vector3.MoveTowards(currentPosition, targetWaypoint.position, speed * Time.fixedDeltaTime);
            }

            // Check if the drone has reached the current waypoint AFTER movement
            if (Vector3.Distance(droneController.transform.position, targetWaypoint.position) < waypointReachedThreshold)
            {
                Debug.Log($"Waypoint {currentWaypointIndex} reached. Moving to next.");
                currentWaypointIndex++; // Move to the next waypoint in the path

                // If all waypoints in the current path are visited, complete the autopilot sequence
                if (currentWaypointIndex >= dronePaths[currentPathIndex].Waypoints.Count)
                {
                    Debug.Log($"End of path {currentPathIndex} reached. Autopilot sequence complete.");
                    KillAutoPilot(); // Stop autopilot
                    yield break; // Exit coroutine
                }
            }

            // Wait for the next physics frame before re-evaluating
            yield return null; // Use null for end of frame, FixedUpdate() is only if using Rigidbody.AddForce
        }

        // If the loop exits (e.g., autopilot was killed prematurely or path finished), ensure autopilot is fully off
        KillAutoPilot();
    }

    /// <summary>
    /// This is a separate routine for LandingBaseWaypoint. It should also incorporate the MoveTowards logic.
    /// </summary>
    private IEnumerator LandingAutoPilotFlightRoutine()
    {
        // Continue as long as autopilot is active and there are waypoints left in the current path
        while (isAutoPilotActive && currentPathIndex >= 0 && currentWaypointIndex < LandingBaseWaypoint[currentPathIndex].Waypoints.Count)
        {
            // Get the current target waypoint
            Transform targetWaypoint = LandingBaseWaypoint[currentPathIndex].Waypoints[currentWaypointIndex];
            if (targetWaypoint == null)
            {
                Debug.LogError($"Waypoint at path index {currentPathIndex}, waypoint index {currentWaypointIndex} is null. Aborting autopilot.");
                KillAutoPilot(); // Abort if a waypoint is unexpectedly null
                yield break; // Exit the coroutine
            }

            Vector3 currentPosition = droneController.transform.position; // Get current position
            Vector3 directionToTarget = targetWaypoint.position - currentPosition;
            float distanceToTarget = directionToTarget.magnitude;

            // --- Rotation Control (Yaw-First Alignment) ---

            // Get the drone's base forward direction, either from FrontOfTheDrone or the main transform.
            Vector3 baseForward = (FrontOfTheDrone != null) ? FrontOfTheDrone.forward : droneController.transform.forward;

            // Apply the rotational offset to the base forward vector directly.
            Vector3 droneForwardForYaw = Quaternion.Euler(forwardAxisRotationOffset) * baseForward;

            // Project the result onto the horizontal plane for yaw-only comparison.
            droneForwardForYaw.y = 0;
            if (droneForwardForYaw.magnitude < 0.01f)
            {
                droneForwardForYaw = Vector3.forward;
            }
            droneForwardForYaw.Normalize();

            // Calculate target horizontal direction for the drone to look at
            Vector3 horizontalDirectionToTarget = new Vector3(directionToTarget.x, 0, directionToTarget.z);
            if (horizontalDirectionToTarget.magnitude < 0.01f)
            {
                horizontalDirectionToTarget = droneForwardForYaw;
            }
            horizontalDirectionToTarget.Normalize();

            // The target rotation is based on where the drone's *effective* forward should point.
            Quaternion targetRotationForDroneTransform = Quaternion.LookRotation(horizontalDirectionToTarget, Vector3.up);
            targetRotationForDroneTransform *= Quaternion.Inverse(Quaternion.Euler(forwardAxisRotationOffset));

            // Rotate the drone towards the target rotation
            droneController.transform.rotation = Quaternion.RotateTowards(droneController.transform.rotation, targetRotationForDroneTransform, rotationSpeed * Time.fixedDeltaTime);

            // Calculate the signed angle between the drone's current effective front and the desired horizontal direction after rotation
            float currentYawAngleToTarget = Vector3.SignedAngle(droneForwardForYaw, horizontalDirectionToTarget, Vector3.up);
            bool isAlignedHorizontally = Mathf.Abs(currentYawAngleToTarget) < yawAlignmentThreshold;

            // --- Positional Movement Control ---
            // Only move towards the target if aligned, or if very close
            if (isAlignedHorizontally || distanceToTarget <= waypointReachedThreshold)
            {
                droneController.transform.position = Vector3.MoveTowards(currentPosition, targetWaypoint.position, speed * Time.fixedDeltaTime);
            }

            // Check if the drone has reached the current waypoint AFTER movement
            if (Vector3.Distance(droneController.transform.position, targetWaypoint.position) < waypointReachedThreshold)
            {
                Debug.Log($"Waypoint {currentWaypointIndex} reached. Moving to next.");
                currentWaypointIndex++; // Move to the next waypoint in the path

                // If all waypoints in the current path are visited, complete the autopilot sequence
                if (currentWaypointIndex >= LandingBaseWaypoint[currentPathIndex].Waypoints.Count)
                {
                    Debug.Log($"End of path {currentPathIndex} reached. Autopilot sequence complete.");
                    KillAutoPilot(); // Stop autopilot
                    yield break; // Exit coroutine
                }
            }

            // Wait for the next physics frame before re-evaluating
            yield return null;
        }

        // If the loop exits (e.g., autopilot was killed prematurely or path finished), ensure autopilot is fully off
        KillAutoPilot();
    }

    /// <summary>
    /// Draws a Gizmo in the Scene view to visualize the drone's effective forward direction.
    /// This helps in setting up the 'FrontOfTheDrone' Transform and 'forwardAxisRotationOffset'.
    /// </summary>
    void OnDrawGizmos()
    {
        if (droneController == null)
        {
            droneController = GetComponent<DroneController>();
            if (droneController == null) return;
        }

        Gizmos.color = Color.blue; // Set gizmo color to blue for visibility

        // Determine the base forward vector for Gizmo visualization
        Vector3 baseGizmoForward = (FrontOfTheDrone != null) ? FrontOfTheDrone.forward : droneController.transform.forward;

        // Apply the rotational offset to the base gizmo forward vector
        // This effectively rotates the drone's 'visual front' for the gizmo.
        Vector3 effectiveGizmoForward = Quaternion.Euler(forwardAxisRotationOffset) * baseGizmoForward;

        effectiveGizmoForward.y = 0; // Project onto the horizontal plane for yaw consideration
        if (effectiveGizmoForward.magnitude < 0.01f) // Fallback if nearly zero after projection
        {
            effectiveGizmoForward = Vector3.forward;
        }
        effectiveGizmoForward.Normalize();

        // Calculate the origin point for the gizmo ray, applying the positional offset in local space
        Vector3 gizmoOrigin = droneController.transform.position + droneController.transform.TransformDirection(forwardGizmoOffset);

        // Draw a ray representing the drone's effective forward direction
        Gizmos.DrawRay(gizmoOrigin, effectiveGizmoForward * 2f); // Draw a ray 2 units long
    }
}
