using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            string datapath = "Results/" + instantiated.name;
            SkinnedMeshRenderer[] renderers = instantiated.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
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

            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combine);

            Matrix4x4[] bindPoses = combinedMesh.bindposes;

            /* Getting bind pose position */
            for (int j = 0; j < bindPoses.Length; j++)
            {
                Matrix4x4 mat = bindPoses[j].inverse;
                bones[j].position = new Vector3(mat[0, 3], mat[1, 3], mat[2, 3]);
            }

            SkinnedMeshRenderer smr = transform.GetComponent<SkinnedMeshRenderer>();
            smr.sharedMesh = combinedMesh;
            smr.bones = bones.ToArray();

            transform.gameObject.SetActive(true);

            ObjExporter.MeshToFile(combinedMesh, datapath);
            ObjExporter.BonesToFile(bones.ToArray(), datapath+"_Skeleton");
            ObjExporter.BoneWeightsToFile(combinedMesh, datapath+"_Weight");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
