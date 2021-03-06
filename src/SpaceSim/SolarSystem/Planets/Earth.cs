﻿using System;
using System.Drawing;
using SpaceSim.Kernels;
using SpaceSim.Orbits;

using SpaceSim.Properties;

namespace SpaceSim.SolarSystem.Planets
{
    class Earth : MassiveBodyBase
    {
        public override string ApoapsisName { get { return "Apogee"; } }
        public override string PeriapsisName { get { return "Perigee"; } }

        public override double Mass
        {
            get { return 5.97219e24; }
        }

        public override double SurfaceRadius
        {
            get { return BaseKernel.EARTH_RADIUS; }
        }

        public override double AtmosphereHeight
        {
            get { return BaseKernel.EARTH_ATMOSPHERE; }
        }

        public override double RotationRate
        {
            get {
                if(Settings.Default.PolarOrbit)
                    return -7.2722052166e-7;
                else
                    return -7.2722052166e-5;
            }
        }

        public override double RotationPeriod
        {
            get {
                if (Settings.Default.PolarOrbit)
                    return 8640000;
                else
                    return 86400;
            }
        }

        public override Color IconColor { get { return Color.Green; } }
        public override Color IconAtmopshereColor { get { return Color.LightSkyBlue; } }

        public Earth()
            : base(OrbitHelper.FromJplEphemeris(1.470669705624798E+08, -3.465263667484938E+07),
                   OrbitHelper.FromJplEphemeris(6.445708901348731E+00, 2.887242866160539E+01), new EarthKernel())
        {
        }

        // Realistic density model based off https://www.grc.nasa.gov/www/k-12/rocket/atmos.html
        public override double GetAtmosphericDensity(double altitude)
        {
            if (altitude > AtmosphereHeight) return 0;

            double temperature;
            double pressure;

            if (altitude > 25098.756)
            {
                temperature = -205.05 + 0.0053805776 * altitude;

                pressure = 51.97 * Math.Pow((temperature + 459.7) / 389.98, -11.388);
            }
            else if (altitude > 11019.13)
            {
                temperature = -70;

                pressure = 473.1 * Math.Exp(1.73 - 0.00015748032 * altitude);
            }
            else
            {
                temperature = 59 - 0.0116797904 * altitude;

                pressure = 2116 * Math.Pow((temperature + 459.7) / 518.6, 5.256);   
            }

            double density = pressure / (1718 * (temperature + 459.7));

            return density * 515.379;
        }

        // Quickly approximated using temperature from here https://www.grc.nasa.gov/www/k-12/rocket/atmos.html
        // and a calculator here http://www.mhtl.uwaterloo.ca/old/onlinetools/airprop/airprop.html
        public override double GetAtmosphericViscosity(double altitude)
        {
            if (altitude > AtmosphereHeight) return 0;

            if (altitude > 10668) return 0.0000089213;

            return -5.37e-10 * altitude + 1.458e-5;
        }

        public override string ToString()
        {
            return "Earth";
        }
    }
}
