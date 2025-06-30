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
    
    GameObject container;
    int totalItems;
    [SerializeField] int id;
    [SerializeField] Scrollbar scrollBar;

    void Start()
    {
        container = GetComponent<ScrollRect>().content.gameObject;
    }
    public void Init(int id)
    {
        this.id = id;
        state = states.SNAPPING;     
        totalItems = container.GetComponentsInChildren<Button>().Length;
        scrollBar.value = (float)id / (float)totalItems;
    }
}
