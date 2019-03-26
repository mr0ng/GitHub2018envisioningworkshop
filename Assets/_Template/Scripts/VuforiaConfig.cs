﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VuforiaConfig : MonoBehaviour
{
	#pragma warning disable 414
	[SerializeField] bool useVuforiaInEditor = false;
	#pragma warning restore 414

	void Awake ()
	{
		#if WINDOWS_UWP
		VuforiaConfiguration.Instance.DigitalEyewear.EyewearType             = DigitalEyewearARController.EyewearType.OpticalSeeThrough;
		VuforiaConfiguration.Instance.DigitalEyewear.SeeThroughConfiguration = DigitalEyewearARController.SeeThroughConfiguration.HoloLens;
		VuforiaConfiguration.Instance.DeviceTracker .AutoInitAndStartTracker = false;
		#else
		VuforiaConfiguration.Instance.DigitalEyewear.EyewearType             = DigitalEyewearARController.EyewearType.None;
		VuforiaConfiguration.Instance.DigitalEyewear.SeeThroughConfiguration = DigitalEyewearARController.SeeThroughConfiguration.Vuforia;
		VuforiaConfiguration.Instance.DeviceTracker .TrackingMode            = DeviceTracker.TRACKING_MODE.POSITIONAL;
		VuforiaConfiguration.Instance.DeviceTracker .AutoInitAndStartTracker = true;
		#endif

		#if UNITY_EDITOR
		if (!useVuforiaInEditor)
		{
			//VuforiaConfiguration.Instance.Vuforia.DelayedInitialization = true;
		}
		#endif
	}
}
