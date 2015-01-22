using UnityEngine;
using UnityEditor;
using System.Collections;

public class SinuousSignsEditor : MaterialEditor
{
	enum Tab { Image, Anim, Effect }
	Tab currentTab = Tab.Image;
	Material targetMat;
	bool custom = true, showHelp = false;

	public override void OnInspectorGUI ()
	{
		if (!isVisible)
			return;

		targetMat = target as Material;

		custom = EditorGUILayout.ToggleLeft("Use Custom Inspector", custom);

		if (!custom)
		{
			base.OnInspectorGUI();
			return;
		}

		showHelp = EditorGUILayout.ToggleLeft("Show Help", showHelp);

		EditorGUILayout.Separator();


		EditorGUILayout.BeginHorizontal();
		if (DrawTab("Image Options", Tab.Image))
			currentTab = Tab.Image;
		if (DrawTab("Animation", Tab.Anim))
			currentTab = Tab.Anim;
		if (DrawTab("Effect Options", Tab.Effect))
			currentTab = Tab.Effect;
		EditorGUILayout.EndHorizontal();
		GUI.color = Color.white;
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();

		switch (currentTab)
		{
			case Tab.Image:
				DrawImageOptions();
				break;
			case Tab.Anim:
				DrawAnimOptions();
				break;
			case Tab.Effect:
				DrawEffectOptions();
				break;
		}
	}

	bool DrawTab (string text, Tab thisTab)
	{
		GUI.color = Color.white;
		if (currentTab == thisTab)
			GUI.color = Color.gray;
		return GUILayout.Button(text);
	}

	void DrawImageOptions ()
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		ColorProperty(GetMaterialProperty(targets, "_Color"), "Base Image Color");
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		EditorGUILayout.BeginVertical();
		TextureProperty(GetMaterialProperty(targets, "_MainTex"), "Base Image", false);
		TextureProperty(GetMaterialProperty(targets, "_Noise"), "Scatter Noise", false);
		EditorGUILayout.EndVertical();

		EditorGUILayout.EndHorizontal();

	}

	void DrawAnimOptions ()
	{
		Vector4 ts = targetMat.GetVector("_TileCount");
		Vector2 tc = new Vector2(ts.x, ts.y);

		tc = EditorGUILayout.Vector2Field("Tile Count X / Y", tc);

		ts.x = tc.x;
		ts.y = tc.y;
		targetMat.SetVector("_TileCount", ts);

		RangeProperty(GetMaterialProperty(targets, "_Phase"), "Current Phase");

		EditorGUILayout.BeginHorizontal();
		float start = targetMat.GetFloat("_PhaseStart");
		float end = targetMat.GetFloat("_PhaseEnd");
		EditorGUILayout.PrefixLabel("Phase Start/End");
		EditorGUILayout.MinMaxSlider(ref start, ref end, 0f, 1f);
		targetMat.SetFloat("_PhaseStart", start);
		targetMat.SetFloat("_PhaseEnd", end);
		EditorGUILayout.EndHorizontal();

		RangeProperty(GetMaterialProperty(targets, "_Sharpness"), "Swipe Sharpness");
		RangeProperty(GetMaterialProperty(targets, "_Direction"), "Swipe Direction");
		RangeProperty(GetMaterialProperty(targets, "_Vertical"), "Swipe Vertical");
		RangeProperty(GetMaterialProperty(targets, "_FromCenter"), "Radial Swipe");
		RangeProperty(GetMaterialProperty(targets, "_Scatter"), "Scatter Amount");
		RangeProperty(GetMaterialProperty(targets, "_IdleAmount"), "Idle Amount");
		FloatProperty(GetMaterialProperty(targets, "_IdleSpeed"), "Idle Speed");

		Vector4 sr = targetMat.GetVector("_ScaleRot");
		Vector2 r = new Vector2(sr.z, sr.w);
		r = EditorGUILayout.Vector2Field("Rotation X / Angle Snap Y", r);
		sr.z = r.x;
		sr.w = r.y;
		targetMat.SetVector("_ScaleRot", sr);
	}

	void DrawEffectOptions ()
	{
		Vector4 ts = targetMat.GetVector("_TileCount");
		Vector2 sl = new Vector2(ts.z, ts.w);

		EditorGUILayout.LabelField("Scanlines:");
		sl = EditorGUILayout.Vector2Field("Count / Sharpness", sl);

		ts.z = sl.x;
		ts.w = sl.y;
		targetMat.SetVector("_TileCount", ts);

		RangeProperty(GetMaterialProperty(targets, "_Flash"), "Flash Amount");

		Vector4 sr = targetMat.GetVector("_ScaleRot");
		Vector2 s = new Vector2(sr.x, sr.y);
		s = EditorGUILayout.Vector2Field("Scaling", s);
		sr.x = s.x;
		sr.y = s.y;
		targetMat.SetVector("_ScaleRot", sr);

		bool tilescaling = (targetMat.GetFloat("_ScaleCenter") == 1f) ? true : false;
		tilescaling = EditorGUILayout.Toggle("Scale Around Tile?", tilescaling);
		targetMat.SetFloat("_ScaleCenter", ((tilescaling) ? 1f : 0f));

		bool scaleClipping = (targetMat.GetFloat("_ClipScaling") == 1f) ? true : false;
		scaleClipping = EditorGUILayout.Toggle("Preserve Tile Content?", scaleClipping);
		targetMat.SetFloat("_ClipScaling", ((scaleClipping) ? 1f : 0f));
	}
}