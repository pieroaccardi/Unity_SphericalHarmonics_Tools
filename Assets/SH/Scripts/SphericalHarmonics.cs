using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float SH_Base(Vector3 v);

public class SphericalHarmonicsBasis
{
    public static float Y0(Vector3 v)
    {
        return 0.2820947917f;
    }

    public static float Y1(Vector3 v)
    {
        return 0.4886025119f * v.y;
    }

    public static float Y2(Vector3 v)
    {
        return 0.4886025119f * v.z;
    }

    public static float Y3(Vector3 v)
    {
        return 0.4886025119f * v.x;
    }

    public static float Y4(Vector3 v)
    {
        return 1.0925484306f * v.x * v.y;
    }

    public static float Y5(Vector3 v)
    {
        return 1.0925484306f * v.y * v.z;
    }

    public static float Y6(Vector3 v)
    {
        return 0.3153915652f * (3.0f * v.z * v.z - 1.0f);
    }

    public static float Y7(Vector3 v)
    {
        return 1.0925484306f * v.x * v.z;
    }

    public static float Y8(Vector3 v)
    {
        return 0.5462742153f * (v.x * v.x - v.y * v.y);
    }

    public static SH_Base[] Eval = { Y0, Y1, Y2, Y3, Y4, Y5, Y6, Y7, Y8 };
}

public class SphericalHarmonics
{
    //Convert a RenderTextureFormat to TextureFormat
    public static TextureFormat ConvertFormat(RenderTextureFormat input_format)
    {
        TextureFormat output_format = TextureFormat.RGBA32;

        switch (input_format)
        {
            case RenderTextureFormat.ARGB32:
                output_format = TextureFormat.RGBA32;
                break;

            case RenderTextureFormat.ARGBHalf:
                output_format = TextureFormat.RGBAHalf;
                break;

            case RenderTextureFormat.ARGBFloat:
                output_format = TextureFormat.RGBAFloat;
                break;

            default:
                string format_string = System.Enum.GetName(typeof(RenderTextureFormat), input_format);
                int format_int = (int)System.Enum.Parse(typeof(TextureFormat), format_string);
                output_format = (TextureFormat)format_int;
                break;
        }

        return output_format;
    }

    //Convert a TextureFormat to RenderTextureFormat
    public static RenderTextureFormat ConvertRenderFormat(TextureFormat input_format)
    {
        RenderTextureFormat output_format = RenderTextureFormat.ARGB32;

        switch (input_format)
        {
            case TextureFormat.RGBA32:
                output_format = RenderTextureFormat.ARGB32;
                break;

            case TextureFormat.RGBAHalf:
                output_format = RenderTextureFormat.ARGBHalf;
                break;

            case TextureFormat.RGBAFloat:
                output_format = RenderTextureFormat.ARGBFloat;
                break;

            default:
                string format_string = System.Enum.GetName(typeof(TextureFormat), input_format);
                int format_int = (int)System.Enum.Parse(typeof(RenderTextureFormat), format_string);
                output_format = (RenderTextureFormat)format_int;
                break;
        }

        return output_format;
    }

    //Convert a RenderTexture to a Cubemap
    public static Cubemap RenderTextureToCubemap(RenderTexture input)
    {
        if (input.dimension != UnityEngine.Rendering.TextureDimension.Cube)
        {
            Debug.LogWarning("Input render texture dimension must be cube");
            return null;
        }

        if (input.width != input.height)
        {
            Debug.LogWarning("Input render texture must be square");
            return null;
        }

        Cubemap output = new Cubemap(input.width, ConvertFormat(input.format), false);
        Texture2D tmp_face = new Texture2D(input.width, input.height, output.format, false);

        RenderTexture active = RenderTexture.active;

        for (int face = 0; face < 6; ++face)
        {
            Graphics.SetRenderTarget(input, 0, (CubemapFace)face);
            tmp_face.ReadPixels(new Rect(0, 0, input.width, input.height), 0, 0);
            output.SetPixels(tmp_face.GetPixels(), (CubemapFace)face);
        }
        output.Apply();

        RenderTexture.active = active;

        Object.DestroyImmediate(tmp_face);

        return output;
    }

