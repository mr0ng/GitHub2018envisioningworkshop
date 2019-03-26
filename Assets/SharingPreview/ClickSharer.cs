using Microsoft.MixedReality.Toolkit.Core.EventDatum.Input;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.InputSystem;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.InputSystem.Handlers;
using Microsoft.MixedReality.Toolkit.SDK.Input.Handlers;
//using Microsoft.MixedReality.Toolkit.InputSystem;
//using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSharer : BaseFocusHandler, IMixedRealityInputHandler, IMixedRealityPointerHandler, IMixedRealityFocusChangedHandler
{

    public int sharingID;
    public bool TimeOut = false;
    public bool isOriginal; //By default, this is false. We'll set this to true if our main script created this, to detect of this is an original ClickSharer.
    public string objectName; //name of the object, to keep track of clones

    public IMixedRealityPointer[] Pointers
    {
        get
        {
            throw new System.NotImplementedException();
        }
    }

    void OnEnable()
    {
        StartCoroutine(CheckIfOriginal());
    }

    IEnumerator CheckIfOriginal()
    {
        yield return new WaitForSeconds(5f); //Add a short delay before checking to see if this was a cloned object or originally created by the Sharing ID Generator

        if (gameObject.name != objectName)
        {
            sharingID = SeanSharingManager.Instance.gameObject.GetComponent<OngSharingIDgenerator>().sharingID++;
            objectName = gameObject.name;

        }
    }
	

    public void time()
    {
        TimeOut = true;
        StartCoroutine(timeout());
    }

    IEnumerator timeout()
    {
        yield return null;
        yield return null;
        yield return null;
        TimeOut = false;
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        if (!TimeOut)
        {
            SeanSharingManager.Instance.InputDown(sharingID);
        }
    }
    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        if (!TimeOut)
            SeanSharingManager.Instance.InputClicked(sharingID);
    }
	
    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        if (!TimeOut)
            SeanSharingManager.Instance.InputUp(sharingID);
    }

    override
    public void OnFocusEnter(FocusEventData eventData)
    {
        if (!TimeOut)
            SeanSharingManager.Instance.FocusEnter(sharingID);
    }
    override
    public void OnFocusExit(FocusEventData eventData)
    {
        if (!TimeOut)
            SeanSharingManager.Instance.FocusExit(sharingID);
    }
	
    public void OnInputPressed(InputEventData<float> eventData)
    {
    }

    public void OnPositionInputChanged(InputEventData<Vector2> eventData)
    {
    }

    public void OnInputUp(InputEventData eventData)
    {
    }

    public void OnInputDown(InputEventData eventData)
    {
    }
}
