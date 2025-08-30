using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

[RequireComponent(typeof(DynamicMoveProvider))]
public class FootstepLooper : MonoBehaviour
{
    public AudioSource footstepSource;  
    public float minSpeed = 0.1f;        

    private DynamicMoveProvider moveProvider;

    void Start()
    {
        moveProvider = GetComponent<DynamicMoveProvider>();
    }

    void Update()
    {
        Vector2 input = moveProvider.leftHandMoveAction.action.ReadValue<Vector2>();

        bool isMoving = input.magnitude > minSpeed;

        if (isMoving && !footstepSource.isPlaying)
        {
            footstepSource.Play();
        }
        else if (!isMoving && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }
}
