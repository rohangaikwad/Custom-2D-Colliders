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

[CustomEditor (typeof(ArcCollider2D))]
public class ArcCollider_Editor : Editor {
    
    ArcCollider2D ac;
    PolygonCollider2D polyCollider;
    Vector2 off;

    void OnEnable()
    {
        ac = (ArcCollider2D)target;

        polyCollider = ac.GetComponent<PolygonCollider2D>();
        if (polyCollider == null) {
            ac.gameObject.AddComponent<PolygonCollider2D>();
            polyCollider = ac.GetComponent<PolygonCollider2D>();
        }
        polyCollider.points = ac.getPoints(polyCollider.offset);
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;
        DrawDefaultInspector();

        ac.advanced = EditorGUILayout.Toggle("Advanced", ac.advanced);
        if (ac.advanced)
        {
            ac.radius = EditorGUILayout.FloatField("Radius", ac.radius);
        }
        else
        {
            ac.radius = EditorGUILayout.Slider("Radius", ac.radius, 1, 25);
        }
        if (!ac.pizzaSlice)
        {
            if (ac.advanced)
            {
                ac.Thickness = EditorGUILayout.FloatField("Thickness", ac.Thickness);
            }
            else
            {
                ac.Thickness = EditorGUILayout.Slider("Thickness", ac.Thickness, 1, 25);
            }
        }

        if (GUI.changed || !off.Equals(polyCollider.offset))
        {
            polyCollider.points = ac.getPoints(polyCollider.offset);
        }
        off = polyCollider.offset;
    }
    
}