using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;

public class ObjExporter
{
    public static string MeshToString(Mesh m)
    {
        BoneWeight1[] boneWeights = m.GetAllBoneWeights().ToArray();
        Debug.Log(boneWeights.Length);

        StringBuilder sb = new StringBuilder();

        for(int i=0; i < m.vertexCount; i++)
        {
            Vector3 v = m.vertices[i];
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
        }
        for (int i=0; i < m.triangles.Length; i+=3)
        {
            sb.Append(string.Format("f {0} {1} {2}\n",
                    m.triangles[i]+1, m.triangles[i+1]+1, m.triangles[i+2]+1));
        }
        
        return sb.ToString();
    }

    public static void MeshToFile(Mesh m, string filename) {
        if (filename.Length < 4 || string.Compare(filename.ToLower().Substring(filename.Length-4), ".obj") != 0)
        {
            filename += ".obj";
        }
        using (StreamWriter streamWriter = new StreamWriter(filename))
        {
            streamWriter.Write(MeshToString(m));
        }
    }

    public static void BoneWeightsToFile(Transform[] bones, Mesh m, string filename)
    {
        if (filename.Length < 4 || string.Compare(filename.ToLower().Substring(filename.Length - 4), ".txt") != 0)
        {
            filename += ".txt";
        }
        using (StreamWriter streamWriter = new StreamWriter(filename))
        {
            StringBuilder sb = new StringBuilder();
            BoneWeight[] boneWeights = m.boneWeights.ToArray();
            sb.Append("# Bone Positions\n");
            for (int i = 0; i < bones.Length; i++)
            {
                sb.Append(string.Format("{0} {1} {2}\n",
                    bones[i].position.x, bones[i].position.y, bones[i].position.z));
            }
            sb.Append("\n");
            sb.Append("# Bone Weights\n");
            for (int i = 0; i < boneWeights.Length; i++)
            {
                BoneWeight b = boneWeights[i];
                sb.Append(string.Format("{0} {1} {2} {3} ", 
                    b.boneIndex0, b.boneIndex1, b.boneIndex2, b.boneIndex3));
                sb.Append(string.Format("{0} {1} {2} {3}\n",
                    b.weight0, b.weight1, b.weight2, b.weight3));
            }

            streamWriter.Write(sb.ToString());
        }
    }
}
