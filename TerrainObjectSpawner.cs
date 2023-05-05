using System.Collections.Generic;
using UnityEngine;

public class TerrainObjectSpawner : MonoBehaviour
{
    [Header("Randomisation")]
    public int Seed;
    public GameObject ObjectToSpawn;
    public int ObjectAmount;

    [Header("Terrain Info")]
    [SerializeField] private Terrain _terrain;
    private Vector3 _terrainSize;

    [Header("Gizmos")]
    [SerializeField] private Color _gizmosColor;
    private List<Ray> _rays = new List<Ray>();


    // Start is called before the first frame update
    void Awake()
    {
        // Set the generation seed
        Random.InitState(Seed);

        _terrainSize = _terrain.terrainData.size;
        
        for (int i = 0; i < ObjectAmount; i++)
        {
            float x = Random.Range(0, _terrainSize.x);
            float z = Random.Range(0, _terrainSize.z);

            Vector3 rayPosition = new Vector3(x, _terrainSize.y, z);
            Ray ray = new Ray(rayPosition, Vector3.down);
            
            // Store rays to draw gizmos
            _rays.Add(ray);

            if (Physics.Raycast(ray, out RaycastHit hit, _terrainSize.y, ~_terrain.gameObject.layer))
            {
                Vector3 position = new Vector3(x, _terrainSize.y - hit.distance, z);
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal); // Change to Quaternion.identity to remove normal rotation
                
                // Spawn Object
                Instantiate(ObjectToSpawn, position, rotation);
            }
            else
            {
                Debug.LogError("Could not spawn object: Raycast failed.");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireCube(_terrain.transform.position + _terrainSize/2f, _terrainSize);

        foreach (var item in _rays)
        {
            Gizmos.DrawRay(item);
        }
    }
}
