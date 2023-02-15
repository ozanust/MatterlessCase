using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// An AR event for object placement to physical(augmented) world.
/// </summary>
public class ARObjectEvent : AREvent
{
	public Pose objectPose;
	public Action<Pose> evt;

	public override void Callback()
	{
		evt?.Invoke(objectPose);
	}
}
