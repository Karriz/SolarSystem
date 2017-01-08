using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit {

    private int century = 36525;

    private float L;
    private float L_rate;
    private float Rp;
    private float Ra;
    private float a;
    private float a_rate;
    private float e;
    private float e_rate;
    private float i;
    private float i_rate;
    private float long_pe;
    private float long_pe_rate;
    private float long_an;
    private float long_an_rate;

    private System.DateTime epoch;
    private double julianEpoch;

    public Orbit(float par_a, float par_a_rate, float par_e, float par_e_rate, float par_i, float par_i_rate, float par_long_an, float par_long_an_rate, float par_long_pe, float par_long_pe_rate, float par_L, float par_L_rate)
    {
        a = par_a;
        a_rate = par_a_rate;
        e = par_e;
        e_rate = par_e_rate;
        i = par_i;
        i_rate = par_i_rate;
        long_an = par_long_an;
        long_an_rate = par_long_an_rate;
        long_pe = par_long_pe;
        long_pe_rate = par_long_pe_rate;
        L = par_L;
        L_rate = par_L_rate;

        epoch = new System.DateTime(2000,1,1,12,0,0,0,System.DateTimeKind.Utc);
        julianEpoch = ToJulianDate(epoch);
    }

    public Vector3 CalculateCartesianCoordinates(System.DateTime date)
    {
        double JD = ToJulianDate(date);

        float centuries = (float)((JD - julianEpoch) / 36525.0);

        //Debug.Log(centuries);

        float current_a = a + (a_rate * centuries);

        float current_e = e + (e_rate * centuries);

        float current_i = i + (i_rate * centuries / 3600);

        float current_long_an = long_an + (long_an_rate * centuries / 3600);

        float current_long_pe = long_pe + (long_pe_rate * centuries / 3600);

        float current_L = L + (L_rate * centuries / 3600);

        float current_Rp = current_a * (1 - current_e);
        float current_Ra = current_a * (1 + current_e);

        float current_M = current_L - current_long_pe;

        float E = CalculateEccentricAnomaly(current_M, current_e);

        //float temp_true_anomaly = Mathf.Sqrt((1 + current_e) / (1 - current_e)) * Mathf.Tan(Mathf.Deg2Rad*(E / 2));
        //float true_anomaly = 2*Mathf.Atan(temp_true_anomaly);

        float true_anomaly = Mathf.Rad2Deg * 2 * Mathf.Atan2(Mathf.Sqrt(1-current_e)*Mathf.Cos(Mathf.Deg2Rad*E/2), Mathf.Sqrt(1 + current_e) * Mathf.Sin(Mathf.Deg2Rad * E / 2));
        true_anomaly = 180 - true_anomaly;
        if (true_anomaly < 0)
        {
            true_anomaly += 360;
        }
        else
        {
            true_anomaly -= 360;
        }

        float r = current_a * (1 - current_e * Mathf.Cos(Mathf.Deg2Rad * E));

        float x = r * (Mathf.Cos(Mathf.Deg2Rad * current_long_an) * Mathf.Cos(Mathf.Deg2Rad * (true_anomaly + current_long_pe - current_long_an)) - Mathf.Sin(Mathf.Deg2Rad * current_long_an) * Mathf.Sin(Mathf.Deg2Rad * (true_anomaly + current_long_pe - current_long_an)) * Mathf.Cos(Mathf.Deg2Rad * i));
        float y = r * (Mathf.Sin(Mathf.Deg2Rad * current_long_an) * Mathf.Cos(Mathf.Deg2Rad * (true_anomaly + current_long_pe - current_long_an)) + Mathf.Cos(Mathf.Deg2Rad * current_long_an) * Mathf.Sin(Mathf.Deg2Rad * (true_anomaly + current_long_pe - current_long_an)) * Mathf.Cos(Mathf.Deg2Rad * i));
        float z = r * (Mathf.Sin(Mathf.Deg2Rad * (true_anomaly + current_long_pe - current_long_an)) * Mathf.Sin(Mathf.Deg2Rad * i));

        /*Debug.Log(r);
        Debug.Log(current_M % 360);
        Debug.Log(E % 360);
        Debug.Log(true_anomaly);*/

        Vector3 coordinates = new Vector3(x, y, z);

        return coordinates;
    }


    public float CalculateEccentricAnomaly(float par_M, float par_e)
    {
        float current_M = Mathf.Deg2Rad * par_M;
        float current_e = par_e;

        float E1 = current_M + current_e * Mathf.Sin(current_M) * (1.0f + current_e * Mathf.Cos(current_M));
        float E0;
        do
        {
            E0 = E1;
            E1 = E0 - (E0 - current_e * Mathf.Sin(E0) - current_M) / (1f - current_e * Mathf.Cos(E0));
        } while (Mathf.Abs(E0 - E1) > 0.0001f);
        return Mathf.Rad2Deg * E1;
    }

    public double ToJulianDate(System.DateTime date)
    {
        double JD = 0f;

        double month = date.Month;
        double day = date.Day;
        double year = date.Year;

        double hour = date.Hour;
        double minute = date.Minute;
        double second = date.Second;
        double millisecond = date.Millisecond;

        if (month < 3)
        {
            month = month + 12.0;
            year = year - 1.0;
        }

        JD = day + (153.0 * month - 457.0) / 5.0 + 365.0 * year + (year / 4.0) - (year / 100.0) + (year / 400.0) + 1721119.0 + hour / 24.0 + minute / 1440.0 + second / 86400.0 + millisecond / 86400000.0;

        return JD;
    }

}
