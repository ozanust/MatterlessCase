using System;

/// <summary>
/// A base AR event for general purpose.
/// </summary>
public class AREvent
{
	public Action baseEvent;
	public virtual void Callback()
	{
		baseEvent?.Invoke();
	}
}
