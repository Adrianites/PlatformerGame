using UnityEngine;
using UnityEngine.UIElements;

public class Attractable : MonoBehaviour
{
    [SerializeField] private bool rotateToCentre = true;
    [SerializeField] private Attractor currentAttractor;
    [SerializeField] private float gravityStrength = 100;
    [SerializeField] private float rotationSpeed = 5f; 

    Transform _transform;
    Collider2D _collider;
    Rigidbody2D rb;
    bool isResettingRotation = false;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (currentAttractor != null)
        {
            if (!currentAttractor.AttractedObjects.Contains(_collider))
            {
                currentAttractor = null;
                isResettingRotation = true;
                return;
            }
            if (rotateToCentre) RotateToCentre();
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1;
            if (isResettingRotation) ResetRotation();
        }
    }

    public void Attract(Attractor attractorObj)
    {
        Vector2 attractionDir = ((Vector2)attractorObj.attractorTransform.position - rb.position).normalized;
        float forceDirection = attractorObj.Pulling ? 1 : -1; 
        rb.AddForce(attractionDir * forceDirection * attractorObj.GravityForce * gravityStrength * Time.fixedDeltaTime);

        if (currentAttractor == null || Vector2.Distance(rb.position, attractorObj.attractorTransform.position) < Vector2.Distance(rb.position, currentAttractor.attractorTransform.position))
        {
            currentAttractor = attractorObj;
        }
    }

    void RotateToCentre()
    {
        Vector2 distanceVector = (Vector2)currentAttractor.attractorTransform.position - (Vector2)_transform.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        _transform.rotation = Quaternion.Lerp(_transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        Debug.Log("Sprite Angle: " + Quaternion.Angle(_transform.rotation, targetRotation));
    }

    void ResetRotation()
    {
        _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.identity, Time.deltaTime * rotationSpeed);
        if (Quaternion.Angle(_transform.rotation, Quaternion.identity) < 0.1f)
        {
            _transform.rotation = Quaternion.identity;
            isResettingRotation = false;
            Debug.Log("Resetting Rotation");
        }
    }

    public bool IsOnSide()
    {
        if (currentAttractor == null) return false;

        Vector2 distanceVector = (Vector2)currentAttractor.attractorTransform.position - (Vector2)_transform.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;

        // Check if the player is on the side (within a certain angle range)
        return angle > -45 && angle < 45;
    }
}
