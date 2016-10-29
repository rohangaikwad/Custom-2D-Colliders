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
using System.Collections;

namespace UnityEditor
{

    [CustomEditor(typeof(RoundedBoxCollider2D))]
    public class RoundedBoxCollider_Editor : Editor
    {

        RoundedBoxCollider2D rb;
        EdgeCollider2D edgeCollider;
        Vector2 off;

        void OnEnable()
        {
            rb = (RoundedBoxCollider2D)target;

            edgeCollider = rb.GetComponent<EdgeCollider2D>();
            if (edgeCollider == null)
            {
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
            rb.radius = EditorGUILayout.Slider("Radius", rb.radius, 0f, lesser);
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
}