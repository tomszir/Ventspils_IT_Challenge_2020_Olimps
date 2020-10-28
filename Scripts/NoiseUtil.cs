using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseUtil {
    // There's no point in going below or above a certain octave anymore
    private static int MIN_OCTAVES = 1;
    private static int MAX_OCTAVES = 6;

    // 2D noise generation with default values preset
    public static float OctaveNoise2D(float x, float z) {
        return OctaveNoise2D(x, z, 6, 2, 0.5f, 0.05f);
    }

    // octaves     : Defines the amount of layers of stacked perlin noise
    // lacunarity  : Controls increase in the frequency of octaves
    // persistence : Controls decrease in amplitude (influence) of octaves
    // scale       : Scales the input coordinates to a better match perlin noise needs
    public static float OctaveNoise2D(float x, float y, int octaves, float lacunarity, float persistence, float scale, int seed = 80000) {
        float result = 0.0f;
        float frequency = 1.0f;
        float amplitude = 1.0f;
        float totalAmplitude = 0.0f;

        // Never let the scale be a whole number, because then every point will
        // be the same
        scale = scale + 0.0001f;
        octaves = Mathf.Min(Mathf.Max(octaves, MIN_OCTAVES), MAX_OCTAVES);

        for (int o = 0; o < octaves; o++) {
            result += Mathf.PerlinNoise(x * scale * frequency + seed, y * scale * frequency + seed) * amplitude;
            totalAmplitude += amplitude;
            amplitude *= persistence;
            frequency *= lacunarity;
        }

        // Scales the value back down to be between 0 and 1
        return result / totalAmplitude;
    }

    public static float OctaveNoise3D(float x, float y, float z) {
        return OctaveNoise3D(x, y, z, 6, 2, 0.5f, 0.05f);
    }

    public static float OctaveNoise3D(float x, float y, float z, int octaves, float lacunarity, float persistence, float scale) {
        float result = 0.0f;
        float frequency = 1.0f;
        float amplitude = 1.0f;
        float totalAmplitude = 0.0f;

        for (int o = 0; o < Mathf.Min(Mathf.Max(octaves, MIN_OCTAVES), MAX_OCTAVES); o++) {
            float xy = Mathf.PerlinNoise(x * scale * frequency, y * scale * frequency) * amplitude;
            float xz = Mathf.PerlinNoise(x * scale * frequency, z * scale * frequency) * amplitude;
            float yz = Mathf.PerlinNoise(y * scale * frequency, z * scale * frequency) * amplitude;
            float yx = Mathf.PerlinNoise(y * scale * frequency, x * scale * frequency) * amplitude;
            float zx = Mathf.PerlinNoise(z * scale * frequency, x * scale * frequency) * amplitude;
            float zy = Mathf.PerlinNoise(x * scale * frequency, y * scale * frequency) * amplitude;

            return (xy + xz + yz + yx + zx + zy) / 6f;

            result += (xy + xz + yz + yx + zx + zy) / 6f;
            totalAmplitude += amplitude;
            amplitude *= persistence;
            frequency *= lacunarity;
        }

        // Scales the value back down to be between 0 and 1
        return result / totalAmplitude;
    }
}
