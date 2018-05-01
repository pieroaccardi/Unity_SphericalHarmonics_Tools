using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VisibilityCompute : MonoBehaviour
{
    public void Compute()
    {
        Material old_skybox = RenderSettings.skybox;
        Material cosine_skybox = new Material(Shader.Find("SH/CosineSkybox"));
        RenderSettings.skybox = cosine_skybox;

        //create the folder with saved mesh assets
        if (!AssetDatabase.IsValidFolder("Assets/ComputedMeshes"))
        {
            AssetDatabase.CreateFolder("Assets", "ComputedMeshes");
        }

        //don't modify the original objects but make a copy
        GameObject parent = new GameObject("computed");
        parent.SetActive(false);
        
        GameObject tmp_camera_object = new GameObject("tmp_camera");
        Camera tmp_camera = tmp_camera_object.AddComponent<Camera>();
        tmp_camera.clearFlags = CameraClearFlags.Skybox;
        tmp_camera.nearClipPlane = 0.01f;
        tmp_camera.farClipPlane = 300;

        //temporary storage for the sh coefficients
        Vector4[] coefficients = new Vector4[9];

        //64x64 cubemap should be enough
        Cubemap visibility_cubemap = new Cubemap(64, TextureFormat.RGBA32, false);
        
        //cycle the children renderers
        MeshRenderer[] to_compute = gameObject.GetComponentsInChildren<MeshRenderer>();

        //get the total number of vertex to compute
        int total_vertex = 0;
        int completed_vertex = 0;
        foreach (MeshRenderer mr in to_compute)
        {
            if (mr.gameObject.isStatic)
                total_vertex = mr.gameObject.GetComponent<MeshFilter>().sharedMesh.vertexCount;
        }

        //shader for rendering the visibility (black = occluded)
        Shader black_shader = Shader.Find("SH/BlackShader");

        foreach (MeshRenderer mr in to_compute)
        {
            if (mr.gameObject.isStatic) //compute only static objects
            {
                //clone the original
                GameObject clone = Instantiate(mr.gameObject);
                clone.name = mr.gameObject.name;
                clone.transform.SetParent(parent.transform);

                Mesh mesh = Instantiate(mr.gameObject.GetComponent<MeshFilter>().sharedMesh);
                Vector3[] vertices = mesh.vertices;
                Vector3[] normals = mesh.normals;
                Vector2[] uv2 = new Vector2[mesh.vertexCount];
                Vector2[] uv3 = new Vector2[mesh.vertexCount];
                Vector2[] uv4 = new Vector2[mesh.vertexCount];
                Color[] colors = new Color[mesh.vertexCount];
 
                for (int v = 0; v < mesh.vertexCount; ++v)
                {
                    //vertices in the mesh are in local space, transform to world coordinate
                    Vector3 vertex_world_position = clone.transform.localToWorldMatrix.MultiplyPoint(vertices[v]);
                    Vector3 world_normal = clone.transform.localToWorldMatrix.MultiplyVector(normals[v]);
                    tmp_camera.transform.position = vertex_world_position + world_normal * 0.011f;  //offset the camera a little in the normal direction
                    cosine_skybox.SetVector("N", world_normal);

                    tmp_camera.SetReplacementShader(black_shader, "");
                    tmp_camera.RenderToCubemap(visibility_cubemap);
                    tmp_camera.ResetReplacementShader();

                    //project the cubemap to the spherical harmonic basis
                    SphericalHarmonics.GPU_Project_Uniform_9Coeff(visibility_cubemap, coefficients);
                    
                    //put 4 coefficients in the vertex color, 2 in the uv2 and 2 in the uv3 and 1 in uv4
                    colors[v] = new Color(coefficients[0].x, coefficients[1].x, coefficients[2].x, coefficients[3].x);
                    uv2[v] = new Vector2(coefficients[4].x, coefficients[5].x);
                    uv3[v] = new Vector2(coefficients[6].x, coefficients[7].x);
                    uv4[v] = new Vector2(coefficients[8].x, 0);

                    for (int i = 0; i < 9; ++i)
                        coefficients[i] = Vector4.zero;

                    completed_vertex++;
                }

                mesh.colors = colors;
                mesh.uv2 = uv2;
                mesh.uv3 = uv3;
                mesh.uv4 = uv4;
                mesh.UploadMeshData(true);

                //save the mesh
                AssetDatabase.CreateAsset(mesh, "Assets/ComputedMeshes/" + mr.name + ".asset");
                
                clone.GetComponent<MeshFilter>().sharedMesh = mesh;

                Material mat = new Material(Shader.Find("SH/SH_Shader"));
                clone.GetComponent<MeshRenderer>().material = mat;
            }
        }

        Object.DestroyImmediate(tmp_camera_object);

        gameObject.SetActive(false);
        parent.SetActive(true);

        RenderSettings.skybox = old_skybox;
    }
}
