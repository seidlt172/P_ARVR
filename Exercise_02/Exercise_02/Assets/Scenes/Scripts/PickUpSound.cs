using UnityEngine;

// Bonus 7a: plays a short chime whenever the Player collects a PickUp.
// The sound is synthesized in code, so no audio asset is needed.
public class PickUpSound : MonoBehaviour
{
    private AudioSource source;
    private AudioClip clip;

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        clip = CreateChime();
    }

    private void OnEnable()
    {
        GameHUD.PickUpCollected += Play;
    }

    private void OnDisable()
    {
        GameHUD.PickUpCollected -= Play;
    }

    private void Play()
    {
        source.PlayOneShot(clip, 0.6f);
    }

    // Two rising sine tones (E6 -> A6) with a fast decay.
    private static AudioClip CreateChime()
    {
        const int sampleRate = 44100;
        const float length = 0.4f;
        float[] data = new float[(int)(sampleRate * length)];

        for (int i = 0; i < data.Length; i++)
        {
            float t = (float)i / sampleRate;
            float frequency = t < 0.1f ? 1318.5f : 1760f;
            float envelope = Mathf.Exp(-6f * t);
            data[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * envelope * 0.5f;
        }

        AudioClip result = AudioClip.Create("PickUpChime", data.Length, 1, sampleRate, false);
        result.SetData(data, 0);
        return result;
    }
}
