using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
	[SerializeField]
	private Camera cam;

	// Inject
    private IARService aRService;
	private List<ARObject> arObjectList = new List<ARObject>();

	private void Start()
	{
		aRService.AddCameraListener(OnCameraMove);
	}

	private void OnCameraMove()
	{
		foreach(ARObject aR in arObjectList)
		{
			aR.UpdateDistanceLabel(aRService.GetDistance(aR, cam));
		}
	}

	private void OnAddARObjEvent(ARObject aRObject)
	{
		arObjectList.Add(aRObject);
	}

	private void OnRemoveARObjEvent(ARObject aRObject)
	{
		arObjectList.Remove(aRObject);
	}
}
