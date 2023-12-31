using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryController : MonoBehaviour
{
    private DeliverySpawnerScript spawner;
    
    [SerializeField] private GameObject player;
    
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    /*public void Initialize(DeliverySpawnerScript spawnerReference) {
        spawner = spawnerReference;
    }*/

    void Start() {
        spawner = FindFirstObjectByType<DeliverySpawnerScript>();
    }

    // Trying to make deliveries go away when players touch them.
    // Does not work.
    // Errors suggest DeliveryController and DeliverySpawner may not be properly linked.
    /*private void OnCollisionEnter2D(Collision2D other) {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null) {
            Die();        
        }
    }*/

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Die();
        } 
    }

    public void Die() {
        if (spawner != null) {
            spawner.DeliveryDied();
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    //void Update()
    //{
         
    //}
}
