using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;
using UnityEngine.UI;

public class ARService : IARService, ITickable
{
	public delegate void AREvent(ARObject aRObject);
	public AREvent OnAdd;
	public AREvent OnRemove;
	public AREvent OnMove;

	private ARRaycastManager arRaycastManager;
	private Camera cam;
	private GameObject arObject;

	public ARService(ARRaycastManager aRRaycastManager, Camera camera)
	{
		arRaycastManager = aRRaycastManager;
		cam = camera;
	}

	public void Tick()
	{
		OnMove(null);

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			PlaceARObject();
		}
	}

	public float GetDistance(ARObject gameObject)
	{
		return Vector3.Distance(gameObject.MainObject.transform.position, cam.gameObject.transform.position);
	}

	public void AddObjectAdditionListener(AREvent aREvent)
	{
		OnAdd += aREvent;
	}

	public void AddCameraListener(AREvent aREvent)
	{
		OnMove += aREvent;
	}

	public void SetARObject(GameObject arObject)
	{
		this.arObject = arObject;
	}

	private void OnAddEvent(ARObject gameObject)
	{
		OnAdd(gameObject);
	}

	public void PlaceARObject()
	{
		Vector2 touchPosition = Input.GetTouch(0).position;
		List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

		if (arRaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
		{
			// Raycast hits are sorted by distance, so the first one
			// will be the closest hit.
			var hitPose = s_Hits[0].pose;
			GameObject obj = Object.Instantiate(arObject, hitPose.position, hitPose.rotation);
			ARObject aRObject = new ARObject(obj);
			OnAddEvent(aRObject);
		}
	}
}