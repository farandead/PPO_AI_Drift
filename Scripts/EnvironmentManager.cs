using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public List<Transform> targetTransforms = new List<Transform>();
    public List<GameObject> spawnedPoints = new List<GameObject>();
    public List<Vector3> initialTargetPositions = new List<Vector3>();
    public List<Transform> spawnPoints = new List<Transform>();
    // public GameObject environmentMaterialObject; // Reference to the GameObject with the material
    public Material environmentMaterial; // Material reference


    public GameObject targetPrefab; // Prefab for spawning targets
    public Vector3 targetScale = new Vector3(1.991018f, 0.772455f, 0.01197605f); // Scale to apply to targets

    void Start()
    {
        InitializeTargets();
        InitializeSpawnPoints();
    }

    void InitializeTargets()
    {
        GameObject[] taggedTargets = GameObject.FindGameObjectsWithTag("Target");
        foreach (var target in taggedTargets)
        {
            if (target.transform.IsChildOf(transform))
            {
                targetTransforms.Add(target.transform);
                initialTargetPositions.Add(target.transform.position);
            }
        }
    }

    void InitializeSpawnPoints()
    {
        GameObject[] taggedSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (var spawnPoint in taggedSpawnPoints)
        {
            if (spawnPoint.transform.IsChildOf(transform))
            {
                spawnPoints.Add(spawnPoint.transform);
            }
        }
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points found. Please ensure that spawn points are correctly tagged and are children of the environment manager's GameObject.");
        }
    }

    public void ResetEnvironment()
    {
        RemoveAllTargets();
        SpawnTargets();
    }

    public void RemoveAllTargets()
    {
        foreach (var target in targetTransforms)
        {
            Destroy(target.gameObject);
        }
        targetTransforms.Clear();
    }

    public void SpawnTargets()
    {
        if (spawnedPoints.Count != 0)
        {
            RemovePoints(spawnedPoints);
        }

        foreach (var initialPosition in initialTargetPositions)
        {
            GameObject newTarget = Instantiate(targetPrefab, initialPosition, Quaternion.identity, transform);
            spawnedPoints.Add(newTarget);
            newTarget.transform.localScale = targetScale; // Apply the scaling
            newTarget.tag = "Target"; // Ensure the new target is tagged correctly
            targetTransforms.Add(newTarget.transform);
        }
    }

    public Transform GetRandomSpawnPoint()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Count);
        return spawnPoints[spawnIndex];
    }

    public void DeactivateTarget(GameObject target)
    {
        target.SetActive(false);
    }

    public bool AreAllTargetsDeactivated()
    {
        foreach (var target in targetTransforms)
        {
            if (target.gameObject.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    private void RemovePoints(List<GameObject> toBeDeletedGameObjects)
    {
        foreach (GameObject point in toBeDeletedGameObjects)
        {
            Destroy(point);
        }
        toBeDeletedGameObjects.Clear();
    }
}
