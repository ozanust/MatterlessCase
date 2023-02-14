using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

/// <summary>
/// Manages and updates game view according to events from AR system.
/// </summary>
public class GameView : MonoBehaviour
{
	[SerializeField]
	private GameObject arObjPrototype;

	[Inject]
	private IARService arService;

	private List<ARObject> arObjectList = new List<ARObject>();

	private void Start()
	{
		AddListeners();
		arService.SetARObject(arObjPrototype);
	}

	/// <summary>
	/// Assigns listeners to AR service.
	/// </summary>
	private void AddListeners()
	{
		ARObjectEvent objEvent = new ARObjectEvent();
		objEvent.evt += OnAddARObjEvent;

		AREvent camEvent = new AREvent();
		camEvent.baseEvent += OnCameraMoveEvent;

		ARPlaneEvent planeEvent = new ARPlaneEvent();
		planeEvent.baseEvent += OnPlaneDetectedEvent;

		arService.AddAREventListener(AREventType.ARCameraEvent, camEvent);
		arService.AddAREventListener(AREventType.ARObjectEvent, objEvent);
		arService.AddAREventListener(AREventType.ARPlaneEvent, planeEvent);
	}

	/// <summary>
	/// Called on each frame from the Tick method of Zenject. Camera always moves in real-time.
	/// Updates the distance label on each object that are placed on the physical world.
	/// </summary>
	private void OnCameraMoveEvent()
	{
		foreach (ARObject aR in arObjectList)
		{
			aR.UpdateDistanceLabel(arService.GetDistance(aR));
		}
	}

	/// <summary>
	/// Called when a new object placed on an AR plane, basically to physical world.
	/// </summary>
	/// <param name="aRObject">Newly placed object.</param>
	private void OnAddARObjEvent(ARObject obj)
	{
		arObjectList.Add(obj);
	}

	private void OnPlaneDetectedEvent()
	{
		// listen ar plane manager
		// tell user s/he can put object on it
	}
}
