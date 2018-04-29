using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CubemapSHProjector : EditorWindow
{
    //PUBLIC FIELDS
    public Cubemap input_cubemap;

    //PRIVATE FIELDS
    private Material    view_mat;
    private float       view_mode;
    private Vector4[]   coefficients;

    private SerializedObject so;
    private SerializedProperty sp_input_cubemap;

    private Texture2D tmp = null;

    [MenuItem("Window/CubemapSHProjector")]
    static void Init()
    {
        CubemapSHProjector window = (CubemapSHProjector)EditorWindow.GetWindow(typeof(CubemapSHProjector));
        window.Show();
    }

    private void OnFocus()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        so = new SerializedObject(this);
        sp_input_cubemap = so.FindProperty("input_cubemap");

        if (view_mat == null)
            view_mat = new Material(Shader.Find("SH/CoeffVisualizer"));
    }

    private void OnGUI()
    {
        EditorGUILayout.PropertyField(sp_input_cubemap, new GUIContent("Input Cubemap"));

        so.ApplyModifiedProperties();

        if (input_cubemap != null)
        {
            EditorGUILayout.Space();

            if (GUILayout.Button("CPU Uniform 9 Coefficients"))
            {
                coefficients = new Vector4[9];
                if (SphericalHarmonics.CPU_Project_Uniform_9Coeff(input_cubemap, coefficients))
                {
                    for (int i = 0; i < 9; ++i)
                    {
                        view_mat.SetVector("c" + i.ToString(), coefficients[i]);
                        view_mat.SetTexture("input", input_cubemap);
                    }

                    SceneView.RepaintAll();
                }
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("CPU Monte Carlo 9 Coefficients"))
            {
                coefficients = new Vector4[9];
                if (SphericalHarmonics.CPU_Project_MonteCarlo_9Coeff(input_cubemap, coefficients, 4096))
                {
                    for (int i = 0; i < 9; ++i)
                    {
                        view_mat.SetVector("c" + i.ToString(), coefficients[i]);
                        view_mat.SetTexture("input", input_cubemap);
                    }

                    SceneView.RepaintAll();
                }
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("GPU Uniform 9 Coefficients"))
            {
                coefficients = new Vector4[9];
                if (SphericalHarmonics.GPU_Project_Uniform_9Coeff(input_cubemap, coefficients))
                {
                    for (int i = 0; i < 9; ++i)
                    {
                        view_mat.SetVector("c" + i.ToString(), coefficients[i]);
                        view_mat.SetTexture("input", input_cubemap);
                    }

                    SceneView.RepaintAll();
                }
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("GPU Monte Carlo 9 Coefficients"))
            {
                coefficients = new Vector4[9];
                
                if (SphericalHarmonics.GPU_Project_MonteCarlo_9Coeff(input_cubemap, coefficients))
                {
                    for (int i = 0; i < 9; ++i)
                    {
                        view_mat.SetVector("c" + i.ToString(), coefficients[i]);
                        view_mat.SetTexture("input", input_cubemap);
                    }
                }
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Show"))
            {
                RenderSettings.skybox = view_mat;
            }

            view_mode = EditorGUILayout.Slider(view_mode, 0, 1);
            view_mat.SetFloat("_Mode", view_mode);

            EditorGUILayout.Space();

            //print the 9 coefficients
            if (coefficients != null)
            {
                for (int i = 0; i < 9; ++i)
                {
                    EditorGUILayout.LabelField("c_" + i.ToString() + ": " + coefficients[i].ToString("f4"));
                }
            }
        }

        EditorGUILayout.Space();
        if (tmp != null)
            GUILayout.Label(tmp);
    }
}
