using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARObject
{
	private Text distanceLabel;
    public ARObject(Text distanceLabel)
	{
		this.distanceLabel = distanceLabel;
	}

	public void UpdateDistanceLabel(float distance)
	{
		distanceLabel.text = string.Format("{0} cm", distance.ToString("0.00"));
	}
}
