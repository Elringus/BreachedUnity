using UnityEngine;
using System;
using System.Collections;
#if UNITY_EDITOR	
using UnityEditor;
#endif

[AddComponentMenu("Relief Terrain/Geometry Blend")]
[RequireComponent (typeof (MeshFilter))]
//[ExecuteInEditMode]
public class GeometryVsTerrainBlend : MonoBehaviour {
	public double UpdTim=0;
	int progress_count_max;
	int progress_count_current;
	const int progress_granulation=1000;
	string progress_description="";
	
	public float blend_distance=0.1f;
	public GameObject blendedObject;
	public bool VoxelBlendedObject=false;
	public float _DeferredBlendGloss=0.8f;
	
	[HideInInspector] public bool undo_flag=false;
	[HideInInspector] public bool paint_flag=false;
	[HideInInspector] public int paint_mode=0;
	[HideInInspector] public float paint_size=0.5f;
	[HideInInspector] public float paint_smoothness=0;
	[HideInInspector] public float paint_opacity=1;
	[HideInInspector] public bool dont_select_aux_object=true;
	[HideInInspector] public RTPColorChannels vertex_paint_channel=RTPColorChannels.A;
	
	[HideInInspector] public int addTrisSubdivision=0;
	[HideInInspector] public float addTrisMinAngle=0;
	[HideInInspector] public float addTrisMaxAngle=90;
	
	private Vector3[] paint_vertices;
	private Vector3[] paint_normals;
	private int[] paint_tris;
	//private Color[] paint_colors;
	private Transform underlying_transform;
	private MeshRenderer underlying_renderer;
	
	[HideInInspector] public RaycastHit paintHitInfo;
	[HideInInspector] public bool paintHitInfo_flag;
	[HideInInspector] Texture2D tmp_globalColorMap;
	
	[HideInInspector] public Vector3[] normals_orig;
	[HideInInspector] public Vector4[] tangents_orig;
	[HideInInspector] public bool baked_normals=false;
	
	[HideInInspector] public Mesh orig_mesh=null;
	[HideInInspector] public Mesh pmesh=null;
	
	[HideInInspector] public bool shader_global_blend_capabilities=false;
	
	[HideInInspector] public float StickOffset=0.03f;
	[HideInInspector] public bool Sticked=false;
	[HideInInspector] public bool StickedOptimized=true;
	[HideInInspector] public bool ModifyTris=false;
	[HideInInspector] public bool BuildMeshFlag=false;
	[HideInInspector] public bool RealizePaint_Flag=false;
	
	[HideInInspector] public string save_path="";
	
	void Start() {
		SetupValues();
	}
	
//#if UNITY_EDITOR		
//	void Update() {
//		if (!Application.isPlaying) {
//			if (EditorApplication.timeSinceStartup<UpdTim) return;
//			UpdTim=EditorApplication.timeSinceStartup+1;	
//			SetupValues();
//		}
//	}
//#endif
	
	public void SetupValues() {
#if UNITY_EDITOR		
		if (!renderer.sharedMaterial) {
			renderer.sharedMaterial=new Material(Shader.Find("Relief Pack/GeometryBlend_PM"));
			renderer.sharedMaterial.name=gameObject.name+"_GeometryBlend_PM";
		}
		if (renderer.sharedMaterial.name=="Default-Diffuse") {
			renderer.sharedMaterial=new Material(Shader.Find("Relief Pack/GeometryBlend_PM"));
			renderer.sharedMaterial.name=gameObject.name+"_GeometryBlend_PM";
		}
#endif		
		if (blendedObject && ((blendedObject.GetComponent(typeof(MeshRenderer))!=null) || (blendedObject.GetComponent(typeof(Terrain))!=null))) {
			GameObject go;
			if (underlying_transform==null) underlying_transform=transform.FindChild("RTP_blend_underlying");
			if (underlying_transform!=null) {
				go=underlying_transform.gameObject;
				underlying_renderer=(MeshRenderer)go.GetComponent(typeof(MeshRenderer));
				#if UNITY_EDITOR	
				StaticEditorFlags flag_mask=StaticEditorFlags.OccluderStatic	| StaticEditorFlags.OccludeeStatic | StaticEditorFlags.BatchingStatic | StaticEditorFlags.NavigationStatic | StaticEditorFlags.OffMeshLinkGeneration;
				StaticEditorFlags flags=GameObjectUtility.GetStaticEditorFlags(go);
				if ((flags & StaticEditorFlags.LightmapStatic)>0) {
					flags = flags & flag_mask;
					GameObjectUtility.SetStaticEditorFlags(go, flags);
				}
				if (Sticked) {
					flags=GameObjectUtility.GetStaticEditorFlags(gameObject);
					if ((flags & StaticEditorFlags.LightmapStatic)>0) {
						flags = flags & flag_mask;
						GameObjectUtility.SetStaticEditorFlags(gameObject, flags);
					}
				}
				#endif
			}
			if (underlying_renderer!=null) {
				ReliefTerrain script=(ReliefTerrain)blendedObject.GetComponent(typeof(ReliefTerrain));
				if (script) {
					Material mat=underlying_renderer.sharedMaterial;
					script.RefreshTextures(mat);
					script.globalSettingsHolder.Refresh(mat);
					if (mat.HasProperty("RTP_DeferredAddPassSpec")) {
						mat.SetFloat ("RTP_DeferredAddPassSpec", _DeferredBlendGloss);
					}
					
					if (script.controlA) mat.SetTexture("_Control", script.controlA);
					if (script.ColorGlobal) mat.SetTexture("_Splat0", script.ColorGlobal);
					// 8 layers mode impossible when redefining textures
					//if (!script.globalSettingsHolder._RTP_LODmanagerScript.RTP_4LAYERS_MODE) {
					//	if (script.controlB) mat.SetTexture("_Splat1", script.controlB);
					//} else {
						if (script.NormalGlobal) mat.SetTexture("_Splat1", script.NormalGlobal);
					//}
					if (script.TreesGlobal) mat.SetTexture("_Splat2", script.TreesGlobal);
					if (script.BumpGlobalCombined) mat.SetTexture("_Splat3", script.BumpGlobalCombined);			
					
				}
				Terrain terrainComp=(Terrain)blendedObject.GetComponent(typeof(Terrain));
				if (terrainComp) {
					underlying_renderer.lightmapIndex=terrainComp.lightmapIndex;
				} else {
					underlying_renderer.lightmapIndex=blendedObject.renderer.lightmapIndex;
				}
				underlying_renderer.lightmapTilingOffset=new Vector4(1,1,0,0);
				if (Sticked) {
					if (terrainComp) {
						renderer.lightmapIndex=terrainComp.lightmapIndex;
					} else {
						renderer.lightmapIndex=blendedObject.renderer.lightmapIndex;
					}
					renderer.lightmapTilingOffset=new Vector4(1,1,0,0);
				}
			}
		}
	}
	
#if UNITY_EDITOR
	[HideInInspector] public static SceneView.OnSceneFunc _SceneGUI;

	public bool PrepareMesh() {
		if (!blendedObject || blendedObject.GetComponent(typeof(Collider))==null || ((blendedObject.GetComponent(typeof(MeshRenderer))==null) && (blendedObject.GetComponent(typeof(Terrain))==null))) {
			Debug.LogError("Select an object (terrain or GameObject with mesh) to be blended with this object.");
			return false;
		}
		Mesh mesh = ((MeshFilter)GetComponent(typeof(MeshFilter))).sharedMesh;
		Vector3[] vertices=mesh.vertices;
		Color[] colors=mesh.colors;
		if (colors==null || colors.Length<vertices.Length) {
			colors=new Color[vertices.Length];
		}
		for(int i=0; i<colors.Length; i++) {
			Terrain terrain=null;
		  	RaycastHit[] hits;
			bool hit_flag=false;
			Vector3 vertexWorldPos=transform.TransformPoint(vertices[i]);
		    hits=Physics.RaycastAll(vertexWorldPos+Vector3.up*20, Vector3.down, 100);
			int o;
			for(o=0; o<hits.Length; o++) {
				if (hits[o].collider==blendedObject.collider) {
					hit_flag=true;
					Component comp=hits[o].collider.GetComponent(typeof(Terrain));
					if (comp) {
						terrain=(Terrain)comp;
					}
					break;
				}
			}
			if (hit_flag) {
				float height;
				if (terrain) {
					height=terrain.SampleHeight(vertexWorldPos);
				} else {
					height=hits[o].point.y;
				}
				float dif=height-vertexWorldPos.y;
				if (dif<-blend_distance) dif=-blend_distance; else if (dif>0) dif=0;
				dif/=-blend_distance;
				colors[i].a=1-dif;
			} else {
				Debug.LogError("Can't find blended object (collider) below an object vertex "+vertexWorldPos+" !");
				//return false;
			}
		}
		mesh.colors=colors;
		if (underlying_renderer && !VoxelBlendedObject) {
			MeshFilter mf=underlying_renderer.GetComponent(typeof(MeshFilter)) as MeshFilter;
			if (mf) {
				Color[] colors2=mf.sharedMesh.colors;
				if (colors2==null || colors2.Length!=colors.Length) colors2=new Color[colors.Length];
				for(int i=0; i<colors.Length; i++) colors2[i].a=colors[i].a;
				mf.sharedMesh.colors=colors2;
				pmesh=mf.sharedMesh;
			}
		}
		return true;
	}
	
	public bool PrepareMeshAtEdges() {
		if (!blendedObject || blendedObject.GetComponent(typeof(Collider))==null || ((blendedObject.GetComponent(typeof(MeshRenderer))==null) && (blendedObject.GetComponent(typeof(Terrain))==null))) {
			Debug.LogError("Select an object (terrain or GameObject with mesh) to be blended with this object.");
			return false;
		}
		
		Mesh mesh = ((MeshFilter)GetComponent(typeof(MeshFilter))).sharedMesh;
		int[] tris=mesh.triangles;
		Vector3[] vertices=mesh.vertices;
		Color[] colors=mesh.colors;
		if (colors==null || colors.Length<vertices.Length) {
			colors=new Color[vertices.Length];
		}
		ArrayList outer_edges=new ArrayList();
		
		System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");		
		ResetProgress(tris.Length/3+colors.Length,"Autoblending mesh edges");
		
		for(int i=0; i<tris.Length; i+=3) {
			if (check_edge(tris[i], tris[i+1], i, tris)) { outer_edges.Add(tris[i]); outer_edges.Add(tris[i+1]); }
			if (check_edge(tris[i+1], tris[i+2], i, tris)) { outer_edges.Add(tris[i+1]); outer_edges.Add(tris[i+2]); }
			if (check_edge(tris[i+2], tris[i], i, tris)) { outer_edges.Add(tris[i+2]); outer_edges.Add(tris[i]); }
			CheckProgress();
		}
		int[] outer_edges_tab=(int[])outer_edges.ToArray(typeof(int));		
		for(int i=0; i<colors.Length; i++) {
			bool outer=false;
			for(int j=0; j<outer_edges_tab.Length; j++) {
				if (outer_edges_tab[j]==i) {
					outer=true;
					break;
					
				}
			}			
			colors[i].a=outer ? 1:0;
			CheckProgress();
		}
		EditorUtility.ClearProgressBar();
		mesh.colors=colors;
		if (underlying_renderer && !VoxelBlendedObject) {
			MeshFilter mf=underlying_renderer.GetComponent(typeof(MeshFilter)) as MeshFilter;
			if (mf) {
				Color[] colors2=mf.sharedMesh.colors;
				if (colors2==null) colors2=new Color[colors.Length];
				for(int i=0; i<colors.Length; i++) colors2[i].a=colors[i].a;
				mf.sharedMesh.colors=colors2;
				pmesh=mf.sharedMesh;
			}
		}
		return true;
	}
	
	public void remove_tris(int cover_num, int[] cover_tris) {
		MeshFilter mf=(MeshFilter)GetComponent(typeof(MeshFilter));
		Mesh sh=((MeshFilter)GetComponent(typeof(MeshFilter))).sharedMesh;
		if (!sh) return;
		
		if (cover_num==0) return;
		int[] tris=sh.triangles;
		ArrayList new_tris=new ArrayList();
		for(int i=0; i<cover_num; i++) {
			tris[cover_tris[i]]=-1;
			tris[cover_tris[i]+1]=-1;
			tris[cover_tris[i]+2]=-1;
		}
		for(int i=0; i<tris.Length; i++) {
			if (tris[i]>=0) new_tris.Add(tris[i]);
		}
		tris=(int[])new_tris.ToArray(typeof(int));
		sh.triangles=tris;
		sh.RecalculateBounds();
		
		Transform tr=transform.FindChild("RTP_blend_underlying");
		if (tr!=null) {
			GameObject go=tr.gameObject;
			mf=(MeshFilter)go.GetComponent(typeof(MeshFilter));
			if (mf) mf.sharedMesh.triangles=tris;
		}
		paint_tris=null;
	}
	
