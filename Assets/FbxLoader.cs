using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class FbxLoader : MonoBehaviour
{
    List<GameObject> meshes = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] fbxs = Resources.LoadAll<GameObject>("FBXs/");

        for (int i = 0; i < fbxs.Length; i++)
        {
            GameObject instantiated = (GameObject) Instantiate(fbxs[i]);
            meshes.Add(instantiated);

            // string datapath = Application.persistentDataPath + fbxs[i].name + ".obj";
            string datapath = "Results/" + fbxs[i].name;
            SkinnedMeshRenderer[] renderers = fbxs[i].transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            CombineInstance[] combine = new CombineInstance[renderers.Length];
            List<Transform> bones = new List<Transform>();
            for (int j = 0; j < renderers.Length; j++)
            {
                // combine[j].mesh = renderers[j].sharedMesh;
                Mesh tmp = new Mesh();
                renderers[j].BakeMesh(tmp);
                combine[j].mesh = tmp;
                combine[j].transform = renderers[j].transform.localToWorldMatrix;
                bones.AddRange(renderers[j].bones);
            }

            transform.GetComponent<MeshFilter>().mesh = new Mesh();
            transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            Matrix4x4[] bindPoses = transform.GetComponent<MeshFilter>().mesh.bindposes;
            
            // GameObject�� transform�� �б� ����
            // bones�� Transform���� ���ְ�... ��� matrix ��������?

            transform.gameObject.SetActive(true);
            Debug.Log(transform.GetComponent<MeshFilter>().mesh.bindposes.Length);

            Debug.Log(bones.Count);
            ObjExporter.MeshToFile(transform.GetComponent<MeshFilter>().mesh, datapath);
            ObjExporter.BonesToFile(bones.ToArray(), datapath+"_Skeleton");
            ObjExporter.BoneWeightsToFile(transform.GetComponent<MeshFilter>().mesh, datapath+"_Weight");
            Destroy(instantiated);
            
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}
