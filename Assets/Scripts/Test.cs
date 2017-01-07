using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Orbit earth = new Orbit(1.00000011f, -0.00000005f, 0.01671022f, -0.00003804f, 0.00005f, -46.94f, -11.26064f, -18228.25f, 102.94719f, 1198.28f, 100.46435f, 129597740.63f);
        earth.CalculateEclipticalCoordinates(System.DateTime.Now);

        Orbit pluto = new Orbit(39.48168677f, -0.00076912f, 0.24880766f, 0.00006465f, 17.14175f, 11.07f, 110.30347f, -37.33f, 224.06676f, -132.25f, 238.92881f, 522747.90f);
        pluto.CalculateEclipticalCoordinates(System.DateTime.Now);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
