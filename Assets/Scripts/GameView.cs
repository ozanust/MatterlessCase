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
	[Inject]
	private IGameController gameController;

	private List<ARObject> arObjectList = new List<ARObject>();

	private void Start()
	{
		AddListeners();
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

	private void Update()
	{
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			gameController.TryPlaceObject(Input.GetTouch(0).position);
		}
	}

	/// <summary>
	/// Calls on each frame from the Tick method of Zenject if AR Camera is active and working.
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
	/// Gets the pose data from ARService whenever a object placement request is successful and puts the object according to pose data.
	/// </summary>
	/// <param name="pose">Pose data from raycast result.</param>
	private void OnAddARObjEvent(Pose pose)
	{
		GameObject obj = Instantiate(arObjPrototype, pose.position, pose.rotation);
		ARObject aRObject = new ARObject(obj);
		arObjectList.Add(aRObject);
	}

	private void OnPlaneDetectedEvent()
	{
		// listen ar plane manager
		// tell user s/he can put object on it
	}
}
