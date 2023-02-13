using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ARObject
{
	private TMP_Text distanceLabel;
	private GameObject mainObject;

	public GameObject MainObject
	{
		get { return mainObject; }
		set { mainObject = value; }
	}
	public ARObject(GameObject obj)
	{
		MainObject = obj;
		distanceLabel = obj.GetComponentInChildren<TMP_Text>();
	}

	public void UpdateDistanceLabel(float distance)
	{
		distanceLabel.text = string.Format("{0} cm", (distance * 100).ToString("0.0"));
	}
}
