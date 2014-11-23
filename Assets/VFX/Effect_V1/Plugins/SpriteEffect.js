#pragma strict
enum CameraFacingMode{ Always, Horizontal, Vertical, Never };

// Sprite properties
var keepMeshSize:boolean =true;
var size:Vector3 = Vector3(1,1,1);	// Size of the sprtie	
var speedGrowing:float; // speed growing of size
var billboarding:CameraFacingMode;  // Bilboardin mode
var randomRotation:boolean;

// Sprite sheet properties
var uvAnimationTileX = 8; //Here you can place the number of columns of your sheet.
var uvAnimationTileY = 8; //Here you can place the number of rows of your sheet.
var framesPerSecond = 26.0; // Animation speed
var oneShot:boolean;

// Light effect
var addLightEffect:boolean=false;
var lightRange:float;
var lightColor:Color;
var lightFadSpeed:float=1;

// Sound effect
private var soundEffect:AudioSource;

// Main camera with tag : Main Camera
private var mainCam:GameObject; // main camera reference

// End indicator
private var effectEnd:boolean=false;

// Random z rotation;
private var randomZAngle:float;

private var startTime:float;

function Awake(){
	// We search the main camera
	mainCam = GameObject.FindGameObjectWithTag("MainCamera");
}

function Start(){

	// do we have sound effect ?
	soundEffect = GetComponent("AudioSource") as AudioSource;
	
	// Apply the new size at start
	if (!keepMeshSize)
		transform.localScale = size;

	// Add light
	if (addLightEffect){
		gameObject.AddComponent("Light");
		gameObject.light.color = lightColor;
		gameObject.light.range = lightRange;
	}
	
	if (randomRotation){
		randomZAngle = Random.Range(-180.0,180.0);
	}
	else{
		randomZAngle = 0;
	}
	
	startTime = Time.time;
	renderer.enabled = false;
	
	
}

function Update () {

	Camera_BillboardingMode();
	Update_Animation();
    
}

function Update_Animation(){

	var end:boolean=false;	
	
    // Calculate index
    var index : int = (Time.time-startTime) * framesPerSecond;
    
    // 
    if ((index<uvAnimationTileX*uvAnimationTileY || !oneShot) && !effectEnd ){
	   
	     // repeat when exhausting all frames
	    index = index % (uvAnimationTileX * uvAnimationTileY);
	   
	    // Size of every tile
	    var size = Vector2 (1.0 / uvAnimationTileX, 1.0 / uvAnimationTileY);
	   
	    // split into horizontal and vertical index
	    var uIndex = index % uvAnimationTileX;
	    var vIndex = index / uvAnimationTileX;
	
	    // build offset
	    var offset = Vector2 (uIndex * size.x , 1.0 - size.y - vIndex * size.y);
		
	   	renderer.material.SetTextureOffset ("_MainTex", offset);
	   	renderer.material.SetTextureScale ("_MainTex", size);
	    
	    // growing
	    transform.localScale += Vector3(speedGrowing,speedGrowing,speedGrowing) * Time.deltaTime ;
	    renderer.enabled = true;
 	}
 	else{
 		effectEnd = true;
		end = true;
		if (soundEffect){
			if (soundEffect.isPlaying){
				end = false;
			}
		}
		
		if (addLightEffect && end){
			if (gameObject.light.intensity>0){
				end = false;
			}
		}
		
		if (end){
			Destroy(gameObject);
				
 		}
 	}
 	
 	
 	// Light effect
 	if (addLightEffect && lightFadSpeed!=0){
		gameObject.light.intensity -= lightFadSpeed*Time.deltaTime;
	}
}

function Camera_BillboardingMode(){

	switch (billboarding){
		case CameraFacingMode.Always:
			transform.eulerAngles.z = randomZAngle;
			transform.eulerAngles.x = mainCam.transform.eulerAngles.x;
			transform.eulerAngles.y = mainCam.transform.eulerAngles.y;
			break;
		case CameraFacingMode.Horizontal:
		
			transform.eulerAngles.y = mainCam.transform.eulerAngles.y;
			transform.eulerAngles.z = mainCam.transform.eulerAngles.z;
			break;
		case CameraFacingMode.Vertical:
		
			transform.eulerAngles.x = mainCam.transform.eulerAngles.x;
			transform.eulerAngles.z = mainCam.transform.eulerAngles.z;
			break;
	}
}