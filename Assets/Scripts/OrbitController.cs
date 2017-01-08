using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitController : MonoBehaviour {

    public float par_a;
    public float par_a_rate;
    public float par_e;
    public float par_e_rate;
    public float par_i;
    public float par_i_rate;
    public float par_long_an;
    public float par_long_an_rate;
    public float par_long_pe;
    public float par_long_pe_rate;
    public float par_L;
    public float par_L_rate;

    public Color color;

    private System.DateTime date;
    private Orbit orbit;

    private UnityEngine.UI.Text dateText;

    // Use this for initialization
    void Start () {
        date = System.DateTime.Now;
        orbit = new Orbit(par_a, par_a_rate, par_e, par_e_rate, par_i, par_i_rate, par_long_an, par_long_an_rate, par_long_pe, par_long_pe_rate, par_L, par_L_rate);
        transform.position = orbit.CalculateCartesianCoordinates(date);

        MeshRenderer renderer = transform.GetComponent<MeshRenderer>();
        renderer.material = Instantiate(renderer.material) as Material;
        renderer.material.color = color;

        dateText = GameObject.Find("DateText").GetComponent<UnityEngine.UI.Text>();
    }
	
	// Update is called once per frame
	void Update () {
        date = date.AddDays(Time.deltaTime*40);
        transform.position = orbit.CalculateCartesianCoordinates(date);

        dateText.text = date.ToString();
    }
}
