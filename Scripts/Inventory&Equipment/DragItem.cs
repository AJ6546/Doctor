using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
     where T : class
{
    Vector3 startPosition;
    Transform originalParent;
    IDragSource<T> source;
    Canvas parentCanvas;
    void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        source = GetComponentInParent<IDragSource<T>>();
    }
    // Called when item is clicked on and drag begins
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position; // Setting start position to slot position
        originalParent = transform.parent; // Setting original parent as slot
        GetComponent<CanvasGroup>().blocksRaycasts = false; // Not block rays
        transform.SetParent(parentCanvas.transform, true); // Setting parent to canvas
    }
    // Called when item is being dragged
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // setting the position of slot item to mousepoint position
    }

    // Called when drag ends
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPosition; // setting position to slot position
        GetComponent<CanvasGroup>().blocksRaycasts = true; // After drag is complete we should block rays, so that item stays in the slot
        transform.SetParent(originalParent, true); // setting the parent as slot
        IDragDestination<T> container;
        
        // If pointer is outside canvas we snap item to slot it was taken from
        if (!IsPointerOverUIObject())
        {
            container = parentCanvas.GetComponent<IDragDestination<T>>();
        }
        // Else we search if there isa slot at position where drag ended
        else
        {
            container = GetContainer(eventData);
        }
        // Dropping item into container
        if (container != null)
        {
            DropItemIntoContainer(container);
        }
    }
    private void DropItemIntoContainer(IDragDestination<T> destination)
    {
        // if source and destination are the same return
        if (object.ReferenceEquals(destination, source)) return;
        // setting source and destination containers
        var destinationContainer = destination as IDragContainer<T>;
        var sourceContainer = source as IDragContainer<T>;
        
        if (destinationContainer == null || sourceContainer == null || destinationContainer.GetItem() == null ||
            object.ReferenceEquals(destinationContainer.GetItem(), sourceContainer.GetItem()))
        {
            // If destination container is empty attempt a simple transfer
            AttemptSimpleTransfer(destination);
            return;
        }
        // If destination container already has an item attempt swap
        AttemptSwap(destinationContainer, sourceContainer);
    }
    void AttemptSwap(IDragContainer<T> destination, IDragContainer<T> source)
    {
        // Setting source and destination items and numbers
        var removedSourceNumber = source.GetNumber(); 
        var removedSourceItem = source.GetItem();
        var removedDestinationNumber = destination.GetNumber();
        var removedDestinationItem = destination.GetItem();

        // Removing items from source and destination
        source.RemoveItem(removedSourceNumber);
        destination.RemoveItem(removedDestinationNumber);

        if (removedDestinationNumber > 0)
        {
            // Adding item to source
            source.AddItems(removedDestinationItem, removedDestinationNumber);
        }
        if (removedSourceNumber > 0)
        {
            // Adding item to destination
            destination.AddItems(removedSourceItem, removedSourceNumber);
        }
    }

    // Returns container if there is a container at position where drag ended, else returns null
    IDragDestination<T> GetContainer(PointerEventData eventData)
    {
        if (eventData.pointerEnter)
        {
            var container = eventData.pointerEnter.GetComponentInParent<IDragDestination<T>>();
            return container;
        }
        return null;
    }


    bool AttemptSimpleTransfer(IDragDestination<T> destination)
    {
        var draggingItem = source.GetItem(); // setting item to transfer
        var draggingNumber = source.GetNumber(); // setting number of items to transfer
        var acceptable = destination.MaxAcceptable(draggingItem); // acceptable number of items in new slot
        var toTransfer = Mathf.Min(acceptable, draggingNumber);
        if (toTransfer > 0)
        {
            source.RemoveItem(toTransfer); // removing item from source container
            destination.AddItems(draggingItem, toTransfer); // adding item to destination container
            return false;
        }
        return true;
    }

    // returns true if the mouse pointer ray falls on a ui element, else returns false
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
