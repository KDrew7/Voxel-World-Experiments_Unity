using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Chunk {

	public Material cubeMaterial;
	public Block[,,] chunkData;
	public GameObject chunk;
    int prob;

	void BuildChunk()
	{
		chunkData = new Block[World.chunkSize,World.chunkSize,World.chunkSize];

		for(int z = 0; z < World.chunkSize; z++)
			for(int y = 0; y < World.chunkSize; y++)
				for(int x = 0; x < World.chunkSize; x++)
				{
                    
                    prob = 1000;
                    //prob = 16 - y;
					Vector3 pos = new Vector3(x,y,z);
                    int worldX = (int)(x+chunk.transform.position.x);
                    int worldY = (int)(y+chunk.transform.position.y);
                    int worldZ = (int)(z+chunk.transform.position.z);

                    if (worldY < 16 && worldY > 10)
                        chunkData[x, y, z] = new Block(Block.BlockType.AIR, pos, chunk.gameObject, this);


                    else if (worldY < GraphNoise.GenHeightStone(worldX, worldZ))
                    {
                        if (GraphNoise.FBM3D(worldX, worldY, worldZ, 2, 0.01f, .1f) < 0.38f)
                        {
                            chunkData[x, y, z] = new Block(Block.BlockType.DIAMOND, pos, chunk.gameObject, this);
                        }
                        else
                        chunkData[x, y, z] = new Block(Block.BlockType.STONE, pos, chunk.gameObject, this);
                    }


                    else if (worldY < GraphNoise.GenHeight(worldX, worldZ))
                        chunkData[x, y, z] = new Block(Block.BlockType.DIRT, pos, chunk.gameObject, this);

                    else if (worldY < GraphNoise.GenHeight(worldX, worldZ) + 1)
                        chunkData[x, y, z] = new Block(Block.BlockType.GRASS, pos, chunk.gameObject, this);

                    else
                        //if (worldY >= GraphNoise.GenHeight(worldX, worldZ))
                        chunkData[x, y, z] = new Block(Block.BlockType.AIR, pos,
                                        chunk.gameObject, this);

                    if (GraphNoise.FBM3D(worldX, worldY, worldZ, 3, 0.5f, .1f) < 0.385f)
                    {
                        chunkData[x, y, z] = new Block(Block.BlockType.AIR, pos, chunk.gameObject, this);
                    }

                    if (worldX > 35 && worldX < 40 && worldZ > 35) {
                        chunkData[x, y, z] = new Block(Block.BlockType.DIRT, pos, chunk.gameObject, this);
                    }
                }
	}

	public void DrawChunk()
	{
		for(int z = 0; z < World.chunkSize; z++)
			for(int y = 0; y < World.chunkSize; y++)
				for(int x = 0; x < World.chunkSize; x++)
				{
					chunkData[x,y,z].Draw();	
				}
		CombineQuads();
        MeshCollider collider = chunk.gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        collider.sharedMesh = chunk.transform.GetComponent<MeshFilter>().mesh;
	}

	// Use this for initialization
	public Chunk (Vector3 position, Material c) {
		
		chunk = new GameObject(World.BuildChunkName(position));
		chunk.transform.position = position;
		cubeMaterial = c;
		BuildChunk();
	}
	
	void CombineQuads()
	{
		//1. Combine all children meshes
		MeshFilter[] meshFilters = chunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        //2. Create a new mesh on the parent object
        MeshFilter mf = (MeshFilter) chunk.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();

        //3. Add combined meshes on children as the parent's mesh
        mf.mesh.CombineMeshes(combine);

        //4. Create a renderer for the parent
		MeshRenderer renderer = chunk.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		renderer.material = cubeMaterial;

		//5. Delete all uncombined children
		foreach (Transform quad in chunk.transform) {
     		GameObject.Destroy(quad.gameObject);
 		}

	}

}
