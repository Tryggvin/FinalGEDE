using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        print("YOYO");
        InventoryItem newItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (transform.childCount == 0)
        {
            print("wow");
            //InventoryItem currentItem = transform.GetChild(0).GetComponent<InventoryItem>();
            gameObject.transform.SetParent(newItem.parentAfterDrag);
            gameObject.transform.position = newItem.parentAfterDrag.position;

            newItem.transform.SetParent(transform);
            newItem.transform.position = transform.position;
        }
        else
        {
            print("nice1");
            newItem.parentAfterDrag = transform;
            newItem.transform.SetParent(transform);
            newItem.transform.position = transform.position;
        }
    }
}
