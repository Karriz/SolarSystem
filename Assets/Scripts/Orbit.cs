using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EclipticCoordinates
{
    public float longitude;
    public float latitude;
    public float distance;

    public EclipticCoordinates(float lat, float lon, float r)
    {
        latitude = lat;
        longitude = lon;
        distance = r;
    }
}

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
    private float julianEpoch;

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

    public EclipticCoordinates CalculateEclipticalCoordinates(System.DateTime date)
    {
        float JD = ToJulianDate(date);

        float centuries = (JD - julianEpoch) / 36525;

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

        float temp_true_anomaly = Mathf.Sqrt((1 + (float)current_e) / (1 - (float)current_e)) * Mathf.Tan(Mathf.Deg2Rad*E / 2);
        float true_anomaly = 2*Mathf.Atan(temp_true_anomaly);

        float u = current_L + true_anomaly - current_M - current_long_an;

        float temp_longitude = Mathf.Cos(Mathf.Deg2Rad * current_i) * Mathf.Tan(Mathf.Deg2Rad*u);
        float longitude = Mathf.Atan(temp_longitude) + current_long_an;

        float temp_latitude = Mathf.Sin(Mathf.Deg2Rad*u) * Mathf.Sin(Mathf.Deg2Rad*current_i);
        float latitude = Mathf.Asin(temp_latitude);

        float r = current_a * (1 - current_e * Mathf.Cos(Mathf.Deg2Rad*E));

        Debug.Log(r);
        Debug.Log(longitude);

        EclipticCoordinates coordinates = new EclipticCoordinates(latitude, longitude, r);

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
        return Mathf.Rad2Deg*E1;
    }

    public Vector3 CalculateCartesianCoordinates(System.DateTime date)
    {
        EclipticCoordinates eclipticCoordinates = CalculateEclipticalCoordinates(date);
        float x = eclipticCoordinates.distance * Mathf.Cos(Mathf.Deg2Rad * eclipticCoordinates.latitude) * Mathf.Cos(Mathf.Deg2Rad * eclipticCoordinates.longitude);
        float y = eclipticCoordinates.distance * Mathf.Cos(Mathf.Deg2Rad * eclipticCoordinates.latitude) * Mathf.Sin(Mathf.Deg2Rad * eclipticCoordinates.longitude);
        float z = eclipticCoordinates.distance * Mathf.Sin(Mathf.Deg2Rad * eclipticCoordinates.latitude);
        return new Vector3(x, y, z);
    }

    public float ToJulianDate(System.DateTime date)
    {
        float JD = 0;

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
