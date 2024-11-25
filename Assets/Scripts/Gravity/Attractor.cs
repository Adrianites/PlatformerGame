using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public LayerMask AttractionLayer;
    public float GravityForce = 10;
    [SerializeField] private float Radius = 10;
    [SerializeField] public bool Pulling = false;
    public List<Collider2D> AttractedObjects = new List<Collider2D>();
    [HideInInspector] public Transform attractorTransform;

    void Awake()
    {
        attractorTransform = GetComponent<Transform>();
    }

    void Update()
    {
        SetAttractedObjects();
    }

    void FixedUpdate()
    {
        AttractObjects();
    }

    void SetAttractedObjects()
    {
        AttractedObjects = Physics2D.OverlapCircleAll(attractorTransform.position, Radius, AttractionLayer).ToList();
    }

    void AttractObjects()
    {
        for (int i = 0; i < AttractedObjects.Count; i++)
        {
            AttractedObjects[i].GetComponent<Attractable>().Attract(this);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}