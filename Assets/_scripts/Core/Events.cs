using System;

public static class Events
{
	public static event EventHandler EngineFixed = delegate { };
	public static void RaiseEngineFixed () { EngineFixed(null, EventArgs.Empty); }

	public static event EventHandler FuelSynthed = delegate { };
	public static void RaiseFuelSynthed () { FuelSynthed(null, EventArgs.Empty); }

	public static string Log ()
	{
		return FuelSynthed.GetInvocationList().Length.ToString();
	}
}