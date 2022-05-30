using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GraphNoise : MonoBehaviour {

	float t = 0;
    float inc = 0.01f;
    public static int maxHeight = 150;
    static float smooth = 0.003f;
    static int octaves = 4;
    static float persistence = 0.8f;

	static float Map(float min, float max, float omin, float omax, float value)
    {
        return Mathf.Lerp (min, max, Mathf.InverseLerp (omin, omax, value));
    }

    public static int GenHeight(float x, float z)
    {
        float height = Map(0, maxHeight, 0, 1, FBM(x * smooth, z * smooth, octaves, persistence));
        return (int)height;
    }

    public static int GenHeightStone(float x, float z)
    {
        float height = Map(0, maxHeight-10, 0, 1, FBM(x * smooth*3, z * smooth*3, octaves+3, persistence));
        return (int)height;
    }

    public static int GenHeightGrass(float x, float z)
    {
        float height = Map(0, maxHeight+1, 0, 1, FBM(x * smooth, z * smooth, octaves, persistence));
        return (int)height;
    }



    void Update () 
	{
        t += inc;
        //float n = FBM(t,6,0.8f);
        //Grapher.Log(n, "Perlin1", Color.red);  
	}

	static float FBM(float x, float z, int octaves, float persistence)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0; 
        for(int i = 0; i < octaves; i++) 
        {
                total += Mathf.PerlinNoise(x * frequency, z * frequency) * amplitude;
                maxValue += amplitude;
                amplitude *= persistence;
                frequency *= 2;
        }

        return total/maxValue;
    }

    public static float FBM3D(float x, float y, float z, int oct, float pers, float smoothCaves)
    {   
        
        float XY = FBM(x * smoothCaves, y * smoothCaves, oct, 0.5f);
        float YZ = FBM(y * smoothCaves, z * smoothCaves, oct, 0.5f);
        float XZ = FBM(x * smoothCaves, z * smoothCaves, oct, 0.5f);

        float YX = FBM(y * smoothCaves, x * smoothCaves, oct, 0.5f);
        float ZY = FBM(z * smoothCaves, y * smoothCaves, oct, 0.5f);
        float ZX = FBM(z * smoothCaves, x * smoothCaves, oct, 0.5f);

        return (XY + YZ + XZ + YX + ZY + ZX) / 6.0f;
    }
}
