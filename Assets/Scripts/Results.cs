﻿#if !UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Results : MonoBehaviour {

    public static Results instance;

    [HideInInspector]
    public string azureResponseCode;

    [HideInInspector]
    public string translationResult;

    [HideInInspector]
    public string dictationResult;

    [HideInInspector]
    public string micStatus;

    public TextMesh microphoneStatusText;

    public TextMesh azureResponseText;

    public TextMesh dictationText;

    public TextMesh translationResultText;

    private void Awake()
    {
        // Set this class to behave similar to singleton 
        instance = this;
    }

    /// <summary>
    /// Stores the Azure response value in the static instance of Result class.
    /// </summary>
    public void SetAzureResponse(string result)
    {
        azureResponseCode = result;
        azureResponseText.text = azureResponseCode;
    }

    /// <summary>
    /// Stores the translated result from dictation in the static instance of Result class. 
    /// </summary>
    public void SetDictationResult(string result)
    {
        dictationResult = result;
        dictationText.text = dictationResult;
    }

    /// <summary>
    /// Stores the translated result from Azure Service in the static instance of Result class. 
    /// </summary>
    public void SetTranslatedResult(string result)
    {
        translationResult = result;
        translationResultText.text = translationResult;
    }

    /// <summary>
    /// Stores the status of the Microphone in the static instance of Result class. 
    /// </summary>
    public void SetMicrophoneStatus(string result)
    {
        micStatus = result;
        microphoneStatusText.text = micStatus;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
#endif