	public void add_tris(int cover_num, Vector3[] cover_verts) {
		MeshFilter mf=(MeshFilter)GetComponent(typeof(MeshFilter));
		if (!mf) return;
		Mesh sh=mf.sharedMesh;
		if (!sh) return;
		if (!blendedObject) return;
		Terrain terrainComp=(Terrain)blendedObject.GetComponent(typeof(Terrain));
		ReliefTerrain terrainScript=(ReliefTerrain)blendedObject.GetComponent(typeof(ReliefTerrain));
		//if (!terrainScript) return;
		
		float terrainSize_x;
		float terrainSize_z;
		Vector3 terrainPosition;
		Vector2 terrainTiling;
		if (terrainScript) {
			terrainSize_x=terrainScript.globalSettingsHolder.terrainTileSizeX;
			terrainSize_z=terrainScript.globalSettingsHolder.terrainTileSizeZ;
			terrainTiling=new Vector2(terrainSize_x/terrainScript.globalSettingsHolder.ReliefTransform.x, terrainSize_z/terrainScript.globalSettingsHolder.ReliefTransform.y);
		} else {
			terrainSize_x=blendedObject.renderer.bounds.size.x;
			terrainSize_z=blendedObject.renderer.bounds.size.z;
			terrainTiling=new Vector2(1,1);
			if (blendedObject.renderer.sharedMaterial.HasProperty("_TERRAIN_ReliefTransformTriplanarZ")) terrainTiling=Vector2.one*blendedObject.renderer.sharedMaterial.GetFloat("_TERRAIN_ReliefTransformTriplanarZ");
		}
		if (terrainComp) {
			terrainPosition=terrainComp.GetPosition();
		} else {
			terrainPosition=blendedObject.renderer.bounds.min;
		}
		
		if (cover_num==0) return;
		int[] current_tris=sh.triangles;
		Vector3[] current_verts=sh.vertices;
		
		float cellW;
		float cellH;
		if (terrainComp) {
			cellW=terrainSize_x/(terrainComp.terrainData.heightmapResolution-1)*Mathf.Pow(2.0f, -1.0f*addTrisSubdivision);
			cellH=terrainSize_z/(terrainComp.terrainData.heightmapResolution-1)*Mathf.Pow(2.0f, -1.0f*addTrisSubdivision);
		} else {
			if (!terrainScript) {
				cellW=cellH=0.5f*Mathf.Pow(2.0f, -1.0f*addTrisSubdivision);
			} else {
				if (!terrainScript.controlA) return;
				cellW=terrainSize_x/(terrainScript.controlA.width)*Mathf.Pow(2.0f, -1.0f*addTrisSubdivision);
				cellH=terrainSize_z/(terrainScript.controlA.height)*Mathf.Pow(2.0f, -1.0f*addTrisSubdivision);
			}
		}
		
		int _w=Mathf.RoundToInt((paint_size*2)/cellW+1);
		int _h=Mathf.RoundToInt((paint_size*2)/cellH+1);
		_w=_w>44 ? 44:_w;
		_h=_h>44 ? 44:_h;
		
		int[] existing_vert_idx=new int[cover_num];
		int existing_vert_count=0;
		for(int i=0; i<cover_num; i++) {
			existing_vert_idx[i]=-1;
			for(int j=0; j<current_verts.Length; j++) {
				if (Vector3.Distance(transform.TransformPoint(current_verts[j]), cover_verts[i])<0.01f) {
					existing_vert_idx[i]=j;
					existing_vert_count++;
					break;
				}
			}
		}
		int idx;
		ArrayList new_tris=new ArrayList();
		for(idx=0; idx<cover_num; idx++) {
			int idx_i=idx/_h;
			int idx_j=idx%_h;
			int idx_i_right=idx_i+1;
			int idx_j_right=idx_j;
			int idx_i_down=idx_i;
			int idx_j_down=idx_j-1;
			int idx_i_topright=idx_i+1;
			int idx_j_topright=idx_j+1;
			int right_idx=(idx_i_right*_h+idx_j_right);
			int down_idx=(idx_i_down*_h+idx_j_down);
			int topright_idx=(idx_i_topright*_h+idx_j_topright);
			bool right_is_valid=idx_i_right<_w;
			bool down_is_valid=idx_j_down>=0;
			bool topright_is_valid=idx_i_right<_w && idx_j_topright<_h;
			if ( right_is_valid && down_is_valid ) {
				bool add_flag=false;
				if (existing_vert_idx[idx]>=0 && existing_vert_idx[right_idx]>=0 && existing_vert_idx[down_idx]>=0) {
					add_flag=true;
					int idxA=existing_vert_idx[idx];
					int idxB=existing_vert_idx[right_idx];
					int idxC=existing_vert_idx[down_idx];
					if (idxB<idxA) Swap(ref idxA, ref idxB);
					if (idxC<idxA) Swap(ref idxC, ref idxA);
					if (idxC<idxB) Swap(ref idxC, ref idxB);
					for(int j=0; j<current_tris.Length; j++) {
						int idxD=current_tris[j++];
						int idxE=current_tris[j++];
						int idxF=current_tris[j];
						if (idxE<idxD) Swap(ref idxD, ref idxE);
						if (idxF<idxD) Swap(ref idxF, ref idxD);
						if (idxF<idxE) Swap(ref idxF, ref idxE);
						if (idxA==idxD && idxB==idxE && idxC==idxF) {
							add_flag=false;
							break;
						}
					}
				} else {
					add_flag=true;
				}
				if (add_flag) {
					Vector3 pntA = cover_verts[idx];
					Vector3 pntB = cover_verts[right_idx];
					Vector3 pntC = cover_verts[down_idx];
					Vector3 tri_norm = Vector3.Cross( pntB - pntA , pntC - pntA );
					float angle=Vector3.Angle(tri_norm, Vector3.up);
					if ( (angle>=addTrisMinAngle) && (angle<=addTrisMaxAngle)) {
						new_tris.Add(existing_vert_idx[idx]>=0 ? -existing_vert_idx[idx]-1 : idx);
						new_tris.Add(existing_vert_idx[right_idx]>=0 ? -existing_vert_idx[right_idx]-1 : right_idx);
						new_tris.Add(existing_vert_idx[down_idx]>=0 ? -existing_vert_idx[down_idx]-1 : down_idx);
					}
				}
			}
			if ( right_is_valid && topright_is_valid ) {
				bool add_flag=false;
				if ( (existing_vert_idx[idx]>=0 && existing_vert_idx[topright_idx]>=0 && existing_vert_idx[right_idx]>=0) ) {
					add_flag=true;
					int idxA=existing_vert_idx[idx];
					int idxB=existing_vert_idx[topright_idx];
					int idxC=existing_vert_idx[right_idx];
					if (idxB<idxA) Swap(ref idxA, ref idxB);
					if (idxC<idxA) Swap(ref idxC, ref idxA);
					if (idxC<idxB) Swap(ref idxC, ref idxB);
					for(int j=0; j<current_tris.Length; j++) {
						int idxD=current_tris[j++];
						int idxE=current_tris[j++];
						int idxF=current_tris[j];
						if (idxE<idxD) Swap(ref idxD, ref idxE);
						if (idxF<idxD) Swap(ref idxF, ref idxD);
						if (idxF<idxE) Swap(ref idxF, ref idxE);
						if (idxA==idxD && idxB==idxE && idxC==idxF) {
							add_flag=false;
							break;
						}
					}					
				} else {
					add_flag=true;
				}
				if (add_flag) {
					Vector3 pntA = cover_verts[idx];
					Vector3 pntB = cover_verts[topright_idx];
					Vector3 pntC = cover_verts[right_idx];
					Vector3 tri_norm = Vector3.Cross( pntB - pntA , pntC - pntA );
					float angle=Vector3.Angle(tri_norm, Vector3.up);
					if ( (angle>=addTrisMinAngle) && (angle<=addTrisMaxAngle)) {
						new_tris.Add(existing_vert_idx[idx]>=0 ? -existing_vert_idx[idx]-1 : idx);
						new_tris.Add(existing_vert_idx[topright_idx]>=0 ? -existing_vert_idx[topright_idx]-1 : topright_idx);
						new_tris.Add(existing_vert_idx[right_idx]>=0 ? -existing_vert_idx[right_idx]-1 : right_idx);
					}
				}
			}
		}		
		if (new_tris.Count==0) return;
		int[] new_tris_mod=(int[])new_tris.ToArray(typeof(int));
		
		for(int i=0; i<cover_num; i++) {
			if (existing_vert_idx[i]>=0) {
				for(int j=0; j<new_tris.Count; j++) {
					if ((int)new_tris[j]>=0 && (int)new_tris[j]>i) {
						new_tris_mod[j]--;
					}
				}
			}
		}
		
		// new tris
		int[] tris=new int[current_tris.Length+new_tris.Count];
		idx=0;
		for(int i=0; i<current_tris.Length; i++) {
			tris[idx]=current_tris[i];
			idx++;
		}
		for(int i=0; i<new_tris.Count; i++) {
			if (new_tris_mod[i]>=0) {
				tris[idx]=new_tris_mod[i]+current_verts.Length;
			} else {
				tris[idx]=-new_tris_mod[i]-1;
			}
			idx++;
		}
		// new vertices
		Vector3[] verts=new Vector3[current_verts.Length+cover_num-existing_vert_count];
		Vector4[] current_tangents=sh.tangents; if (current_tangents==null || current_tangents.Length==0) current_tangents=new Vector4[current_verts.Length];
		Vector4[] tangents=new Vector4[verts.Length];
		Color[] current_colors=sh.colors; if (current_colors==null || current_colors.Length==0) current_colors=new Color[current_verts.Length];
		Color[] colors=new Color[verts.Length];
		Vector2[] current_uv=sh.uv; if (current_uv==null || current_uv.Length==0) current_uv=new Vector2[current_verts.Length];
		Vector2[] uv=new Vector2[verts.Length];
		Vector2[] current_uv2=sh.uv2; if (current_uv2==null || current_uv2.Length==0) current_uv2=new Vector2[current_verts.Length];
		Vector2[] uv2=new Vector2[verts.Length];
		idx=0;
		for(int i=0; i<current_verts.Length; i++) {
			verts[idx]=current_verts[i];
			colors[idx]=current_colors[i];
			uv[idx]=current_uv[i];
			uv2[idx]=current_uv2[i];
			tangents[idx]=current_tangents[i];
			idx++;
		}
		for(int i=0; i<cover_num; i++) {
			if (existing_vert_idx[i]==-1) {
				verts[idx]=transform.InverseTransformPoint(cover_verts[i]+Vector3.up*0.003f);
				colors[idx]=new Color(0,0,0,0);
				uv2[idx]=new Vector2( (cover_verts[i].x-terrainPosition.x)/terrainSize_x, (cover_verts[i].z-terrainPosition.z)/terrainSize_z );
				uv[idx]=new Vector2( (cover_verts[i].x-terrainPosition.x)/terrainTiling.x, (cover_verts[i].z-terrainPosition.z)/terrainTiling.y );
				idx++;
			}
		}
		sh.triangles=null;
		sh.vertices=verts;
		sh.colors=colors;
		sh.uv=uv;
		sh.uv2=uv2;
		sh.triangles=tris;
		sh.RecalculateBounds();
		sh.RecalculateNormals();
		if (terrainComp) {
			Vector3[] normals=sh.normals;
			for(int i=current_verts.Length; i<tangents.Length; i++) {
				Vector3 vec=Vector3.Cross(normals[i], Vector3.forward);
				//Vector3 vec=Vector3.Cross(normals[i], new Vector3(0, -normals[i].z, normals[i].y));
				vec.Normalize();
				tangents[i]=new Vector4(vec.x, vec.y, vec.z, -1.0f);
			}
			sh.tangents=tangents;
		} else {
			RTPTangentSolver.Solve(sh);
		}
	}	
	
