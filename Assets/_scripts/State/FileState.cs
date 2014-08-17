using System;
using System.IO;
using System.Xml.Serialization;

public class FileState : BaseState
{
	private const string FILE_NAME = "game-state.xml";

	public static IState Load ()
	{
		if (File.Exists(Path.Combine(Environment.CurrentDirectory, FILE_NAME)))
		{
			preventSave = true;
			var serializer = new XmlSerializer(typeof(BaseState), new Type[] { typeof(FileState) });
			using (var stream = new FileStream(Path.Combine(Environment.CurrentDirectory, FILE_NAME), FileMode.Open))
			{
				var state = serializer.Deserialize(stream) as BaseState;
				preventSave = false;
				return state;
			}
		}
		else
		{
			var state = new FileState();
			state.Reset(true);
			ServiceLocator.Logger.Log(string.Format("The {0} file cannot be found and was created.", FILE_NAME));
			return state;
		}
	}

	public override bool Save ()
	{
		if (!base.Save()) return false;

		var serializer = new XmlSerializer(typeof(BaseState), new Type[] { typeof(FileState) });
		using (var stream = new FileStream(Path.Combine(Environment.CurrentDirectory, FILE_NAME), FileMode.Create))
			serializer.Serialize(new StreamWriter(stream, System.Text.Encoding.UTF8), this);

		return true;
	}
}