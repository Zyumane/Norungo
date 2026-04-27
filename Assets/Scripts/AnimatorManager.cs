using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;
    float snappedVertical;
    float snappedHorizontal;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("AnimatorManager: no se encontró Animator en " + gameObject.name);
    }

    public void HandleAnimatorValues(float horizontalMovement, float verticalMovement)
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

        animator.SetFloat("Horizontal", snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat("Vertical", snappedVertical, 0.1f, Time.deltaTime);
    }
}