using UnityEngine;
using System.Collections;

public class FPSStorage : MonoBehaviour {
	
	private float fps = 15.0f;
	
	public float GetCurrentFPS() {
		return fps;
	}
	
	public void FPSChanged(float fps) {
		this.fps = fps;
	}
	
}
