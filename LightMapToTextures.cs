using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMapToTextures : MonoBehaviour
{
    public GameObject parent;
    public Shader lightmapUV1Shader; // Drag your custom shader here

    void Start()
    {
        foreach (Transform child in parent.transform)
        {
            GameObject childObject = child.gameObject;

            // Check if the object is not in the "Mirror" layer
            if (LayerMask.LayerToName(childObject.layer) != "Mirror")
            {
                MeshRenderer renderer = childObject.GetComponent<MeshRenderer>();

                if (LayerMask.LayerToName(childObject.layer) != "Light")
                {
                    

                    if (renderer != null)
                    {
                        int lightmapIndex = renderer.lightmapIndex;

                        // Skip if no lightmap is assigned
                        if (lightmapIndex == -1)
                        {
                            Debug.LogWarning("No lightmap assigned to " + childObject.name);
                            continue;
                        }

                        // Load the lightmap texture dynamically based on the index
                        string lightmapPath = $"Lightmaps/L{lightmapIndex}";
                        Texture2D tex = Resources.Load<Texture2D>(lightmapPath);

                        if (tex == null)
                        {
                            Debug.LogError($"Failed to load lightmap texture at path: {lightmapPath}");
                            continue;
                        }

                        Vector4 lightmapScaleOffset = renderer.lightmapScaleOffset;

                        // Create a new material instance using the custom shader
                        Material newMaterial = new Material(lightmapUV1Shader);

                        // Apply the loaded lightmap as the main texture
                        newMaterial.SetTexture("_MainTex", tex);

                        // Pass the lightmap scale and offset to the shader
                        newMaterial.SetVector("_ScaleOffset", lightmapScaleOffset);

                        // Assign the new material to the renderer
                        renderer.material = newMaterial;
                        renderer.lightmapIndex = -1;
                    }
                    else
                    {
                        Debug.LogError("MeshRenderer component not found on " + childObject.name);
                    }
                }
                else
                {
                    renderer.lightmapIndex = -1;
                }
            }
        }
    }
}
