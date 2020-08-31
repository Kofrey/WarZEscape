using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class ZombieManager : MonoBehaviour
{
    [SerializeField] private HealthAndEnergy _healthAndEnergy;
    [SerializeField] public List<ZombiePlace> _zombiePlace;
    private int numberOfPlacers;

    [SerializeField] private SphereCollider _sCollidForPlaceActivate;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private List<Zombie> _zombieTemplates;
    //private Zombie _zombie;
    //private static float Timer;
    //public event UnityAction<Zombie> ZombieReadyForCollection;
    

    private void DispenceZombie(int numberOfPlacer)
    {
        int countOfZombs = Random.Range(3, 4);
        for (int i = 0; i < countOfZombs; i++) 
        {
            int randomNumber = Random.Range(0, _zombieTemplates.Count);
            Zombie randomZombie = _zombieTemplates[randomNumber];         
            _zombiePlace[numberOfPlacer].SetZombie(randomZombie);
        }
        
    }

    void Start()
    {
        numberOfPlacers = 0;
        _zombiePlace = new List<ZombiePlace>();
    }

   
    void Update()
    {
        //Timer = _healthAndEnergy.TimeM;
        
        if (_zombiePlace.Count > numberOfPlacers)
        {
            DispenceZombie(numberOfPlacers);
            numberOfPlacers++;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ZombPlacer")
        {
            ZombiePlace zombPlace = other.GetComponent<ZombiePlace>();
            if (!_zombiePlace.Contains(zombPlace))
            {
                _zombiePlace.Add(zombPlace);
            }
            
        }
    }
}
