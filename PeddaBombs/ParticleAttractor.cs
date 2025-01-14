using UnityEngine;

[ExecuteInEditMode]
public class ParticleAttractor : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Transform target;
    public float speed = 1f;
    public AnimationCurve affectionCurve;

    private ParticleSystem.Particle[] particles;

    private void Update()
    {
        if (target == null || particleSystem == null)
            return;

        // Initialize particle array if necessary
        if (particles == null || particles.Length < particleSystem.main.maxParticles)
        {
            particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        }

        // Get current particles
        int particleCount = particleSystem.GetParticles(particles);

        Vector3 targetPosition = target.position;

        // Process each particle
        for (int i = 0; i < particleCount; i++)
        {
            float lifetimeFraction = 1f - particles[i].remainingLifetime / particles[i].startLifetime;

            // Use affectionCurve if defined, otherwise default to linear influence
            float curveValue = affectionCurve != null ? affectionCurve.Evaluate(lifetimeFraction) : lifetimeFraction;

            // Adjust particle velocity to move towards the target
            Vector3 direction = (targetPosition - particles[i].position).normalized;
            particles[i].velocity += direction * speed * curveValue * Time.deltaTime;

            // Smoothly move particles towards the target
            particles[i].position = Vector3.Lerp(particles[i].position, targetPosition, curveValue * Time.deltaTime);
        }

        // Apply modified particles back to the particle system
        particleSystem.SetParticles(particles, particleCount);
    }
}
