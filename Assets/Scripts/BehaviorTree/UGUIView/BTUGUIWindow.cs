using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bt.Ugui
{
    public class BTUGUIWindow : MonoBehaviour
    {
        private static Bt.Ugui.ConnectPoint selectedInPoint;
        private static Bt.Ugui.ConnectPoint selectedOutPoint;

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Pressed left click.");
                OnClickNode();
            }

            if (Input.GetMouseButtonDown(1))
                Debug.Log("Pressed right click.");

            if (Input.GetMouseButtonDown(2))
                Debug.Log("Pressed middle click.");
        }

        private void OnDraw()
        {
            if(selectedInPoint != null && selectedOutPoint == null)
            {
                //BTUtils.DrawBezier(
                //    selectedInPoint.transform.position,
                //    e.mousePosition,
                //    selectedInPoint.transform.position + Vector2.left * 50f,
                //    e.mousePosition - Vector2.left * 50f
                //);
            }
        }

        private void OnClickNode()
        {

        }

        private void OnClickSlot()
        {

        }
    }
}

