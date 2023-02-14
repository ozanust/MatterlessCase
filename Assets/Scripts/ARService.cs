using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;
using UnityEngine.UI;
using System;

public class ARService : IARService, ITickable
{
	private ARRaycastManager arRaycastManager;
	private ARPlaneManager arPlaneManager;
	private Camera arCamera;
	private GameObject arObjectPrototype;

	Dictionary<AREventType, AREvent> eventDict = new Dictionary<AREventType, AREvent>();
	List<ARPlane> detectedARPlanes = new List<ARPlane>();

	public ARService(ARRaycastManager arRaycastManager, ARPlaneManager arPlaneManager, Camera camera)
	{
		this.arRaycastManager = arRaycastManager;
		this.arPlaneManager = arPlaneManager;
		arCamera = camera;

		this.arPlaneManager.planesChanged += OnPlaneDetected;
	}

	public void Tick()
	{
		if (eventDict.ContainsKey(AREventType.ARCameraEvent))
		{
			eventDict[AREventType.ARCameraEvent].Callback();
		}

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			TryPlaceARObject();
		}
	}

	public void AddAREventListener(AREventType type, AREvent arEvent)
	{
		if (eventDict.ContainsKey(type))
		{
			eventDict[type].baseEvent += arEvent.Callback;
			return;
		}

		eventDict.Add(type, arEvent);
	}

	public float GetDistance(ARObject gameObject)
	{
		return Vector3.Distance(gameObject.MainObject.transform.position, arCamera.gameObject.transform.position);
	}

	public void SetARObject(GameObject arObject)
	{
		arObjectPrototype = arObject;
	}

	private void OnPlaneDetected(ARPlanesChangedEventArgs args)
	{
		if(args.added.Count > detectedARPlanes.Count)
		{
			ARPlaneEvent evt = (ARPlaneEvent)eventDict[AREventType.ARObjectEvent];
			evt.Callback();
		}
	}

	private void OnAddEvent(ARObject gameObject)
	{
		ARObjectEvent evt = (ARObjectEvent)eventDict[AREventType.ARObjectEvent];
		evt.updatedObject = gameObject;
		evt.Callback();
	}

	private void TryPlaceARObject()
	{
		Vector2 touchPosition = Input.GetTouch(0).position;
		List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

		if (arRaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
		{
			// Raycast hits are sorted by distance, so the first one
			// will be the closest hit.
			var hitPose = s_Hits[0].pose;
			GameObject obj = UnityEngine.Object.Instantiate(arObjectPrototype, hitPose.position, hitPose.rotation);
			ARObject aRObject = new ARObject(obj);
			OnAddEvent(aRObject);
		}
	}
}