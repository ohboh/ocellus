using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    // variable to store the number of collectibles collected
    public int collectiblesCollected = 0;
    [SerializeField] SpriteRenderer pizzaCounter;
    // sprites to change "pizzaCounter" to;
    [SerializeField] private Sprite[] pizzaSprites;

    //AudioManager
    [SerializeField] AudioManager audioManager;

    private void OnTriggerEnter(Collider other)
    {
        // check if the collider is tagged "collectible"
        if (other.gameObject.tag == "collectible")
        {
            // increase the collectibles collected count
            collectiblesCollected++;
            // change the pizza counter sprite
            pizzaCounter.sprite = pizzaSprites[collectiblesCollected];
            // play "thankies"
            audioManager.Play("thankies");
            // destroy the collectible game object
            Destroy(other.gameObject);
        }
    }
}