using UnityEngine;
using System.Collections;

//
// apply to object (or select material to work on) which you'd like to use prebaked (in ReliefTerrain script) gloss data affected by bumpmap variance
// select glossBakedData from previously saved file
// REMEMBER that originalTexture (taken from material) must be readabe
//
[AddComponentMenu("Relief Terrain/Helpers/Use baked gloss texture")]
[ExecuteInEditMode]
public class GlossBakedTextureReplacement : MonoBehaviour {
	public RTPGlossBaked glossBakedData;
	public bool RTPStandAloneShader=false;
	public int layerNumber=1;
	public Material CustomMaterial;
	public Texture2D originalTexture=null;
	[System.NonSerialized] public Texture2D bakedTexture=null;
	
	public GlossBakedTextureReplacement() {
		bakedTexture=originalTexture=null;
	}
	
	void Start () {
		Refresh();
	}
	
	void Update () {
		if (!Application.isPlaying) {
			Refresh();
		}
	}
	
	public void Refresh() {
		string shaderProp="_MainTex";
		if (RTPStandAloneShader) {
			shaderProp="_SplatA0";
			if (layerNumber==2) {
				shaderProp="_SplatA1";
			} else if (layerNumber==3) {
				shaderProp="_SplatA2";
			} else if (layerNumber==4) {
				shaderProp="_SplatA3";
			}
		} else {
			if (layerNumber==2) {
				shaderProp="_MainTex2";
			}
		}
		Material _mat;
		if (CustomMaterial!=null) {
			_mat=CustomMaterial;
		} else {
			if (!renderer) return;
			_mat=renderer.sharedMaterial;
			if (!_mat) return;
			if (_mat.HasProperty(shaderProp)) {
				if (bakedTexture) {
					_mat.SetTexture(shaderProp, bakedTexture);
				} else {
					if (originalTexture==null) {
						originalTexture=(Texture2D)_mat.GetTexture(shaderProp);
					}
					if (originalTexture!=null) {
						if ( (glossBakedData!=null) && (!glossBakedData.used_in_atlas) && glossBakedData.CheckSize(originalTexture) ) {
							// mamy przygotowany gloss - zrób texturę tymczasową
							bakedTexture=glossBakedData.MakeTexture(originalTexture);
							// i zapodaj shaderowi
							if (bakedTexture) _mat.SetTexture(shaderProp, bakedTexture);
						}
					}
				}
			}
		}
		
	}
}
