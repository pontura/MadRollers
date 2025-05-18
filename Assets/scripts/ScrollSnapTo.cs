using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSnapTo : MonoBehaviour
{
    public states state;
    public enum states
    {
        STOPPED,
        DRAGGING,
        WAITING_TO_SNAP,
        SNAPPING
    }
    float smoothToSnap = 90;
    ScrollRect scrollRect;
    float buttonWidth = 107;
    GameObject container;
    int totalItems;
    public int id;
    int totalItemsToCenter = 1;
    float detinationX = 0;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        container = GetComponent<ScrollRect>().content.gameObject;
    }
   public void Init(int id)
    {
        this.id = id;
        state = states.SNAPPING;     
        totalItems = container.GetComponentsInChildren<Button>().Length;
    }
    public void StartDrag()
    {
        scrollRect.inertia = true;
        state = states.DRAGGING;
    }
    public void StopDrag()
    {       
        state = states.WAITING_TO_SNAP;
    }
    void Update()
    {
        if (state == states.STOPPED) return;
        if (state == states.DRAGGING)  return;

        if (state == states.WAITING_TO_SNAP)
        {
            if (scrollRect.velocity.x < smoothToSnap && scrollRect.velocity.x > -smoothToSnap)
            {
                float _x = container.transform.localPosition.x;
                id = (int)Mathf.Round((container.transform.localPosition.x - buttonWidth) / buttonWidth) * -1;
                state = states.SNAPPING;
                scrollRect.inertia = false;
            }
        }
        if(state == states.SNAPPING)
            Snap();
    }
    void Snap()
    {
        int itemID = id - totalItemsToCenter;
        if (itemID < 0)
            itemID = 0;
        else if (itemID >= totalItems - totalItemsToCenter - 2)
            itemID = totalItems - totalItemsToCenter - 2;

        detinationX = ((float)itemID * buttonWidth) * -1;
        container.transform.localPosition = Vector2.Lerp(container.transform.localPosition, new Vector2(detinationX, container.transform.localPosition.y), 0.1f);
    }
}
