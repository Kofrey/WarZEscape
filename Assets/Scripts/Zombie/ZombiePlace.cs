using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePlace : MonoBehaviour
{
    [SerializeField] private Transform[] points;

    private Zombie _zombie;

    public void SetZombie(Zombie zombie)
    {
        int randomNumber = Random.Range(0, points.Length);
        _zombie = Instantiate(zombie, points[randomNumber].transform);
    }

    public void RemoveZombie(Zombie zombie)
    {
        Destroy(zombie);
    }
}