	// działa jak meshcopy, ale nie resetuje koloru vertexów (a) i po zrobieniu kopii mesha dodaje lekkie rozsadzanie wierzchołków wraz z rekalkulacja normalnych i tangentow
	public bool FlattenMesh() {
		MeshFilter mf=(MeshFilter)GetComponent(typeof(MeshFilter));
		Mesh sh=((MeshFilter)GetComponent(typeof(MeshFilter))).sharedMesh;
		if (!sh) return false;
		Mesh new_mesh=(Mesh)Instantiate(sh);
		Component comp;
		
		// rozsuwanie vertexów
		Vector3[] vertices=new_mesh.vertices;
		Vector3[] normals=new_mesh.normals;
		Color[] colors=new_mesh.colors;
		if (colors==null || colors.Length<vertices.Length) {
			colors=new Color[vertices.Length];
			for(int i=0; i<vertices.Length; i++) {
				colors[i]=Color.white;
			}
		}		
		Vector3[] normals_smoothed=new Vector3[normals.Length];
		Color[] colors_smoothed=new Color[normals.Length];
		float[] normals_weight=new float[normals.Length];
		for(int i=0; i<vertices.Length; i++) {
			normals_weight[i]=1;
			normals_smoothed[i]=new Vector3(normals[i].x, normals[i].y, normals[i].z);
			colors_smoothed[i]=new Color(colors[i].r, colors[i].g, colors[i].b, colors[i].a);
		}
		for(int i=0; i<vertices.Length; i++) {
			for(int j=0; j<vertices.Length; j++) {
				if (i!=j && Vector3.Distance(vertices[i], vertices[j])<0.01f) {
					normals_weight[i]++;
					normals_smoothed[i]+=new Vector3(normals[j].x, normals[j].y, normals[j].z);
					colors_smoothed[i]+=new Color(colors[i].r, colors[i].g, colors[i].b, colors[i].a);
				}
			}		
		}
		for(int i=0; i<vertices.Length; i++) {
			normals_smoothed[i]*=1.0f / normals_weight[i];
			colors_smoothed[i]*=1.0f / normals_weight[i];
		}
		Vector4[] tangents=new_mesh.tangents;
		for(int i=0; i<vertices.Length; i++) {
			Terrain terrain_tmp=null;
		  	RaycastHit[] hits;
			bool hit_flag=false;
			Vector3 vertexWorldPos=transform.TransformPoint(vertices[i]);
		    hits=Physics.RaycastAll(vertexWorldPos+Vector3.up*20, Vector3.down, 100);
			int o;
			for(o=0; o<hits.Length; o++) {
				if (hits[o].collider==blendedObject.collider) {
					hit_flag=true;
					comp=hits[o].collider.GetComponent(typeof(Terrain));
					if (comp) {
						terrain_tmp=(Terrain)comp;
					}
					break;
				}
			}
			if (hit_flag) {
				float height;
				Vector3 ground_norm;
				if (terrain_tmp) {
					height=terrain_tmp.SampleHeight(vertexWorldPos);
					ground_norm=terrain_tmp.terrainData.GetInterpolatedNormal((vertexWorldPos.x-terrain_tmp.transform.position.x)/terrain_tmp.terrainData.size.x, (vertexWorldPos.z-terrain_tmp.transform.position.z)/terrain_tmp.terrainData.size.z);
				} else {
					height=hits[o].point.y;
					ground_norm=hits[o].normal;
				}
				ground_norm=transform.InverseTransformDirection(ground_norm);
				float underground=height-vertexWorldPos.y;
				if (underground<0) underground=0;
				float dif=height-vertexWorldPos.y;
				float move_blend_distance=blend_distance;//0.15f;
				if (dif<-move_blend_distance) dif=-move_blend_distance; else if (dif>0) dif=0;
				dif/=-move_blend_distance;
				dif=1-dif;
				dif*=dif;
				Vector3 move_vec=normals_smoothed[i] - Vector3.Dot(normals_smoothed[i], ground_norm)*ground_norm;
				float mag=move_vec.magnitude;
				mag*=mag;
				move_vec.Normalize();
				move_vec*=mag;
				move_vec*=dif*0.1f;
				vertices[i]+=move_vec*colors_smoothed[i].a;
				//vertices[i]+=ground_norm*underground*move_vec.magnitude*colors_smoothed[i].a;
			} else {
				Debug.LogError("Can't find blended object (collider) below an object vertex "+vertexWorldPos+" !");
				//return false;
			}
		}		
		new_mesh.vertices=vertices;
		new_mesh.RecalculateNormals();
		RTPTangentSolver.Solve(new_mesh);
		
		pmesh=mf.sharedMesh=new_mesh;
		mf.sharedMesh.name=orig_mesh.name+" (copy)";
		Mesh new_mesh_underlying=new Mesh();
		GameObject go;
		Transform tr=transform.FindChild("RTP_blend_underlying");
		MeshRenderer mr;
		if (tr==null) {
			go=new GameObject("RTP_blend_underlying");
			tr=go.transform;
			mf=(MeshFilter)go.AddComponent(typeof(MeshFilter));
			mr=(MeshRenderer)go.AddComponent(typeof(MeshRenderer));
			tr.parent=transform;
			tr.localScale=Vector3.one;
			tr.localPosition=Vector3.zero;
			tr.localRotation=Quaternion.identity;
			go.AddComponent(typeof(SelectorHelperClass));
		} else {
			go=tr.gameObject;
			mf=(MeshFilter)go.GetComponent(typeof(MeshFilter));
			mr=(MeshRenderer)go.GetComponent(typeof(MeshRenderer));
		}

		vertices=new_mesh.vertices;
		normals=new_mesh.normals;
		if (!VoxelBlendedObject) {
			for(int i=0; i<colors.Length; i++) {
				Vector3 normal_world=tr.TransformDirection(normals[i]);
				colors[i].r=(normal_world.x+1)*0.5f;
				colors[i].g=(normal_world.y+1)*0.5f;
				colors[i].b=(normal_world.z+1)*0.5f;
				//colors[i].a=0;
			}
		}
		
		Terrain terrain=null;
		tangents=null;
		Vector4[] blended_tangents=null;
		Vector3[] blended_normals=null;
		//Vector2[] blended_uvs=null;
		Color[] blended_colors=null;
		int[] tris_blended=null;
		Vector2[] _uvs=new Vector2[vertices.Length];
		Vector2[] _uvs2=new Vector2[vertices.Length];
		comp=blendedObject.collider.GetComponent(typeof(Terrain));
		if (comp) {
			terrain=(Terrain)comp;
		}
		if (!terrain) {
			MeshFilter meshFilter_blended=(MeshFilter)blendedObject.GetComponent(typeof(MeshFilter));
			if (meshFilter_blended==null) {
				Debug.LogError("Underlying blended object is missing MeshFilter component !");
				return false;
			}
			Mesh mesh_blended=meshFilter_blended.sharedMesh;
			blended_tangents=mesh_blended.tangents;
			blended_normals=mesh_blended.normals;
			blended_colors=mesh_blended.colors;	
			//blended_uvs=mesh_blended.uv;
			tris_blended=mesh_blended.triangles;
			if (blended_tangents!=null && blended_tangents.Length>0) {
				tangents=new Vector4[vertices.Length];
			}
		}
		
		for(int i=0; i<colors.Length; i++) {
		  	RaycastHit[] hits;
			bool hit_flag=false;
			Vector3 vertexWorldPos=transform.TransformPoint(vertices[i]);
			int minIdx=0;
			if (terrain) {
				hits=Physics.RaycastAll(vertexWorldPos+Vector3.up*20, Vector3.down, 100);
				int o;
				for(o=0; o<hits.Length; o++) {
					if (hits[o].collider==blendedObject.collider) {
						hit_flag=true;
						break;
					}
				}
			} else {
				MeshFilter meshFilter_blended=(MeshFilter)blendedObject.GetComponent(typeof(MeshFilter));
				Vector3[] blended_vertices=meshFilter_blended.sharedMesh.vertices;
				float minDist=Vector3.SqrMagnitude(blendedObject.transform.TransformPoint(blended_vertices[0])-vertexWorldPos);
				for(int k=1; k<blended_vertices.Length; k++) {
					float dist=Vector3.SqrMagnitude(blendedObject.transform.TransformPoint(blended_vertices[k])-vertexWorldPos);
					if (dist<minDist) {
						minDist=dist;
						minIdx=k;
					}
				}
			}
			if (hit_flag || !terrain) {
				Vector3 ground_norm;
				Vector2 _uv, _uv2;
				if (terrain) {
					ground_norm=terrain.terrainData.GetInterpolatedNormal((vertexWorldPos.x-terrain.transform.position.x)/terrain.terrainData.size.x, (vertexWorldPos.z-terrain.transform.position.z)/terrain.terrainData.size.z);
					ground_norm.Normalize();
					_uv=new Vector2((vertexWorldPos.x-terrain.transform.position.x)/terrain.terrainData.size.x, (vertexWorldPos.z-terrain.transform.position.z)/terrain.terrainData.size.z);
					_uv2=new Vector2(_uv.x, _uv.y);
					ground_norm=tr.InverseTransformDirection(ground_norm);
					normals[i]=ground_norm;
					_uvs[i]=_uv;
					_uvs2[i]=_uv2;					
				} else {
					//hits=Physics.RaycastAll(vertexWorldPos+hits[o].normal*20, -hits[o].normal, 100);
					Vector3 closestNorm=blendedObject.transform.TransformDirection(blended_normals[minIdx]);
					hits=Physics.RaycastAll(vertexWorldPos+closestNorm*20, -closestNorm, 100);
					int o;
					for(o=0; o<hits.Length; o++) if (hits[o].collider==blendedObject.collider) break;
					if (o<hits.Length) {
						Vector3 b_coords=hits[o].barycentricCoordinate;
						int tri_idx=hits[o].triangleIndex*3;
						Vector3 normal1=blended_normals[tris_blended[tri_idx + 0]];
						Vector3 normal2=blended_normals[tris_blended[tri_idx + 1]];
						Vector3 normal3=blended_normals[tris_blended[tri_idx + 2]];
						ground_norm = normal1 * b_coords.x + normal2 * b_coords.y + normal3 * b_coords.z;
						ground_norm.Normalize();
						ground_norm=blendedObject.transform.TransformDirection(ground_norm);
						ground_norm=tr.InverseTransformDirection(ground_norm);
						
						VoxelBlendedObject=blendedObject && blendedObject.GetComponent(typeof(ReliefTerrainVertexBlendTriplanar))!=null;
						if (blended_colors!=null && VoxelBlendedObject) {
							Color color1=blended_colors[tris_blended[tri_idx + 0]];
							Color color2=blended_colors[tris_blended[tri_idx + 1]];
							Color color3=blended_colors[tris_blended[tri_idx + 2]];
							Color ground_color = color1 * b_coords.x + color2 * b_coords.y + color3 * b_coords.z;
							colors[i] = ground_color;
						}
						
						_uv=hits[o].textureCoord;
						_uv2=hits[o].lightmapCoord;
						
						if (	tangents!=null) {
							Vector4 tangent1=blended_tangents[tris_blended[tri_idx + 0]];
							Vector4 tangent2=blended_tangents[tris_blended[tri_idx + 1]];
							Vector4 tangent3=blended_tangents[tris_blended[tri_idx + 2]];
							Vector3 ground_tangent = new Vector3(tangent1.x, tangent1.y, tangent1.z) * b_coords.x + new Vector3(tangent2.x, tangent2.y, tangent2.z) * b_coords.y + new Vector3(tangent3.x, tangent3.y, tangent3.z) * b_coords.z;
							ground_tangent.Normalize();
							ground_tangent=blendedObject.transform.TransformDirection(ground_tangent);
							ground_tangent=tr.InverseTransformDirection(ground_tangent);
							tangents[i] = new Vector4(ground_tangent.x, ground_tangent.y, ground_tangent.z, -1);
						}
						normals[i]=ground_norm;
						_uvs[i]=_uv;
						_uvs2[i]=_uv2;
					}
				}
			} else {
				Debug.LogError("Can't find blended object (collider) below an object vertex "+vertexWorldPos+" !");
				//return false;
			}
			//colors[i].a=0;
		}
		new_mesh_underlying.vertices=vertices;
		new_mesh_underlying.triangles=new_mesh.triangles;
		new_mesh_underlying.normals=normals;
		new_mesh_underlying.colors=colors;
		new_mesh_underlying.uv=_uvs;
		new_mesh_underlying.uv2=_uvs2;
		if (tangents!=null) {
			new_mesh_underlying.tangents=tangents;
		}
		mf.sharedMesh=new_mesh_underlying;
		mr.castShadows=false;
		mr.receiveShadows=true;
		go.isStatic=false;
		if (!Sticked || !StickedOptimized) {
			if (terrain) {
				mr.sharedMaterial=new Material(Shader.Find("Relief Pack/ReliefTerrainGeometryBlendBase"));
			} else {
				if (VoxelBlendedObject) {
					mr.sharedMaterial=blendedObject.renderer.sharedMaterial;
				} else {
					mr.sharedMaterial=new Material(Shader.Find("Relief Pack/ReliefTerrain2GeometryBlendBase"));
				}
			}
		}
		//ClearBlend();
		return true;
	}
	
	float GetCrossPoint(Vector3 vertexWorldPos, Vector3 _dir) {
		  	RaycastHit[] hits=Physics.RaycastAll(vertexWorldPos, _dir, 100);
			for(int o=0; o<hits.Length; o++) {
				if (hits[o].collider==blendedObject.collider) {
					return Vector3.Distance(hits[o].point, vertexWorldPos);
				}
			}
			return 0;
	}
	
	bool IsBelowGround(Vector3 vertexWorldPos) {
		RaycastHit[] hits=Physics.RaycastAll(vertexWorldPos, Vector3.down, 100);
		for(int o=0; o<hits.Length; o++) {
			if (hits[o].collider==blendedObject.collider) {
				return false;
			}
		}
		return true;
	}
	
	Color CrossInterpolate(Color[] tab, int idxA, int idxB, float t) {
		return Color.Lerp(tab[idxA], tab[idxB], t);
	}
	Vector2 CrossInterpolate(Vector2[] tab, int idxA, int idxB, float t) {
		return tab[idxA]*(1-t) + tab[idxB]*t;
	}
	Vector3 CrossInterpolate(Vector3[] tab, int idxA, int idxB, float t) {
		return tab[idxA]*(1-t) + tab[idxB]*t;
	}
	Vector4 CrossInterpolate(Vector4[] tab, int idxA, int idxB, float t) {
		Vector4 ret=tab[idxA]*(1-t) + tab[idxB]*t;
		ret.w=tab[idxA].w;
		return ret;
	}
	
	Color[] MergeCrossTab(Color[] tabA, Color[] tabB, int lenB) {
		if ((tabA.Length==0) || (tabA==null)) return null;
		Color[] ret=new Color[tabA.Length+lenB];
		int i=0;
		for(; i<tabA.Length; i++) {
			ret[i]=tabA[i];
		}
		for(;i<(tabA.Length+lenB); i++) {
			ret[i]=tabB[i-tabA.Length];
		}
		return ret;
	}
	Vector2[] MergeCrossTab(Vector2[] tabA, Vector2[] tabB, int lenB) {
		if ((tabA.Length==0) || (tabA==null)) return null;		
		Vector2[] ret=new Vector2[tabA.Length+lenB];
		int i=0;
		for(; i<tabA.Length; i++) {
			ret[i]=tabA[i];
		}
		for(;i<(tabA.Length+lenB); i++) {
			ret[i]=tabB[i-tabA.Length];
		}
		return ret;
	}	
	Vector3[] MergeCrossTab(Vector3[] tabA, Vector3[] tabB, int lenB) {
		if ((tabA.Length==0) || (tabA==null)) return null;		
		Vector3[] ret=new Vector3[tabA.Length+lenB];
		int i=0;
		for(; i<tabA.Length; i++) {
			ret[i]=tabA[i];
		}
		for(;i<(tabA.Length+lenB); i++) {
			ret[i]=tabB[i-tabA.Length];
		}
		return ret;
	}	
	Vector4[] MergeCrossTab(Vector4[] tabA, Vector4[] tabB, int lenB) {
		if ((tabA.Length==0) || (tabA==null)) return null;		
		Vector4[] ret=new Vector4[tabA.Length+lenB];
		int i=0;
		for(; i<tabA.Length; i++) {
			ret[i]=tabA[i];
		}
		for(;i<(tabA.Length+lenB); i++) {
			ret[i]=tabB[i-tabA.Length];
		}
		return ret;
	}	
	int[] MergeCrossTab(int[] tabA, int[] tabB, int lenB) {
		if ((tabA.Length==0) || (tabA==null)) return null;		
		int[] ret=new int[tabA.Length+lenB];
		int i=0;
		for(; i<tabA.Length; i++) {
			ret[i]=tabA[i];
		}
		for(;i<(tabA.Length+lenB); i++) {
			ret[i]=tabB[i-tabA.Length];
		}
		return ret;
	}	
	
