Sky and Atmospherics
=======

Properties of a Sky Keyframe
-------------------------

To select a sky keyframe, click on it in the timeline; to select the overall sky properties, click "Sky" on the left of the timeline.

These are the editable properties - in brackets are given the properties as you would specify them when scripting with GetKeyframeValue and SetKeyframeValue.

**Time:** ("time") The time of this keyframe on the timeline.

**Daytime:** ("daytime") The time this keyframe represents, in a floating-point number of days from the starting midnight of the sequence. If the sky layer's "Link Keyframe Time and Daytime" value is true, this is the same as the keyframe's time value. Otherwise, it can be modified freely.

**Mie:** ("MieRed","MieGreen","MieBlue") The Mie scattering coefficients (x=red,y=green,z=blue).

**Auto Mie:** ("AutoMie") The Mie scattering coefficients will be calculated based on wavelength and haze if this is set - overrides Mie.

**Haze:** ("Haze") The amount of haze, mist or fog.

**Haze Base:** ("HazeBaseKm") The base altitude, above which haze starts to decrease in density.

**Haze Scale :** ("HazeScaleKm") The vertical scale over which haze reduces with altitude.

**Eccentricity:** ("Eccentricity") The anisotropy of Mie scattering.


A keyframe's haze value determines how much Mie-scattered haze (i.e. mist or fog) is present. Haze is considered to have a density that falls-off
 exponentially with altitude, so the Haze scale height property determines the scaling height for this exponential.

Fog and mist are both effectively low-level clouds. Fog is defined as having visibility less than 1km, it is called mist when visibility is between 1 and 2 km.



