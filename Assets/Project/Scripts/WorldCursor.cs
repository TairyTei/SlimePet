using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCursor : MonoBehaviour
{
   public static WorldCursor instance;
   private Camera cam;
   void Start()
   {
        instance = this;
        cam = Camera.main;
   }
   void Update()
   {
        transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
   }
}