	// działa jak meshcopy, ale przykleja obiekt do gruntu
	public void StickToBlendedObject(bool normals_flag=true) {
		MeshFilter mf=(MeshFilter)GetComponent(typeof(MeshFilter));
		Mesh sh=((MeshFilter)GetComponent(typeof(MeshFilter))).sharedMesh;
		if (!sh) return;
		
		Terrain terrain=null;
		if (!blendedObject || !blendedObject.collider) return;
		Component comp=blendedObject.collider.GetComponent(typeof(Terrain));
		if (comp) {
			terrain=(Terrain)comp;
		}
		
		Mesh new_mesh=(Mesh)Instantiate(sh);
		Vector3[] vertices=new_mesh.vertices;
		Vector3[] normals=new_mesh.normals;
		int[] tris=new_mesh.triangles;
		ArrayList outer_edges=new ArrayList();
		
		System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");		
		ResetProgress(tris.Length/3+vertices.Length, "Sticking mesh to its blended object");	
		for(int i=0; i<tris.Length; i+=3) {
			if (check_edge(tris[i], tris[i+1], i, tris)) { outer_edges.Add(tris[i]); outer_edges.Add(tris[i+1]); }
			if (check_edge(tris[i+1], tris[i+2], i, tris)) { outer_edges.Add(tris[i+1]); outer_edges.Add(tris[i+2]); }
			if (check_edge(tris[i+2], tris[i], i, tris)) { outer_edges.Add(tris[i+2]); outer_edges.Add(tris[i]); }
			CheckProgress();
		}
		int[] outer_edges_tab=(int[])outer_edges.ToArray(typeof(int));
		for(int i=0; i<vertices.Length; i++) {
		  	RaycastHit hit;
			Vector3 vertexWorldPos=transform.TransformPoint(vertices[i]);
			if (normals[i].magnitude>0.001f) {
			    bool hit_flag;
				if (normals_flag) {
					hit_flag=blendedObject.collider.Raycast(new Ray(vertexWorldPos, transform.TransformDirection(normals[i])), out hit, 100);
					if (!hit_flag) hit_flag=blendedObject.collider.Raycast(new Ray(vertexWorldPos, -transform.TransformDirection(normals[i])), out hit, 100);
				} else {
					hit_flag=blendedObject.collider.Raycast(new Ray(vertexWorldPos+Vector3.up*10, Vector3.down), out hit, 100);
				}
				if (hit_flag) {
					bool outer=false;
					for(int j=0; j<outer_edges_tab.Length; j++) {
						if (outer_edges_tab[j]==i) {
							outer=true;
							break;
						}
					}
					if (terrain) {
						Vector3 ground_norm=terrain.terrainData.GetInterpolatedNormal((vertexWorldPos.x-terrain.transform.position.x)/terrain.terrainData.size.x, (vertexWorldPos.z-terrain.transform.position.z)/terrain.terrainData.size.z);
						ground_norm.Normalize();
						vertices[i]=transform.InverseTransformPoint(hit.point+ground_norm*(outer ? 0.003f : StickOffset));
					} else {
						vertices[i]=transform.InverseTransformPoint(hit.point+hit.normal*(outer ? 0.003f : StickOffset));
					}
				} else {
					Debug.LogError("Can't find blended object (collider) below an object vertex "+vertexWorldPos+" !");
					//return;
				}
			}
			CheckProgress();
		}		
		EditorUtility.ClearProgressBar();
		
		new_mesh.vertices=vertices;
		new_mesh.RecalculateBounds();
		new_mesh.RecalculateNormals();
		RTPTangentSolver.Solve(new_mesh);
		normals=new_mesh.normals;
		
		pmesh=mf.sharedMesh=new_mesh;
		mf.sharedMesh.name=orig_mesh.name+" (copy)";
		Mesh new_mesh_underlying=new Mesh();
		GameObject go;
		Transform tr=transform.FindChild("RTP_blend_underlying");
		MeshRenderer mr;
		go=null;
		mf=null;
		mr=null;
		if (tr==null) {
			if (!VoxelBlendedObject) {
				go=new GameObject("RTP_blend_underlying");
				tr=go.transform;
				mf=(MeshFilter)go.AddComponent(typeof(MeshFilter));
				mr=(MeshRenderer)go.AddComponent(typeof(MeshRenderer));
				tr.parent=transform;
				tr.localScale=Vector3.one;
				tr.localPosition=Vector3.zero;
				tr.localRotation=Quaternion.identity;
				go.AddComponent(typeof(SelectorHelperClass));
			}
		} else {
			if (!VoxelBlendedObject) {
				go=tr.gameObject;
				mf=(MeshFilter)go.GetComponent(typeof(MeshFilter));
				mr=(MeshRenderer)go.GetComponent(typeof(MeshRenderer));
			}
		}

		Color[] colors=new_mesh.colors;
		if (colors==null || colors.Length<vertices.Length) {
			colors=new Color[vertices.Length];
		}
		if (!VoxelBlendedObject) {
			for(int i=0; i<colors.Length; i++) {
				Vector3 normal_world=tr.TransformDirection(normals[i]);
				colors[i].r=(normal_world.x+1)*0.5f;
				colors[i].g=(normal_world.y+1)*0.5f;
				colors[i].b=(normal_world.z+1)*0.5f;
				//colors[i].a=0;
			}
		}
		
		renderer.castShadows=false;
		if (VoxelBlendedObject) {		
			underlying_transform=null;
			underlying_renderer=null;
			if (tr) UnityEngine.Object.DestroyImmediate(tr.gameObject);
		} else {
			Vector4[] tangents=null;
			Vector4[] blended_tangents=null;
			Vector3[] blended_normals=null;
			//Vector2[] blended_uvs=null;
			int[] tris_blended=null;
			Vector2[] _uvs=new Vector2[vertices.Length];
			Vector2[] _uvs2=new Vector2[vertices.Length];
			if (!terrain) {
				MeshFilter meshFilter_blended=(MeshFilter)blendedObject.GetComponent(typeof(MeshFilter));
				if (meshFilter_blended==null) {
					Debug.LogError("Underlying blended object is missing MeshFilter component !");
					return;
				}
				Mesh mesh_blended=meshFilter_blended.sharedMesh;
				blended_tangents=mesh_blended.tangents;
				blended_normals=mesh_blended.normals;
				//blended_uvs=mesh_blended.uv;
				tris_blended=mesh_blended.triangles;
				if (blended_tangents!=null && blended_tangents.Length>0) {
					tangents=new Vector4[vertices.Length];
				}
			}
			
			for(int i=0; i<colors.Length; i++) {
				RaycastHit[] hits;
				bool hit_flag=false;
				Vector3 vertexWorldPos=transform.TransformPoint(vertices[i]);
				int minIdx=0;
				if (terrain) {
					hits=Physics.RaycastAll(vertexWorldPos+Vector3.up*20, Vector3.down, 100);
					int o;
					for(o=0; o<hits.Length; o++) {
						if (hits[o].collider==blendedObject.collider) {
							hit_flag=true;
							break;
						}
					}
				} else {
					MeshFilter meshFilter_blended=(MeshFilter)blendedObject.GetComponent(typeof(MeshFilter));
					Vector3[] blended_vertices=meshFilter_blended.sharedMesh.vertices;
					float minDist=Vector3.SqrMagnitude(blendedObject.transform.TransformPoint(blended_vertices[0])-vertexWorldPos);
					for(int k=1; k<blended_vertices.Length; k++) {
						float dist=Vector3.SqrMagnitude(blendedObject.transform.TransformPoint(blended_vertices[k])-vertexWorldPos);
						if (dist<minDist) {
							minDist=dist;
							minIdx=k;
						}
					}
				}
				if (hit_flag || !terrain) {
					Vector3 ground_norm;
					Vector2 _uv, _uv2;
					if (terrain) {
						ground_norm=terrain.terrainData.GetInterpolatedNormal((vertexWorldPos.x-terrain.transform.position.x)/terrain.terrainData.size.x, (vertexWorldPos.z-terrain.transform.position.z)/terrain.terrainData.size.z);
						ground_norm.Normalize();
						_uv=new Vector2((vertexWorldPos.x-terrain.transform.position.x)/terrain.terrainData.size.x, (vertexWorldPos.z-terrain.transform.position.z)/terrain.terrainData.size.z);
						_uv2=new Vector2(_uv.x, _uv.y);
						ground_norm=tr.InverseTransformDirection(ground_norm);
						normals[i]=ground_norm;
						_uvs[i]=_uv;
						_uvs2[i]=_uv2;					
					} else {
						//hits=Physics.RaycastAll(vertexWorldPos+hits[o].normal*20, -hits[o].normal, 100);
						Vector3 closestNorm=blendedObject.transform.TransformDirection(blended_normals[minIdx]);
						hits=Physics.RaycastAll(vertexWorldPos+closestNorm*20, -closestNorm, 100);
						int o;
						for(o=0; o<hits.Length; o++) if (hits[o].collider==blendedObject.collider) break;
						if (o<hits.Length) {
							Vector3 b_coords=hits[o].barycentricCoordinate;
							int tri_idx=hits[o].triangleIndex*3;
							Vector3 normal1=blended_normals[tris_blended[tri_idx + 0]];
							Vector3 normal2=blended_normals[tris_blended[tri_idx + 1]];
							Vector3 normal3=blended_normals[tris_blended[tri_idx + 2]];
							ground_norm = normal1 * b_coords.x + normal2 * b_coords.y + normal3 * b_coords.z;
							ground_norm.Normalize();
							ground_norm=blendedObject.transform.TransformDirection(ground_norm);
							ground_norm=tr.InverseTransformDirection(ground_norm);
							
							_uv=hits[o].textureCoord;
							_uv2=hits[o].lightmapCoord;
							
							if (	tangents!=null) {
								Vector4 tangent1=blended_tangents[tris_blended[tri_idx + 0]];
								Vector4 tangent2=blended_tangents[tris_blended[tri_idx + 1]];
								Vector4 tangent3=blended_tangents[tris_blended[tri_idx + 2]];
								Vector3 ground_tangent = new Vector3(tangent1.x, tangent1.y, tangent1.z) * b_coords.x + new Vector3(tangent2.x, tangent2.y, tangent2.z) * b_coords.y + new Vector3(tangent3.x, tangent3.y, tangent3.z) * b_coords.z;
								ground_tangent.Normalize();
								ground_tangent=blendedObject.transform.TransformDirection(ground_tangent);
								ground_tangent=tr.InverseTransformDirection(ground_tangent);
								tangents[i] = new Vector4(ground_tangent.x, ground_tangent.y, ground_tangent.z, -1);
							}
							normals[i]=ground_norm;
							_uvs[i]=_uv;
							_uvs2[i]=_uv2;
						}
					}
				} else {
					Debug.LogError("Can't find blended object (collider) below an object vertex "+vertexWorldPos+" !");
					//return;
				}
				//colors[i].a=0;
			}
			new_mesh_underlying.vertices=vertices;
			new_mesh_underlying.triangles=new_mesh.triangles;
			new_mesh_underlying.normals=normals;
			new_mesh_underlying.colors=colors;
			new_mesh_underlying.uv=_uvs;
			new_mesh_underlying.uv2=_uvs2;
			
			MeshFilter mf_orig=(MeshFilter)GetComponent(typeof(MeshFilter));
			if (mf_orig) mf_orig.sharedMesh.uv2=_uvs2;
			
			if (tangents!=null) {
				new_mesh_underlying.tangents=tangents;
			}
					
			mf.sharedMesh=new_mesh_underlying;
			if (StickedOptimized) {
				mr.castShadows=false;
				//mr.receiveShadows=true;
				mr.receiveShadows=false; // cutout geometry - nic nie robi tylko optymizuje render terenu
				go.isStatic=false;
				mr.sharedMaterial=new Material(Shader.Find("Relief Pack/ReliefTerrainBlendBaseCutout"));
			}
		}
		//ClearBlend();
		if (collider && collider is MeshCollider) {
			(collider as MeshCollider).sharedMesh=new_mesh;
		}
		Sticked=true;
	}
		
