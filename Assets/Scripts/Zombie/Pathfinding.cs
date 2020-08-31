using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    //public List<Transform> points = new List<Transform>();
    public Zombie zombie;
    //public Transform[] points;
    public Transform TargetPoint;
    private NavMeshAgent nav;
    private int destPoint;
    private int kadrihz;
    private int promezhutok;
    private int attackCooldown;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        zombie = GetComponent<Zombie>();
        attackCooldown = 4;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        kadrihz++;
        if (nav.remainingDistance < 17f)
        { 
            if (kadrihz >= promezhutok)  //!nav.pathPending && nav.remainingDistance < 0.5f ||
            {
                GoToNextPoint();
                if (!zombie.zombRun) { zombie.IsRunning(true); }
                promezhutok = kadrihz + 15;
                attackCooldown--;
                if (TargetPoint != null & nav.remainingDistance > 0.6f & attackCooldown <= 0 & nav.remainingDistance < 2.3f)
                { 
                    zombie.AttackPlayer();
                    attackCooldown = 4;
                }
            }
        }
        if (kadrihz >= 20000) //Обнуляем счётчик
        {
            kadrihz = 0;
            promezhutok = 20;
            attackCooldown = 0;
        }
    }
    void GoToNextPoint()
    {
        if (TargetPoint == null)
        {
            return;
        }
        nav.destination = TargetPoint.position;
        //nav.destination = points[destPoint].position;
        //destPoint = (destPoint + 1) % points.Length;
    }
}
