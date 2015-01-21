using System;
using System.Reflection;

public static class Events
{
	public static event Action StateUpdated = delegate { };
	public static void RaiseStateUpdated () { StateUpdated(); }

	public static event Action EngineFixed = delegate { };
	public static void RaiseEngineFixed () { EngineFixed(); }

	public static event Action FuelSynthed = delegate { };
	public static void RaiseFuelSynthed () { FuelSynthed(); }

	public static event Action TextUpdated = delegate { };
	public static void RaiseTextUpdated () { TextUpdated(); }

	public static event Action DayEnded = delegate { };
	public static void RaiseDayEnded () { DayEnded(); }

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