using System;
using System.Reflection;

public static class Events
{
	public static event EventHandler StateUpdated = delegate { };
	public static void RaiseStateUpdated () { StateUpdated(null, EventArgs.Empty); }

	public static event EventHandler EngineFixed = delegate { };
	public static void RaiseEngineFixed () { EngineFixed(null, EventArgs.Empty); }

	public static event EventHandler FuelSynthed = delegate { };
	public static void RaiseFuelSynthed () { FuelSynthed(null, EventArgs.Empty); }

	public static event EventHandler TextUpdated = delegate { };
	public static void RaiseTextUpdated () { TextUpdated(null, EventArgs.Empty); }

	public static event EventHandler DayEnded = delegate { };
	public static void RaiseDayEnded () { DayEnded(null, EventArgs.Empty); }

	public static void LogHandlersCount ()
	{
		int handlersCount = 0;

		Type t = typeof(Events);
		foreach (EventInfo e in t.GetEvents())
		{
			FieldInfo f = t.GetField(e.Name, BindingFlags.Static | 
				BindingFlags.NonPublic | BindingFlags.Instance | 
				BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField);
			Delegate d = (Delegate)f.GetValue(t);
			handlersCount += d.GetInvocationList().Length;
		}

		ServiceLocator.Logger.Log("Event handlers count: " + handlersCount.ToString());
	}
}