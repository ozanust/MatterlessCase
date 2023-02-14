using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// An AR event for object placement to physical(augmented) world.
/// </summary>
public class ARObjectEvent : AREvent
{
	public ARObject updatedObject;
	public Action<ARObject> evt;

	public override void Callback()
	{
		evt?.Invoke(updatedObject);
	}
}
