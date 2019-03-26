using Microsoft.MixedReality.Toolkit.Core.EventDatum.Input;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.InputSystem.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationTapResponder : MonoBehaviour, IMixedRealityPointerHandler
{
    [SerializeField] private ModelPiece model;
    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        model.CloseModel();
        gameObject.transform.localScale = Vector3.zero;
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
