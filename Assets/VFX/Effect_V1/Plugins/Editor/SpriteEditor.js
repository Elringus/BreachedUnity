@CustomEditor( SpriteEffect)
class SpriteEditor extends Editor{
		
	function OnInspectorGUI(){

		var style:GUIStyle;
		var t:SpriteEffect;
		
		t = target as SpriteEffect;
		style = new GUIStyle();
		style.fontStyle =FontStyle.Bold;
			
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		// Turret properties
		GUILayout.Label("Sprite properties",style);		
		
		t.keepMeshSize = EditorGUILayout.Toggle( "Keep mesh size",t.keepMeshSize);
		if (!t.keepMeshSize){
			t.size = EditorGUILayout.Vector3Field("Size",t.size);
		}
		t.speedGrowing = EditorGUILayout.FloatField("Speed growing",t.speedGrowing);
		t.randomRotation = EditorGUILayout.Toggle( "Random rotation",t.randomRotation);
		t.billboarding = EditorGUILayout.EnumPopup("Camera facing",t.billboarding);	

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		// Sprite sheet properties
		GUILayout.Label("Sprite sheet properties",style);		
		
		t.uvAnimationTileX = EditorGUILayout.IntField("Tile X",t.uvAnimationTileX);
		t.uvAnimationTileY = EditorGUILayout.IntField("Tile Y",t.uvAnimationTileY);
		t.framesPerSecond = EditorGUILayout.FloatField("Frames per second",t.framesPerSecond);
		t.oneShot = EditorGUILayout.Toggle( "One shot",t.oneShot);
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		// Light Effect
		GUILayout.Label("Light properties",style);			
		t.addLightEffect = EditorGUILayout.Toggle( "Add light effect",t.addLightEffect);
		if ( t.addLightEffect ){
			t.lightRange = EditorGUILayout.FloatField("Light range",t.lightRange);	
			t.lightColor = EditorGUILayout.ColorField( "Light color", t.lightColor);
			t.lightFadSpeed = EditorGUILayout.FloatField("Light fad speed",t.lightFadSpeed);	
		}



		// Refresh
		if (GUI.changed){
			EditorUtility.SetDirty (target);
		}
		 		 	
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		 	
	}


			
}