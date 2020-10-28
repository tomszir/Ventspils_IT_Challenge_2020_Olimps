using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTexture : MonoBehaviour {
    [Header("Texture Size")]
    public int width = 200;
    public int height = 200;

    [Header("Noise Generation")]
    public int xOffset = 0;
    public int zOffset = 0;
    public int octaves = 6;
    public float lacunarity = 2f;
    public float persistence = 0.5f;
    [Range(0.0001f, 1f)]
    public float scale = 0.05f;
    public bool autoUpdate = true;

    private Vector3[] voxelCenterpoints;
    private GameObject[] cubes;

    void Start() {
        ApplyTexture();
        GenerateVoxels();
    }

    public void ApplyTexture() {
        Texture2D texture = CreateTexture();
        GetComponent<Renderer>().sharedMaterial.mainTexture = texture;
        texture.Apply();
    }

    public Texture2D CreateTexture() {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                float noise = NoiseUtil.OctaveNoise2D(x + xOffset, z + zOffset, octaves, lacunarity, persistence, scale);
                Color color = Color.Lerp(Color.black, Color.white, noise);
                texture.SetPixel(x, z, color);
            }
        }

        return texture;
    }

    public void GenerateVoxels() {
        int h = (int)Mathf.Floor(height / 16);
        int w = (int)Mathf.Floor(width / 16);

        for (int i = 0; i < cubes.Length; i++) {
          if (cubes[i]) {
            DestroyImmediate(cubes[i]);
          }
        }

        cubes = new GameObject[w * h];
        voxelCenterpoints = new Vector3[w * h];

        for (int x = 0; x < w; x++) {
            for (int z = 0; z < h; z++) {
                float noise = NoiseUtil.OctaveNoise2D(x + xOffset, z + zOffset, octaves, lacunarity, persistence, scale);
                Vector3 position = new Vector3(x, Mathf.Round(noise * 16), z);
                cubes[x * w + z] = Instantiate(GameObject.Find("Cube"), position, Quaternion.identity);
                voxelCenterpoints[x * w + z] = position;
            }
        }
    }

    private void OnDrawGizmos() {
        /*float w = Mathf.Floor(width / 16);
        for (int i = 0; i < voxelCenterpoints.Length; i++) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(voxelCenterpoints[i], 0.1f);
        }*/
    }
}
