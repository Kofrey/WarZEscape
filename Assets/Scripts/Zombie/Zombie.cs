using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private HealthAndEnergy _healthAndEnergy;
    public Pathfinding pathfinding;
    private static readonly int WhatsWithParam = Animator.StringToHash("WhatsWith");
    private Animator animatorComponent;
    private SphereCollider _sphereCollider;

    public AudioSource RunSound;
    public AudioSource IdleSound;
    public AudioSource AttackSound;
    public PlayerControls target;
    public bool zombRun;

    private void Start()
    {
        animatorComponent = GetComponent<Animator>();
        _sphereCollider = GetComponent<SphereCollider>();
        IdleSound.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pathfinding.GetComponent<Pathfinding>().enabled = true;
            IsRunning(true);
            _sphereCollider.radius += 12;
            pathfinding.TargetPoint = other.transform;
            _healthAndEnergy = other.GetComponent<HealthAndEnergy>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _sphereCollider.radius -= 12;
            pathfinding.TargetPoint = null;
            animatorComponent.SetInteger(WhatsWithParam, 0);
            IdleSound.Play();
        }
    }

    public void AttackPlayer()
    {
        float distance = Vector3.Distance(pathfinding.TargetPoint.position, transform.position);
        Vector3 dir = (pathfinding.TargetPoint.position - transform.position).normalized;
        float direction = Vector3.Dot(dir, transform.forward);
        if (direction > 0 && distance < 2.4f)
        {
            int damage = Random.Range(18, 32);
            _healthAndEnergy.DamageTaken(damage);
            animatorComponent.SetInteger(WhatsWithParam, 2);
            AttackSound.Play();
            zombRun = false;
        }
    }

    public void IsRunning(bool bezhit)
    {
        if (bezhit)
        {
            animatorComponent.SetInteger(WhatsWithParam, 1);
            RunSound.Play();
            zombRun = true;
        }
        else
        {
            animatorComponent.SetInteger(WhatsWithParam, 0);
            zombRun = false;
            RunSound.Stop();
        }
    }

}