	// działa jak meshcopy, ale przeprowadza tesellację ścianek przecinających grunt
	public bool TessellateMesh() {
		MeshFilter mf=(MeshFilter)GetComponent(typeof(MeshFilter));
		Mesh sh=((MeshFilter)GetComponent(typeof(MeshFilter))).sharedMesh;
		if (!sh) return false;
		Mesh new_mesh=(Mesh)Instantiate(sh);
		Component comp;
		
		// rozsuwanie vertexów
		Vector3[] vertices=new_mesh.vertices;
		Vector3[] vertices_add=new Vector3[new_mesh.vertices.Length*2];
		Vector3[] normals=new_mesh.normals;
		Vector3[] normals_add=new Vector3[new_mesh.vertices.Length*2];
		Vector4[] tangents=new_mesh.tangents;
		Vector4[] tangents_add=new Vector4[new_mesh.vertices.Length*2];
		Vector2[] _uvs=new_mesh.uv;
		Vector2[] _uvs_add=new Vector2[new_mesh.vertices.Length*2];
		Vector2[] _uvs2=new_mesh.uv2;
		Vector2[] _uvs2_add=new Vector2[new_mesh.vertices.Length*2];
		Color[] colors=new_mesh.colors;
		Color[] colors_add=new Color[new_mesh.vertices.Length*2];
		int[] triangles=new_mesh.triangles;
		int[] triangles_add=new int[new_mesh.triangles.Length*2];
		int vertex_add_idx=0;
		int triangle_add_idx=0;
		for(int i=0; i<triangles.Length; i+=3) {
			Vector3 pA=transform.TransformPoint(vertices[triangles[i]]);
			Vector3 pB=transform.TransformPoint(vertices[triangles[i+1]]);
			Vector3 pC=transform.TransformPoint(vertices[triangles[i+2]]);
			float lenAB=Vector3.Distance(pA, pB);
			float lenAC=Vector3.Distance(pA, pC);
			float lenBC=Vector3.Distance(pB, pC);
				
			float distA;
			if ((distA=GetCrossPoint(pA, Vector3.Normalize(pB-pA)))>0) {
				distA=distA / lenAB;
			} else if ((distA=GetCrossPoint(pB, Vector3.Normalize(pA-pB)))>0) {
				distA=1- (distA / lenAB);
			}
			float distB;
			if ((distB=GetCrossPoint(pA, Vector3.Normalize(pC-pA)))>0) {
				distB=distB / lenAC;
			} else if ((distB=GetCrossPoint(pC, Vector3.Normalize(pA-pC)))>0) {
				distB=1- (distB / lenAC);
			}
			float distC;
			if ((distC=GetCrossPoint(pB, Vector3.Normalize(pC-pB)))>0) {
				distC=distC / lenBC;
			} else if ((distC=GetCrossPoint(pC, Vector3.Normalize(pB-pC)))>0) {
				distC=1- (distC / lenBC);
			}
			if (distA>0.001f && distB>0.001f && distA<0.999f && distB<0.999f) {
				// dzielimy krawędz AB
				vertices_add[vertex_add_idx]=CrossInterpolate(vertices, triangles[i], triangles[i+1], distA);
				if (normals.Length>0) normals_add[vertex_add_idx]=CrossInterpolate(normals, triangles[i], triangles[i+1], distA);
				if (tangents.Length>0) tangents_add[vertex_add_idx]=CrossInterpolate(tangents, triangles[i], triangles[i+1], distA);
				if (_uvs.Length>0) _uvs_add[vertex_add_idx]=CrossInterpolate(_uvs, triangles[i], triangles[i+1], distA);
				if (_uvs2.Length>0) _uvs2_add[vertex_add_idx]=CrossInterpolate(_uvs2, triangles[i], triangles[i+1], distA);
				if (colors.Length>0) colors_add[vertex_add_idx]=CrossInterpolate(colors, triangles[i], triangles[i+1], distA);
				vertex_add_idx++;
				// dzielimy krawędz AC
				vertices_add[vertex_add_idx]=CrossInterpolate(vertices, triangles[i], triangles[i+2], distB);
				if (normals.Length>0) normals_add[vertex_add_idx]=CrossInterpolate(normals, triangles[i], triangles[i+2], distB);
				if (tangents.Length>0) tangents_add[vertex_add_idx]=CrossInterpolate(tangents, triangles[i], triangles[i+2], distB);
				if (_uvs.Length>0) _uvs_add[vertex_add_idx]=CrossInterpolate(_uvs, triangles[i], triangles[i+2], distB);
				if (_uvs2.Length>0) _uvs2_add[vertex_add_idx]=CrossInterpolate(_uvs2, triangles[i], triangles[i+2], distB);
				if (colors.Length>0) colors_add[vertex_add_idx]=CrossInterpolate(colors, triangles[i], triangles[i+2], distB);
				vertex_add_idx++;
				// dodaj 2 trójkąty
				triangles_add[triangle_add_idx++] = vertices.Length+vertex_add_idx-2;
				triangles_add[triangle_add_idx++] = triangles[i+1];
				triangles_add[triangle_add_idx++] = vertices.Length+vertex_add_idx-1;
				
				triangles_add[triangle_add_idx++] = vertices.Length+vertex_add_idx-1;
				triangles_add[triangle_add_idx++] = triangles[i+1];
				triangles_add[triangle_add_idx++] = triangles[i+2];
				// modyfikuj wyjściowy trójkąt
				triangles[i+1] = vertices.Length+vertex_add_idx-2;
				triangles[i+2] = vertices.Length+vertex_add_idx-1;
			} else if (distA>0.001f && distC>0.001f && distA<0.999f && distC<0.999f) {
				// dzielimy krawędz AB
				vertices_add[vertex_add_idx]=CrossInterpolate(vertices, triangles[i], triangles[i+1], distA);
				if (normals.Length>0) normals_add[vertex_add_idx]=CrossInterpolate(normals, triangles[i], triangles[i+1], distA);
				if (tangents.Length>0) tangents_add[vertex_add_idx]=CrossInterpolate(tangents, triangles[i], triangles[i+1], distA);
				if (_uvs.Length>0) _uvs_add[vertex_add_idx]=CrossInterpolate(_uvs, triangles[i], triangles[i+1], distA);
				if (_uvs2.Length>0) _uvs2_add[vertex_add_idx]=CrossInterpolate(_uvs2, triangles[i], triangles[i+1], distA);
				if (colors.Length>0) colors_add[vertex_add_idx]=CrossInterpolate(colors, triangles[i], triangles[i+1], distA);
				vertex_add_idx++;
				// dzielimy krawędz BC
				vertices_add[vertex_add_idx]=CrossInterpolate(vertices, triangles[i+1], triangles[i+2], distC);
				if (normals.Length>0) normals_add[vertex_add_idx]=CrossInterpolate(normals, triangles[i+1], triangles[i+2], distC);
				if (tangents.Length>0) tangents_add[vertex_add_idx]=CrossInterpolate(tangents, triangles[i+1], triangles[i+2], distC);
				if (_uvs.Length>0) _uvs_add[vertex_add_idx]=CrossInterpolate(_uvs, triangles[i+1], triangles[i+2], distC);
				if (_uvs2.Length>0) _uvs2_add[vertex_add_idx]=CrossInterpolate(_uvs2, triangles[i+1], triangles[i+2], distC);
				if (colors.Length>0) colors_add[vertex_add_idx]=CrossInterpolate(colors, triangles[i+1], triangles[i+2], distC);
				vertex_add_idx++;
				// dodaj 2 trójkąty
				triangles_add[triangle_add_idx++] = vertices.Length+vertex_add_idx-2;
				triangles_add[triangle_add_idx++] = vertices.Length+vertex_add_idx-1;
				triangles_add[triangle_add_idx++] = triangles[i];
				
				triangles_add[triangle_add_idx++] = vertices.Length+vertex_add_idx-2;
				triangles_add[triangle_add_idx++] = triangles[i+1];
				triangles_add[triangle_add_idx++] = vertices.Length+vertex_add_idx-1;
				// modyfikuj wyjściowy trójkąt
				triangles[i+1] = vertices.Length+vertex_add_idx-1;
			} else if (distB>0.001f && distC>0.001f && distB<0.999f && distC<0.999f) {
				// dzielimy krawędz BC
				vertices_add[vertex_add_idx]=CrossInterpolate(vertices, triangles[i+1], triangles[i+2], distC);
				if (normals.Length>0) normals_add[vertex_add_idx]=CrossInterpolate(normals, triangles[i+1], triangles[i+2], distC);
				if (tangents.Length>0) tangents_add[vertex_add_idx]=CrossInterpolate(tangents, triangles[i+1], triangles[i+2], distC);
				if (_uvs.Length>0) _uvs_add[vertex_add_idx]=CrossInterpolate(_uvs, triangles[i+1], triangles[i+2], distC);
				if (_uvs2.Length>0) _uvs2_add[vertex_add_idx]=CrossInterpolate(_uvs2, triangles[i+1], triangles[i+2], distC);
				if (colors.Length>0) colors_add[vertex_add_idx]=CrossInterpolate(colors, triangles[i+1], triangles[i+2], distC);
				vertex_add_idx++;
				// dzielimy krawędz AC
				vertices_add[vertex_add_idx]=CrossInterpolate(vertices, triangles[i], triangles[i+2], distB);
				if (normals.Length>0) normals_add[vertex_add_idx]=CrossInterpolate(normals, triangles[i], triangles[i+2], distB);
				if (tangents.Length>0) tangents_add[vertex_add_idx]=CrossInterpolate(tangents, triangles[i], triangles[i+2], distB);
				if (_uvs.Length>0) _uvs_add[vertex_add_idx]=CrossInterpolate(_uvs, triangles[i], triangles[i+2], distB);
				if (_uvs2.Length>0) _uvs2_add[vertex_add_idx]=CrossInterpolate(_uvs2, triangles[i], triangles[i+2], distB);
				if (colors.Length>0) colors_add[vertex_add_idx]=CrossInterpolate(colors, triangles[i], triangles[i+2], distB);
				vertex_add_idx++;
				// dodaj 2 trójkąty
				triangles_add[triangle_add_idx++] = vertices.Length+vertex_add_idx-1;
				triangles_add[triangle_add_idx++] = triangles[i+1];
				triangles_add[triangle_add_idx++] = vertices.Length+vertex_add_idx-2;
				
				triangles_add[triangle_add_idx++] = triangles[i+2];
				triangles_add[triangle_add_idx++] = vertices.Length+vertex_add_idx-1;
				triangles_add[triangle_add_idx++] = vertices.Length+vertex_add_idx-2;
				// modyfikuj wyjściowy trójkąt
				triangles[i+2] = vertices.Length+vertex_add_idx-1;
			}

		}
		new_mesh.vertices = MergeCrossTab(vertices, vertices_add, vertex_add_idx);
		normals=MergeCrossTab(normals, normals_add, vertex_add_idx);
		new_mesh.normals = normals;
		new_mesh.tangents = MergeCrossTab(tangents, tangents_add, vertex_add_idx);
		colors=MergeCrossTab(colors, colors_add, vertex_add_idx);
		new_mesh.colors = colors;
		new_mesh.uv = MergeCrossTab(_uvs, _uvs_add, vertex_add_idx);
		new_mesh.uv2 = MergeCrossTab(_uvs2, _uvs2_add, vertex_add_idx);
		new_mesh.triangles = MergeCrossTab(triangles, triangles_add, triangle_add_idx);
		new_mesh.RecalculateBounds();
		
		pmesh=mf.sharedMesh=new_mesh;
		mf.sharedMesh.name=orig_mesh.name+" (copy)";
		Mesh new_mesh_underlying=new Mesh();
		GameObject go;
		Transform tr=transform.FindChild("RTP_blend_underlying");
		MeshRenderer mr;
		if (tr==null) {
			go=new GameObject("RTP_blend_underlying");
			tr=go.transform;
			mf=(MeshFilter)go.AddComponent(typeof(MeshFilter));
			mr=(MeshRenderer)go.AddComponent(typeof(MeshRenderer));
			tr.parent=transform;
			tr.localScale=Vector3.one;
			tr.localPosition=Vector3.zero;
			tr.localRotation=Quaternion.identity;
			go.AddComponent(typeof(SelectorHelperClass));
		} else {
			go=tr.gameObject;
			mf=(MeshFilter)go.GetComponent(typeof(MeshFilter));
			mr=(MeshRenderer)go.GetComponent(typeof(MeshRenderer));
		}

		vertices=new_mesh.vertices;
		normals=new_mesh.normals;
		Color[] colors2=new Color[vertices.Length];
		if (!VoxelBlendedObject) {
			for(int i=0; i<colors.Length; i++) {
				Vector3 normal_world=tr.TransformDirection(normals[i]);
				colors2[i].r=(normal_world.x+1)*0.5f;
				colors2[i].g=(normal_world.y+1)*0.5f;
				colors2[i].b=(normal_world.z+1)*0.5f;
				colors2[i].a=colors[i].a;
			}		
		}
		
		Terrain terrain=null;
		tangents=null;
		Vector4[] blended_tangents=null;
		Vector3[] blended_normals=null;
		//  for voxel terrains
		Color[] blended_colors=null;
		//Vector2[] blended_uvs=null;
		int[] tris_blended=null;
		_uvs=new Vector2[vertices.Length];
		_uvs2=new Vector2[vertices.Length];
		comp=blendedObject.collider.GetComponent(typeof(Terrain));
		if (comp) {
			terrain=(Terrain)comp;
		}
		if (!terrain) {
			MeshFilter meshFilter_blended=(MeshFilter)blendedObject.GetComponent(typeof(MeshFilter));
			if (meshFilter_blended==null) {
				Debug.LogError("Underlying blended object is missing MeshFilter component !");
				return false;
			}
			Mesh mesh_blended=meshFilter_blended.sharedMesh;
			blended_tangents=mesh_blended.tangents;
			blended_normals=mesh_blended.normals;
			blended_colors=mesh_blended.colors;			
			//blended_uvs=mesh_blended.uv;
			tris_blended=mesh_blended.triangles;
			if (blended_tangents!=null && blended_tangents.Length>0) {
				tangents=new Vector4[vertices.Length];
			}
		}
		
		for(int i=0; i<colors.Length; i++) {
		  	RaycastHit[] hits;
			bool hit_flag=false;
			Vector3 vertexWorldPos=transform.TransformPoint(vertices[i]);
			int minIdx=0;
			if (terrain) {
				hits=Physics.RaycastAll(vertexWorldPos+Vector3.up*20, Vector3.down, 100);
				int o;
				for(o=0; o<hits.Length; o++) {
					if (hits[o].collider==blendedObject.collider) {
						hit_flag=true;
						break;
					}
				}
			} else {
				MeshFilter meshFilter_blended=(MeshFilter)blendedObject.GetComponent(typeof(MeshFilter));
				Vector3[] blended_vertices=meshFilter_blended.sharedMesh.vertices;
				float minDist=Vector3.SqrMagnitude(blendedObject.transform.TransformPoint(blended_vertices[0])-vertexWorldPos);
				for(int k=1; k<blended_vertices.Length; k++) {
					float dist=Vector3.SqrMagnitude(blendedObject.transform.TransformPoint(blended_vertices[k])-vertexWorldPos);
					if (dist<minDist) {
						minDist=dist;
						minIdx=k;
					}
				}
			}
			if (hit_flag || !terrain) {
				Vector3 ground_norm;
				Vector2 _uv, _uv2;
				if (terrain) {
					ground_norm=terrain.terrainData.GetInterpolatedNormal((vertexWorldPos.x-terrain.transform.position.x)/terrain.terrainData.size.x, (vertexWorldPos.z-terrain.transform.position.z)/terrain.terrainData.size.z);
					ground_norm.Normalize();
					_uv=new Vector2((vertexWorldPos.x-terrain.transform.position.x)/terrain.terrainData.size.x, (vertexWorldPos.z-terrain.transform.position.z)/terrain.terrainData.size.z);
					_uv2=new Vector2(_uv.x, _uv.y);
					ground_norm=tr.InverseTransformDirection(ground_norm);
					normals[i]=ground_norm;
					_uvs[i]=_uv;
					_uvs2[i]=_uv2;					
				} else {
					//hits=Physics.RaycastAll(vertexWorldPos+hits[o].normal*20, -hits[o].normal, 100);
					Vector3 closestNorm=blendedObject.transform.TransformDirection(blended_normals[minIdx]);
					hits=Physics.RaycastAll(vertexWorldPos+closestNorm*20, -closestNorm, 100);
					int o;
					for(o=0; o<hits.Length; o++) if (hits[o].collider==blendedObject.collider) break;
					if (o<hits.Length) {
						Vector3 b_coords=hits[o].barycentricCoordinate;
						int tri_idx=hits[o].triangleIndex*3;
						Vector3 normal1=blended_normals[tris_blended[tri_idx + 0]];
						Vector3 normal2=blended_normals[tris_blended[tri_idx + 1]];
						Vector3 normal3=blended_normals[tris_blended[tri_idx + 2]];
						ground_norm = normal1 * b_coords.x + normal2 * b_coords.y + normal3 * b_coords.z;
						ground_norm.Normalize();
						ground_norm=blendedObject.transform.TransformDirection(ground_norm);
						ground_norm=tr.InverseTransformDirection(ground_norm);
						
						VoxelBlendedObject=blendedObject && blendedObject.GetComponent(typeof(ReliefTerrainVertexBlendTriplanar))!=null;
						if (blended_colors!=null && VoxelBlendedObject) {
							Color color1=blended_colors[tris_blended[tri_idx + 0]];
							Color color2=blended_colors[tris_blended[tri_idx + 1]];
							Color color3=blended_colors[tris_blended[tri_idx + 2]];
							Color ground_color = color1 * b_coords.x + color2 * b_coords.y + color3 * b_coords.z;
							colors[i] = ground_color;
						}
						
						_uv=hits[o].textureCoord;
						_uv2=hits[o].lightmapCoord;
						
						if (	tangents!=null) {
							Vector4 tangent1=blended_tangents[tris_blended[tri_idx + 0]];
							Vector4 tangent2=blended_tangents[tris_blended[tri_idx + 1]];
							Vector4 tangent3=blended_tangents[tris_blended[tri_idx + 2]];
							Vector3 ground_tangent = new Vector3(tangent1.x, tangent1.y, tangent1.z) * b_coords.x + new Vector3(tangent2.x, tangent2.y, tangent2.z) * b_coords.y + new Vector3(tangent3.x, tangent3.y, tangent3.z) * b_coords.z;
							ground_tangent.Normalize();
							ground_tangent=blendedObject.transform.TransformDirection(ground_tangent);
							ground_tangent=tr.InverseTransformDirection(ground_tangent);
							tangents[i] = new Vector4(ground_tangent.x, ground_tangent.y, ground_tangent.z, -1);
						}
						normals[i]=ground_norm;
						_uvs[i]=_uv;
						_uvs2[i]=_uv2;
					}
				}
			} else {
				Debug.LogError("Can't find blended object (collider) below an object vertex "+vertexWorldPos+" !");
				//return false;
			}
			//colors[i].a=0;
		}
		new_mesh_underlying.vertices=vertices;
		new_mesh_underlying.triangles=new_mesh.triangles;
		new_mesh_underlying.normals=normals;
		if (VoxelBlendedObject) {
			new_mesh_underlying.colors=colors;
		} else {
			new_mesh_underlying.colors=colors2;
		}
		new_mesh_underlying.uv=_uvs;
		new_mesh_underlying.uv2=_uvs2;
		if (tangents!=null) {
			new_mesh_underlying.tangents=tangents;
		}
		mf.sharedMesh=new_mesh_underlying;
		mr.castShadows=false;
		mr.receiveShadows=true;
		go.isStatic=false;
		if (!Sticked || !StickedOptimized) {
			if (terrain) {
				mr.sharedMaterial=new Material(Shader.Find("Relief Pack/ReliefTerrainGeometryBlendBase"));
			} else {
				if (VoxelBlendedObject) {
					mr.sharedMaterial=blendedObject.renderer.sharedMaterial;
				} else {
					mr.sharedMaterial=new Material(Shader.Find("Relief Pack/ReliefTerrain2GeometryBlendBase"));
				}
			}
		}
		//ClearBlend();
		return true;
	}
	
	
	// działa jak meshcopy usuwając niewidoczne wierzchołki i ich trójkąty spod powierzchni gruntu (niewidoczne)
	public void GetTrisBelowGroundLevel(ref int cover_num, int[] cover_tris) {
		MeshFilter mf=(MeshFilter)GetComponent(typeof(MeshFilter));
		if (mf==null) return;
		Mesh sh=mf.sharedMesh;
		if (sh==null) return;
		
		// rozsuwanie vertexów
		Vector3[] vertices=sh.vertices;
		int[] triangles=sh.triangles;
		cover_num=0;
		for(int i=0; i<triangles.Length; i+=3) {
			Vector3 pA=transform.TransformPoint(vertices[triangles[i]]);
			Vector3 pB=transform.TransformPoint(vertices[triangles[i+1]]);
			Vector3 pC=transform.TransformPoint(vertices[triangles[i+2]]);
			
			if (IsBelowGround(pA) && IsBelowGround(pB) && IsBelowGround(pC)) {
				cover_tris[cover_num]=i;
				cover_num++;
			}
		}
	}	
	
