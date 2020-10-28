using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen2 : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;
    [Range(0, 254)]
    public int xSize = 254;
    [Range(0, 254)]
    public int zSize = 254;
    [Range(0f, 10f)]
    public float strength = 2f;
    [Range(0f, 5f)]
    public float noiseSize = .3f;
    public Gradient gradient;
    private float minHeight = 0;
    private float maxHeight = 0;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape () {
        vertices = new Vector3 [(xSize+1) * (zSize+1)];

        int i = 0;

        vertices[i] = new Vector3(1-(xSize/2), 0, 1-(zSize/2));
        i++;

        for (int x = 1; x < xSize; x++){
            vertices[i] = new Vector3(x - (xSize/2), 0, 1-(zSize/2));
            i++;
        }

        vertices[i] = new Vector3(xSize-(xSize/2)-1, 0, 1-(zSize/2));
        i++;

        for (int z = 1; z < zSize; z++){
            vertices[i] = new Vector3(1-(xSize/2), 0, z-(zSize/2));
            i++;

            for (int x = 1; x < xSize; x++){
                float y = 
                (Mathf.PerlinNoise((x - xSize/2) * noiseSize / 10, (z - zSize/2) * noiseSize / 10) * strength * 10) +
                (Mathf.PerlinNoise((x - xSize/2) * noiseSize / 5, (z - zSize/2) * noiseSize / 5) * strength * 5) + 
                (Mathf.PerlinNoise((x - xSize/2) * noiseSize, (z - zSize/2) * noiseSize) * strength) + 
                (xSize-x)*(xSize-x)/300;

                vertices[i] = new Vector3(x - (xSize/2), y, z - (zSize/2));
                if(vertices[i].y > maxHeight) maxHeight = vertices[i].y;
                i++;
            }

            vertices[i] = new Vector3(xSize-(xSize/2)-1, 0, z - (zSize/2));
            i++;
        }

        vertices[i] = new Vector3(1-(xSize/2), 0, zSize-(zSize/2)-1);
        i++;

        for (int x = 1; x < xSize; x++){
            vertices[i] = new Vector3(x - (xSize/2), 0, zSize-(zSize/2)-1);
            i++;
        }

        vertices[i] = new Vector3(xSize-(xSize/2)-1, 0, zSize-(zSize/2)-1);
        i++;

        triangles = new int[xSize * zSize * 6];

        for (int z = 0, vert = 0, tris = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++){
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        i = 0;
        colors = new Color[vertices.Length];

        for (int z = 0; z < zSize+1; z++){
            for (int x = 0; x < xSize+1; x++){
                float height = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    void UpdateMesh() {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.Optimize();
        mesh.RecalculateNormals();
    }
}
