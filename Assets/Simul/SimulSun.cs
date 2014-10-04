using UnityEngine;
using System.Collections;

namespace simul
{
	[ExecuteInEditMode]
	public class SimulSun : MonoBehaviour
	{
		public float multiplier = 0.05F;
		public trueSKY trueSky = null;
		void Awake()
		{
		}
		// Use this for initialization
		void Start()
		{
		}
		// Update is called once per frame
		void Update()
		{
			if (trueSky != null)
			{
				this.transform.rotation = trueSky.getSunRotation();
				Vector3 vec = multiplier * trueSky.getSunColour();
				Color c;
				c.r = vec.x;
				c.g = vec.y;
				c.b = vec.z;
				c.a = 1.0F;
#if TRUESKY_LOGGING
				if(c.r<1.0)
					UnityEngine.Debug.Log("Light colour "+c.r+","+c.g+","+c.b);
#endif
				this.light.color = c;
			}
			else
			{
				UnityEngine.Debug.LogError("SimulSun script needs trueSKY object");
			}
		}
	}
}