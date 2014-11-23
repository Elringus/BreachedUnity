using UnityEngine;

[ExecuteInEditMode]
public class CameraGlitch : ImageEffectBase
{
	public Texture2D DisplacementMap;
	public float Intensity;

	private float glitchUp, glitchDown, flicker, glitchUpTime = .05f, glitchDownTime = .05f, flickerTime = .5f;

	private void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Intensity", Intensity);
		material.SetTexture("_DispTex", DisplacementMap);

		glitchUp += Time.deltaTime * Intensity;
		glitchDown += Time.deltaTime * Intensity;
		flicker += Time.deltaTime * Intensity;

		if (flicker > flickerTime)
		{
			material.SetFloat("filterRadius", Random.Range(-3f, 3f) * Intensity);
			flicker = 0;
			flickerTime = Random.value;
		}

		if (glitchUp > glitchUpTime)
		{
			if (Random.value < .1f * Intensity) material.SetFloat("flip_up", Random.Range(0, 1f) * Intensity);
			else material.SetFloat("flip_up", 0);

			glitchUp = 0;
			glitchUpTime = Random.value / 10f;
		}

		if (glitchDown > glitchDownTime)
		{
			if (Random.value < .1f * Intensity) material.SetFloat("flip_down", 1f - Random.Range(0f, 1f) * Intensity);
			else material.SetFloat("flip_down", 1f);

			glitchDown = 0;
			glitchDownTime = Random.value / 10f;
		}

		if (Random.value < .05f * Intensity)
		{
			material.SetFloat("displace", Random.value * Intensity);
			material.SetFloat("scale", 1 - Random.value * Intensity);
		}
		else material.SetFloat("displace", 0);

		Graphics.Blit(source, destination, material);
	}
}