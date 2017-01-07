using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Orbit earth = new Orbit(1.00000011, -0.00000005, 0.01671022, -0.00003804, 0.00005, -46.94, -11.26064, -18228.25, 102.94719, 1198.28, 100.46435, 129597740.63);
        earth.CalculateEclipticalCoordinates(System.DateTime.Now);

        Orbit pluto = new Orbit(39.48168677, -0.00076912, 0.24880766, 0.00006465, 17.14175, 11.07, 110.30347, -37.33, 224.06676, -132.25, 238.92881, 522747.90);
        pluto.CalculateEclipticalCoordinates(System.DateTime.Now);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