    //differential solid angle
    public static float AreaElement(float x, float y)
    {
        return Mathf.Atan2(x * y, Mathf.Sqrt(x * x + y * y + 1));
    }

    public static float DifferentialSolidAngle(int textureSize, float U, float V)
    {
        float inv = 1.0f / textureSize;
        float u = 2.0f * (U + 0.5f * inv) - 1;
        float v = 2.0f * (V + 0.5f * inv) - 1;
        float x0 = u - inv;
        float y0 = v - inv;
        float x1 = u + inv;
        float y1 = v + inv;
        return AreaElement(x0, y0) - AreaElement(x0, y1) - AreaElement(x1, y0) + AreaElement(x1, y1);
    }

    public static Vector3 DirectionFromCubemapTexel(int face, float u, float v)
    {
        Vector3 dir = Vector3.zero;

        switch (face)
        {
            case 0: //+X
                dir.x = 1;
                dir.y = v * -2.0f + 1.0f;
                dir.z = u * -2.0f + 1.0f;
                break;

            case 1: //-X
                dir.x = -1;
                dir.y = v * -2.0f + 1.0f;
                dir.z = u * 2.0f - 1.0f;
                break;

            case 2: //+Y
                dir.x = u * 2.0f - 1.0f;
                dir.y = 1.0f;
                dir.z = v * 2.0f - 1.0f;
                break;

            case 3: //-Y
                dir.x = u * 2.0f - 1.0f;
                dir.y = -1.0f;
                dir.z = v * -2.0f + 1.0f;
                break;

            case 4: //+Z
                dir.x = u * 2.0f - 1.0f;
                dir.y = v * -2.0f + 1.0f;
                dir.z = 1;
                break;

            case 5: //-Z
                dir.x = u * -2.0f + 1.0f;
                dir.y = v * -2.0f + 1.0f;
                dir.z = -1;
                break;
        }

        return dir.normalized;
    }

    public static int FindFace(Vector3 dir)
    {
        int f = 0;
        float max = Mathf.Abs(dir.x);
        if (Mathf.Abs(dir.y) > max)
        {
            max = Mathf.Abs(dir.y);
            f = 2;
        }
        if (Mathf.Abs(dir.z) > max)
        {
            f = 4;
        }

        switch (f)
        {
            case 0:
                if (dir.x < 0)
                    f = 1;
                break;

            case 2:
                if (dir.y < 0)
                    f = 3;
                break;

            case 4:
                if (dir.z < 0)
                    f = 5;
                break;
        }

        return f;
    }

    public static int GetTexelIndexFromDirection(Vector3 dir, int cubemap_size)
    {
        float u = 0, v = 0;

        int f = FindFace(dir);
        
        switch (f)
        {
            case 0:
                dir.z /= dir.x;
                dir.y /= dir.x;
                u = (dir.z - 1.0f) * -0.5f;
                v = (dir.y - 1.0f) * -0.5f;
                break;

            case 1:
                dir.z /= -dir.x;
                dir.y /= -dir.x;
                u = (dir.z + 1.0f) * 0.5f;
                v = (dir.y - 1.0f) * -0.5f;
                break;

            case 2:
                dir.x /= dir.y;
                dir.z /= dir.y;
                u = (dir.x + 1.0f) * 0.5f;
                v = (dir.z + 1.0f) * 0.5f;
                break;

            case 3:
                dir.x /= -dir.y;
                dir.z /= -dir.y;
                u = (dir.x + 1.0f) * 0.5f;
                v = (dir.z - 1.0f) * -0.5f;
                break;

            case 4:
                dir.x /= dir.z;
                dir.y /= dir.z;
                u = (dir.x + 1.0f) * 0.5f;
                v = (dir.y - 1.0f) * -0.5f;
                break;

            case 5:
                dir.x /= -dir.z;
                dir.y /= -dir.z;
                u = (dir.x - 1.0f) * -0.5f;
                v = (dir.y - 1.0f) * -0.5f;
                break;
        }

        if (v == 1.0f) v = 0.999999f;
        if (u == 1.0f) u = 0.999999f;

        int index = (int)(v * cubemap_size) * cubemap_size + (int)(u * cubemap_size);

        return index;
    }

