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
