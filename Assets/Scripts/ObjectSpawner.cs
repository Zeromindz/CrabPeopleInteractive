using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    private PoolManager m_ObjectPooler; // Pool manager instance

    private void Start()
    {
        m_ObjectPooler = PoolManager.m_Instance;

        for(int i = 0; i < m_ObjectPooler.m_Pools[0].size; i++)
        {
            SpawnObject();
        }
    }

    private void Update()
    {

    }

    void SpawnObject()
    {
        Vector3 spawnPos = Random.insideUnitSphere * 100f;

        GameObject spawnedObject = m_ObjectPooler.SpawnFromPool("Ghost", transform.position + spawnPos, Quaternion.identity);

    }
}
