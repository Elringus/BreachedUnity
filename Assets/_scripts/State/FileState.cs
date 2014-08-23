using System;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;

public class FileState : BaseState
{
	private const string FILE_NAME = "game-state.xml";
	private BackgroundWorker asyncWriter;
	private bool postponedWrite;

	public FileState ()
		: base()
	{
		asyncWriter = new BackgroundWorker();
		asyncWriter.DoWork += new DoWorkEventHandler(WriteDataAsync);
		asyncWriter.RunWorkerCompleted +=
				new RunWorkerCompletedEventHandler(
					(object sender, RunWorkerCompletedEventArgs e) => { if (postponedWrite) { postponedWrite = false; asyncWriter.RunWorkerAsync(); } }
				);
	}

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

		if (asyncWriter.IsBusy) postponedWrite = true;
		else asyncWriter.RunWorkerAsync();

		return true;
	}

	private void WriteDataAsync (object sender, DoWorkEventArgs e)
	{
		var serializer = new XmlSerializer(typeof(BaseState), new Type[] { typeof(FileState) });
		using (var stream = new FileStream(Path.Combine(Environment.CurrentDirectory, FILE_NAME), FileMode.Create))
			serializer.Serialize(new StreamWriter(stream, System.Text.Encoding.UTF8), this);
	}
}