	public bool MakeMeshCopy() {
		MeshFilter mf=(MeshFilter)GetComponent(typeof(MeshFilter));
		Mesh sh=((MeshFilter)GetComponent(typeof(MeshFilter))).sharedMesh;
		if (!sh) return false;
		Mesh new_mesh=(Mesh)Instantiate(sh);
		pmesh=mf.sharedMesh=new_mesh;
		mf.sharedMesh.name=orig_mesh.name+" (copy)";
		Mesh new_mesh_underlying=new Mesh();
		GameObject go;
		Transform tr=transform.FindChild("RTP_blend_underlying");
		MeshRenderer mr;
		if (tr==null) {
			go=new GameObject("RTP_blend_underlying");
			tr=go.transform;
			mf=(MeshFilter)go.AddComponent(typeof(MeshFilter));
			mr=(MeshRenderer)go.AddComponent(typeof(MeshRenderer));
			tr.parent=transform;
			tr.localScale=Vector3.one;
			tr.localPosition=Vector3.zero;
			tr.localRotation=Quaternion.identity;
			go.AddComponent(typeof(SelectorHelperClass));
		} else {
			go=tr.gameObject;
			mf=(MeshFilter)go.GetComponent(typeof(MeshFilter));
			mr=(MeshRenderer)go.GetComponent(typeof(MeshRenderer));
		}

		Vector3[] vertices=new_mesh.vertices;
		Vector3[] normals=new_mesh.normals;
		Color[] colors=new Color[vertices.Length];
		for(int i=0; i<colors.Length; i++) {
			Vector3 normal_world=tr.TransformDirection(normals[i]);
			colors[i].r=(normal_world.x+1)*0.5f;
			colors[i].g=(normal_world.y+1)*0.5f;
			colors[i].b=(normal_world.z+1)*0.5f;
			colors[i].a=0;
		}
		
		Terrain terrain=null;
		Vector4[] tangents=null;
		Vector4[] blended_tangents=null;
		Vector3[] blended_normals=null;
		//  for voxel terrains
		Color[] blended_colors=null;
		//Vector2[] blended_uvs=null;
		int[] tris_blended=null;
		Vector2[] _uvs=new Vector2[vertices.Length];
		Vector2[] _uvs2=new Vector2[vertices.Length];
		Component comp=blendedObject.collider.GetComponent(typeof(Terrain));
		if (comp) {
			terrain=(Terrain)comp;
		}
		if (!terrain) {
			MeshFilter meshFilter_blended=(MeshFilter)blendedObject.GetComponent(typeof(MeshFilter));
			if (meshFilter_blended==null) {
				Debug.LogError("Underlying blended object is missing MeshFilter component !");
				return false;
			}
			Mesh mesh_blended=meshFilter_blended.sharedMesh;
			blended_tangents=mesh_blended.tangents;
			blended_normals=mesh_blended.normals;
			blended_colors=mesh_blended.colors;
			
			//blended_uvs=mesh_blended.uv;
			tris_blended=mesh_blended.triangles;
			if (blended_tangents!=null && blended_tangents.Length>0) {
				tangents=new Vector4[vertices.Length];
			}
		}
		
		for(int i=0; i<colors.Length; i++) {
		  	RaycastHit[] hits;
			bool hit_flag=false;
			Vector3 vertexWorldPos=transform.TransformPoint(vertices[i]);
			int minIdx=0;
			if (terrain) {
			    hits=Physics.RaycastAll(vertexWorldPos+Vector3.up*20, Vector3.down, 100);
				int o;
				for(o=0; o<hits.Length; o++) {
					if (hits[o].collider==blendedObject.collider) {
						hit_flag=true;
						break;
					}
				}
			} else {
				MeshFilter meshFilter_blended=(MeshFilter)blendedObject.GetComponent(typeof(MeshFilter));
				Vector3[] blended_vertices=meshFilter_blended.sharedMesh.vertices;
				float minDist=Vector3.SqrMagnitude(blendedObject.transform.TransformPoint(blended_vertices[0])-vertexWorldPos);
				for(int k=1; k<blended_vertices.Length; k++) {
					float dist=Vector3.SqrMagnitude(blendedObject.transform.TransformPoint(blended_vertices[k])-vertexWorldPos);
					if (dist<minDist) {
						minDist=dist;
						minIdx=k;
					}
				}
			}
			if (hit_flag || !terrain) {
				Vector3 ground_norm;
				Vector2 _uv, _uv2;
				if (terrain) {
					ground_norm=terrain.terrainData.GetInterpolatedNormal((vertexWorldPos.x-terrain.transform.position.x)/terrain.terrainData.size.x, (vertexWorldPos.z-terrain.transform.position.z)/terrain.terrainData.size.z);
					ground_norm.Normalize();
					_uv=new Vector2((vertexWorldPos.x-terrain.transform.position.x)/terrain.terrainData.size.x, (vertexWorldPos.z-terrain.transform.position.z)/terrain.terrainData.size.z);
					_uv2=new Vector2(_uv.x, _uv.y);
					ground_norm=tr.InverseTransformDirection(ground_norm);
					normals[i]=ground_norm;
					_uvs[i]=_uv;
					_uvs2[i]=_uv2;					
				} else {
				    //hits=Physics.RaycastAll(vertexWorldPos+hits[o].normal*20, -hits[o].normal, 100);
					Vector3 closestNorm=blendedObject.transform.TransformDirection(blended_normals[minIdx]);
					hits=Physics.RaycastAll(vertexWorldPos+closestNorm*20, -closestNorm, 100);
					int o;
					for(o=0; o<hits.Length; o++) if (hits[o].collider==blendedObject.collider) break;
					if (o<hits.Length) {
						Vector3 b_coords=hits[o].barycentricCoordinate;
						int tri_idx=hits[o].triangleIndex*3;
						Vector3 normal1=blended_normals[tris_blended[tri_idx + 0]];
						Vector3 normal2=blended_normals[tris_blended[tri_idx + 1]];
						Vector3 normal3=blended_normals[tris_blended[tri_idx + 2]];
						ground_norm = normal1 * b_coords.x + normal2 * b_coords.y + normal3 * b_coords.z;
						ground_norm.Normalize();
						ground_norm=blendedObject.transform.TransformDirection(ground_norm);
						ground_norm=tr.InverseTransformDirection(ground_norm);
						
						VoxelBlendedObject=blendedObject && blendedObject.GetComponent(typeof(ReliefTerrainVertexBlendTriplanar))!=null;
						if (blended_colors!=null && VoxelBlendedObject) {
							Color color1=blended_colors[tris_blended[tri_idx + 0]];
							Color color2=blended_colors[tris_blended[tri_idx + 1]];
							Color color3=blended_colors[tris_blended[tri_idx + 2]];
							Color ground_color = color1 * b_coords.x + color2 * b_coords.y + color3 * b_coords.z;
							colors[i] = ground_color;
						}
	
						_uv=hits[o].textureCoord;
						_uv2=hits[o].lightmapCoord;
						
						if (	tangents!=null) {
							Vector4 tangent1=blended_tangents[tris_blended[tri_idx + 0]];
							Vector4 tangent2=blended_tangents[tris_blended[tri_idx + 1]];
							Vector4 tangent3=blended_tangents[tris_blended[tri_idx + 2]];
							Vector3 ground_tangent = new Vector3(tangent1.x, tangent1.y, tangent1.z) * b_coords.x + new Vector3(tangent2.x, tangent2.y, tangent2.z) * b_coords.y + new Vector3(tangent3.x, tangent3.y, tangent3.z) * b_coords.z;
							ground_tangent.Normalize();
							ground_tangent=blendedObject.transform.TransformDirection(ground_tangent);
							ground_tangent=tr.InverseTransformDirection(ground_tangent);
							tangents[i] = new Vector4(ground_tangent.x, ground_tangent.y, ground_tangent.z, -1);
						}
						normals[i]=ground_norm;
						_uvs[i]=_uv;
						_uvs2[i]=_uv2;
					}
				}
			} else {
				Debug.LogError("Can't find blended object (collider) below an object vertex "+vertexWorldPos+" !");
				//return false;
			}
		}
		new_mesh_underlying.vertices=vertices;
		new_mesh_underlying.triangles=new_mesh.triangles;
		new_mesh_underlying.normals=normals;
		new_mesh_underlying.colors=colors;
		new_mesh_underlying.uv=_uvs;
		new_mesh_underlying.uv2=_uvs2;
		if (tangents!=null) {
			new_mesh_underlying.tangents=tangents;
		}
		mf.sharedMesh=new_mesh_underlying;
		mr.castShadows=false;
		mr.receiveShadows=true;
		go.isStatic=false;
		if (!Sticked || !StickedOptimized) {
			if (terrain) {
				mr.sharedMaterial=new Material(Shader.Find("Relief Pack/ReliefTerrainGeometryBlendBase"));
			} else {
				if (VoxelBlendedObject) {
					mr.sharedMaterial=blendedObject.renderer.sharedMaterial;
				} else {
					mr.sharedMaterial=new Material(Shader.Find("Relief Pack/ReliefTerrain2GeometryBlendBase"));
				}
			}
		}
		if (shader_global_blend_capabilities) SyncMaterialProps();
		
		ClearBlend();
		return true;
	}	
	
