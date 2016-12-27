/*
The MIT License (MIT)

Copyright (c) 2016 GuyQuad

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

You can contact me by email at guyquad27@gmail.com or on Reddit at https://www.reddit.com/user/GuyQuad
*/


using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(EllipseCollider2D))]
public class EllipseCollider_Editor : Editor {

    EllipseCollider2D ec;
    PolygonCollider2D polyCollider;
    Vector2 off;

    void OnEnable()
    {
        ec = (EllipseCollider2D)target;

        polyCollider = ec.GetComponent<PolygonCollider2D>();
        if (polyCollider == null) {
            polyCollider = ec.gameObject.AddComponent<PolygonCollider2D>();
        }
        polyCollider.points = ec.getPoints();
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;
        DrawDefaultInspector();

        ec.advanced = EditorGUILayout.Toggle("Advanced", ec.advanced);
        if (ec.advanced)
        {
            ec.radiusX = EditorGUILayout.FloatField("RadiusX", ec.radiusX);
            ec.radiusY = EditorGUILayout.FloatField("RadiusY", ec.radiusY);
        }
        else
        {
            ec.radiusX = EditorGUILayout.Slider("RadiusX", ec.radiusX, 1, 25);
            ec.radiusY = EditorGUILayout.Slider("RadiusY", ec.radiusY, 1, 25);
        }

        if (GUI.changed || !off.Equals(polyCollider.offset))
        {
            polyCollider.points = ec.getPoints();
        }

        off = polyCollider.offset;
    }
    
}