    public static bool CPU_Project_Uniform_9Coeff(Cubemap input, Vector4[] output)
    {
        if (output.Length != 9)
        {
            Debug.LogWarning("output size must be 9 for 9 coefficients");
            return false;
        }

        if (input.width != input.height)
        {
            Debug.LogWarning("input cubemap must be square");
            return false;
        }

        Color[] input_face;
        int size = input.width;

        //cycle on all 6 faces of the cubemap
        for (int face = 0; face < 6; ++face)
        {
            input_face = input.GetPixels((CubemapFace)face);

            //cycle all the texels
            for (int texel = 0; texel < size * size; ++texel)
            {
                float u = (texel % size) / (float)size;
                float v = ((int)(texel / size)) / (float)size;

                //get the direction vector
                Vector3 dir = DirectionFromCubemapTexel(face, u, v);
                Color radiance = input_face[texel];

                //compute the differential solid angle
                float d_omega = DifferentialSolidAngle(size, u, v);

                //cycle for 9 coefficients
                for (int c = 0; c < 9; ++c)
                {
                    //compute shperical harmonic
                    float sh = SphericalHarmonicsBasis.Eval[c](dir);

                    output[c].x += radiance.r * d_omega * sh;
                    output[c].y += radiance.g * d_omega * sh;
                    output[c].z += radiance.b * d_omega * sh;
                    output[c].w += radiance.a * d_omega * sh;
                }
            }
        }

        return true;
    }

    public static bool CPU_Project_MonteCarlo_9Coeff(Cubemap input, Vector4[] output, int sample_count)
    {
        if (output.Length != 9)
        {
            Debug.LogWarning("output size must be 9 for 9 coefficients");
            return false;
        }

        //cache the cubemap faces
        List<Color[]> faces = new List<Color[]>();
        
        for (int f = 0; f < 6; ++f)
        {
            faces.Add(input.GetPixels((CubemapFace)f, 0));
        }

        for (int c = 0; c < 9; ++c)
        {
            for (int s = 0; s < sample_count; ++s)
            {
                Vector3 dir = Random.onUnitSphere;
                int index = GetTexelIndexFromDirection(dir, input.height);
                int face = FindFace(dir);

                //read the radiance texel
                Color radiance = faces[face][index];

                //compute shperical harmonic
                float sh = SphericalHarmonicsBasis.Eval[c](dir);

                output[c].x += radiance.r * sh;
                output[c].y += radiance.g * sh;
                output[c].z += radiance.b * sh;
                output[c].w += radiance.a * sh;
            }

            output[c].x = output[c].x * 4.0f * Mathf.PI / (float)sample_count;
            output[c].y = output[c].y * 4.0f * Mathf.PI / (float)sample_count;
            output[c].z = output[c].z * 4.0f * Mathf.PI / (float)sample_count;
            output[c].w = output[c].w * 4.0f * Mathf.PI / (float)sample_count;
        }

        return true;
    }

