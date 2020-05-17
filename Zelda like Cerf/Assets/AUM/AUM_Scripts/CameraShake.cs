using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using XInputDotNetPure;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera cmVcam;

    private CinemachineBasicMultiChannelPerlin cmVcamNoise;

    // Start is called before the first frame update
    void Start()
    {
        cmVcamNoise = cmVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator CameraShakeFor(float time, float force)
    {
        cmVcamNoise.m_AmplitudeGain = force;

        GamePad.SetVibration(0, force / 5, force / 5);

        yield return new WaitForSeconds(time);

        cmVcamNoise.m_AmplitudeGain = 0;

        yield return new WaitForSeconds(time);

        GamePad.SetVibration(0, 0, 0);
    }
}
