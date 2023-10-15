using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryController : MonoBehaviour
{
    private DeliverySpawner spawner;
    [SerializeField] private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(DeliverySpawner spawnerReference) {
        spawner = spawnerReference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
