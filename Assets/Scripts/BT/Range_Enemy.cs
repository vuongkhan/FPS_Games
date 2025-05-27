using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range_Enemy : EnemyBase
{
    [SerializeField] private Transform firePoint; 

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    public void RangeAOEAttack(GameObject projectileToShoot)
    {
        if (projectileToShoot == null)
        {
            return;
        }
        GameObject proj = Instantiate(projectileToShoot, firePoint.position, projectileToShoot.transform.rotation);
    }
    public void SpawnProjectileAtTarget(GameObject projectileToSpawn)
    {
        if (projectileToSpawn == null)
        {
            return;
        }

        if (Target == null)
        {
            return;
        }
        Vector3 spawnPosition = Target.transform.position;
        Quaternion spawnRotation = Quaternion.identity; 

        Instantiate(projectileToSpawn, spawnPosition, spawnRotation);
    }

}
