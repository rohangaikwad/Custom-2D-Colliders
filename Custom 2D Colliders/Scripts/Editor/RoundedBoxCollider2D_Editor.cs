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

        edgeCollider.points = rb.getPoints(edgeCollider.offset);
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;
        DrawDefaultInspector();


        float lesser = (rb.width > rb.height) ? rb.height : rb.width;

        rb.radius = EditorGUILayout.Slider("Radius",rb.radius, 0f, lesser/2f);
        rb.radius = Mathf.Clamp(rb.radius, 0f, lesser / 2);

        if (GUI.changed || !off.Equals(edgeCollider.offset))
        {
            edgeCollider.points = rb.getPoints(edgeCollider.offset);
        }

        off = edgeCollider.offset;
    }
    
}