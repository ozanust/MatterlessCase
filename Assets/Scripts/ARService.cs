using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;

public class ARService : IARService, ITickable
{
	private ARRaycastManager arRaycastManager;
	private ARPlaneManager arPlaneManager;
	private Camera arCamera;

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
		if (arCamera.isActiveAndEnabled && eventDict.ContainsKey(AREventType.ARCameraEvent))
		{
			eventDict[AREventType.ARCameraEvent].Callback();
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

	/// <summary>
	/// Gets the distance between AR Camera and the specified AR game object. Returns Unity metrics which is equivalent to real world.
	/// </summary>
	/// <param name="gameObject"></param>
	/// <returns></returns>
	public float GetDistance(ARObject gameObject)
	{
		return Vector3.Distance(gameObject.MainObject.transform.position, arCamera.gameObject.transform.position);
	}

	/// <summary>
	/// Fires a raycast to real world and calls object placement event with the Pose info if ray hits any detected AR plane.
	/// </summary>
	/// <param name="touchPosition">2D touch position on screen.</param>
	public void TryPlaceObject(Vector2 touchPosition)
	{
		List<ARRaycastHit> hits = new List<ARRaycastHit>();

		if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
		{
			// Raycast hits are sorted by distance, so the first one will be the closest hit.
			var hitPose = hits[0].pose;
			OnPlaceEvent(hitPose);
		}
	}

	/// <summary>
	/// Calls on new AR plane detection. Notifies the view with the data.
	/// </summary>
	/// <param name="args">Detected AR planes data.</param>
	private void OnPlaneDetected(ARPlanesChangedEventArgs args)
	{
		if(args.added.Count > detectedARPlanes.Count)
		{
			ARPlaneEvent evt = (ARPlaneEvent)eventDict[AREventType.ARPlaneEvent];
			evt.planesData = args;
			evt.Callback();
		}

		detectedARPlanes = args.added;
	}

	private void OnPlaceEvent(Pose pose)
	{
		ARObjectEvent evt = (ARObjectEvent)eventDict[AREventType.ARObjectEvent];
		evt.objectPose = pose;
		evt.Callback();
	}
}