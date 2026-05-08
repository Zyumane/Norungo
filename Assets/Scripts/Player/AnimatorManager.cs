using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;
    PlayerLocomotionManager playerLocomotionManager;

    float snappedVertical;
    float snappedHorizontal;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("AnimatorManager: no se encontro Animator en " + gameObject.name);
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();

    }

    public void PlayAnimationWithoutRootMotion(string targetAnimation, bool isPerformingAction)
    {
        animator.SetBool("isPerformingAction", isPerformingAction);
        animator.applyRootMotion = false;
        animator.CrossFade(targetAnimation, 0.2f, 1);
    }

    public void HandleAnimatorValues(float horizontalMovement, float verticalMovement, bool isRunning)
    {
        if (horizontalMovement > 0)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalMovement < 0)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }

        if (verticalMovement > 0)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }

        if (isRunning && snappedVertical > 0) //We dont want to be able to run backwards or run whilst moving backwards
        {
            snappedVertical = 2;
        }

        animator.SetFloat("Horizontal", snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat("Vertical", snappedVertical, 0.1f, Time.deltaTime);
    }

    /// <summary>
    /// IMPORTANT: Este método debe permanecer activo aunque las animaciones no tengan Root Motion.
    /// Su presencia modifica el comportamiento interno de Unity respecto a la rotación del Rigidbody.
    /// Comentarlo provoca jitter y pérdida de control en la rotación del personaje y la cámara.
    /// </summary> 
    private void OnAnimatorMove()
    {
        if (Time.deltaTime == 0) return;

        Vector3 animatorDeltaPosition = animator.deltaPosition;
        animatorDeltaPosition.y = 0;
    
        Vector3 velocity = animatorDeltaPosition / Time.deltaTime;
        playerLocomotionManager.playerRigidbody.drag = 0;
        playerLocomotionManager.playerRigidbody.velocity = velocity;
        transform.rotation *= animator.deltaRotation;
    }
}