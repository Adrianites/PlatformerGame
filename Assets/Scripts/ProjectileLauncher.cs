using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform FireLeft, FireRight, FireLeftCrouch, FireRightCrouch;
    public GameObject projectilePrefab;
    public PlayerController player;

    public void FireProjectile()
    {
        if (player.IsCrouching)
        {
            if (player.IsFacingRight)
            {
                Instantiate(projectilePrefab, FireRightCrouch.position, projectilePrefab.transform.rotation);
            }
            else
            {
                Instantiate(projectilePrefab, FireLeftCrouch.position, projectilePrefab.transform.rotation);
            }
        }
        else
        {
            if (player.IsFacingRight)
            {
                Instantiate(projectilePrefab, FireRight.position, projectilePrefab.transform.rotation);
            }
            else
            {
                Instantiate(projectilePrefab, FireLeft.position, projectilePrefab.transform.rotation);
            }
        }
    }

}
