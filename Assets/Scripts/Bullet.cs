using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{   
    public Transform SpawnPoint1;
    public Transform SpawnPoint2;
    public GameObject projectilePrefab;

    public void BulletProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, SpawnPoint1.position, SpawnPoint1.rotation);
        Vector3 origScale = projectile.transform.localScale;

        projectile.transform.localScale = new Vector3(
            origScale.x * transform.localScale.x > 0 ? 1 : -1, 
            origScale.y,
            origScale.z
        );
    }

}
