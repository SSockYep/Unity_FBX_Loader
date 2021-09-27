using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FbxLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] fbxs = Resources.LoadAll<GameObject>("FBXs/");

        for (int i = 0; i < fbxs.Length; i++)
        {
            Instantiate(fbxs[i]);
            string datapath = Application.persistentDataPath + fbxs[i].name + ".obj";
            SkinnedMeshRenderer[] renderers = fbxs[i].transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            CombineInstance[] combine = new CombineInstance[renderers.Length];
            for (int j = 0; j < renderers.Length; j++)
            {
                combine[j].mesh = renderers[j].sharedMesh;
                combine[j].transform = renderers[j].transform.localToWorldMatrix;
            }
            transform.GetComponent<MeshFilter>().mesh = new Mesh();
            transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            transform.gameObject.SetActive(true);
            ObjExporter.MeshToFile(transform.GetComponent<MeshFilter>().mesh, datapath);
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
