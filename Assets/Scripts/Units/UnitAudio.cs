using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAudio : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip dañoAudio;
    [SerializeField] private AudioClip muerteAudio;
    [SerializeField] private AudioClip explosionAudio;
    [SerializeField] private AudioClip curarAudio;
    [SerializeField] private AudioClip espadaAudio;
    [SerializeField] private AudioClip caminarAudio;
    [SerializeField] private AudioClip disparoAudio;

	

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMovingAudio;
            moveAction.OnStopMoving += MoveAction_OnStopMovingAudio;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction)) //disparo
        {
            shootAction.OnShoot += ShootAction_OnShootAudio;
        }

		if(TryGetComponent<SwordAction>(
			out SwordAction swordAction))
		{
			swordAction.OnSwordActionStarted +=
				SwordActionStartedFunctionAudio;
				
			swordAction.OnSwordActionCompleted +=
				SwordActionCompletedFunctionAudio;
		}

        if(TryGetComponent<HealthAction>(out HealthAction healthAction)) //salud
        {
            healthAction.OnHealthActionStarted += HealthAction_OnCureAudio;
        }

        if(TryGetComponent<ExplosionAction>(out ExplosionAction explosionAction)) //explosion
        {
            explosionAction.OnExplosionActionStarted += ExplosionAction_OnExplosionAudio;
        }

        if(TryGetComponent<HealthSystem>(out HealthSystem healthSystem)) //muerte
        {
            healthSystem.OnDead += HealthAction_OnDeathAudio;
            healthSystem.OnDamaged += HealthAction_OnDamageAudio;
        }
    }
    //daño
    private void HealthAction_OnDamageAudio(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(dañoAudio);
    }

    //muerte
    private void HealthAction_OnDeathAudio(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(muerteAudio);
    }

    //explosion
    private void ExplosionAction_OnExplosionAudio(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(explosionAudio);

    }
    //salud
    private void HealthAction_OnCureAudio(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(curarAudio);
    }
    //muerte
	
	private void SwordActionStartedFunctionAudio(
		object sender, EventArgs e)
	{
		audioSource.PlayOneShot(espadaAudio);
	}
	
	private void SwordActionCompletedFunctionAudio(
		object sender, EventArgs e)
	{
		// to do...
	}

    private void MoveAction_OnStartMovingAudio(object sender, EventArgs e)
    {
        audioSource.clip = caminarAudio;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void MoveAction_OnStopMovingAudio(object sender, EventArgs e)
    {
        audioSource.Stop();

    }

    private void ShootAction_OnShootAudio(object sender, ShootAction.OnShootEventArgs e)
    {
        audioSource.PlayOneShot(disparoAudio);
    }

}
