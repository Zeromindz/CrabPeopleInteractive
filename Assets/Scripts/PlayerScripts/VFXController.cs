using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    internal PlayerController m_PlayerController;

    [Header("Particles")]
    [SerializeField] private ParticleSystem[] m_RocketTrails;
    [SerializeField] private ParticleSystem m_GroundedTrail;
    [SerializeField] private ParticleSystem m_Sparkle;

    void Start()
    {
        m_PlayerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set vfx emissions
        int rocketEmissionRate = 0;
        if (m_PlayerController.playerMovement.m_Boosting)
        {
            rocketEmissionRate = 10;

        }
        foreach (var trails in m_RocketTrails)
        {
            var rocketEmission = trails.emission;
            rocketEmission.rateOverTime = new ParticleSystem.MinMaxCurve(rocketEmissionRate);
        }

        int groundEmissionRate = 0;
        if (m_PlayerController.playerMovement.m_Grounded)
        {
            groundEmissionRate = 10;
            var groundEmission = m_GroundedTrail.emission;
            groundEmission.rateOverTime = new ParticleSystem.MinMaxCurve(groundEmissionRate);
        }

        //Sparkle testing
        if (m_PlayerController.playerInput.SpacePressed() > 0)
        {
            m_Sparkle.Play();
        }
    }
}
