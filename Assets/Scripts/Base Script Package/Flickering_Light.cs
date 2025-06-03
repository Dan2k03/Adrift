using UnityEngine;

[RequireComponent(typeof(Light))]
public class Flickering_Light : MonoBehaviour
{
	public float minIntensity = 0.25f;
	public float maxIntensity = 0.5f;
	public float frequency = 1f;
	
	float random;
	private Light m_light;
	
	void Start()
	{
		m_light = GetComponent<Light>();
		random = Random.Range(0.0f, 65535.0f);
	}
	
	void Update()
	{
		float noise = Mathf.PerlinNoise(random, Time.time * frequency);
		m_light.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
	}
}