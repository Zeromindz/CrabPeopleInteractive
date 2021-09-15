using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [Header("Floater Settings")]
    public Rigidbody m_RigidBody;
    public float m_DepthBeforeSubmerged = 1.0f;
    public float m_DisplacementAmount = 3.0f;
    public int m_FloaterCount = 1;
    public float m_WaterDrag = 0.99f;
    public float m_WaterAngularDrag = 0.5f;

    private void FixedUpdate()
    {
        m_RigidBody.AddForceAtPosition(Physics.gravity / m_FloaterCount, transform.position, ForceMode.Acceleration);

        float waveHeight = WaveManager.m_Instance.GetWaveHeight(transform.position);
        // Check if the floaters y position is below the waveheight
        if(transform.position.y < waveHeight)
        {
            // Approximate a percentage of how much the object is submerged and
            //  multiply the whole thing by the displacement amount
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / m_DepthBeforeSubmerged) * m_DisplacementAmount;
            // We then add a force equal to gravity multiplied by the displacement multiplier, using the acceleration forcemode.
            m_RigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            m_RigidBody.AddForce(displacementMultiplier * -m_RigidBody.velocity * m_WaterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            m_RigidBody.AddTorque(displacementMultiplier * -m_RigidBody.angularVelocity * m_WaterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

}

