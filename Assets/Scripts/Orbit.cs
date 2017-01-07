using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EclipticCoordinates
{
    private double longitude;
    private double latitude;
    private double distance;

    public EclipticCoordinates(double lon, double lat, double r)
    {
        longitude = lon;
        latitude = lat;
        distance = r;
    }
}

public class Orbit {

    private int century = 36525;

    private double L;
    private double L_rate;
    private double Rp;
    private double Ra;
    private double a;
    private double a_rate;
    private double e;
    private double e_rate;
    private double i;
    private double i_rate;
    private double long_pe;
    private double long_pe_rate;
    private double long_an;
    private double long_an_rate;

    private System.DateTime epoch;
    private double julianEpoch;

    public Orbit(double par_a, double par_a_rate, double par_e, double par_e_rate, double par_i, double par_i_rate, double par_long_an, double par_long_an_rate, double par_long_pe, double par_long_pe_rate, double par_L, double par_L_rate)
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

    public EclipticCoordinates CalculateEclipticalCoordinates(System.DateTime date)
    {
        double JD = ToJulianDate(date);

        double centuries = (JD - julianEpoch) / 36525;
        Debug.Log(centuries);

        double current_a = a + (a_rate * centuries);

        double current_e = e + (e_rate * centuries);

        double current_i = i + (i_rate * centuries / 3600);

        double current_long_an = long_an + (long_an_rate * centuries / 3600);

        double current_long_pe = long_pe + (long_pe_rate * centuries / 3600);

        double current_L = L + (L_rate * centuries / 3600);

        double current_Rp = current_a * (1 - current_e);
        double current_Ra = current_a * (1 + current_e);

        double current_M = current_L - current_long_pe;

        
        float E = CalculateEccentricAnomaly(current_M, current_e);

        float tanv2 = Mathf.Sqrt((1 + (float)current_e) / (1 - (float)current_e)) * Mathf.Tan(E / 2);

        float true_anomaly = 2*Mathf.Atan(tanv2);

        double r = current_a * (1 - current_e * Mathf.Cos(Mathf.Deg2Rad*E));

        Debug.Log(r);

        EclipticCoordinates coordinates = new EclipticCoordinates(0,0,0);

        return coordinates;
    }

    public float CalculateEccentricAnomaly(double par_M, double par_e)
    {
        float current_M = Mathf.Deg2Rad * (float)par_M;
        float current_e = (float)par_e;

        float E1 = current_M + current_e * Mathf.Sin(current_M) * (1.0f + current_e * Mathf.Cos(current_M));
        float E0;
        do
        {
            E0 = E1;
            E1 = E0 - (E0 - current_e * Mathf.Sin(E0) - current_M) / (1f - current_e * Mathf.Cos(E0));
        } while (Mathf.Abs(E0 - E1) > 0.0001f);
        return Mathf.Rad2Deg*E1;
    }

    public Vector3 CalculateCartesianCoordinates(System.DateTime date)
    {
        return new Vector3();
    }

    public double ToJulianDate(System.DateTime date)
    {
        double JD = 0;

        int month = date.Month;
        int day = date.Day;
        int year = date.Year;

        if (month < 3)
        {
            month = month + 12;
            year = year - 1;
        }
        JD = day + (153 * month - 457) / 5 + 365 * year + (year / 4) - (year / 100) + (year / 400) + 1721119;

        return JD;
    }

}
