///  Custom 2D Colliders
///  Description: A set of useful custom 2d colliders creation scripts to use in your game.
///  Copyright(C) 2016 GuyQuad

///  This program is free software: you can redistribute it and/or modify
///  it under the terms of the GNU General Public License as published by
///  the Free Software Foundation, either version 3 of the License, or
///  any later version.

///  This program is distributed in the hope that it will be useful,
///  but WITHOUT ANY WARRANTY; without even the implied warranty of
///  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
///  GNU General Public License for more details.

///  You should have received a copy of the GNU General Public License
///  along with this program.If not, see<http://www.gnu.org/licenses/>.
///  You can contact me by email at guyquad27@gmail.com or on Reddit at https://www.reddit.com/user/GuyQuad


using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(RoundedBoxCollider2D))]
public class RoundedBoxCollider_Editor : Editor {

    RoundedBoxCollider2D rb;
    EdgeCollider2D edgeCollider;
    Vector2 off;

    void OnEnable()
    {
        rb = (RoundedBoxCollider2D)target;

        edgeCollider = rb.GetComponent<EdgeCollider2D>();
        if (edgeCollider == null) {
            rb.gameObject.AddComponent<EdgeCollider2D>();
            edgeCollider = rb.GetComponent<EdgeCollider2D>();
        }

        Vector2[] pts = rb.getPoints(edgeCollider.offset);
        if (pts != null) edgeCollider.points = pts;
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;
        DrawDefaultInspector();       


        // automatically adjust the radius according to width and height
        float lesser = (rb.width > rb.height) ? rb.height : rb.width;
        lesser /= 2f;
        lesser = Mathf.Round(lesser * 100f) / 100f; 
        rb.radius = EditorGUILayout.Slider("Radius",rb.radius, 0f, lesser);
        rb.radius = Mathf.Clamp(rb.radius, 0f, lesser);

        if (GUILayout.Button("Reset"))
        {
            rb.smoothness = 15;
            rb.width = 2;
            rb.height = 2;
            rb.trapezoid = 0.5f;
            rb.radius = 0.5f;
            edgeCollider.offset = Vector2.zero;
        }

        if (GUI.changed || !off.Equals(edgeCollider.offset))
        {
            Vector2[] pts = rb.getPoints(edgeCollider.offset);
            if (pts != null) edgeCollider.points = pts;
        }

        off = edgeCollider.offset;
    }
    
}