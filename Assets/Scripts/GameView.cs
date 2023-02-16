using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;
using TMPro;

/// <summary>
/// Manages and updates game view according to events from AR system.
/// </summary>
public class GameView : MonoBehaviour
{
	[SerializeField]
	private GameObject arObjPrototype;
	[SerializeField]
	private TMP_Text informativeText;
	[SerializeField]
	private CanvasGroup informativeTextCanvas;

	[Inject]
	private IARService arService;
	[Inject]
	private IGameController gameController;

	private List<ARObject> arObjectList = new List<ARObject>();

	private float informativeTextFadeOutTime = 2f;
	private float informativeTextOnScreenTime = 5f;
	private string informativeTextOnPlaneDetectedText = "A surface detected! You can start measuring distances by placing objects.";

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
		planeEvent.evt += OnPlaneDetectedEvent;

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

	/// <summary>
	/// Gets detected/removed/updated plane data from ARService.
	/// </summary>
	/// <param name="args">Planes data.</param>
	private void OnPlaneDetectedEvent(ARPlanesChangedEventArgs args)
	{
		// Inform the user after detecting the first plane
		if (args.added.Count == 1)
		{
			informativeText.text = informativeTextOnPlaneDetectedText;
			LeanTween.delayedCall(informativeTextOnScreenTime, () => LeanTween.alphaCanvas(informativeTextCanvas, 0, informativeTextFadeOutTime));
		}
	}
}
