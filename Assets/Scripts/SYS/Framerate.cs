using UnityEngine;

public class Framerate : MonoBehaviour
{
    public int targetFrameRate;

    private void Start()
    {
        if (!Application.isEditor)
            Application.targetFrameRate = targetFrameRate;
    }
}
