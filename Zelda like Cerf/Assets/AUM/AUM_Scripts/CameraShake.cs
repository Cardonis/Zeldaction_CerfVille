using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using XInputDotNetPure;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera cmVcam;

    private CinemachineBasicMultiChannelPerlin cmVcamNoise;

    [HideInInspector] public Coroutine lastCameraShake;

    [HideInInspector] public bool shaking;
    
    // Start is called before the first frame update
    void Start()
    {
        cmVcamNoise = cmVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        shaking = false;
    }

    public IEnumerator CameraShakeFor(float time, float secondTime, float force)
    {
        shaking = true;

        cmVcamNoise.m_AmplitudeGain = force;

        GamePad.SetVibration(0, force / 5, force / 5);

        for(float i = time; i > 0; i -= Time.deltaTime)
        {
            cmVcamNoise.m_AmplitudeGain = force;

            GamePad.SetVibration(0, force / 5, force / 5);

            yield return new WaitForFixedUpdate();
        }

        cmVcamNoise.m_AmplitudeGain = 0;

        for (float i = secondTime; i > 0; i -= Time.deltaTime)
        {
            GamePad.SetVibration(0, force / 5, force / 5);

            yield return new WaitForFixedUpdate();
        }

        GamePad.SetVibration(0, 0, 0);

        shaking = false;
    }

    public void StopCameraShake()
    {
        cmVcamNoise.m_AmplitudeGain = 0;

        GamePad.SetVibration(0, 0, 0);

        if(lastCameraShake != null)
        StopCoroutine(lastCameraShake);
    }
}
