using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script includes a simple drop system for an unpublished game prototype.
public class DropScript : MonoBehaviour
{
    [SerializeField] int blockCount;
    public int remainingBlocks;
    public enum EntityType { SMALL, MEDIUM, LARGE}; //types of entities
    EntityType entType; // type of this building

    [SerializeField] private GameObject dropLocation;
    [SerializeField] GameObject smallDrop, mediumDrop, largeDrop;

    void Start()
    {
        remainingBlocks = blockCount;

        if(blockCount < 30)
        {
            entType = EntityType.SMALL;
        }
        if(blockCount > 30 && blockCount < 60)
        {
            entType = EntityType.MEDIUM;
        }
        else
        {
            entType = EntityType.LARGE;
        }

    }

    // Update is called once per frame
    public void BlockDestroyed()
    {
        remainingBlocks -= 1;
        if(remainingBlocks == 0)
        {
            Drop(entType);
        }
    }

    void Drop(BuildingType type)
    {
        if(type == EntityType.SMALL)
        {
            if(smallDrop)
            {
                Instantiate(smallDrop, dropLocation.transform.position, Quaternion.identity);
            }
        }
        if(type == EntityType.MEDIUM)
        {
            if(mediumDrop)
            {
                Instantiate(mediumDrop, dropLocation.transform.position, Quaternion.identity);
            }
        }
        if(type == EntityType.LARGE)
        {
            if(largeDrop)
            {
                Instantiate(largeDrop, dropLocation.transform.position, Quaternion.identity);
            }
        }
    }
}
