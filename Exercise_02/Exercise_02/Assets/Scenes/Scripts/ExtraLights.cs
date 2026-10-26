using UnityEngine;

// Bonus 7d: four additional lights of different types, colors and intensities.
// The existing Directional Light is not changed.
public class ExtraLights : MonoBehaviour
{
    private void Start()
    {
        CreateLight("Warm Point Light", LightType.Point, new Vector3(-7f, 2.5f, -7f),
            new Color(1f, 0.6f, 0.25f), 10f, 15f);
        CreateLight("Magenta Point Light", LightType.Point, new Vector3(7f, 2f, -7f),
            new Color(1f, 0.2f, 0.8f), 6f, 12f);
        CreateLight("Green Point Light", LightType.Point, new Vector3(-7f, 2f, 7f),
            new Color(0.3f, 1f, 0.4f), 4f, 10f);

        Light spot = CreateLight("Blue Spot Light", LightType.Spot, new Vector3(7f, 8f, 6f),
            new Color(0.3f, 0.6f, 1f), 60f, 25f);
        spot.spotAngle = 55f;
        spot.transform.LookAt(Vector3.zero);
    }

    private Light CreateLight(string lightName, LightType type, Vector3 position, Color color,
        float intensity, float range)
    {
        GameObject obj = new GameObject(lightName);
        obj.transform.SetParent(transform);
        obj.transform.position = position;

        Light light = obj.AddComponent<Light>();
        light.type = type;
        light.color = color;
        light.intensity = intensity;
        light.range = range;
        light.shadows = LightShadows.None;
        return light;
    }
}
