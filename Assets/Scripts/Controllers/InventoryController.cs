using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {
    //Note: Current inventory implementation is temporary and subject to change.

    public int bobblesCollected=0;
    public float itemSearchRadius=5.0f;
    public KeyCode collectKey = KeyCode.E;

    public List<GameObject> itemsContained;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        List<GameObject> nearbyObjects = ItemCheck();
        if (nearbyObjects.Count>0)
        {
            Debug.Log("An item is nearby. Items collected: "+bobblesCollected);
            if (Input.GetKeyDown(collectKey))
            {
                for (int i = 0; i < nearbyObjects.Count; i++)
                {
                    AddItemToInventory(nearbyObjects[i]);
                }
            }
        }
        else
        {
            Debug.Log("An item is not nearby. Items collected: " + bobblesCollected);
        }
	}

    //Check to see if there are any Items nearby.
    //Return whether there are Items nearby
    List<GameObject> ItemCheck()
    {
        List<GameObject> tempItems =new List<GameObject>();
        //Get all items
        //Used presuming that there will be fewer items than nearby GameObjects in collision radius.
        //This might need refactoring later, depending on if there's a more efficient method I could use.
        GameObject[] itemsGlobal = GameObject.FindGameObjectsWithTag("Item");
        for (int i = 0; i < itemsGlobal.Length; i++)
        {
            Vector3 temp = itemsGlobal[i].gameObject.transform.position-gameObject.transform.position;
            float itemDistSqrd = temp.sqrMagnitude; //squared distance used instead of distance to avoid expensive sqrt calculations

            if (itemDistSqrd<Mathf.Pow(itemSearchRadius,2))
            {
                tempItems.Add(itemsGlobal[i]);
            }
        }
        return tempItems;
    }

    //Takes an item from the game world, moves it to the inventory and adds it to your inventory.
    void AddItemToInventory (GameObject item)
    {
        itemsContained.Add(item);
        item.gameObject.SetActive(false);
        bobblesCollected++;
    }

    //Takes an item at the given index in the inventory, 
    void RemoveItemFromInventory(int index)
    {
        
    }
}
