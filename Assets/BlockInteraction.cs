using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockInteraction : MonoBehaviour {

	public GameObject cam;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if (Input.GetMouseButton(1)) {
			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100))
            {
   				Vector3 hitBlock0 = hit.point - hit.normal/2.0f; 

   				int x = (int) (Mathf.Round(hitBlock0.x) - hit.collider.gameObject.transform.position.x);
   				int y = (int) (Mathf.Round(hitBlock0.y) - hit.collider.gameObject.transform.position.y);
   				int z = (int) (Mathf.Round(hitBlock0.z) - hit.collider.gameObject.transform.position.z);

				float xPrec = ((hitBlock0.x) - hit.collider.gameObject.transform.position.x) -  x;
				float zPrec = ((hitBlock0.z) - hit.collider.gameObject.transform.position.z) -  z;
				//float angle = Vector3.Angle(hit.normal, hit);
				//Debug.Log("hit.pt: "+hit.point + " transform.pos: "+transform.position+" transform.up: "+transform.up);
				Vector3 from = hit.point - transform.position;
				Vector3 to   = transform.up;
				float angle0 = Vector3.Angle(from, to);
				//Debug.Log(angle0);
				// Debug.Log("preX" + xPrec);
				// Debug.Log("preZ" + zPrec);
				//xPrec = (xPrec*2) - .5f;
				// zPrec = (zPrec*2) - .5f;
				// Debug.Log(xPrec);
				// Debug.Log(zPrec);

				Vector3 hp = hit.point;
				Vector3 tp = transform.position;
				// double angle = Mathf.Atan((tp.y - hp.y)/( tp.x - hp.x)) * (180 / Mathf.PI);
				float angle = Mathf.Atan((tp.x - hp.x)/( tp.z - hp.z)) * (180 / Mathf.PI);
				//Debug.Log("pre" +  ((tp.z - hp.z)/( tp.x - hp.x)) * (180 / Mathf.PI));
				// Debug.Log(angle);
				// Debug.Log("post" + Mathf.Tan(angle));

   				List<string> updates = new List<string>();
   				float thisChunkx = hit.collider.gameObject.transform.position.x;
   				float thisChunky = hit.collider.gameObject.transform.position.y;
   				float thisChunkz = hit.collider.gameObject.transform.position.z;

				//update neighbours?
   				if(x == 0) 
   					updates.Add(World.BuildChunkName(new Vector3(thisChunkx-World.chunkSize,thisChunky,thisChunkz)));
				if(x == World.chunkSize - 1) 
					updates.Add(World.BuildChunkName(new Vector3(thisChunkx+World.chunkSize,thisChunky,thisChunkz)));
				if(y == 0) 
					updates.Add(World.BuildChunkName(new Vector3(thisChunkx,thisChunky-World.chunkSize,thisChunkz)));
				if(y == World.chunkSize - 1) 
					updates.Add(World.BuildChunkName(new Vector3(thisChunkx,thisChunky+World.chunkSize,thisChunkz)));
				if(z == 0) 
					updates.Add(World.BuildChunkName(new Vector3(thisChunkx,thisChunky,thisChunkz-World.chunkSize)));
				if(z == World.chunkSize - 1) 
					updates.Add(World.BuildChunkName(new Vector3(thisChunkx,thisChunky,thisChunkz+World.chunkSize)));

				World.chunks.TryGetValue(hit.collider.gameObject.name, out Chunk c);
				if (!(c.chunkData[x,y,z].bType == Block.BlockType.STONE)) {
					DestroyImmediate(c.chunk.GetComponent<MeshFilter>());
					DestroyImmediate(c.chunk.GetComponent<MeshRenderer>());
					DestroyImmediate(c.chunk.GetComponent<Collider>());

					
					if (c.chunkData[x,y,z].init == false) {

						float intercept = (tp.x - hp.x)/( tp.z - hp.z);
						Debug.Log(intercept);
						intercept = intercept * (-1);

						if (zPrec > -9999) {
							c.chunkData[x,y,z].q0 = new Vector3(  intercept, -0.5f, -0.5f);
							c.chunkData[x,y,z].q1 = new Vector3(  xPrec, -0.5f,  0.5f);
							c.chunkData[x,y,z].q2 = new Vector3(  intercept,  0.5f, -0.5f);
							c.chunkData[x,y,z].q3 = new Vector3(  xPrec,  0.5f,  0.5f);
						}

						else {
							c.chunkData[x,y,z].q0 = new Vector3(  xPrec, -0.5f, -0.5f);
							c.chunkData[x,y,z].q1 = new Vector3(  Random.Range(-0.3f,0.3f), -0.5f,  0.5f);
							c.chunkData[x,y,z].q2 = new Vector3(  xPrec,  0.5f, -0.5f);
							c.chunkData[x,y,z].q3 = new Vector3(  Random.Range(-0.3f,0.3f),  0.5f,  0.5f);
						}
					
					c.chunkData[x,y,z].init = true;
					}

					// c.chunkData[x0,y0,z0].q0 = new Vector3( 0.0f, -0.1f, -0.3f);
					// c.chunkData[x0,y0,z0].q1 = new Vector3( 0.0f, -0.2f,  0.2f);
					// c.chunkData[x0,y0,z0].q2 = new Vector3( 0.0f,  0.3f, -0.1f);
					// c.chunkData[x0,y0,z0].q3 = new Vector3( 0.0f,  0.1f,  0.2f);
					if(xPrec > 0)
						c.chunkData[x,y,z].SetType(Block.BlockType.REDSTONER);
					else 
						c.chunkData[x,y,z].SetType(Block.BlockType.REDSTONEL);
					
					c.DrawChunk();
				}
				
   				// updates.Add(hit.collider.gameObject.name);

				//    foreach(string cname in updates) {
				// 	Chunk c;
				// 	if(World.chunks.TryGetValue(cname, out c))
				// 	{
				// 		c.chunkData[x0,y0,z0].SetType(Block.BlockType.STONE);
				// 	}
				// }
			}
		}
		



		if (Input.GetMouseButton(0))
        {
            //RaycastHit hit;
            
            //for mouse clicking
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
   			//if ( Physics.Raycast (ray,out hit,10)) 
   			//{
            
   			//for cross hairs
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100))
            {
   				Vector3 hitBlock = hit.point - hit.normal/2.0f; 

   				int x = (int) (Mathf.Round(hitBlock.x) - hit.collider.gameObject.transform.position.x);
   				int y = (int) (Mathf.Round(hitBlock.y) - hit.collider.gameObject.transform.position.y);
   				int z = (int) (Mathf.Round(hitBlock.z) - hit.collider.gameObject.transform.position.z);

   				List<string> updates = new List<string>();
   				float thisChunkx = hit.collider.gameObject.transform.position.x;
   				float thisChunky = hit.collider.gameObject.transform.position.y;
   				float thisChunkz = hit.collider.gameObject.transform.position.z;

   				updates.Add(hit.collider.gameObject.name);
				
			

   				//update neighbours?
   				if(x == 0) 
   					updates.Add(World.BuildChunkName(new Vector3(thisChunkx-World.chunkSize,thisChunky,thisChunkz)));
				if(x == World.chunkSize - 1) 
					updates.Add(World.BuildChunkName(new Vector3(thisChunkx+World.chunkSize,thisChunky,thisChunkz)));
				if(y == 0) 
					updates.Add(World.BuildChunkName(new Vector3(thisChunkx,thisChunky-World.chunkSize,thisChunkz)));
				if(y == World.chunkSize - 1) 
					updates.Add(World.BuildChunkName(new Vector3(thisChunkx,thisChunky+World.chunkSize,thisChunkz)));
				if(z == 0) 
					updates.Add(World.BuildChunkName(new Vector3(thisChunkx,thisChunky,thisChunkz-World.chunkSize)));
				if(z == World.chunkSize - 1) 
					updates.Add(World.BuildChunkName(new Vector3(thisChunkx,thisChunky,thisChunkz+World.chunkSize)));

	   			foreach(string cname in updates)
	   			{
	   				Chunk c;
					if(World.chunks.TryGetValue(cname, out c))
					{
						DestroyImmediate(c.chunk.GetComponent<MeshFilter>());
						DestroyImmediate(c.chunk.GetComponent<MeshRenderer>());
						DestroyImmediate(c.chunk.GetComponent<Collider>());
						c.chunkData[x,y,z].SetType(Block.BlockType.AIR);
				   		c.DrawChunk();
			   		}
			   	}
		   	}
   		}

		if (Input.GetMouseButtonDown(2)) {
			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100)) {
			Vector3 hitBlock = hit.point + hit.normal/2.0f; 

   				int x = (int) (Mathf.Round(hitBlock.x) - hit.collider.gameObject.transform.position.x);
   				int y = (int) (Mathf.Round(hitBlock.y) - hit.collider.gameObject.transform.position.y);
   				int z = (int) (Mathf.Round(hitBlock.z) - hit.collider.gameObject.transform.position.z);

   				List<string> updates = new List<string>();
   				float thisChunkx = hit.collider.gameObject.transform.position.x;
   				float thisChunky = hit.collider.gameObject.transform.position.y;
   				float thisChunkz = hit.collider.gameObject.transform.position.z;

				World.chunks.TryGetValue(hit.collider.gameObject.name, out Chunk c);
				
				DestroyImmediate(c.chunk.GetComponent<MeshFilter>());
				DestroyImmediate(c.chunk.GetComponent<MeshRenderer>());
				DestroyImmediate(c.chunk.GetComponent<Collider>());
				c.chunkData[x,y,z].SetType(Block.BlockType.DIRT);
				c.DrawChunk();
			}

	}
	}
}

