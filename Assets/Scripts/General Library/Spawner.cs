using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnerField
    {
        public GameObject prefab;
        public Transform point;
        public Transform parent;

        public GameObject Spawn()
        {
            if (parent != null)
                return Instantiate(prefab, point.position, point.rotation, parent);
            else
                return Instantiate(prefab, point.position, point.rotation);

        }
        public GameObject Spawn(Vector3 position)
        {
            return Instantiate(prefab, position, Quaternion.identity);
        }
    }

    [SerializeField] private int index = 0;
    [SerializeField] private List<SpawnerField> fields = new List<SpawnerField>();

    [Header("Callback Event")]
    [SerializeField] private UnityEvent OnSpawn;


    /// <summary>
    /// Spawn given prefab based on index value
    /// </summary>
    public void Spawn() => SpawnAt(index);


    /// <summary>
    /// spawn a gameobject from spawnPoints list base on index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject SpawnAt(int index)
    {
        SpawnerField spawnPoint = GetSpawnPointAt(index);
        if (spawnPoint == null)
            return null;


        OnSpawn.Invoke();
        GameObject spawnedObject = spawnPoint.Spawn();

        return spawnedObject;
    }

    /// <summary>
    /// return spawn point object from spawnpoints list based on index
    /// </summary>
    /// <param name="index">spawnpoints list index no</param>
    /// <returns></returns>
    private SpawnerField GetSpawnPointAt(int index)
    {
        if (index < 0 || index >= fields.Count)
        {
#if UNITY_EDITOR
            Debug.Log("<color=red>spawn point index out of bound</color>");
#endif
            return null;
        }

        return fields[index];
    }
}