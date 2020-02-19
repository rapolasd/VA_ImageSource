﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Debug Image Source Method tracer
/// </summary>
public class ISMDebug : MonoBehaviour
{
    /// <summary>
    /// Simulation settings
    /// </summary>
    public ISMRenderSettings renderSettings;

    /// <summary>
    /// Debug line script for drawing the impulse response
    /// </summary>
    public ISMDebugLine debugLine;

    /// <summary>
    /// Test the effect of air absorption to the signal
    /// </summary>
    public bool testAirAbsorption = false;

    /// <summary>
    /// A list of sources that can be debugged
    /// </summary>
    public ISMReverb[] sources;

    /// <summary>
    /// An index to the list of sources that is being debugged
    /// </summary>
    public int sourceToDebug = -1;

    /// <summary>
    /// Show image source debug objects
    /// </summary>
    public bool showImageSources = true;

    /// <summary>
    /// Show traced image source rays
    /// </summary>
    public bool showISMRays = true;

    /// <summary>
    /// Show the impulse response that is generated by the selected source
    /// </summary>
    public bool showImpulseResponse = true;

    /// <summary>
    /// A list of GameObjects representing image sources
    /// </summary>
    List<GameObject> debugImageSources = new List<GameObject>();


    /// <summary>
    /// The ISMReverberation script that is currently being debugged
    /// </summary>
    ISMReverb SourceToDebug
    {
        get { return sources[sourceToDebug]; }
    }


    // Called before any Start functions
    private void Awake()
    {
        sources = FindObjectsOfType<ISMReverb>();
        System.Array.Sort(
            sources, 
            delegate(ISMReverb lhs, ISMReverb rhs)
            {
                return lhs.name.CompareTo(rhs.name);
            });
    }


    // Called after all Update calls have been finished
    private void LateUpdate()
    {
        DebugMirroringPlanes();
        if (IsValidSourceIndex())
        {
            DebugImageSourcePositions();
            DebugISMRays();
        }
        if (IsValidSourceIndex() || testAirAbsorption)
        {
            DebugImpulseResponse();
        }
    }


    // Called when the object is disabled
    void OnDisable()
    {
        foreach (GameObject dbg_obj in debugImageSources)
        {
            Destroy(dbg_obj);
        }
        debugImageSources.Clear();
    }


    void DebugMirroringPlanes()
    {
        Vector3[] pos = renderSettings.PlaneCenters;
        Vector3[] n = renderSettings.PlaneNormals;
        var normalLength = 0.2f;
        for (var i = 0; i < n.Length; ++i)
        {
            Debug.DrawLine(pos[i], pos[i] + normalLength * n[i], Color.red);
            Vector3 perpN = Quaternion.Euler(90, 90, 0) * n[i];
            Debug.DrawLine(pos[i] - normalLength * perpN, 
                           pos[i] + normalLength * perpN, 
                           Color.blue);
        }
    }

    /// <summary>
    /// Manage and update image source positions
    /// </summary>
    void DebugImageSourcePositions()
    {
        if (showImageSources)
        {
            if (debugImageSources.Count != SourceToDebug.imageSources.Count)
            {
                // Allocate / free resources
                float dbg_obj_scale = 0.2f;
                while (debugImageSources.Count 
                       < SourceToDebug.imageSources.Count)
                {
                    GameObject obj = 
                        GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    obj.transform.localScale = new Vector3(dbg_obj_scale, 
                                                           dbg_obj_scale, 
                                                           dbg_obj_scale);
                    // Get the Renderer component
                    var objRenderer = obj.GetComponent<Renderer>();
                    // Make it white
                    objRenderer.material = new Material(Shader.Find("Unlit/Color"));
                    objRenderer.material.color = Color.white;

                    debugImageSources.Add(obj);
                }
                if (debugImageSources.Count > SourceToDebug.imageSources.Count)
                {
                    for (var i = debugImageSources.Count - 1; 
                        i > SourceToDebug.imageSources.Count - 1; 
                        --i)
                    {
                        Destroy(debugImageSources[i]);
                        debugImageSources.RemoveAt(i);
                    }
                }
            }
            // Assign new locations
            for (var i = 0; i < SourceToDebug.imageSources.Count; ++i)
            {
                debugImageSources[i].transform.position = 
                    SourceToDebug.imageSources[i].pos;
            }
        }
    }


    /// <summary>
    /// Draw debug rays for image source method ray paths
    /// </summary>
    void DebugISMRays()
    {
        if (showISMRays)
        {
            Vector3 p_source = SourceToDebug.SourcePosition;
            Vector3 p_listener = renderSettings.ListenerPosition;
            foreach (ISMReverb.RaycastHitPath path in SourceToDebug.hitPaths)
            {
                // Draw the first path
                if (path.points.Count == 0)
                {
                    Debug.DrawLine(p_listener, p_source);
                    continue;
                }
                Debug.DrawLine(p_listener, path.points[0]);
                for (var i_hit = 0; i_hit < path.points.Count - 1; ++i_hit)
                {
                    // Draw intermediate paths
                    Debug.DrawLine(path.points[i_hit], path.points[i_hit + 1]);
                }
                // Draw last path
                Debug.DrawLine(path.points[path.points.Count - 1], p_source);
            }
        }
    }


    /// <summary>
    /// Update the impulse response debug object
    /// </summary>
    void DebugImpulseResponse()
    {
        float[] IR = showImpulseResponse ? SourceToDebug.IR : null;
        if (IR != null && debugLine != null)
        {
            debugLine.DrawIR(IR);
        }
    }


    /// <summary>
    /// Check the validity of the source index
    /// </summary>
    /// <returns>
    /// True if the sourceToDebug index is within range, false otherwise.
    /// </returns>
    bool IsValidSourceIndex()
    {
        return 0 <= sourceToDebug && sourceToDebug < sources.Length;
    }

}