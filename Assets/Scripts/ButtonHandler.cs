using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IDeselectHandler, IPointerDownHandler
{
    private Selectable itemSelected;

    public void OnDeselect(BaseEventData eventData) //when I move the selection with keyboard
    {
        itemSelected.OnPointerExit(null);
    }

    public void OnPointerDown(PointerEventData eventData) //execute the button press on (press)
    {
        if (eventData.selectedObject.GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) //when you move the selection with mouse
    {
        itemSelected.Select();
    }

    // Start is called before the first frame update
    void Start()
    {
        itemSelected = GetComponent<Selectable>();
    }

}
