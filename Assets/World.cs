using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

	public Material textureAtlas;
	public static int columnHeight = 10;
    public static int rowWidth = 4;
    public static int rowDepth = 4;

	public static int chunkSize = 10;
	public static Dictionary<string, Chunk> chunks;

	public static string BuildChunkName(Vector3 v)
	{
		return (int)v.x + "_" + 
			         (int)v.y + "_" + 
			         (int)v.z;
	}

	IEnumerator BuildChunkColumn()
	{
		for(int y = 0; y < columnHeight; y++)
        { 
            for(int x = 0; x < rowWidth; x++) 
            {
                for (int z = 0; z < rowDepth; z++)
                {
                    Vector3 chunkPosition = new Vector3(x * chunkSize,
                                                        y * chunkSize,
                                                        z * chunkSize);
                    Chunk c = new Chunk(chunkPosition, textureAtlas);
                    c.chunk.transform.parent = this.transform;
                    chunks.Add(c.chunk.name, c);
                }
            }
		}

		foreach(KeyValuePair<string, Chunk> c in chunks)
		{
			c.Value.DrawChunk();
			yield return null;
		}
		
	}

	// Use this for initialization
	void Start () {
		chunks = new Dictionary<string, Chunk>();
		this.transform.position = Vector3.zero;
		this.transform.rotation = Quaternion.identity;
		StartCoroutine(BuildChunkColumn());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
