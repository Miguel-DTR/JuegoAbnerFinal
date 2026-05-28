using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;

	[SerializeField] private Transform swordTransform;
	

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction)) //disparo
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

		if(TryGetComponent<SwordAction>(
			out SwordAction swordAction))
		{
			swordAction.OnSwordActionStarted +=
				SwordActionStartedFunction;
				
			swordAction.OnSwordActionCompleted +=
				SwordActionCompletedFunction;
		}

        if(TryGetComponent<HealthAction>(out HealthAction healthAction)) //salud
        {
            healthAction.OnHealthActionStarted += HealthAction_OnCure;
        }

        if(TryGetComponent<ExplosionAction>(out ExplosionAction explosionAction)) //explosion
        {
            explosionAction.OnExplosionActionStarted += ExplosionAction_OnExplosion;
        }

        if(TryGetComponent<HealthSystem>(out HealthSystem healthSystem)) //muerte
        {
            healthSystem.OnDead += HealthAction_OnDeath;
            healthSystem.OnDamaged += HealthAction_OnDamage;
        }
    }
    //daño
    private void HealthAction_OnDamage(object sender, EventArgs e)
    {
        animator.SetTrigger("damage");
    }

    //muerte
    private void HealthAction_OnDeath(object sender, EventArgs e)
    {
        animator.SetTrigger("death");
    }

    //explosion
    private void ExplosionAction_OnExplosion(object sender, EventArgs e)
    {
        animator.SetTrigger("explosion");

    }
    //salud
    private void HealthAction_OnCure(object sender, EventArgs e)
    {
        animator.SetTrigger("curar");
    }
    //muerte
	
	private void SwordActionStartedFunction(
		object sender, EventArgs e)
	{
		EquipSword();
		animator.SetTrigger("swordSlash");
	}
	
	private void EquipSword()
	{
		swordTransform.gameObject.SetActive(true);
	}
	
	private void SwordActionCompletedFunction(
		object sender, EventArgs e)
	{
		// to do...
	}

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = 
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);

        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

}
