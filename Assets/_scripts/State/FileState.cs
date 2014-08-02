using System;
using System.IO;
using System.Xml.Serialization;

public class FileState : BaseState
{
	public static IState Load ()
	{
		if (File.Exists(Path.Combine(Environment.CurrentDirectory, "state.xml")))
		{
			preventSave = true;
			var serializer = new XmlSerializer(typeof(BaseState), new Type[] { typeof(FileState) });
			using (var stream = new FileStream(Path.Combine(Environment.CurrentDirectory, "state.xml"), FileMode.Open))
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
			ServiceLocator.Logger.Log("The state.xml file cannot be found and was created.");
			return state;
		}
	}

	protected override bool Save ()
	{
		if (!base.Save()) return false;

		var serializer = new XmlSerializer(typeof(BaseState), new Type[] { typeof(FileState) });
		using (var stream = new FileStream(Path.Combine(Environment.CurrentDirectory, "state.xml"), FileMode.Create))
			serializer.Serialize(new StreamWriter(stream, System.Text.Encoding.UTF8), this);

		return true;
	}
}