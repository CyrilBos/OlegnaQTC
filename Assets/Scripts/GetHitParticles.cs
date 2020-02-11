using UnityEngine;

public class GetHitParticles : MonoBehaviour
{
    private Character character;
    private ParticleSystem particleSystem;

    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        character = GetComponentInParent<Character>();
        character.OnHit += playParticles;
    }

    void OnDisable()
    {
        character.OnHit -= playParticles;
    }

    private void playParticles()
    {
        particleSystem.Play();
    }
}
