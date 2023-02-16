using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;

/// <summary>
/// An AR event for surface detection/lose.
/// </summary>
public class ARPlaneEvent : AREvent
{
	public ARPlanesChangedEventArgs planesData;
	public Action<ARPlanesChangedEventArgs> evt;
	public override void Callback()
	{
		base.Callback();
		evt?.Invoke(planesData);
	}
}