	public void SyncMaterialProps() {
		if (!blendedObject) return;
		
		ReliefTerrain script_terrain=(ReliefTerrain)blendedObject.GetComponent(typeof(ReliefTerrain));
		if (!script_terrain && VoxelBlendedObject) {
			// sync from voxel object material
			Vector4 underlying_size=Vector4.zero;
			if (blendedObject.renderer.sharedMaterial.HasProperty("_TERRAIN_PosSize")) underlying_size=blendedObject.renderer.sharedMaterial.GetVector("_TERRAIN_PosSize");
			renderer.sharedMaterial.SetVector("_TERRAIN_PosSize", underlying_size);
			float tile_size=1;
			if (blendedObject.renderer.sharedMaterial.HasProperty("_TERRAIN_ReliefTransformTriplanarZ")) tile_size=blendedObject.renderer.sharedMaterial.GetFloat("_TERRAIN_ReliefTransformTriplanarZ");
			renderer.sharedMaterial.SetVector("_TERRAIN_Tiling", new Vector4(tile_size,tile_size,0,0));

			SyncFloat("_TERRAIN_distance_start");
			SyncFloat("_TERRAIN_distance_transition");
			SyncFloat("_TERRAIN_distance_start_bumpglobal");
			SyncFloat("_TERRAIN_distance_transition_bumpglobal");
			
			SyncFloatBoth("TERRAIN_IBLRefl_SpecAO_Damp");
			
			SyncTexture("_ColorMapGlobal");
			SyncVectorBoth("_GlobalColorMapBlendValues");
			SyncFloatBoth("_GlobalColorMapSaturation");
			SyncFloatBoth("_GlobalColorMapSaturationFar");
			SyncFloatBoth("_GlobalColorMapBrightness");
			SyncFloatBoth("_GlobalColorMapBrightnessFar");
			SyncFloatBoth("_GlobalColorMapNearMIP");
			SyncFloatBoth("_GlobalColorMapDistortByPerlin");
			
			SyncFloatBoth("RTP_ReflexLightDiffuseSoftness");
			SyncColorBoth("RTP_ReflexLightDiffuseColor1");
			SyncColorBoth("RTP_ReflexLightDiffuseColor2");
			SyncColorBoth("RTP_ReflexLightSpecColor");
			SyncFloatBoth("RTP_ReflexLightSpecularity");
			SyncFloatBoth("RTP_BackLightStrength");
			
			SyncColorBoth("rtp_customAmbientCorrection");
			
			SyncColorBoth("_FColor");
			SyncFloatBoth("_Fdensity");
			SyncFloatBoth("_Fstart");
			SyncFloatBoth("_Fend");
					
			SyncTexture("_BumpMapGlobal");
			SyncTexture("_BumpMapGlobal", "_BumpMapPerlin"); //  different name on the texture in legacy RTP3.0 geom blend shaders
			SyncFloat("_BumpMapGlobalScale");
			SyncFloat("rtp_perlin_start_val");
			SyncFloat("rtp_mipoffset_globalnorm_offset");
			//SyncFloat("_FarNormalDamp");
			
			SyncColor("TERRAIN_ReflColorA");
			SyncColor("TERRAIN_ReflColorB");
			SyncColor("TERRAIN_ReflColorC");
			SyncFloat("TERRAIN_ReflColorCenter");
			SyncFloat("TERRAIN_ReflGlossAttenuation");
			SyncFloat("TERRAIN_ReflectionRotSpeed");
			
			SyncFloat("TERRAIN_WetHeight_Treshold");
			SyncFloat("TERRAIN_WetHeight_Transition");
			SyncFloat("TERRAIN_FlowSpeed");
			SyncFloat("TERRAIN_FlowCycleScale");
			SyncFloat("TERRAIN_FlowScale");
			SyncFloat("TERRAIN_FlowMipOffset");
			SyncFloat("TERRAIN_mipoffset_flowSpeed");
			SyncFloat("TERRAIN_WetDarkening");
			
			SyncTexture("TERRAIN_RippleMap");
			SyncFloat("TERRAIN_RainIntensity");
			SyncFloat("TERRAIN_WetDropletsStrength");
			SyncFloat("TERRAIN_DropletsSpeed");
			SyncFloat("TERRAIN_RippleScale");
		
			SyncTexture("_VerticalTexture");
			SyncFloat("_VerticalTextureTiling");
			SyncFloat("_VerticalTextureGlobalBumpInfluence");
			
			SyncFloat("rtp_snow_strength");
			SyncFloat("rtp_snow_slope_factor");
			SyncFloat("rtp_snow_height_treshold");
			SyncFloat("rtp_snow_height_transition");
			SyncColor("rtp_snow_color");
			SyncFloat("rtp_snow_specular");
			SyncFloat("rtp_snow_gloss");
			SyncFloat("rtp_snow_fresnel");
			SyncFloat("rtp_snow_diff_fresnel");
			SyncFloat("rtp_snow_IBL_SpecStrength");
			SyncFloat("rtp_snow_edge_definition");
			SyncFloat("rtp_snow_deep_factor");
			
			SyncFloat("TERRAIN_CausticsAnimSpeed");
			SyncColor("TERRAIN_CausticsColor");
			SyncFloat("TERRAIN_CausticsWaterLevel");
			SyncFloat("TERRAIN_CausticsWaterLevelByAngle");
			SyncFloat("TERRAIN_CausticsWaterShallowFadeLength");
			SyncFloat("TERRAIN_CausticsWaterDeepFadeLength");
			SyncFloat("TERRAIN_CausticsTilingScale");
			SyncTexture("TERRAIN_CausticsTex");
			
			SyncTexture("_AmbientEmissiveMapGlobal");
			SyncFloat("_AmbientEmissiveMultiplier");
			SyncFloat("_AmbientEmissiveRelief");
			SyncFloat("_shadow_distance_start");
			SyncFloat("_shadow_distance_transition");
			SyncFloat("_shadow_value");
			
			SyncFloat("TERRAIN_IBL_DiffAO_Damp");
			SyncFloat("TERRAIN_IBL_SpecAO_Damp");
			
		} else if (script_terrain) {
			
			// sync from ReliefTerrain object
			renderer.sharedMaterial.SetVector("_TERRAIN_PosSize", new Vector4(blendedObject.transform.position.x, blendedObject.transform.position.z, script_terrain.globalSettingsHolder.terrainTileSizeX, script_terrain.globalSettingsHolder.terrainTileSizeZ));
			float tile_sizex=script_terrain.globalSettingsHolder.terrainTileSizeX/script_terrain.globalSettingsHolder.ReliefTransform.x;
			float tile_sizey=script_terrain.globalSettingsHolder.terrainTileSizeZ/script_terrain.globalSettingsHolder.ReliefTransform.y;
			renderer.sharedMaterial.SetVector("_TERRAIN_Tiling", new Vector4(script_terrain.globalSettingsHolder.terrainTileSizeX/script_terrain.globalSettingsHolder.ReliefTransform.x, script_terrain.globalSettingsHolder.terrainTileSizeZ/script_terrain.globalSettingsHolder.ReliefTransform.y, script_terrain.globalSettingsHolder.ReliefTransform.z*tile_sizex, script_terrain.globalSettingsHolder.ReliefTransform.w*tile_sizey));
			renderer.sharedMaterial.SetFloat("_TERRAIN_distance_start", script_terrain.globalSettingsHolder.distance_start);
			renderer.sharedMaterial.SetFloat("_TERRAIN_distance_transition", script_terrain.globalSettingsHolder.distance_transition);
			renderer.sharedMaterial.SetFloat("_TERRAIN_distance_start_bumpglobal", script_terrain.globalSettingsHolder.distance_start_bumpglobal);
			renderer.sharedMaterial.SetFloat("_TERRAIN_distance_transition_bumpglobal", script_terrain.globalSettingsHolder.distance_transition_bumpglobal);
			
			renderer.sharedMaterial.SetFloat("TERRAIN_IBLRefl_SpecAO_Damp", script_terrain.globalSettingsHolder.TERRAIN_IBLRefl_SpecAO_Damp);
			
			if (script_terrain.ColorGlobal) {
				renderer.sharedMaterial.SetTexture("_ColorMapGlobal", script_terrain.ColorGlobal);
			}
			
			renderer.sharedMaterial.SetVector("_GlobalColorMapBlendValues", script_terrain.globalSettingsHolder.GlobalColorMapBlendValues);
			renderer.sharedMaterial.SetFloat("_GlobalColorMapSaturation", script_terrain.globalSettingsHolder.GlobalColorMapSaturation);
			renderer.sharedMaterial.SetFloat("_GlobalColorMapSaturationFar", script_terrain.globalSettingsHolder.GlobalColorMapSaturationFar);
			renderer.sharedMaterial.SetFloat("_GlobalColorMapBrightness", script_terrain.globalSettingsHolder.GlobalColorMapBrightness);
			renderer.sharedMaterial.SetFloat("_GlobalColorMapBrightnessFar", script_terrain.globalSettingsHolder.GlobalColorMapBrightnessFar);
			renderer.sharedMaterial.SetFloat("_GlobalColorMapNearMIP", script_terrain.globalSettingsHolder._GlobalColorMapNearMIP);
			renderer.sharedMaterial.SetFloat("_GlobalColorMapDistortByPerlin", script_terrain.globalSettingsHolder.GlobalColorMapDistortByPerlin);
			
			renderer.sharedMaterial.SetFloat("RTP_ReflexLightDiffuseSoftness", script_terrain.globalSettingsHolder.RTP_LightDefVector.y);
			renderer.sharedMaterial.SetColor("RTP_ReflexLightDiffuseColor1", script_terrain.globalSettingsHolder.RTP_ReflexLightDiffuseColor);
			renderer.sharedMaterial.SetColor("RTP_ReflexLightDiffuseColor2", script_terrain.globalSettingsHolder.RTP_ReflexLightDiffuseColor2);
			renderer.sharedMaterial.SetColor("RTP_ReflexLightSpecColor", script_terrain.globalSettingsHolder.RTP_ReflexLightSpecColor);
			renderer.sharedMaterial.SetFloat("RTP_ReflexLightSpecularity", script_terrain.globalSettingsHolder.RTP_LightDefVector.w);
			renderer.sharedMaterial.SetFloat("RTP_BackLightStrength", script_terrain.globalSettingsHolder.RTP_LightDefVector.x);
			renderer.sharedMaterial.SetColor("rtp_customAmbientCorrection", script_terrain.globalSettingsHolder.rtp_customAmbientCorrection);
			
			renderer.sharedMaterial.SetColor("_FColor", RenderSettings.fogColor);
			renderer.sharedMaterial.SetFloat("_Fdensity",  RenderSettings.fogDensity);
			renderer.sharedMaterial.SetFloat("_Fstart",  RenderSettings.fogStartDistance);
			renderer.sharedMaterial.SetFloat("_Fend",  RenderSettings.fogEndDistance);
					
			if (script_terrain.BumpGlobalCombined) {
				renderer.sharedMaterial.SetTexture("_BumpMapGlobal", script_terrain.BumpGlobalCombined);
				renderer.sharedMaterial.SetTexture("_BumpMapPerlin", script_terrain.BumpGlobalCombined); //  different name on the texture in legacy RTP3.0 geom blend shaders
			}
			renderer.sharedMaterial.SetFloat("_BumpMapGlobalScale", script_terrain.globalSettingsHolder.BumpMapGlobalScale);
			renderer.sharedMaterial.SetFloat("rtp_perlin_start_val", script_terrain.globalSettingsHolder.rtp_perlin_start_val);
			renderer.sharedMaterial.SetFloat("rtp_mipoffset_globalnorm_offset", script_terrain.globalSettingsHolder.rtp_mipoffset_globalnorm);
			//renderer.sharedMaterial.SetFloat("_FarNormalDamp", script_terrain.globalSettingsHolder._FarNormalDamp);
			
			renderer.sharedMaterial.SetColor("TERRAIN_ReflColorA", script_terrain.globalSettingsHolder.TERRAIN_ReflColorA);
			renderer.sharedMaterial.SetColor("TERRAIN_ReflColorB", script_terrain.globalSettingsHolder.TERRAIN_ReflColorB);
			renderer.sharedMaterial.SetColor("TERRAIN_ReflColorC", script_terrain.globalSettingsHolder.TERRAIN_ReflColorC);
			renderer.sharedMaterial.SetFloat("TERRAIN_ReflColorCenter", script_terrain.globalSettingsHolder.TERRAIN_ReflColorCenter);
			renderer.sharedMaterial.SetFloat("TERRAIN_ReflGlossAttenuation", script_terrain.globalSettingsHolder.TERRAIN_ReflGlossAttenuation);
			renderer.sharedMaterial.SetFloat("TERRAIN_ReflectionRotSpeed", script_terrain.globalSettingsHolder.TERRAIN_ReflectionRotSpeed);
			
			renderer.sharedMaterial.SetFloat("TERRAIN_WetHeight_Treshold", script_terrain.globalSettingsHolder.TERRAIN_WetHeight_Treshold);
			renderer.sharedMaterial.SetFloat("TERRAIN_WetHeight_Transition", script_terrain.globalSettingsHolder.TERRAIN_WetHeight_Transition);
			renderer.sharedMaterial.SetFloat("TERRAIN_FlowSpeed", script_terrain.globalSettingsHolder.TERRAIN_FlowSpeed);
			renderer.sharedMaterial.SetFloat("TERRAIN_FlowCycleScale", script_terrain.globalSettingsHolder.TERRAIN_FlowCycleScale);
			renderer.sharedMaterial.SetFloat("TERRAIN_FlowScale", script_terrain.globalSettingsHolder.TERRAIN_FlowScale);
			renderer.sharedMaterial.SetFloat("TERRAIN_FlowMipOffset", script_terrain.globalSettingsHolder.TERRAIN_FlowMipOffset);
			renderer.sharedMaterial.SetFloat("TERRAIN_mipoffset_flowSpeed", script_terrain.globalSettingsHolder.TERRAIN_mipoffset_flowSpeed);
			renderer.sharedMaterial.SetFloat("TERRAIN_WetDarkening", script_terrain.globalSettingsHolder.TERRAIN_WetDarkening);
			
			if (script_terrain.globalSettingsHolder.TERRAIN_RippleMap) renderer.sharedMaterial.SetTexture("TERRAIN_RippleMap", script_terrain.globalSettingsHolder.TERRAIN_RippleMap);
			renderer.sharedMaterial.SetFloat("TERRAIN_RainIntensity", script_terrain.globalSettingsHolder.TERRAIN_RainIntensity);
			renderer.sharedMaterial.SetFloat("TERRAIN_WetDropletsStrength", script_terrain.globalSettingsHolder.TERRAIN_WetDropletsStrength);
			renderer.sharedMaterial.SetFloat("TERRAIN_DropletsSpeed", script_terrain.globalSettingsHolder.TERRAIN_DropletsSpeed);
			renderer.sharedMaterial.SetFloat("TERRAIN_RippleScale", script_terrain.globalSettingsHolder.TERRAIN_RippleScale);
		
			if (script_terrain.globalSettingsHolder.VerticalTexture) renderer.sharedMaterial.SetTexture("_VerticalTexture", script_terrain.globalSettingsHolder.VerticalTexture);
			renderer.sharedMaterial.SetFloat("_VerticalTextureTiling", script_terrain.globalSettingsHolder.VerticalTextureTiling);
			renderer.sharedMaterial.SetFloat("_VerticalTextureGlobalBumpInfluence", script_terrain.globalSettingsHolder.VerticalTextureGlobalBumpInfluence);
			
			renderer.sharedMaterial.SetFloat("rtp_snow_strength", script_terrain.globalSettingsHolder._snow_strength);
			renderer.sharedMaterial.SetFloat("rtp_snow_slope_factor", script_terrain.globalSettingsHolder._snow_slope_factor);
			renderer.sharedMaterial.SetFloat("rtp_snow_height_treshold", script_terrain.globalSettingsHolder._snow_height_treshold);
			renderer.sharedMaterial.SetFloat("rtp_snow_height_transition", script_terrain.globalSettingsHolder._snow_height_transition);
			renderer.sharedMaterial.SetColor("rtp_snow_color", script_terrain.globalSettingsHolder._snow_color);
			renderer.sharedMaterial.SetFloat("rtp_snow_specular", script_terrain.globalSettingsHolder._snow_specular);
			renderer.sharedMaterial.SetFloat("rtp_snow_gloss", script_terrain.globalSettingsHolder._snow_gloss);
			renderer.sharedMaterial.SetFloat("rtp_snow_fresnel", script_terrain.globalSettingsHolder._snow_fresnel);
			renderer.sharedMaterial.SetFloat("rtp_snow_diff_fresnel", script_terrain.globalSettingsHolder._snow_diff_fresnel);
			renderer.sharedMaterial.SetFloat("rtp_snow_IBL_DiffuseStrength", script_terrain.globalSettingsHolder._snow_IBL_DiffuseStrength);
			renderer.sharedMaterial.SetFloat("rtp_snow_IBL_SpecStrength", script_terrain.globalSettingsHolder._snow_IBL_SpecStrength);
			renderer.sharedMaterial.SetFloat("rtp_snow_edge_definition", script_terrain.globalSettingsHolder._snow_edge_definition);
			renderer.sharedMaterial.SetFloat("rtp_snow_deep_factor", script_terrain.globalSettingsHolder._snow_deep_factor);
			
			renderer.sharedMaterial.SetFloat("TERRAIN_CausticsAnimSpeed", script_terrain.globalSettingsHolder.TERRAIN_CausticsAnimSpeed);
			renderer.sharedMaterial.SetColor("TERRAIN_CausticsColor", script_terrain.globalSettingsHolder.TERRAIN_CausticsColor);
			renderer.sharedMaterial.SetFloat("TERRAIN_CausticsWaterLevel", script_terrain.globalSettingsHolder.TERRAIN_CausticsWaterLevel);
			renderer.sharedMaterial.SetFloat("TERRAIN_CausticsWaterLevelByAngle", script_terrain.globalSettingsHolder.TERRAIN_CausticsWaterLevelByAngle);
			renderer.sharedMaterial.SetFloat("TERRAIN_CausticsWaterShallowFadeLength", script_terrain.globalSettingsHolder.TERRAIN_CausticsWaterShallowFadeLength);
			renderer.sharedMaterial.SetFloat("TERRAIN_CausticsWaterDeepFadeLength", script_terrain.globalSettingsHolder.TERRAIN_CausticsWaterDeepFadeLength);
			renderer.sharedMaterial.SetFloat("TERRAIN_CausticsTilingScale", script_terrain.globalSettingsHolder.TERRAIN_CausticsTilingScale);
			if (script_terrain.globalSettingsHolder.TERRAIN_CausticsTex) renderer.sharedMaterial.SetTexture("TERRAIN_CausticsTex", script_terrain.globalSettingsHolder.TERRAIN_CausticsTex);
			
			if (script_terrain.AmbientEmissiveMap) renderer.sharedMaterial.SetTexture("_AmbientEmissiveMapGlobal", script_terrain.AmbientEmissiveMap);
			renderer.sharedMaterial.SetFloat("_AmbientEmissiveMultiplier", script_terrain.globalSettingsHolder._AmbientEmissiveMultiplier);
			renderer.sharedMaterial.SetFloat("_AmbientEmissiveRelief", script_terrain.globalSettingsHolder._AmbientEmissiveRelief);
			renderer.sharedMaterial.SetFloat("_shadow_distance_start", script_terrain.globalSettingsHolder.trees_shadow_distance_start);
			renderer.sharedMaterial.SetFloat("_shadow_distance_transition", script_terrain.globalSettingsHolder.trees_shadow_distance_transition);
			renderer.sharedMaterial.SetFloat("_shadow_value", script_terrain.globalSettingsHolder.trees_shadow_value);
		
			renderer.sharedMaterial.SetFloat("TERRAIN_IBL_DiffAO_Damp", script_terrain.globalSettingsHolder.TERRAIN_IBL_DiffAO_Damp);
			renderer.sharedMaterial.SetFloat("TERRAIN_IBL_SpecAO_Damp", script_terrain.globalSettingsHolder.TERRAIN_IBLRefl_SpecAO_Damp);
			
			
			if (blendedObject && blendedObject.gameObject.collider) {
				Ray ray = new Ray(transform.position+Vector3.up*200, Vector3.down);
				RaycastHit hitInfo;
				if (blendedObject.gameObject.collider.Raycast(ray, out hitInfo, Mathf.Infinity)) {
					if (script_terrain.controlC!=null) {
						CheckReadability(script_terrain.controlC);
						try {
							Color col=script_terrain.controlC.GetPixelBilinear(hitInfo.textureCoord.x, hitInfo.textureCoord.y);
							if (col.r+col.g+col.b+col.a>0.5f) {
								renderer.sharedMaterial.SetTexture("_TERRAIN_Control", script_terrain.controlC);
								if (script_terrain.globalSettingsHolder.HeightMap3!=null) {
									renderer.sharedMaterial.SetTexture("_TERRAIN_HeightMap", script_terrain.globalSettingsHolder.HeightMap3);
								}
							}
						} catch(Exception) {
						}
					}
					if (script_terrain.controlB!=null) {
						CheckReadability(script_terrain.controlB);
						try {
							Color col=script_terrain.controlB.GetPixelBilinear(hitInfo.textureCoord.x, hitInfo.textureCoord.y);
							if (col.r+col.g+col.b+col.a>0.5f) {
								renderer.sharedMaterial.SetTexture("_TERRAIN_Control", script_terrain.controlB);
								if (script_terrain.globalSettingsHolder.HeightMap2!=null) {
									renderer.sharedMaterial.SetTexture("_TERRAIN_HeightMap", script_terrain.globalSettingsHolder.HeightMap2);
								}
							}
						} catch(Exception) {
						}
					}
					if (script_terrain.controlA!=null) {
						CheckReadability(script_terrain.controlA);
						try {
							Color col=script_terrain.controlA.GetPixelBilinear(hitInfo.textureCoord.x, hitInfo.textureCoord.y);
							if (col.r+col.g+col.b+col.a>0.5f) {
								renderer.sharedMaterial.SetTexture("_TERRAIN_Control", script_terrain.controlA);
								if (script_terrain.globalSettingsHolder.HeightMap!=null) {
									renderer.sharedMaterial.SetTexture("_TERRAIN_HeightMap", script_terrain.globalSettingsHolder.HeightMap);
								}
							}
						} catch(Exception) {
						}
					}
				}
			}
			// sync from ReliefTerrain object
		}

			
	}
	
	// set renderer material prop when its present as material prop in blended object
	void SyncFloat(string propNameSource, string propNameTarget="") {
		if (!blendedObject) return;
		if (!blendedObject.renderer) return;
		if (!renderer) return;
		if (propNameTarget=="") propNameTarget=propNameSource;
		if (blendedObject.renderer.sharedMaterial.HasProperty(propNameSource)) renderer.sharedMaterial.SetFloat(propNameTarget, blendedObject.renderer.sharedMaterial.GetFloat(propNameSource));
	}
	void SyncVector(string propNameSource, string propNameTarget="") {
		if (!blendedObject) return;
		if (!blendedObject.renderer) return;
		if (!renderer) return;
		if (propNameTarget=="") propNameTarget=propNameSource;
		if (blendedObject.renderer.sharedMaterial.HasProperty(propNameSource)) renderer.sharedMaterial.SetVector(propNameTarget, blendedObject.renderer.sharedMaterial.GetVector(propNameSource));
	}
	void SyncColor(string propNameSource, string propNameTarget="") {
		if (!blendedObject) return;
		if (!blendedObject.renderer) return;
		if (!renderer) return;
		if (propNameTarget=="") propNameTarget=propNameSource;
		if (blendedObject.renderer.sharedMaterial.HasProperty(propNameSource)) renderer.sharedMaterial.SetColor(propNameTarget, blendedObject.renderer.sharedMaterial.GetColor(propNameSource));
	}
	void SyncTexture(string propNameSource, string propNameTarget="") {
		if (!blendedObject) return;
		if (!blendedObject.renderer) return;
		if (!renderer) return;
		if (propNameTarget=="") propNameTarget=propNameSource;
		if (blendedObject.renderer.sharedMaterial.HasProperty(propNameSource) && blendedObject.renderer.sharedMaterial.GetTexture(propNameSource)!=null) renderer.sharedMaterial.SetTexture(propNameTarget, blendedObject.renderer.sharedMaterial.GetTexture(propNameSource));
	}
	
