using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine;
using System.ComponentModel;
using UnityEditor.Rendering;
using System.Linq;
using System;

public class closeInventory : MonoBehaviour
{

    //public GameObject parentPrefab; // The parent prefab that you want to add the child to
    //public GameObject childPrefab; // The child prefab that you want to add
    // Start is called before the first frame update
    private readonly List<InventoryItem> items = new List<InventoryItem>();
    public List<GameObject> itemsToSpawn;
    public GameObject obj;
    public Canvas canvas;
    private List<int> itemList;
    public int id;
    public void Start()
    {
        itemList = GameManager.instance.currentInventory.itemIds;
        foreach(int index in itemList)
        {
            var newItem = Instantiate(itemsToSpawn[index-1]).GetComponent<InventoryItem>();
            newItem.name = itemsToSpawn[index-1].name;
            items.Add(newItem);
            print(items.Count);

        }
        //print(items[0]);

        int count = 0;
        obj = GameObject.Find("mainInventoryGroup");
        obj.SetActive(false);
        canvas = GameObject.FindObjectOfType<Canvas>();
        // Loop through all of the objects in the Canvas

        foreach (Transform parent in canvas.transform)
        {
            // Loop through all of the child objects of the parent object
            foreach (Transform child in parent)
            {
                foreach (Transform child_child in child)
                {
                    if(count < itemList.Count)
                    {
                        // Check if the child object is the desired prefab
                        if (child_child.name.StartsWith("InventorySlot") || child_child.name.StartsWith("inventorySlot") || count == 3)
                        {

                            items[count].transform.SetParent(child_child);
                            items[count].transform.localScale = Vector3.one;


                            count++;


                        }
                    }
                    
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.instance.isInInventory = true;
            if (obj.activeSelf)
            {
                obj.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                GameManager.instance.isInInventory = false;
                obj.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }


        }
    }
}