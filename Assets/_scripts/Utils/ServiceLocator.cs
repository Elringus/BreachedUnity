
public static class ServiceLocator
{
	private static IState _state;
	public static IState State
	{
		get { if (_state == null) return new NullState(); else return _state; }
		set { _state = value; }
	}

	private static ILogger _logger;
	public static ILogger Logger
	{
		get { if (_logger == null) return new NullLogger(); else return _logger; }
		set { _logger = value; }
	}

	private static IText _text;
	public static IText Text
	{
		get { if (_text == null) return new NullText(); else return _text; }
		set { _text = value; }
	}
}
