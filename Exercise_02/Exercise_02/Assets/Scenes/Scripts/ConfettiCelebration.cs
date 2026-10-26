using UnityEngine;

// Bonus 7j: confetti particle system that rains down around the Player when all PickUps are collected.
public class ConfettiCelebration : MonoBehaviour
{
    private void OnEnable()
    {
        GameHUD.AllCollected += SpawnConfetti;
    }

    private void OnDisable()
    {
        GameHUD.AllCollected -= SpawnConfetti;
    }

    private void SpawnConfetti()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        Vector3 center = player != null ? player.transform.position : Vector3.zero;

        GameObject obj = new GameObject("Confetti");
        obj.transform.position = center + Vector3.up * 7f;
        obj.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // emit downwards

        ParticleSystem ps = obj.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        Gradient colors = new Gradient();
        colors.SetKeys(
            new[]
            {
                new GradientColorKey(new Color(1f, 0.25f, 0.3f), 0f),
                new GradientColorKey(new Color(1f, 0.85f, 0.2f), 0.25f),
                new GradientColorKey(new Color(0.3f, 0.9f, 0.4f), 0.5f),
                new GradientColorKey(new Color(0.3f, 0.6f, 1f), 0.75f),
                new GradientColorKey(new Color(0.8f, 0.4f, 1f), 1f)
            },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) });

        ParticleSystem.MainModule main = ps.main;
        main.duration = 3f;
        main.loop = false;
        main.playOnAwake = false;
        main.maxParticles = 1000;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.startLifetime = new ParticleSystem.MinMaxCurve(4f, 6f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(2f, 6f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.15f, 0.35f);
        main.startRotation = new ParticleSystem.MinMaxCurve(0f, 2f * Mathf.PI);
        main.startColor = new ParticleSystem.MinMaxGradient(colors) { mode = ParticleSystemGradientMode.RandomColor };
        main.gravityModifier = 0.4f;

        ParticleSystem.EmissionModule emission = ps.emission;
        emission.rateOverTime = 60f;
        emission.SetBursts(new[]
        {
            new ParticleSystem.Burst(0f, 150),
            new ParticleSystem.Burst(0.6f, 100)
        });

        ParticleSystem.ShapeModule shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(14f, 14f, 1f);

        ParticleSystem.RotationOverLifetimeModule rotation = ps.rotationOverLifetime;
        rotation.enabled = true;
        rotation.z = new ParticleSystem.MinMaxCurve(-4f, 4f);

        ParticleSystem.NoiseModule noise = ps.noise;
        noise.enabled = true;
        noise.strength = 0.6f;
        noise.frequency = 0.4f;

        ParticleSystem.SizeOverLifetimeModule size = ps.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
            new Keyframe(0f, 1f), new Keyframe(0.8f, 1f), new Keyframe(1f, 0f)));

        ParticleSystemRenderer particleRenderer = obj.GetComponent<ParticleSystemRenderer>();
        Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
        if (shader == null) shader = Shader.Find("Sprites/Default");
        particleRenderer.material = new Material(shader);

        ps.Play();
        Destroy(obj, 12f);
    }
}
