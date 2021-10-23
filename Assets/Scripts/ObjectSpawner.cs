using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    private PoolManager m_ObjectPooler; // Pool manager instance

    private void Start()
    {
        m_ObjectPooler = PoolManager.m_Instance;
    }

    private void Update()
    {
        SpawnObject();
    }

    void SpawnObject()
    {
        Vector3 spawnPos = Random.insideUnitSphere * 100f;

        GameObject spawnedObject = m_ObjectPooler.SpawnFromPool("Ghost", transform.position + spawnPos, Quaternion.identity);
    }
}
