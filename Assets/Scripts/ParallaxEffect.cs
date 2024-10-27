using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    // Starting Position for the parallax game object
    UnityEngine.Vector2 startingPosition;

    // Start Z value of the parallax game object
    float startingZ;


    // Distance that the camera  has moved from the starting position of the parallax object
    UnityEngine.Vector2 camMoveSinceStart => (UnityEngine.Vector2) cam.transform.position - startingPosition;

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float ClippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    // the further the object from the player the faster the parallax effect object will move. drag its Z value closer to the target to make it move slower
    float parallaxFactor => MathF.Abs(zDistanceFromTarget) / ClippingPlane;



    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }



    // Update is called once per frame
    void Update()
    {
        UnityEngine.Vector2 newPostion = startingPosition + camMoveSinceStart * parallaxFactor;

        transform.position = new UnityEngine.Vector3(newPostion.x, newPostion.y, startingZ);
    }
}
