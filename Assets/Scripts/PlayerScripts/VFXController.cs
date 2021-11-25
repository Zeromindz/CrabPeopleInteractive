using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    private PoolManager m_PoolManager;

    internal PlayerController m_PlayerController;

    public float m_TrickGlowDuration = 0.5f; 
    public float m_GlowSmooth = 2f;

    private Material m_GondolaMat;

    [Space(10)]
    [Header("Particles")]
    public int m_MinIdleRocketEmissionRate = 25;
    public int m_MaxIdleRocketEmissionRate = 50;
    [SerializeField] private ParticleSystem[] m_IdleRocketTrails;
    public int m_BoostRocketEmissionRate = 50;
    [SerializeField] private ParticleSystem[] m_BoostRocketTrails;
    [SerializeField] private ParticleSystem[] m_GroundedFX;
    [SerializeField] private ParticleSystem m_Sparkle;

    [Space(10)]
    [Header("Pickups")]
    public ParticleSystem m_GhostPuff;
    

    void Start()
    {
        m_PoolManager = PoolManager.m_Instance;

        m_PlayerController = GetComponent<PlayerController>();
        m_GondolaMat = GetComponentInChildren<MeshRenderer>().material;

        
    }

    // Update is called once per frame
    void Update()
    {
        // Set vfx emissions
        int idleRocketEmissionRate = m_MinIdleRocketEmissionRate;
        if (m_PlayerController.playerMovement.m_CurrentVel.magnitude > 50f)
        {
            idleRocketEmissionRate = m_MaxIdleRocketEmissionRate + (int)m_PlayerController.playerMovement.m_CurrentVel.magnitude;

        }
        foreach (var trails in m_IdleRocketTrails)
        {
            var rocketEmission = trails.emission;
            rocketEmission.rateOverTime = new ParticleSystem.MinMaxCurve(idleRocketEmissionRate);
        }

        // Set vfx emissions
        int boostRocketEmissionRate = 0;
        if (m_PlayerController.playerMovement.m_Boosting)
        {
            boostRocketEmissionRate = m_BoostRocketEmissionRate;

        }
        foreach (var trails in m_BoostRocketTrails)
        {
            var rocketEmission = trails.emission;
            rocketEmission.rateOverTime = new ParticleSystem.MinMaxCurve(boostRocketEmissionRate);
        }

        int groundEmissionRate = 0;
        if (m_PlayerController.playerMovement.m_Grounded && m_PlayerController.playerMovement.m_CurrentVel.magnitude > 25f)
        {
            groundEmissionRate = (int)m_PlayerController.playerMovement.m_CurrentVel.magnitude;

            foreach (var fx in m_GroundedFX)
            {
                fx.Play();
                //var groundEmission = fx.emission;
                //groundEmission.rateOverTime = new ParticleSystem.MinMaxCurve(groundEmissionRate);
            }
        }
        else
        {
            foreach (var fx in m_GroundedFX)
            {
                fx.Stop();
            }
        }

        TrickGlow();
    }

    void TrickGlow()
    {
        if (m_PlayerController.playerMovement.m_TrickPerformed)
        {
            //m_GlowSmooth += Time.deltaTime;
            m_GondolaMat.SetFloat("Vector1_78B50D9D", Mathf.Lerp(m_GondolaMat.GetFloat("Vector1_78B50D9D"), 1, Time.deltaTime * m_GlowSmooth));
        }
        else
        {
            m_GondolaMat.SetFloat("Vector1_78B50D9D", 0f);
        }
    }

    public void Sparkle()
    {
        m_Sparkle.Play();
    }

    public void PlayPuffEffect(Vector3 _targetPos)
    {

        GameObject objectToSpawn = m_PoolManager.SpawnFromPool("GhostPuff", _targetPos, Quaternion.identity);
        ParticleSystem ps = objectToSpawn.GetComponent<ParticleSystem>();
        ps.Play();

        StartCoroutine(DisableObject(objectToSpawn, ps.main.duration));

    }

    IEnumerator DisableObject(GameObject _object, float _duration)
    {
        yield return new WaitForSeconds(_duration);

        _object.SetActive(false);
    }
}
