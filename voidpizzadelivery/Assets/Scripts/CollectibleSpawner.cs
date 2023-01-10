using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class CollectibleSpawner : MonoBehaviour
{
// prefab to spawn
[SerializeField] private GameObject collectiblePrefab;
// height above the NavMesh to spawn collectibles
[SerializeField] private float spawnHeight = 1f;
// number of collectibles to spawn
[SerializeField] private int spawnCount = 10;
// span of potential spawnpoints
[SerializeField] private float spawnSpan = 350f;

void Start()
{
    // get reference to NavMeshSurface component
    NavMeshSurface navMesh = GetComponent<NavMeshSurface>();
    // build the NavMesh
    navMesh.BuildNavMesh();

    // loop through the number of collectibles to spawn
    for (int i = 0; i < spawnCount; i++)
    {
        // generate a random point on the NavMesh surface
        Vector3 spawnPoint = GetRandomPointOnNavMesh();
        // set the y position of the spawn point to the spawn height
        spawnPoint.y = spawnHeight;
        // instantiate the collectible prefab at the spawn point
        GameObject collectible = Instantiate(collectiblePrefab, spawnPoint, Quaternion.identity);
        // add the collectible to the spawner's hierarchy
        collectible.transform.parent = transform;
    }
}

// Generate a random point on the NavMesh surface
Vector3 GetRandomPointOnNavMesh()
{
    // create a NavMeshHit to store the result of the query
    NavMeshHit hit;
        // generate a random point within the bounds of the NavMesh and a fixed distance of the spawner
    Vector3 randomPoint = Random.insideUnitSphere * spawnSpan + transform.position;
    // raycast down from the point to find the intersection with the NavMesh
    bool hasHit = NavMesh.SamplePosition(randomPoint, out hit, spawnSpan, NavMesh.AllAreas);
    // if the raycast hit the NavMesh, return the point of intersection
    if (hasHit)
    {
        return hit.position;
    }
    // if the raycast did not hit the NavMesh, return the original random point
    return randomPoint;
}

}