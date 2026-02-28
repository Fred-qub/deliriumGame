using UnityEngine;
using System.Collections;

public class MoveToTarget : MonoBehaviour
{
    [Header("Where to go?")]
    public Transform destinationMarker;

    [Header("Timing")]
    [Tooltip("How long to wait at the destination before returning.")]
    public float stayDuration = 1.0f; 
    public float moveSpeed = 5.0f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    
    //For now I'm making this script call the animation one to enable/disable walking -Fred
    [SerializeField]
    DoctorAnimationStateController doctorAnimationStateController;

    public void Activate()
    {
        if (destinationMarker == null)
        {
            Debug.LogWarning($"No destination set for {gameObject.name}!");
            return;
        }

        //Remember where object started
        startPosition = transform.position;
        startRotation = transform.rotation;

        //Start the sequence
        StopAllCoroutines();
        StartCoroutine(BoomerangRoutine());
    }

    private IEnumerator BoomerangRoutine()
    {
        //Go to the Marker
        while (Vector3.Distance(transform.position, destinationMarker.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                destinationMarker.position, 
                moveSpeed * Time.deltaTime
            );
            
            //start walking animation
            doctorAnimationStateController.startWalking();
            
            //Rotate to face destination
            transform.rotation = Quaternion.Lerp(transform.rotation, destinationMarker.rotation, moveSpeed * Time.deltaTime);
            
            yield return null;
        }
        
        //Snap to exact position
        transform.position = destinationMarker.position;
        
        //stop walking animation
        doctorAnimationStateController.stopWalking();

        //Wait
        Debug.Log($"{name} reached target. Waiting for {stayDuration} seconds...");
        yield return new WaitForSeconds(stayDuration);

        //Go back to start
        Debug.Log($"{name} is returning to start.");
        while (Vector3.Distance(transform.position, startPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                startPosition, 
                moveSpeed * Time.deltaTime
            );
            
            //start walking animation
            doctorAnimationStateController.startWalking();

            //Rotate back to original facing
            transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, moveSpeed * Time.deltaTime);

            yield return null;
        }

        // Snap to start
        transform.position = startPosition;
        transform.rotation = startRotation;
        Debug.Log($"{name} has returned to start.");
        
        //stop walking animation
        doctorAnimationStateController.stopWalking();
    }
}    