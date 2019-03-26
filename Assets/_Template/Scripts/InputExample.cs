using Microsoft.MixedReality.Toolkit.Core.EventDatum.Input;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.InputSystem.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputExample : MonoBehaviour, IMixedRealityPointerHandler {
	public Color Color { get{return _color;} set{ _color = value; GetComponent<MeshRenderer>().material.color = _color; }}
	Color _color;

	public void OnPointerClicked(MixedRealityPointerEventData eventData){
		Color = Color.HSVToRGB(Random.value, 0.4f, 0.8f);
	}
	public void OnPointerDown   (MixedRealityPointerEventData eventData){

        Debug.Log("testing Pointer Down");

    }
	public void OnPointerUp     (MixedRealityPointerEventData eventData){
        Debug.Log("testing Pointer Up");
	}
}
