using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System
using TMPro;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour // Assuming this is the filename you are using
{
    public float LinearDrag, AngularDrag;


    [Header("Input Action Properties (Assign from your Input Actions Asset)")]
    public InputActionProperty leftJoystickActionProperty;   // Left Stick (Vector2)
    public InputActionProperty rightJoystickActionProperty;  // Right Stick (Vector2)

    [Header("Movement Speeds")]
    // throttleIncrementForce is removed as throttle is now directly controlled by joystick Y-axis
    public float pitchForce = 1.5f;
    public float rollForce = 1.5f;
    public float yawTorque = 2f;

    [Header("Input Settings")]
    public float joystickDeadzone = 0.2f; // Values below this threshold are ignored

    [Header("Stabilization (Auto-Leveling)")]
    public float stabilizationTorque = 1.5f;
    public bool enableAutoLeveling = true;

    [Header("Limits")]
    public float maxSpeed = 10f;
    public float maxAngularVelocity = 5f; // Radians/sec
    public float currentUpwardThrottle = 0f;
    public float maxUpwardThrottle = 20f;
    public float minEffectiveThrottleToHover = 9.81f; // Mass * Gravity (approx for hovering against gravity)

    private Rigidbody rb;

    // Input storage - now stores analog values from joysticks after deadzone and remapping
    public float pitchInput;    // Derived from Left Joystick Y
    public float rollInput;     // Derived from Left Joystick X
    public float yawInput;      // Derived from Right Joystick X
    public float throttleInput; // Derived from Right Joystick Y

    public bool IsAutoPiloting { get; private set; } = false;

    public TextMeshProUGUI Altitude, Speed;

    public Transform LastSafePosition;

    public bool UseScreenFade;


    public UnityEvent ScreenFadeIn, ScreenFadeOut;
    void Awake()
    {
      
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError(GetType().Name + " requires a Rigidbody component.");
            enabled = false;
            return;
        }

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.linearDamping = 1f;
        rb.angularDamping = 5f; // Increased angular drag can help with stability after changing thrust model

        LinearDrag = rb.linearDamping;
        AngularDrag = rb.angularDamping;
        rb.maxAngularVelocity = maxAngularVelocity;

        // Optional: Start with enough throttle to hover.
        // currentUpwardThrottle = minEffectiveThrottleToHover; // Uncomment if you want drone to start hovering
    }
    
    void OnEnable() 
    {
        // Enable input actions for both joysticks
        EnableInputAction(leftJoystickActionProperty, OnLeftJoystickPerformed, OnLeftJoystickCanceled);
        EnableInputAction(rightJoystickActionProperty, OnRightJoystickPerformed, OnRightJoystickCanceled);


        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError(GetType().Name + " requires a Rigidbody component.");
            enabled = false;
            return;
        }

        rb.useGravity = true;
        rb.linearDamping = 1f;
        rb.angularDamping = 5f; // Increased angular drag can help with stability after changing thrust model

        LinearDrag = rb.linearDamping;
        AngularDrag = rb.angularDamping;
        rb.maxAngularVelocity = maxAngularVelocity;

        rb.isKinematic = false;
        rb.useGravity = true;
    }

    void OnDisable()
    {
        // Disable input actions for both joysticks
        DisableInputAction(leftJoystickActionProperty, OnLeftJoystickPerformed, OnLeftJoystickCanceled);
        DisableInputAction(rightJoystickActionProperty, OnRightJoystickPerformed, OnRightJoystickCanceled);
    }

    /// <summary>
    /// Helper method to enable an input action and subscribe to its performed and canceled events.
    /// </summary>
    private void EnableInputAction(InputActionProperty property,
                                   System.Action<InputAction.CallbackContext> performedCallback,
                                   System.Action<InputAction.CallbackContext> canceledCallback)
    {
        if (property != null && property.action != null)
        {
            property.action.Enable();
            property.action.performed += performedCallback;
            property.action.canceled += canceledCallback;
        }
        else
        {
            // Debug.LogWarning($"Action Property for {property?.name ?? "Unnamed Action"} is not assigned or its action is null.", this);
        }
    }

    /// <summary>
    /// Helper method to disable an input action and unsubscribe from its performed and canceled events.
    /// </summary>
    private void DisableInputAction(InputActionProperty property,
                                    System.Action<InputAction.CallbackContext> performedCallback,
                                    System.Action<InputAction.CallbackContext> canceledCallback)
    {
        if (property != null && property.action != null)
        {
            property.action.performed -= performedCallback;
            property.action.canceled -= canceledCallback;
            property.action.Disable();
        }
    }

    /// <summary>
    /// Applies a deadzone to an input value and then remaps the value
    /// from the range [deadzone, 1] or [-1, -deadzone] to [0, 1] or [-1, 0] respectively.
    /// This provides smoother analog control by ignoring small inputs and then scaling the effective range.
    /// </summary>
    /// <param name="value">The raw input value (e.g., from a joystick axis, typically -1 to 1).</param>
    /// <param name="deadzone">The threshold below which the input is considered zero.</param>
    /// <returns>The deadzoned and remapped input value.</returns>
    private float ApplyDeadzoneAndRemap(float value, float deadzone)
    {
        if (Mathf.Abs(value) < deadzone)
        {
            return 0f; // Input is within the deadzone, return 0
        }
        // Remap the value:
        // If value is positive, map from [deadzone, 1] to [0, 1]
        // If value is negative, map from [-1, -deadzone] to [-1, 0]
        float sign = Mathf.Sign(value);
        return sign * (Mathf.Abs(value) - deadzone) / (1f - deadzone);
    }

    // --- Input Callbacks ---

    /// <summary>
    /// Called when the left joystick's input is performed (moved).
    /// Controls Pitch (Left Y) and Roll (Left X).
    /// </summary>
    private void OnLeftJoystickPerformed(InputAction.CallbackContext context)
    {
        if (!IsAutoPiloting) // <--- Add this check
        
        { 
            Vector2 leftStickValue = context.ReadValue<Vector2>();
            
             //= -ApplyDeadzoneAndRemap(leftStickValue.y, joystickDeadzone); // Left Y for Pitch (Forward/Backward)
            throttleInput = ApplyDeadzoneAndRemap(leftStickValue.y, joystickDeadzone); // Left Y for Pitch (Forward/Backward)

            yawInput = ApplyDeadzoneAndRemap(leftStickValue.x, joystickDeadzone);  // Left X for Roll (Strafing Left/Right)
             //= ApplyDeadzoneAndRemap(leftStickValue.x, joystickDeadzone);  // Left X for Roll (Strafing Left/Right)
    } }

    /// <summary>
    /// Called when the left joystick's input is canceled (released).
    /// Resets Pitch and Roll inputs to zero.
    /// </summary>
    private void OnLeftJoystickCanceled(InputAction.CallbackContext context)
    {
        
            pitchInput = 0f;
            rollInput = 0f;
        
    }

    /// <summary>
    /// Called when the right joystick's input is performed (moved).
    /// Controls Throttle (Right Y) and Yaw (Right X).
    /// </summary>
    private void OnRightJoystickPerformed(InputAction.CallbackContext context)
    {
        if (!IsAutoPiloting) // <--- Add this check

        {
            Vector2 rightStickValue = context.ReadValue<Vector2>();
            rollInput = -ApplyDeadzoneAndRemap(rightStickValue.y, joystickDeadzone); // Right Y for Throttle (Up/Down)
            pitchInput = ApplyDeadzoneAndRemap(rightStickValue.x, joystickDeadzone);     // Right X for Yaw (Rotate Left/Right)
        }
    }

    /// <summary>
    /// Called when the right joystick's input is canceled (released).
    /// Resets Throttle and Yaw inputs to zero.
    /// </summary>
    private void OnRightJoystickCanceled(InputAction.CallbackContext context)
    {
        throttleInput = 0f;
        yawInput = 0f;
    }

    // The Update method is no longer needed for throttle logic, as it's handled in FixedUpdate
    // based on continuous joystick input. You can remove this method entirely if no other
    // non-physics related updates are needed.
    void Update()
    {
        // This method is intentionally left empty as per the new control scheme.
        // All movement logic is now handled in FixedUpdate for physics consistency.
    }


    void FixedUpdate()
    {

        rb.angularDamping = AngularDrag;
        rb.linearDamping = LinearDrag;
        // Calculate currentUpwardThrottle based on the analog throttleInput from the Right Joystick Y-axis.
        // throttleInput ranges from -1 (full down) to 1 (full up) after deadzone and remapping.
        // We want:
        //   - If throttleInput is 0 (joystick centered), currentUpwardThrottle should be minEffectiveThrottleToHover.
        //   - If throttleInput is 1 (joystick full up), currentUpwardThrottle should be maxUpwardThrottle.
        //   - If throttleInput is -1 (joystick full down), currentUpwardThrottle should be 0 (or a very small minimum).

        if (throttleInput >= 0)
        {
            // When pushing joystick up (throttleInput from 0 to 1),
            // smoothly increase throttle from hover thrust to max thrust.
            currentUpwardThrottle = Mathf.Lerp(minEffectiveThrottleToHover, maxUpwardThrottle, throttleInput);
        }
        else // throttleInput is negative (from -1 to 0)
        {
            // When pushing joystick down (throttleInput from -1 to 0),
            // smoothly decrease throttle from hover thrust down to 0.
            // We remap throttleInput from [-1, 0] to [0, 1] for Lerp using (throttleInput + 1f).
            currentUpwardThrottle = Mathf.Lerp(0f, minEffectiveThrottleToHover, (throttleInput + 1f));
        }
        // Ensure the throttle stays within defined bounds.
        currentUpwardThrottle = Mathf.Clamp(currentUpwardThrottle, 0f, maxUpwardThrottle);

        HandleMovement();
        if (enableAutoLeveling)
        {
            HandleAutoLeveling();
        }
        LimitSpeed();
        
        Altitude.text = ((this.transform.position.y * 3.28084)).ToString("G3")+ "ft";
        Speed.text = (rb.linearVelocity.magnitude).ToString("G3");   
        
        
        string altitude = ((this.transform.position.y * 3.28084)).ToString("G3")+ "ft";
        string Speedtext = (rb.linearVelocity.magnitude).ToString();
    
    }
  
    /// <summary>
    /// Applies forces and torques to the Rigidbody based on current input values.
    /// </summary>
    void HandleMovement()
    {
        float effectiveThrottle = currentUpwardThrottle;

        // --- Main Thrust ---
        // Apply force in the drone's local upward direction.
        // This force will propel the drone vertically and also horizontally if it's tilted.
        rb.AddRelativeForce(Vector3.up * effectiveThrottle, ForceMode.Acceleration);

        // --- Rotational Control ---
        // Pitch: Applies torque around the drone's local right axis based on Left Joystick Y.
        rb.AddRelativeTorque(Vector3.right * pitchInput * pitchForce, ForceMode.Acceleration);

        // Roll: Applies torque around the drone's local forward axis based on Left Joystick X.
        // Note: -rollInput is used for conventional flight simulator roll (pushing left stick left rolls left).
        rb.AddRelativeTorque(Vector3.forward * -rollInput * rollForce, ForceMode.Acceleration);

        // Yaw: Applies torque around the drone's local upward axis based on Right Joystick X.
        rb.AddRelativeTorque(Vector3.up * yawInput * yawTorque, ForceMode.Acceleration);
    }

    /// <summary>
    /// Attempts to auto-level the drone by applying counter-torque if no pitch or roll input is detected.
    /// </summary>
    void HandleAutoLeveling()
    {
        // Only auto-level if there's no active pitch or roll input from the user.
        if (Mathf.Approximately(pitchInput, 0f) && Mathf.Approximately(rollInput, 0f))
        {
            // Calculate the torque needed to align the drone's 'up' vector with the world's 'up' vector.
            Vector3 torqueToLevel = Vector3.Cross(transform.up, Vector3.up);
            // Apply this torque in the drone's local space.
            rb.AddRelativeTorque(transform.InverseTransformDirection(torqueToLevel) * stabilizationTorque, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Limits the drone's linear velocity to prevent it from exceeding maxSpeed.
    /// </summary>
    void LimitSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
    public void SetAutoPilotActive(bool active)
    {
              IsAutoPiloting = active;
                if (active)
                {
                    // When autopilot is active, disable manual input actions
                    if (leftJoystickActionProperty != null && leftJoystickActionProperty.action != null)
                        leftJoystickActionProperty.action.Disable();
                    if (rightJoystickActionProperty != null && rightJoystickActionProperty.action != null)
                        rightJoystickActionProperty.action.Disable();
        
                    // Reset inputs to zero for autopilot to take over cleanly
                    pitchInput = 0f;
                    rollInput = 0f;
                    yawInput = 0f;
                    throttleInput = 0f;
                }
                else
                {
                    // When autopilot is deactivated, re-enable manual input actions
                    if (leftJoystickActionProperty != null && leftJoystickActionProperty.action != null)
                        leftJoystickActionProperty.action.Enable();
                    if (rightJoystickActionProperty != null && rightJoystickActionProperty.action != null)
                        rightJoystickActionProperty.action.Enable();
        
                    // Reset inputs to zero for manual control to take over cleanly
                    pitchInput = 0f;
                    rollInput = 0f;
                    yawInput = 0f;
                    throttleInput = 0f;
                }
            }


    public void InjectLastSafePos(Transform t)
    {
        LastSafePosition = t;
        IsAutoPiloting = true;
    }
    public void JumpToPosition(int index)
    {
        //FadeIn 
        rb.isKinematic = true;

        if (UseScreenFade)
        {
            StartCoroutine(DroneScreenfadeIn(index));
        }
        else
        {
            transform.position = LastSafePosition.position;

            GetComponent<AutoPilotController>().FlyToDestination(index);
        }

    }

    private System.Collections.IEnumerator DroneScreenfadeIn(int index)
    {

        ScreenFadeIn?.Invoke();
        yield return null;
        yield return new WaitForSeconds(1f);
        transform.position = LastSafePosition.position;

        GetComponent<AutoPilotController>().FlyToDestination(index);
        ScreenFadeOut?.Invoke();
        yield return new WaitForSeconds(1f);


    }
    }