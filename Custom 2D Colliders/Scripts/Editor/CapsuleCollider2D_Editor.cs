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

[CustomEditor (typeof(CapsuleCollider2D))]
public class CapsuleCollider_Editor : Editor {

    CapsuleCollider2D capCol;
    PolygonCollider2D polyCollider;
    Vector2 off;
    bool advanced;

    void OnEnable()
    {
        capCol = (CapsuleCollider2D)target;

        polyCollider = capCol.GetComponent<PolygonCollider2D>();
        if (polyCollider == null) {
            polyCollider = capCol.gameObject.AddComponent<PolygonCollider2D>();
        }

        polyCollider.points = capCol.getPoints();
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;
        DrawDefaultInspector();

        capCol.radius = Mathf.Clamp(capCol.radius, 0.5f, capCol.height / 2);
        capCol.radius = EditorGUILayout.Slider("Radius", capCol.radius, 0.25f, capCol.height / 2f);

        GUILayout.Space(8);
        capCol.bullet = EditorGUILayout.Toggle("Bullet", capCol.bullet);
        if(capCol.bullet) capCol.flip = EditorGUILayout.Toggle("Flip", capCol.flip);


        if (GUI.changed || !off.Equals(polyCollider.offset))
        {
            polyCollider.points = capCol.getPoints();
        }

        off = polyCollider.offset;
    }
    
}
