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
                /* Get Shared Mesh (T-pose) */
                combine[j].mesh = renderers[j].sharedMesh;

                /* Get Baked Mesh */
                // Mesh tmp = new Mesh();
                // renderers[j].BakeMesh(tmp);
                // combine[j].mesh = tmp;

                combine[j].transform = renderers[j].transform.localToWorldMatrix;
                bones.AddRange(renderers[j].bones);
            }

            transform.GetComponent<MeshFilter>().mesh = new Mesh();
            transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            Matrix4x4[] bindPoses = transform.GetComponent<MeshFilter>().mesh.bindposes;

            // bindPoses[0] = bones[0].worldToLocalMatrix * transform.localToWorldMatrix;
            // position = new Vector3(mat[0,3], mat[1,3], mat[2,3]);
            // quaternion = Quaternion.LookRotation(new Vector3(mat[0,2], mat[1,2], mat[2,2]), new Vector3(mat[0,1], mat[1,1], mat[2,1]));
            // scale = new Vector3(mat.GetColumn(0).magnitude, mat.GetColumn(1).magnitude, mat.GetColumn(2).magnitude);

            /* Getting bind pose position */
            for (int j = 0; j < bindPoses.Length; j++)
            {
                Matrix4x4 mat = bones[j].localToWorldMatrix * bindPoses[j];
                bones[j].position = new Vector3(mat[0, 3], mat[1, 3], mat[2, 3]);
            }

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
