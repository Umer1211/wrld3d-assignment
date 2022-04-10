using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReciveDamage(float damagerecieved)
    {
        health = health - damagerecieved;
    }

    void checkHealth()
    {
        if (health <= 0)
            Destroy(gameObject);
    }
}
