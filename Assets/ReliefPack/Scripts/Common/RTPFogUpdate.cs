using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RTPFogUpdate : MonoBehaviour {
	public bool UpdateOnEveryFrame=true;
	public bool LinearColorSpace=false;
	
	void Start () {
		RTPFogUpdate.Refresh(LinearColorSpace);	
		Invoke("RefreshAll", 0.2f);
	}
	
	void Update () {
		if (UpdateOnEveryFrame) RTPFogUpdate.Refresh(LinearColorSpace);
	}
	
    void OnApplicationFocus(bool focusStatus) {
		if (focusStatus) {
			RefreshAll();
		}
    }
	
	void RefreshAll() {
		ReliefTerrain rt=(ReliefTerrain)GameObject.FindObjectOfType(typeof(ReliefTerrain));
		if (rt!=null && rt.globalSettingsHolder!=null) {
			rt.globalSettingsHolder.RefreshAll();
		}
	}
	
	static void Refresh(bool _LinearColorSpace=false) {
		if (RenderSettings.fog) {
			Shader.SetGlobalFloat("_Fdensity", RenderSettings.fogDensity);
			if (_LinearColorSpace) {
				Shader.SetGlobalColor("_FColor", RenderSettings.fogColor.linear);
			} else {
				Shader.SetGlobalColor("_FColor", RenderSettings.fogColor);
			}
			Shader.SetGlobalFloat("_Fstart", RenderSettings.fogStartDistance);
			Shader.SetGlobalFloat("_Fend", RenderSettings.fogEndDistance);
		} else {
			Shader.SetGlobalFloat("_Fdensity", 0);
			Shader.SetGlobalFloat("_Fstart", 1000000);
			Shader.SetGlobalFloat("_Fend", 2000000);
		}
	}
}