	// set renderer material prop when its present as material prop in both renderer AND blended object
	void SyncFloatBoth(string propNameSource, string propNameTarget="") {
		if (!blendedObject) return;
		if (!blendedObject.renderer) return;
		if (!renderer) return;
		if (propNameTarget=="") propNameTarget=propNameSource;
		if (blendedObject.renderer.sharedMaterial.HasProperty(propNameSource) && renderer.sharedMaterial.HasProperty(propNameSource)) renderer.sharedMaterial.SetFloat(propNameTarget, blendedObject.renderer.sharedMaterial.GetFloat(propNameSource));
	}
	void SyncVectorBoth(string propNameSource, string propNameTarget="") {
		if (!blendedObject) return;
		if (!blendedObject.renderer) return;
		if (!renderer) return;
		if (propNameTarget=="") propNameTarget=propNameSource;
		if (blendedObject.renderer.sharedMaterial.HasProperty(propNameSource) && renderer.sharedMaterial.HasProperty(propNameSource)) renderer.sharedMaterial.SetVector(propNameTarget, blendedObject.renderer.sharedMaterial.GetVector(propNameSource));
	}
	void SyncColorBoth(string propNameSource, string propNameTarget="") {
		if (!blendedObject) return;
		if (!blendedObject.renderer) return;
		if (!renderer) return;
		if (propNameTarget=="") propNameTarget=propNameSource;
		if (blendedObject.renderer.sharedMaterial.HasProperty(propNameSource) && renderer.sharedMaterial.HasProperty(propNameSource)) renderer.sharedMaterial.SetColor(propNameTarget, blendedObject.renderer.sharedMaterial.GetColor(propNameSource));
	}
	void SyncTextureBoth(string propNameSource, string propNameTarget="") {
		if (!blendedObject) return;
		if (!blendedObject.renderer) return;
		if (!renderer) return;
		if (propNameTarget=="") propNameTarget=propNameSource;
		if (blendedObject.renderer.sharedMaterial.HasProperty(propNameSource) && renderer.sharedMaterial.HasProperty(propNameSource) && blendedObject.renderer.sharedMaterial.GetTexture(propNameSource)!=null) renderer.sharedMaterial.SetTexture(propNameTarget, blendedObject.renderer.sharedMaterial.GetTexture(propNameSource));
	}
	
	public void MakeMaterialCopy() {
		if (!renderer.sharedMaterial) return;
		string nam=renderer.sharedMaterial.name;
		Material mat=(Material)Instantiate(renderer.sharedMaterial);
		renderer.sharedMaterial=mat;		
		renderer.sharedMaterial.name=nam;//+" (copy)";
	}
	
	public bool ClearBlend(RTPColorChannels channel=RTPColorChannels.A, float val=0) {
		if (!blendedObject || blendedObject.GetComponent(typeof(Collider))==null || ((blendedObject.GetComponent(typeof(MeshRenderer))==null) && (blendedObject.GetComponent(typeof(Terrain))==null))) {
			Debug.LogError("Select an object (terrain or GameObject with mesh) to be blended with this object.");
			return false;
		}
		Mesh mesh = ((MeshFilter)GetComponent(typeof(MeshFilter))).sharedMesh;
		Color[] colors=mesh.colors;
		if (colors==null || colors.Length==0) {
			colors=new Color[mesh.vertices.Length];
		}
		for(int i=0; i<colors.Length; i++) {
			colors[i][(int)channel]=val;
		}
		mesh.colors=colors;
		pmesh=mesh;
		if (channel==RTPColorChannels.A && !VoxelBlendedObject) {
			if (underlying_renderer) {
				MeshFilter mf=underlying_renderer.GetComponent(typeof(MeshFilter)) as MeshFilter;
				if (mf) {
					colors=mf.sharedMesh.colors;
					if (colors==null || colors.Length==0) {
						colors=new Color[mf.sharedMesh.vertices.Length];
					}
					for(int i=0; i<colors.Length; i++) {
						colors[i][(int)channel]=val;
					}
					mf.sharedMesh.colors=colors;
				}
			}		
		}
		return true;
	}
	
	public Vector3[] get_vertices() {
		MeshFilter filter = gameObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
		if (filter!=null) {
			if ((paint_vertices==null) || (paint_vertices.Length==0)) {
				paint_vertices=filter.sharedMesh.vertices;
			}
		}
		return paint_vertices;
	}
	public Vector3[] get_normals() {
		MeshFilter filter = gameObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
		if (filter!=null) {
			if ((paint_normals==null) || (paint_normals.Length==0)) {
				paint_normals=filter.sharedMesh.normals;
			}
		}
		return paint_normals;
	}
	public int[] get_tris() {
		MeshFilter filter = gameObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
		if (filter!=null) {
			if ((paint_tris==null) || (paint_tris.Length==0)) {
				paint_tris=filter.sharedMesh.triangles;
			}
		}
		return paint_tris;
	}
	
	public void ClearPaintMesh() {
		paint_vertices=null;
		paint_normals=null;
		paint_tris=null;
	}
//	public Color[] get_colors() {
//		MeshFilter filter = gameObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
//		if (filter!=null) {
//			if ((paint_colors==null) || (paint_colors.Length==0)) {
//				paint_colors=filter.sharedMesh.colors;
//			}
//		}
//		return paint_colors;
//	}	
	
	public int modify_blend(int cover_verts_num, int[] cover_indices, float[] cover_strength, bool upflag) {
		if (paint_mode<1) {
			MeshFilter filter=GetComponent(typeof(MeshFilter)) as MeshFilter;
			if (filter && filter.sharedMesh) {
				Color[] colors=filter.sharedMesh.colors;
				float val;
				int color_channel;
				color_channel=(int)vertex_paint_channel;
				if (!upflag) {
					for(int i=0; i<cover_verts_num; i++) {
						val=colors[cover_indices[i]][color_channel]-cover_strength[i]*paint_opacity*0.25f;
						val=(val<0) ? 0 : val;
						colors[cover_indices[i]][color_channel]=val;
					}
				} else {
					for(int i=0; i<cover_verts_num; i++) {
						val=colors[cover_indices[i]][color_channel]+cover_strength[i]*paint_opacity*0.25f;
						val=(val>1) ? 1 : val;
						colors[cover_indices[i]][color_channel]=val;
					}
				}
				filter.sharedMesh.colors=colors;
				pmesh=filter.sharedMesh;
				if (underlying_renderer && color_channel==3 && !VoxelBlendedObject) {
					MeshFilter mf=underlying_renderer.GetComponent(typeof(MeshFilter)) as MeshFilter;
					if (mf) {
						Color[] colors2=mf.sharedMesh.colors;
						if (colors2==null) colors2=new Color[colors.Length];
						for(int i=0; i<colors.Length; i++) colors2[i].a=colors[i].a;
						mf.sharedMesh.colors=colors2;
					}
				}
			}
		} else {
			if (paintHitInfo_flag) {
				if (prepare_tmpColorMap()) {
					if (!blendedObject) return -1;
					int w;
					int h;
					Terrain terrain=(Terrain)blendedObject.GetComponent(typeof(Terrain));
					Vector4 _PosSize=Vector4.one;
					if (VoxelBlendedObject) {
						_PosSize=blendedObject.renderer.sharedMaterial.GetVector("_TERRAIN_PosSize");
					}
					if (terrain) {
						w=Mathf.RoundToInt(paint_size/terrain.terrainData.size.x * tmp_globalColorMap.width);
						h=Mathf.RoundToInt(paint_size/terrain.terrainData.size.z * tmp_globalColorMap.height);
					} else {
						if (VoxelBlendedObject) {
							w=Mathf.RoundToInt(paint_size/_PosSize.z * tmp_globalColorMap.width);
							h=Mathf.RoundToInt(paint_size/_PosSize.w * tmp_globalColorMap.height);
							if (w<1) w=1;
							if (h<1) h=1;
						} else {
							w=Mathf.RoundToInt(paint_size/blendedObject.renderer.bounds.size.x * tmp_globalColorMap.width);
							h=Mathf.RoundToInt(paint_size/blendedObject.renderer.bounds.size.z * tmp_globalColorMap.height);
						}
					}
					int _left;
					if (VoxelBlendedObject) {
						_left = Mathf.RoundToInt(Mathf.Clamp01((paintHitInfo.point.x-_PosSize.x)/_PosSize.z)*tmp_globalColorMap.width-w);
					} else {
						_left = Mathf.RoundToInt(paintHitInfo.textureCoord.x*tmp_globalColorMap.width-w);
					}
					if (_left<0) _left=0;
					w*=2;
					if (_left+w>=tmp_globalColorMap.width) _left=tmp_globalColorMap.width - w;
					int _top;
					if (VoxelBlendedObject) {
						_top = Mathf.RoundToInt(Mathf.Clamp01((paintHitInfo.point.z-_PosSize.y)/_PosSize.w)*tmp_globalColorMap.height-h);
					} else {
						_top = Mathf.RoundToInt(paintHitInfo.textureCoord.y*tmp_globalColorMap.height-h);
					}
					if (_top<0) _top=0;
					//Debug.Log (_left+" , "+_top+" - "+w+" , "+h);
					h*=2;
					if (_top+h>=tmp_globalColorMap.height) _top=tmp_globalColorMap.height - h;
					Color[] cols=tmp_globalColorMap.GetPixels(_left, _top, w, h);
					int idx=0;
					float d=upflag ? -1f : 1f;
					for(int j=0; j<h; j++) {
						idx=j*w;
						float disty=2.0f*j/h-1.0f;
						for(int i=0; i<w; i++) {
							float distx=2.0f*i/w-1.0f;
							float dist=1-Mathf.Sqrt(distx*distx+disty*disty);
							if (dist<0) dist=0;
							dist=dist > paint_smoothness ? 1 : dist/paint_smoothness;
							cols[idx].a+=d*paint_opacity*dist;
							cols[idx].a=Mathf.Clamp01(cols[idx].a);
							if (cols[idx].a<0.008f) cols[idx].a=0.008f;
							idx++;
						}
					}
					tmp_globalColorMap.SetPixels(_left, _top, w, h, cols);
					tmp_globalColorMap.Apply(false,false);
				} else {
					return -2;
				}
			}
		}
		return 0;
	}
	
	public bool prepare_tmpColorMap() {
		if (!blendedObject) return false;
		ReliefTerrain terrain_script=(ReliefTerrain)blendedObject.GetComponent(typeof(ReliefTerrain));
		Texture2D colorMap;
		if ((terrain_script && terrain_script.ColorGlobal) || VoxelBlendedObject) {
			if (terrain_script && terrain_script.ColorGlobal) {
				colorMap=terrain_script.ColorGlobal;
			} else {
				if (blendedObject.renderer.sharedMaterial.HasProperty("_ColorMapGlobal") && blendedObject.renderer.sharedMaterial.GetTexture("_ColorMapGlobal")!=null) {
					colorMap=blendedObject.renderer.sharedMaterial.GetTexture("_ColorMapGlobal") as Texture2D;
					#if UNITY_EDITOR	
					string path=AssetDatabase.GetAssetPath(colorMap);
					if (path!="") {
						AssetImporter _importer=AssetImporter.GetAtPath(path);
						if (_importer) {
							TextureImporter tex_importer=(TextureImporter)_importer;
							if (!tex_importer.isReadable) {
								Debug.LogWarning("Texture ("+colorMap.name+") has been reimported as readable.");
								tex_importer.isReadable=true;
								AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(colorMap),  ImportAssetOptions.ForceUpdate);
							}
						}
					}
					#endif
				} else {
					return false;
				}
			}
			if (tmp_globalColorMap!=colorMap) {
				try { 
					colorMap.GetPixels(0,0,4,4,0);
				} catch (Exception e) {
					Debug.LogError("Global ColorMap has to be marked as isReadable...");
					Debug.LogError(e.Message);
					return false;
				}
				if (colorMap.format==TextureFormat.Alpha8) {
					tmp_globalColorMap=new Texture2D(colorMap.width, colorMap.height, TextureFormat.Alpha8, true); 
				} else {
					tmp_globalColorMap=new Texture2D(colorMap.width, colorMap.height, TextureFormat.ARGB32, true); 
				}
				Color[] cols=colorMap.GetPixels();
				tmp_globalColorMap.SetPixels(cols);
				tmp_globalColorMap.Apply(true,false);
				if (VoxelBlendedObject) {
					ReliefTerrainVertexBlendTriplanar script=blendedObject.GetComponent(typeof(ReliefTerrainVertexBlendTriplanar)) as ReliefTerrainVertexBlendTriplanar;
					if (script) {
						script.tmp_globalColorMap=tmp_globalColorMap;
						script.painterInstance=this;
					}
					blendedObject.renderer.sharedMaterial.SetTexture("_ColorMapGlobal", tmp_globalColorMap);
					if (renderer.sharedMaterial.HasProperty("_ColorMapGlobal")) {
						renderer.sharedMaterial.SetTexture("_ColorMapGlobal", tmp_globalColorMap);
					}
				} else {
					terrain_script.ColorGlobal=tmp_globalColorMap;
					terrain_script.globalColorModifed_flag=true;
					terrain_script.RefreshTextures();
				}
			}
			return true;
		}
		return false;
	}
	
	public Texture2D get_tmpColorMap() {
		return tmp_globalColorMap;
	}
	
	public GameObject GetUnderlyingGameObject() {
		if (!underlying_transform) return null;
		return underlying_transform.gameObject;
	}
	
	private bool check_edge(int ev0, int ev1, int i, int[] triangles) {
		int eV0;
		int eV1;
		
		for(int j=0; j<triangles.Length; j+=3) {
			if (i!=j) {
				eV0=triangles[j]; eV1=triangles[j+1];
				if (((eV0==ev0) && (eV1==ev1)) || ((eV1==ev0) && (eV0==ev1))) return false;
				eV0=triangles[j+1]; eV1=triangles[j+2];
				if (((eV0==ev0) && (eV1==ev1)) || ((eV1==ev0) && (eV0==ev1))) return false;
				eV0=triangles[j+2]; eV1=triangles[j];
				if (((eV0==ev0) && (eV1==ev1)) || ((eV1==ev0) && (eV0==ev1))) return false;
			}
		}
		return true;		
	}		
	private void Swap(ref int a, ref int b) {
		int tmp;
		tmp=b;
		b=a;
		a=tmp;
	}

	private void ResetProgress(int progress_count, string _progress_description="") {
		progress_count_max=progress_count;
		progress_count_current=0;
		progress_description=_progress_description;
	}
	
	private void CheckProgress() {
      if ( ((progress_count_current++) % progress_granulation) == (progress_granulation-1) )
      {
         EditorUtility.DisplayProgressBar( "Processing...", progress_description, 1.0f*progress_count_current/progress_count_max );
      }

	}
	
	public void CheckReadability(Texture2D tex) {
		AssetImporter _importer=AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(tex));
		if (_importer && (System.IO.Path.GetExtension(_importer.assetPath)!=".asset")) {
			TextureImporter tex_importer=(TextureImporter)_importer;
			if (!tex_importer.isReadable) {
				Debug.LogWarning("Texture ("+tex.name+") has been reimported as readable.");
				tex_importer.isReadable=true;
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(tex),  ImportAssetOptions.ForceUpdate);
			}
		}		
	}
	
#endif
}
