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
    [CustomEditor(typeof(ArcCollider2D))]
    public class ArcCollider_Editor : Editor
    {

        ArcCollider2D ac;
        EdgeCollider2D edgeCollider;
        Vector2 off;

        void OnEnable()
        {
            ac = (ArcCollider2D)target;

            edgeCollider = ac.GetComponent<EdgeCollider2D>();
            if (edgeCollider == null)
            {
                ac.gameObject.AddComponent<EdgeCollider2D>();
                edgeCollider = ac.GetComponent<EdgeCollider2D>();
            }
            edgeCollider.points = ac.getPoints(edgeCollider.offset);
        }

        public override void OnInspectorGUI()
        {
            GUI.changed = false;
            DrawDefaultInspector();

            if (GUI.changed || !off.Equals(edgeCollider.offset))
            {
                edgeCollider.points = ac.getPoints(edgeCollider.offset);
            }
            off = edgeCollider.offset;
        }

    }
}