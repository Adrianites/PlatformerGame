using UnityEngine;
using UnityEngine.UIElements;

public class Attractable : MonoBehaviour
{
    [SerializeField] private bool rotateToCentre = true;
    [SerializeField] private Attractor currentAttractor;
    [SerializeField] private float gravityStrength = 100;

    Transform _transform;
    Collider2D _collider;
    Rigidbody2D rb;

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
                return;
            }
            if (rotateToCentre) RotateToCentre();
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1;
        }
    }

    public void Attract(Attractor attractorObj)
    {
        Vector2 attractionDir = ((Vector2)attractorObj.attractorTransform.position - rb.position).normalized;
        rb.AddForce(attractionDir * -attractorObj.Gravity * gravityStrength * Time.fixedDeltaTime);

        if (currentAttractor == null)
        {
            currentAttractor = attractorObj;
        }
    }

    void RotateToCentre()
    {
        Vector2 distanceVector = (Vector2)currentAttractor.attractorTransform.position - (Vector2)_transform.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        _transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
}
