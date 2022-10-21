using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] Material terrainMaterial;
    [SerializeField] Material edgeMaterial;
    [SerializeField] Color terrainColor;

    [SerializeField] float waterLevel = .4f;
    [SerializeField] float scale = .1f;
    [SerializeField] public int size = 100;
    [SerializeField] float treeNoiseScale;
    [SerializeField] float berryDensity;
    [SerializeField] float treeDensity;
    [SerializeField] float rockDensity;

    [SerializeField] GameObject[] treePrefabs;
    [SerializeField] GameObject[] rockPrefabs;
    [SerializeField] GameObject[] berryBushPrefabs;

    public Cell[,] grid;
    float[,] getNoiseMap()
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }
        int r = 20;
        for (int y = size / 2 - r; y < r + size / 2; y++)
        {
            int yDiff = Mathf.Abs(y - size / 2);
            int absX = (int)Mathf.Sqrt(r * r - yDiff * yDiff);
            for (int x = size / 2 - absX; x < size / 2 + absX; x++)
            {
                float noiseValue = 1f;
                noiseMap[x, y] = noiseValue;
            }
        }
        return noiseMap;
    }

    void Awake()
    {

        float[,] noiseMap = getNoiseMap();
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                noiseMap[x, y] -= Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }

        grid = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = noiseMap[x, y];
                bool isWater = noiseValue < waterLevel;
                Cell cell = new Cell(isWater);
                grid[x, y] = cell;
            }
        }
        DrawTerrainMesh();
        DrawEdgeMesh();
        DrawTexture();
        GenerateTrees();
        coast();
    }
    void DrawTexture()
    {
        Color[] ColorMap = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (cell.isWater)
                    ColorMap[y * size + x] = new Color();
                else
                    ColorMap[y * size + x] = terrainColor;
            }
        }
        Texture2D texture = new Texture2D(size, size);
        texture.filterMode = FilterMode.Point;

        texture.SetPixels(ColorMap);
        texture.Apply();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial;
        meshRenderer.material.mainTexture = texture;
    }
    void DrawEdgeMesh()
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    if (x > 0)
                    {
                        Cell left = grid[x - 1, y];
                        if (left.isWater)
                        {
                            Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                            Vector3 b = new Vector3(x - .5f, 0, y - .5f);
                            Vector3 c = new Vector3(x - .5f, -1, y + .5f);
                            Vector3 d = new Vector3(x - .5f, -1, y - .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (x < size - 1)
                    {
                        Cell right = grid[x + 1, y];
                        if (right.isWater)
                        {
                            Vector3 a = new Vector3(x + .5f, 0, y - .5f);
                            Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                            Vector3 c = new Vector3(x + .5f, -1, y - .5f);
                            Vector3 d = new Vector3(x + .5f, -1, y + .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (y > 0)
                    {
                        Cell down = grid[x, y - 1];
                        if (down.isWater)
                        {
                            Vector3 a = new Vector3(x - .5f, 0, y - .5f);
                            Vector3 b = new Vector3(x + .5f, 0, y - .5f);
                            Vector3 c = new Vector3(x - .5f, -1, y - .5f);
                            Vector3 d = new Vector3(x + .5f, -1, y - .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (y < size - 1)
                    {
                        Cell up = grid[x, y + 1];
                        if (up.isWater)
                        {
                            Vector3 a = new Vector3(x + .5f, 0, y + .5f);
                            Vector3 b = new Vector3(x - .5f, 0, y + .5f);
                            Vector3 c = new Vector3(x + .5f, -1, y + .5f);
                            Vector3 d = new Vector3(x - .5f, -1, y + .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                }
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GameObject edgeObj = new GameObject("Edge");
        edgeObj.transform.SetParent(transform);
        edgeObj.transform.localPosition = Vector3.zero;
        MeshFilter meshFilter = edgeObj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = edgeObj.AddComponent<MeshRenderer>();
        meshRenderer.material = edgeMaterial;
    }
    void DrawTerrainMesh()
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                    Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                    Vector3 c = new Vector3(x - .5f, 0, y - .5f);
                    Vector3 d = new Vector3(x + .5f, 0, y - .5f);
                    Vector2 uvA = new Vector2(x / (float)size, y / (float)size);
                    Vector2 uvB = new Vector2((x + 1) / (float)size, y / (float)size);
                    Vector2 uvC = new Vector2(x / (float)size, (y + 1) / (float)size);
                    Vector2 uvD = new Vector2((x + 1) / (float)size, (y + 1) / (float)size);
                    Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                    Vector2[] uv = new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC };
                    for (int k = 0; k < 6; k++)
                    {
                        vertices.Add(v[k]);
                        triangles.Add(triangles.Count);
                        uvs.Add(uv[k]);
                    }
                }
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshCollider>();
    }
    void GenerateTrees()
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * treeNoiseScale + xOffset, y * treeNoiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }
        int r = 20;
        for (int y = size / 2 - r; y < r + size / 2; y++)
        {
            int yDiff = Mathf.Abs(y - size / 2);
            int absX = (int)Mathf.Sqrt(r * r - yDiff * yDiff);
            for (int x = size / 2 - absX; x < size / 2 + absX; x++)
            {
                float noiseValue = 1f;
                noiseMap[x, y] = noiseValue;
            }
        }
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    float v = Random.Range(0, berryDensity);
                    if (noiseMap[x, y] < v)
                    {
                        GameObject prefab;
                        if (noiseMap[x, y] < rockDensity)
                            prefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
                        else if (noiseMap[x, y] < treeDensity)
                            prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                        else
                            prefab = berryBushPrefabs[Random.Range(0, berryBushPrefabs.Length)];
                        GameObject item = Instantiate(prefab, transform);
                        item.transform.position = new Vector3(x, 0, y);
                        item.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        item.transform.localScale = Vector3.one * Random.Range(.5f, 1f);
                    }
                }
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (cell.isWater)
                    Gizmos.color = Color.blue;
                else
                    Gizmos.color = Color.green;
                Vector3 pos = new Vector3(x - size / 2, 0, y - size / 2);
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }
    }


    private void coast()
    {

        for (int y = 1; y < size - 1; y++)
        {
            for (int x = 1; x < size - 1; x++)
            {
                if (grid[x, y].isWater) continue;
                Cell up = grid[x, y + 1];
                Cell down = grid[x, y - 1];
                Cell right = grid[x + 1, y];
                Cell left = grid[x - 1, y];

                if (up.isWater || down.isWater || right.isWater || left.isWater)
                {
                    grid[x, y].isCoast = true;
                }
            }
        }
    }
    public bool isWater(Vector2 position, Vector2 BuildSize)
    {
        int index1 = ((int)(position.x - ((int)BuildSize.x / 2)));
        int index2 = ((int)(position.y - ((int)BuildSize.y / 2)));


        for (int y = index2; y < index2 + BuildSize.y; y++)
            for (int x = index1; x < index1 + BuildSize.x; x++)
            {
                if (x < 0 || x > 150 || y < 0 || y > 150) continue;
                if (grid[x, y].isWater) return true;
            }

        return false;
    }
   
}

