Cloud Layers and Cloud Keyframes
========

To select a cloud keyframe, click it. To select all the keyframes of a cloud layer, double-click on the space between them. To select and modify the properties of a whole cloud layer, click on the "Clouds" text at left.

**Generation Properties**
--------
These are the editable properties - in brackets are given the properties as you would specify them when scripting with GetKeyframeValue and SetKeyframeValue.
Some cloud keyframe properties are used as per-keyframe values to generated the 3D or 2D cloud textures. These textures are then interpolated for rendering.

**Cloudiness:** ("cloudiness") How cloudy it will be at this keyframe.

**Distribution base:** ("distributionBase") Proportion of the volume vertically that forms the base of the clouds.

**Distribution transition:** ("distributionTransition") Proportion that transitions from the cloudbase to the upper layer.

**Upper density:** ("upperDensity") Density of the upper layer as a proportion of the base density.

**Octaves:** ("octaves" - int) How many fractal octaves to use.

**Persistence:** ("persistence") Fractal persistence.

**Extinction:** ("extinction") How much light is lost per metre through the clouds.

**Diffusivity:** ("diffusivity") How diffuse are the cloud edges.

The cloud shape can be controlled using the keyframe properties **distribution base**, **distribution transition** and **upper density**.

**Interpolated Properties**
---------------
Some are used as interpolated values per-frame when rendering, either to determine the geometry of the clouds:

**Wind_speed:** ("windSpeed") How fast, in m/s.

**Wind direction:** ("WindHeadingDegrees") Direction in compass degrees.

**Cloud base:** ("cloudBaseKm") Height of the cloudbase in km above sea level.

**Cloud height:** ("cloudHeightKm") Height of the clouds in km.


**Visual Properties**
---------------

**Direct light:** ("DirectLight") How much direct sunlight (moonlight) affects the clouds.

**Indirect light:** ("IndirectLight") How much indirectly scattered light is shown.

**Ambient light:** ("AmbientLight") How strongly the clouds reflect ambient light.

**Light asymmetry:** ("Asymmetry") How asymmetric is the direct light scattering.

**Fractal amplitude:** ("FractalAmplitude") Amplitude of the fractal edge effect.

**Fractal wavelength:** ("FractalWavelength") Wavelength of the fractal edge effect.

**Sharpness:** ("Sharpness") Sharpness of cloud edges.

**Churn:** ("Churn") How much cloud turbulence is shown.

**Base noise factor:** ("BaseFactor") How much the edge effect is shown at cloudbase.

**Effect Properties**
---------------

Other properties are used to inform different parts of the rendering system:

**Precipitation**: ("Precipitation") Strength of rain or snow.

**Rain to snow**: ("RainToSnow") Between zero (rain) and one (snow).

**Godray strength**: ("GodrayStrength") Brightness of crepuscular rays.



