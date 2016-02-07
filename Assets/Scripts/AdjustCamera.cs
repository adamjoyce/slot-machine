﻿using UnityEngine;
using System.Collections;

public class AdjustCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
    // Set the aspect ratio.
    float targetAspect = 16.0f / 9.0f;
    // Determine the window size.
    float windowAspect = (float)Screen.width / (float)Screen.height;
    // Value the current viewport should be scaled by.
    float scaleHeight = windowAspect / targetAspect;
    // Camera.
    Camera camera = GetComponent<Camera>();

    if (scaleHeight < 1.0f) {
      // Add letterbox.
      Rect rect = camera.rect;
      rect.width = 1.0f;
      rect.height = scaleHeight;
      rect.x = 0.0f;
      rect.y = (1.0f - scaleHeight) * 0.5f;
      camera.rect = rect;
    } else {
      // Add pillarbox.
      float scaleWidth = 1.0f / scaleHeight;
      Rect rect = camera.rect;
      rect.width = scaleWidth;
      rect.height = 1.0f;
      rect.x = (1.0f - scaleWidth) / 2.0f;
      rect.y = 0.0f;
      camera.rect = rect;
    }
	}
}
