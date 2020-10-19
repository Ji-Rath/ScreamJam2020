using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;// Required when using Event data.

public class TextColorHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler// required interface when using the OnPointerExit method.// required interface when using the OnPointerEnter method.
{
	//private Text text;
	public Color basicColor = Color.green;
	public Color hoverColor = Color.red;
	public TextMeshProUGUI text;

	void Start()
	{
		text.color = basicColor;
	}

	//Do this when the cursor enters the rect area of this selectable UI object.
	public void OnPointerEnter(PointerEventData eventData)
	{
		text.color = hoverColor;
	}

	//Do this when the cursor exits the rect area of this selectable UI object.
	public void OnPointerExit(PointerEventData eventData)
	{
		text.color = basicColor;
	}
}
