using UnityEngine;
using System.Collections.Generic;

// This is the PocketRPG weapon trail. For best results itterate it and your animation several times a frame.
// It is based on the Tron trails Unity example
// PocketRPG trails run faster than the framerate... (default is 300 frames a second)... that is how they are so smooth (30fps trails are rather jerky)
// This code was written by Evan Greenwood (of Free Lives) and used in the game PocketRPG by Tasty Poison Games.
// But you can use this how you please... Make great games... that's what this is about.
// This code is provided as is. Please discuss this on the Unity Forums if you need help.
//
class TronTrailSection
{
    public Vector3 point;
    public Vector3 upDir;
    public float time;
    public TronTrailSection()
    {

    }
    public TronTrailSection(Vector3 p, float t)
    {
        point = p;
        time = t;
    }
}

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[AddComponentMenu("PocketRPG/Weapon Trail")]
public class WeaponTrail : MonoBehaviour
{
    private WeaponTrail myTrail;

    private float t = 0.033f;
    private float tempT = 0;
    private float animationIncrement = 0.003f;

    void LateUpdate()
    {
        t = Mathf.Clamp(Time.deltaTime, 0, 0.066f);

        if (t > 0)
        {
            while (tempT < t)
            {
                tempT += animationIncrement;

                if (myTrail.time > 0)
                {
                    myTrail.Itterate(Time.time - t + tempT);
                }
                else
                {
                    myTrail.ClearTrail();
                }
            }

            tempT -= t;

            if (myTrail.time > 0)
            {
                myTrail.UpdateTrail(Time.time, t);
            }
        }
    }

    #region Public
    public float height = 1.4f;
    public float time = 0.15f;
    public bool alwaysUp = false;
    public float minDistance = 0f;
    public float timeTransitionSpeed = 1f;
    public float desiredTime = 0.15f;
    public Color startColor = Color.white;
    public Color endColor = new Color(1, 1, 1, 0);
    #endregion

    #region Temporary
    Vector3 position;
    float now = 0;
    TronTrailSection currentSection;
    Matrix4x4 localSpaceTransform;
    #endregion

    #region Internal
    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;
    private Vector2[] uv;
    #endregion

    #region Customisers 
    private MeshRenderer meshRenderer;
    public Material trailMaterial;
    #endregion

    private List<TronTrailSection> sections = new List<TronTrailSection>();

    void Awake()
    {
        this.myTrail = this.GetComponent<WeaponTrail>();
        MeshFilter meshF = GetComponent(typeof(MeshFilter)) as MeshFilter;
        mesh = meshF.mesh;
        meshRenderer = GetComponent(typeof(MeshRenderer)) as MeshRenderer;
        trailMaterial = meshRenderer.material;
        var trailStart = gameObject.transform.parent.Find("TrailStart");
        var trailEnd = gameObject.transform.parent.Find("TrailEnd");
        if (trailStart && trailEnd)
        {
            height = Vector3.Distance(trailStart.position, trailEnd.position);
            trailStart.LookAt(trailEnd);
            trailStart.Rotate(new Vector3(90, 0, 0));
        }
        else
        {
            enabled = false;
        }
    }
    public void StartTrail(float timeToTweenTo, float fadeInTime)
    {
        desiredTime = timeToTweenTo;
        if (time != desiredTime)
        {
            timeTransitionSpeed = Mathf.Abs(desiredTime - time) / fadeInTime;
        }
        if (time <= 0)
        {
            time = 0.01f;
        }
    }
    public void SetTime(float trailTime, float timeToTweenTo, float tweenSpeed)
    {
        time = trailTime;
        desiredTime = timeToTweenTo;
        timeTransitionSpeed = tweenSpeed;
        if (time <= 0)
        {
            ClearTrail();
        }
    }
    public void FadeOut(float fadeTime)
    {
        desiredTime = 0;
        if (time > 0)
        {
            timeTransitionSpeed = time / fadeTime;
        }
    }
    public void SetTrailColor(Color color)
    {
        trailMaterial.SetColor("_TintColor", color);
    }
    public void Itterate(float itterateTime)
    { // ** call everytime you sample animation **

        position = transform.position;
        now = itterateTime;

        // Add a new trail section
        if (sections.Count == 0 || (sections[0].point - position).sqrMagnitude > minDistance * minDistance)
        {
            TronTrailSection section = new TronTrailSection();
            section.point = position;
            if (alwaysUp)
                section.upDir = Vector3.up;
            else
                section.upDir = transform.TransformDirection(Vector3.up);

            section.time = now;
            sections.Insert(0, section);

        }
    }
    public void UpdateTrail(float currentTime, float deltaTime)
    { // ** call once a frame **

        // Rebuild the mesh	
        mesh.Clear();
        //
        // Remove old sections
        while (sections.Count > 0 && currentTime > sections[sections.Count - 1].time + time)
        {
            sections.RemoveAt(sections.Count - 1);
        }
        // We need at least 2 sections to create the line
        if (sections.Count < 2)
            return;
        //
        vertices = new Vector3[sections.Count * 2];
        colors = new Color[sections.Count * 2];
        uv = new Vector2[sections.Count * 2];
        //
        currentSection = sections[0];
        //
        // Use matrix instead of transform.TransformPoint for performance reasons
        localSpaceTransform = transform.worldToLocalMatrix;

        // Generate vertex, uv and colors
        for (var i = 0; i < sections.Count; i++)
        {
            //
            currentSection = sections[i];
            // Calculate u for texture uv and color interpolation
            float u = 0.0f;
            if (i != 0)
                u = Mathf.Clamp01((currentTime - currentSection.time) / time);
            //
            // Calculate upwards direction
            Vector3 upDir = currentSection.upDir;

            // Generate vertices
            vertices[i * 2 + 0] = localSpaceTransform.MultiplyPoint(currentSection.point);
            vertices[i * 2 + 1] = localSpaceTransform.MultiplyPoint(currentSection.point + upDir * height);

            uv[i * 2 + 0] = new Vector2(u, 0);
            uv[i * 2 + 1] = new Vector2(u, 1);

            // fade colors out over time
            Color interpolatedColor = Color.Lerp(startColor, endColor, u);
            colors[i * 2 + 0] = interpolatedColor;
            colors[i * 2 + 1] = interpolatedColor;
        }

        // Generate triangles indices
        int[] triangles = new int[(sections.Count - 1) * 2 * 3];
        for (int i = 0; i < triangles.Length / 6; i++)
        {
            triangles[i * 6 + 0] = i * 2;
            triangles[i * 6 + 1] = i * 2 + 1;
            triangles[i * 6 + 2] = i * 2 + 2;

            triangles[i * 6 + 3] = i * 2 + 2;
            triangles[i * 6 + 4] = i * 2 + 1;
            triangles[i * 6 + 5] = i * 2 + 3;
        }

        // Assign to mesh	
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.uv = uv;
        mesh.triangles = triangles;
        //
        // Tween to the desired time
        //
        if (time > desiredTime)
        {
            time -= deltaTime * timeTransitionSpeed;
            if (time <= desiredTime) time = desiredTime;
        }
        else if (time < desiredTime)
        {
            time += deltaTime * timeTransitionSpeed;
            if (time >= desiredTime) time = desiredTime;
        }
    }
    public void ClearTrail()
    {
        desiredTime = 0;
        time = 0;
        if (mesh != null)
        {
            mesh.Clear();
            sections.Clear();
        }
    }
}