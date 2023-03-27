using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{


    [Header("UI")]
    public Image image;

    [HideInInspector] public Item item;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform dropTarget;

    public void InitiliseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }
    public void OnDrag(PointerEventData eventData)
    {
        print("dragging");
        //transform.position = Input.mousePosition;
        //Transform dropTarget = eventData.pointerEnter.transform;
        //transform.position = Input.mousePosition;
        dropTarget = eventData.pointerEnter?.transform;
        //Transform dropTarget = eventData.pointerEnter?.transform;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"OnEndDrag called. dropTarget: {dropTarget}");
        Debug.Log($"Event system current selected game object: {EventSystem.current.currentSelectedGameObject}");
        image.raycastTarget = true;
        print("end drag");
        //transform.SetParent(parentAfterDrag);
        if (dropTarget != null)
        {
            parentAfterDrag = dropTarget;
            transform.SetParent(parentAfterDrag);
        }
        else
        {
            if (parentAfterDrag != null) // add a null check here to prevent another possible error
            {
                transform.SetParent(parentAfterDrag);
                transform.localPosition = Vector3.zero;
            }
        }
    }

}