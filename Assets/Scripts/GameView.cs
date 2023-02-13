using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameView : MonoBehaviour
{
	[SerializeField]
	private GameObject arObjPrototype;

	[Inject]
	private IARService arService;

	private List<ARObject> arObjectList = new List<ARObject>();

	private void Start()
	{
		arService.AddCameraListener(OnCameraMove);
		arService.AddObjectAdditionListener(OnAddARObjEvent);
		arService.SetARObject(arObjPrototype);
	}

	private void OnCameraMove(ARObject aRObject)
	{
		foreach (ARObject aR in arObjectList)
		{
			aR.UpdateDistanceLabel(arService.GetDistance(aR));
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
