using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_SolarSystemEducationApp.Models
{

    public class Bodies
    {
        public List<Body> bodies { get; set; }
    }

    public class Body
    {
        public string id { get; set; }
        public string name { get; set; }
        public string englishName { get; set; }
        public bool isPlanet { get; set; }
        public Moon[] moons { get; set; }
        public float semimajorAxis { get; set; }
        public long perihelion { get; set; }
        public long aphelion { get; set; }
        public float eccentricity { get; set; }
        public float inclination { get; set; }
        public Mass mass { get; set; }
        public Vol vol { get; set; }
        public float density { get; set; }
        public float gravity { get; set; }
        public float escape { get; set; }
        public float meanRadius { get; set; }
        public float equaRadius { get; set; }
        public float polarRadius { get; set; }
        public float flattening { get; set; }
        public string dimension { get; set; }
        public float sideralOrbit { get; set; }
        public float sideralRotation { get; set; }
        public Aroundplanet aroundPlanet { get; set; }
        public string discoveredBy { get; set; }
        public string discoveryDate { get; set; }
        public string alternativeName { get; set; }
        public string rel { get; set; }
    }

    public class Mass
    {
        public float massValue { get; set; }
        public int massExponent { get; set; }
    }

    public class Vol
    {
        public float volValue { get; set; }
        public int volExponent { get; set; }
    }

    public class Aroundplanet
    {
        public string planet { get; set; }
        public string rel { get; set; }
    }

    public class Moon
    {
        public string moon { get; set; }
        public string rel { get; set; }
    }

}
