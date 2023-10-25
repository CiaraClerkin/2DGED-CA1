using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySpawnerScript : MonoBehaviour
{

    [SerializeField] private GameObject deliveryPrefab;
    [SerializeField] private float spawnRate = 10f;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private int maxDeliveries = 2;
    private int currentDeliveries = 0;
    DeliveryController deliveryController;

    void Start()
    {
        //Calls SpawnDelivery 2 seconds after game is run every 2 seconds; 
        InvokeRepeating("SpawnDelivery", spawnRate, spawnRate);
    }

    void SpawnDelivery() {
        if (currentDeliveries >= maxDeliveries) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject spawnedDelivery = Instantiate(deliveryPrefab, spawnPoint.position, Quaternion.identity);

        deliveryController = spawnedDelivery.GetComponent<DeliveryController>();
        deliveryController.Initialize(this);

        currentDeliveries++;
    }
    
    public void DeliveryDied() {
        currentDeliveries--;
    }
}