    public static bool GPU_Project_Uniform_9Coeff(Cubemap input, Vector4[] output)
    {        
        //the starting number of groups 
        int ceiled_size = Mathf.CeilToInt(input.width / 8.0f);

        ComputeBuffer output_buffer = new ComputeBuffer(9, 16);  //the output is a buffer with 9 float4
        ComputeBuffer ping_buffer = new ComputeBuffer(ceiled_size * ceiled_size * 6, 16);
        ComputeBuffer pong_buffer = new ComputeBuffer(ceiled_size * ceiled_size * 6, 16);

        ComputeShader reduce = Resources.Load<ComputeShader>("Shaders/Reduce_Uniform");

        //can't have direct access to the cubemap in the compute shader (I think), so i copy the cubemap faces onto a texture2d array
        RenderTextureDescriptor desc = new RenderTextureDescriptor();
        desc.autoGenerateMips = false;
        desc.bindMS = false;
        desc.colorFormat = ConvertRenderFormat(input.format);
        desc.depthBufferBits = 0;
        desc.dimension = UnityEngine.Rendering.TextureDimension.Tex2DArray;
        desc.enableRandomWrite = false;
        desc.height = input.height;
        desc.width = input.width;
        desc.msaaSamples = 1;
        desc.sRGB = true;
        desc.useMipMap = false;
        desc.volumeDepth = 6;
        RenderTexture converted_input = new RenderTexture(desc);
        converted_input.Create();

        for (int face = 0; face < 6; ++face)
            Graphics.CopyTexture(input, face, 0, converted_input, face, 0);

        //cycle 9 coefficients
        for (int c = 0; c < 9; ++c)
        {
            ceiled_size = Mathf.CeilToInt(input.width / 8.0f);

            int kernel = reduce.FindKernel("sh_" + c.ToString());
            reduce.SetInt("coeff", c);

            //first pass, I compute the integral and make a first pass of reduction
            reduce.SetTexture(kernel, "input_data", converted_input);
            reduce.SetBuffer(kernel, "output_buffer", ping_buffer);
            reduce.SetBuffer(kernel, "coefficients", output_buffer);
            reduce.SetInt("ceiled_size", ceiled_size);
            reduce.SetInt("input_size", input.width);
            reduce.SetInt("row_size", ceiled_size);
            reduce.SetInt("face_size", ceiled_size * ceiled_size);
            reduce.Dispatch(kernel, ceiled_size, ceiled_size, 1);

            //second pass, complete reduction
            kernel = reduce.FindKernel("Reduce");

            int index = 0;
            ComputeBuffer[] buffers = { ping_buffer, pong_buffer };
            while(ceiled_size > 1)
            {
                reduce.SetInt("input_size", ceiled_size);
                ceiled_size = Mathf.CeilToInt(ceiled_size / 8.0f);
                reduce.SetInt("ceiled_size", ceiled_size);
                reduce.SetBuffer(kernel, "coefficients", output_buffer);
                reduce.SetBuffer(kernel, "input_buffer", buffers[index]);
                reduce.SetBuffer(kernel, "output_buffer", buffers[(index + 1) % 2]);
                reduce.Dispatch(kernel, ceiled_size, ceiled_size, 1);
                index = (index + 1) % 2;
            }
        }

        Vector4[] data = new Vector4[9];
        output_buffer.GetData(data);
        for (int c = 0; c < 9; ++c)
            output[c] = data[c];        

        pong_buffer.Release();
        ping_buffer.Release();
        output_buffer.Release();
        return true;
    }

    public static bool GPU_Project_MonteCarlo_9Coeff(Cubemap input, Vector4[] output)
    {
        if (output.Length != 9)
        {
            Debug.LogWarning("output size must be 9 for 9 coefficients");
            return false;
        }

        //there are 9 tiles for 9 coefficients, each tile is 32x32 = 1024 samples
        RenderTexture rt = new RenderTexture(96, 96, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

        //generating random samples on CPU
        //TODO: move the generation to the GPU
        Texture2D random_samples = new Texture2D(96, 96, TextureFormat.RGFloat, false, true);
        Color[] random_data = new Color[96 * 96];
        for (int i = 0; i < random_data.Length; ++i)
        {
            random_data[i] = new Color(Random.value, Random.value, 0, 0);
        }
        random_samples.SetPixels(random_data);
        random_samples.Apply();

        Material mat = new Material(Shader.Find("SH/MonteCarloProject"));
        mat.SetTexture("random_samples", random_samples);
        mat.SetTexture("input_cubemap", input);

        for (int i = 0; i < 9; ++i)
            Graphics.Blit(null, rt, mat, i);

        //use a compute shader to reduce the input, for 1024 samples i need 32x32 threads, grouped with 3x3 groups (9 tiles)
        ComputeBuffer coefficients_buffer = new ComputeBuffer(9, 16);
        ComputeShader reduce = Resources.Load<ComputeShader>("Shaders/Reduce_MC_1024");
        int kernel_index = reduce.FindKernel("Reduce_MC_1024");
        reduce.SetTexture(kernel_index, "input_data", rt);
        reduce.SetBuffer(kernel_index, "coefficients_buffer", coefficients_buffer);
        reduce.Dispatch(kernel_index, 3, 3, 1);

        //read back the data
        Vector4[] data = new Vector4[9];
        coefficients_buffer.GetData(data);
        for (int i = 0; i < 9; ++i)
        {
            output[i] = data[i];
        }

        coefficients_buffer.Release();

        return true;
    }
}
