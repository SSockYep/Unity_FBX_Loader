using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;

struct Edge{
        public int v1;
        public int v2;
        public int boneIndex;
};

class ItemEqualityComparer:IEqualityComparer<Edge>
{
    public bool Equals(Edge x, Edge y)
    {
        return x.v1 == y.v1 && x.v2 == y.v2;
    }
    public int GetHashCode(Edge e)
    {
        return e.GetHashCode();
    }
}
public class ObjExporter
{
    public static string MeshToString(Mesh m)
    {
        BoneWeight1[] boneWeights = m.GetAllBoneWeights().ToArray();
        List<Edge> edges = new List<Edge>();

        StringBuilder sb = new StringBuilder();

        foreach(Vector3 v in m.vertices)
        {
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
        }
        for (int i=0; i < m.triangles.Length; i+=3)
        {
            sb.Append(string.Format("f {0} {1} {2}\n",
                    m.triangles[i], m.triangles[i+1], m.triangles[i+2]));
            BoneWeight1[] weights = 
            {
                boneWeights[m.triangles[i]],
                boneWeights[m.triangles[i+1]],
                boneWeights[m.triangles[i+2]]
            };
            for (int j=0; j<3; j++)
            {
                int bi;
                if (weights[j].boneIndex == weights[(j+1)%3].boneIndex || 
                        weights[j].weight >= weights[(j+1)%3].weight)
                {
                    bi = weights[j].boneIndex;
                }
                else
                {
                    bi = weights[(j+1)%3].boneIndex;
                }
                Edge edge;
                edge.v1 = m.triangles[i+j] < m.triangles[i+(j+1)%3] ? m.triangles[i+j] : m.triangles[i+(j+1)%3];
                edge.v2 = m.triangles[i+j] >= m.triangles[i+(j+1)%3] ? m.triangles[i+j] : m.triangles[i+(j+1)%3];
                edge.boneIndex = bi;
                edges.Add(edge);
            }
            
        }

        edges = edges.Distinct().ToList();

        /*foreach (Edge e in edges)
        {
            sb.Append(string.Format("e {0} {1} {2}\n", e.v1, e.v2, e.boneIndex));
        }*/
        return edges.ToString();
    }

    public static void MeshToFile(Mesh m, string filename) {
        if (filename.Length < 4 || string.Compare(filename.Substring(filename.Length-4), ".obj") != 0)
        {
            filename += ".obj";
        }
        using (StreamWriter streamWriter = new StreamWriter(filename))
        {
            streamWriter.Write(MeshToString(m));
        }
    }
}
