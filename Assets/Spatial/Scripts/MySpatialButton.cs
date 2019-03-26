using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Core.EventDatum.Input;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.InputSystem.Handlers;



public class MySpatialButton : MonoBehaviour, IMixedRealityPointerHandler
{

    int _buttonState = 0;
    public int ButtonState
    {
        get {
            return _buttonState;
        }
        set
        {
            _buttonState = value;
            UpdateButtonState((MySpatialButtonState)_buttonState);
        }
    }


    [SerializeField] private UnityEngine.UI.RawImage frontRawImage;
    [SerializeField] private Texture micIcon;
    [SerializeField] private Texture stopIcon;

    public void UpdateButtonState(MySpatialButtonState newState)
    {
        switch (newState)
        {
            case MySpatialButtonState.Closed:
                this.transform.localScale = Vector3.zero;
                break;
            case MySpatialButtonState.RecordFront:
                this.transform.localScale = Vector3.one;
                this.transform.localRotation = Quaternion.identity;
                frontRawImage.texture = micIcon;
                break;
            case MySpatialButtonState.StopRecordFront:
                this.transform.localScale = Vector3.one;
                this.transform.localRotation = Quaternion.identity;
                frontRawImage.texture = stopIcon;
                break;
            case MySpatialButtonState.CloseIconFront:
                this.transform.localScale = Vector3.one;
                this.transform.localRotation = Quaternion.Euler(0, 180, 0);
                break;
            default:
                break;
        }
    }

    [SerializeField] private GameObject recordingPanel;
    bool recordingStarted = false;
    private void StartRecording()
    {
        recordingStarted = true;
        recordingPanel.transform.localScale = Vector3.one * 0.5f;

#if !UNITY_IOS
        MicrophoneManager.instance.StartCapturingAudio();
#endif
    }

    private void StopRecording()
    {
        recordingStarted = false;
        recordingPanel.transform.localScale = Vector3.zero;

        #if !UNITY_IOS
        MicrophoneManager.instance.StopCapturingAudio();
#endif
    }

    [SerializeField] private UnityEngine.TextMesh originalTextMesh;
    [SerializeField] private UnityEngine.TextMesh translationTextMesh;
    private void Update()
    {
        if (recordingStarted && originalTextMesh.text != "")
        {
            Debug.Log("Updating text");
            model.AnnotationText = originalTextMesh.text;
            model.TranslationText = translationTextMesh.text;
        }
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
    }

    [SerializeField] private ModelPiece model;
    //public TextMesh debugtextMesh;
    float prevClickTime=0;
    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        var currentTime = Time.time;

        //Debug.Log("Time since last click is " + (currentTime - prevClickTime));
        //Bypass double clicks.
        if (currentTime - prevClickTime < 0.5f) return;
        prevClickTime = currentTime;


        eventData.Use();
        //textMesh.text += "CLicked !!!! \n";
        //Debug.Log("Number of clicks "+ eventData.Count);
        switch ((MySpatialButtonState)_buttonState)
        {
            case MySpatialButtonState.Closed:
                break;
            case MySpatialButtonState.RecordFront:
                StartRecording();
                Debug.Log("Recording started.");
                break;
            case MySpatialButtonState.StopRecordFront:
                StopRecording();
                Debug.Log("Recording finished.");
                model.ModelState = (int)ModelStateEnum.Annotated;
                break;
            case MySpatialButtonState.CloseIconFront:
                model.CloseModel();
                break;
            default:
                break;
        }
        int newState = ButtonState + 1;
        if (newState > 3)
        {
            newState = 0;
        }
        ButtonState = newState;

    }
}


