using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System;

namespace simul
{
	public class TrueSkyBuildPostprocessor
	{
		[PostProcessBuild]
		public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
		{
			if (target != BuildTarget.StandaloneWindows && target != BuildTarget.StandaloneWindows64)
			{
				Debug.LogError("trueSKY Build Postprocessor: don't know this platform: " + target.ToString());
				return;
			}
			string buildDirectory = pathToBuiltProject.Replace(".exe", "_Data");// Path.GetDirectoryName(pathToBuiltProject);
			char s = Path.DirectorySeparatorChar;
			Debug.Log("trueSKY Build Postprocessor: copying shader binaries to " + buildDirectory + s + "Simul" + s + "shaderbin");
			string assetsPath = Environment.CurrentDirectory + s + "Assets";
			string simul = assetsPath + s + "Simul";
			string shaderbinSource = simul + s + "shaderbin";
			string shaderbinBuild = buildDirectory + s + "Simul" + s + "shaderbin";
			DirectoryCopy.Copy(shaderbinSource, shaderbinBuild, true, false, false, false);

			string MediaSource = simul + s + "Media";
			string MediaBuild = buildDirectory + s + "Simul" + s + "Media";
			DirectoryCopy.Copy(MediaSource, MediaBuild, true, false, false, false);

			//String dllPath=assetsPath+s+"Plugins"+s+"x86"+s+"dependencies";
			//FileInfo file=new FileInfo(dllPath+s+""
			//file.CopyTo(targetPath,true);
		}
	}
}