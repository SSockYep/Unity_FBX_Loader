using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System;
using UnityEngine;

public class ObjExporter
{
    public static string MeshToString(Mesh m)
    {
        BoneWeight1[] boneWeights = m.GetAllBoneWeights().ToArray();

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

    public static void BonesToFile(Transform[] bones, string filename)
    {
        if (filename.Length < 4 || string.Compare(filename.ToLower().Substring(filename.Length - 4), ".txt") != 0)
        {
            filename += ".txt";
        }
        using (StreamWriter streamWriter = new StreamWriter(filename))
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bones.Length; i++)
            {
                sb.Append(string.Format("{0} {1} {2}\n",
                    bones[i].position.x, bones[i].position.y, bones[i].position.z));
            }

            streamWriter.Write(sb.ToString());
        }
    }
    public static void BoneWeightsToFile(Mesh m, string filename)
    {
        if (filename.Length < 4 || string.Compare(filename.ToLower().Substring(filename.Length - 4), ".txt") != 0)
        {
            filename += ".txt";
        }
        using (StreamWriter streamWriter = new StreamWriter(filename))
        {
            StringBuilder sb = new StringBuilder();
            BoneWeight[] boneWeights = m.boneWeights.ToArray();
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
    
    public static void VertexLocalPositionToFile(Mesh m, Matrix4x4[] bindPoses, string filename)
    {
        if (filename.Length < 4 || string.Compare(filename.ToLower().Substring(filename.Length - 4), ".txt") != 0)
        {
            filename += ".txt";
        }
        using (StreamWriter streamWriter = new StreamWriter(filename))
        {
            StringBuilder sb = new StringBuilder();
            Vector3[] vertices = m.vertices;
            BoneWeight[] boneWeights = m.boneWeights.ToArray();
            if (vertices.Length != boneWeights.Length)
            {
                throw new Exception("different number of vertices");
            }
            for (int i = 0; i < vertices.Length; i++)
            {
                BoneWeight b = boneWeights[i];
                Vector3 localpos0 = bindPoses[b.boneIndex0].MultiplyPoint3x4(vertices[i]);
                sb.Append(string.Format("{0} {1} {2}\t", localpos0.x, localpos0.y, localpos0.z));
                if (b.weight1 > 0)
                {
                    Vector3 localpos1 = bindPoses[b.boneIndex1].MultiplyPoint3x4(vertices[i]);
                    sb.Append(string.Format("{0} {1} {2}\t", localpos1.x, localpos1.y, localpos1.z));
                }
                if (b.weight2 > 0)
                {
                    Vector3 localpos2 = bindPoses[b.boneIndex2].MultiplyPoint3x4(vertices[i]);
                    sb.Append(string.Format("{0} {1} {2}\t", localpos2.x, localpos2.y, localpos2.z));
                }
                if (b.weight3 > 0)
                {
                    Vector3 localpos3 = bindPoses[b.boneIndex3].MultiplyPoint3x4(vertices[i]);
                    sb.Append(string.Format("{0} {1} {2}\t", localpos3.x, localpos3.y, localpos3.z));
                }
                sb.Append("\n");
            }

            streamWriter.Write(sb.ToString());
        }
    }
}
