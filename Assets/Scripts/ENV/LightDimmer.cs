using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum Orientation
{
    Horizontal,
    Vertical
}

public class LightDimmer : MonoBehaviour
{
    public Light2D light;
    public Transform player;

    public float maxIntensity;
    public float minIntensity;

    public GameObject maxGO;
    public GameObject minGO;

    public Orientation orientation;

    public void Update()
    {
        float playerX = player.position.x;
        float playerY = player.position.y;
        float maxGX = maxGO.transform.position.x;
        float minGX = minGO.transform.position.x;
        float maxGY = maxGO.transform.position.y;
        float minGY = minGO.transform.position.y;

        if (orientation == Orientation.Horizontal)
        {
            if (playerY >= minGY && playerY <= maxGY && playerX <= minGX && playerX >= maxGX)
            {
                float percent = Mathf.InverseLerp(maxGX, minGX, playerX);
                float intensity = Mathf.Lerp(maxIntensity, minIntensity, percent);

                // Adjust intensity to reach 0 at 90% distance
                intensity *= 1 - (0.9f * percent);

                light.intensity = intensity;
            }
        }
        else if (orientation == Orientation.Vertical)
        {
            if (playerX >= minGX && playerX <= maxGX && playerY <= maxGY && playerY >= minGY)
            {
                float percent = Mathf.InverseLerp(maxGY, minGY, playerY);
                float intensity = Mathf.Lerp(maxIntensity, minIntensity, percent);

                // Adjust intensity to reach 0 at 90% distance
                intensity *= 1 - (0.9f * percent);

                light.intensity = intensity;
            }
        }
    }
}
