using UnityEngine;
using UnityEngine.AI;

public class SetTerrainObstaclesStatic : MonoBehaviour
{
    static TreeInstance[] Obstacle;
    static Terrain terrain;
    static float width;
    static float lenght;
    static float hight;

    public static void BakeTreeObstacles()
    {
        terrain = Terrain.activeTerrain;
        Obstacle = terrain.terrainData.treeInstances;

        lenght = terrain.terrainData.size.z;
        width = terrain.terrainData.size.x;
        hight = terrain.terrainData.size.y;

        GameObject parent = new GameObject("Tree_Obstacles");
        int successCount = 0;

        Debug.Log("Processing " + Obstacle.Length + " trees...");

        foreach (TreeInstance tree in Obstacle)
        {
            Vector3 worldPosition = Vector3.Scale(tree.position, terrain.terrainData.size) + terrain.transform.position;
            GameObject prefab = terrain.terrainData.treePrototypes[tree.prototypeIndex].prefab;

            // Skip if prefab has no collider
            Collider coll = prefab.GetComponent<Collider>();
            if (coll == null)
            {
                Debug.LogWarning($"Skipped tree: No collider on prefab '{prefab.name}'");
                continue;
            }

            // Skip unsupported collider types
            if (!(coll is CapsuleCollider) && !(coll is BoxCollider))
            {
                Debug.LogWarning($"Skipped tree: Unsupported collider type '{coll.GetType().Name}' on prefab '{prefab.name}'");
                continue;
            }

            // Create obstacle
            GameObject obs = new GameObject($"Obstacle_{successCount}");
            obs.transform.SetParent(parent.transform);
            obs.transform.position = worldPosition;
            obs.transform.rotation = Quaternion.AngleAxis(tree.rotation * Mathf.Rad2Deg, Vector3.up);

            NavMeshObstacle obstacle = obs.AddComponent<NavMeshObstacle>();
            obstacle.carving = true;
            obstacle.carveOnlyStationary = true;

            // Configure based on collider type
            if (coll is CapsuleCollider capsuleColl)
            {
                obstacle.shape = NavMeshObstacleShape.Capsule;
                obstacle.center = capsuleColl.center;
                obstacle.radius = capsuleColl.radius;
                obstacle.height = capsuleColl.height;
            }
            else if (coll is BoxCollider boxColl)
            {
                obstacle.shape = NavMeshObstacleShape.Box;
                obstacle.center = boxColl.center;
                obstacle.size = boxColl.size;
            }

            successCount++;
        }

        Debug.Log($"Successfully added {successCount}/{Obstacle.Length} NavMeshObstacles. Check warnings for skipped trees.");
    }
}