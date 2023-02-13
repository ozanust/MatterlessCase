using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameView : MonoBehaviour
{
	[SerializeField]
	private Camera cam;

	[Inject]
    private IARService arService;
	private List<ARObject> arObjectList = new List<ARObject>();

	private void Start()
	{
		arService.AddCameraListener(OnCameraMove);
	}

	private void OnCameraMove()
	{
		foreach(ARObject aR in arObjectList)
		{
			aR.UpdateDistanceLabel(arService.GetDistance(aR, cam));
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
