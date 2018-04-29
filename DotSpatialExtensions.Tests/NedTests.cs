using System;
using DotSpatial.Positioning;
using NUnit.Framework;

namespace DotSpatialExtensions.Tests
{
    [TestFixture]
    public class NedConversions
    {
        [TestCase(1, -1000)]
        [TestCase(2, 60)]
        [TestCase(3, -600)]
        [TestCase(4, -2000)]
        [TestCase(5, 100)]
        public void DownTranslation(int seed, double downInMeters)
        {
            Random randomizer = new Random(seed);

            Position3D reference = new Position3D(
                new Latitude(randomizer.NextDouble() * 180 - 90),
                new Longitude(randomizer.NextDouble() * 360 - 180),
                new Distance(randomizer.NextDouble() * 1000, DistanceUnit.Meters));

            NedPoint vehicleLocation = new NedPoint(Distance.FromMeters(0), Distance.FromMeters(0), Distance.FromMeters(downInMeters));

            Position3D vehicleLla = vehicleLocation.ToPosition3D(reference);

            Assert.AreEqual(reference.Latitude.InDegrees(), vehicleLla.Latitude.InDegrees(), 0.0001, "Latitude incorrect");
            Assert.AreEqual(reference.Longitude.InDegrees(), vehicleLla.Longitude.InDegrees(), 0.00001, "Longitude incorrect");
            Assert.AreEqual(reference.Altitude.InMeters() - downInMeters, vehicleLla.Altitude.InMeters(), 0.0001, "Altitude incorrect");
        }

        [TestCase(1, 10)]
        [TestCase(2, 15)]
        [TestCase(3, -10)]
        [TestCase(4, -15)]
        [TestCase(5, 20)]
        public void EastTranslation(int seed, double eastInMeters)
        {
            Random randomizer = new Random(seed);

            Position3D reference = new Position3D(
                new Latitude(randomizer.NextDouble() * 80 - 40),
                new Longitude(randomizer.NextDouble() * 360 - 180),
                new Distance(randomizer.NextDouble() * 1000, DistanceUnit.Meters));

            NedPoint vehicleLocation = new NedPoint(Distance.FromMeters(0), Distance.FromMeters(eastInMeters), Distance.FromMeters(0));

            Position3D vehicleLla = vehicleLocation.ToPosition3D(reference);

            // One degree longitude cos(lat) * 111km =>
            //
            // cos(lat) * 1/111000.0 deg/meter =~
            //
            // cost(lat) * 0.00001 deg/meter
            Assert.AreEqual(reference.Latitude.InDegrees(), vehicleLla.Latitude.InDegrees(), 0.00001, "Latitude incorrect");
            Assert.AreEqual(reference.Longitude.InDegrees() + eastInMeters * (Math.Cos(reference.Latitude.InRadians()) * (1 / 111000.0)), vehicleLla.Longitude.InDegrees(), 0.0001, "Longitude incorrect");
            Assert.AreEqual(reference.Altitude.InMeters(), vehicleLla.Altitude.InMeters(), 0.0001, "Altitude incorrect");
        }

        [TestCase(1, 10)]
        [TestCase(2, 15)]
        [TestCase(3, -10)]
        [TestCase(4, -15)]
        [TestCase(5, 20)]
        public void NorthTranslation(int seed, double northInMeters)
        {
            Random randomizer = new Random(seed);

            Position3D reference = new Position3D(
                new Latitude(randomizer.NextDouble() * 180 - 90),
                new Longitude(randomizer.NextDouble() * 360 - 180),
                new Distance(randomizer.NextDouble() * 1000, DistanceUnit.Meters));

            NedPoint vehicleLocation = new NedPoint(Distance.FromMeters(northInMeters), Distance.FromMeters(0), Distance.FromMeters(0));

            Position3D vehicleLla = vehicleLocation.ToPosition3D(reference);

            // One degree latitude ~111 km => 1/111000.0 deg/meter =~ 0.00001 deg/meter
            Assert.AreEqual(reference.Latitude.InDegrees() + northInMeters * 0.00001, vehicleLla.Latitude.InDegrees(), 0.00002, "Latitude incorrect");
            Assert.AreEqual(reference.Longitude.InDegrees(), vehicleLla.Longitude.InDegrees(), 0.00001, "Longitude incorrect");
            Assert.AreEqual(reference.Altitude.InMeters(), vehicleLla.Altitude.InMeters(), 0.0001, "Altitude incorrect");
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void Reversable(int seed)
        {
            Random randomizer = new Random(seed);

            Position3D reference = new Position3D(
                new Latitude(randomizer.NextDouble() * 180 - 90),
                new Longitude(randomizer.NextDouble() * 360 - 180),
                new Distance(randomizer.NextDouble() * 1000, DistanceUnit.Meters));
            Position3D position = new Position3D(
                new Latitude(reference.Latitude.InDegrees() + (randomizer.NextDouble() * 1 - 0.5)),
                new Longitude(reference.Longitude.InDegrees() + (randomizer.NextDouble() * 1 - 0.5)),
                new Distance(reference.Altitude.InMeters() + (randomizer.NextDouble() * 1000 - 200), DistanceUnit.Meters));

            NedPoint nedPoint = position.ToNedPoint(reference);
            Position3D reversed = nedPoint.ToPosition3D(reference);

            Assert.AreEqual(position.Latitude.InDegrees(), reversed.Latitude.InDegrees(), 0.0001, "Latitude incorrect");
            Assert.AreEqual(position.Longitude.InDegrees(), reversed.Longitude.InDegrees(), 0.0001, "Longitude incorrect");
            Assert.AreEqual(position.Altitude.InMeters(), reversed.Altitude.InMeters(), 0.001, "Altitude incorrect");
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void Zero(int seed)
        {
            Random randomizer = new Random(seed);

            Position3D reference = new Position3D(
                new Latitude(randomizer.NextDouble() * 180 - 90),
                new Longitude(randomizer.NextDouble() * 360 - 180),
                new Distance(randomizer.NextDouble() * 1000, DistanceUnit.Meters));

            Position3D position = reference;
            NedPoint nedPoint = position.ToNedPoint(reference);

            Assert.AreEqual(0.0, nedPoint.N.InMeters(), 0.001, "North incorrect");
            Assert.AreEqual(0.0, nedPoint.E.InMeters(), 0.001, "East incorrect");
            Assert.AreEqual(0.0, nedPoint.D.InMeters(), 0.001, "Down incorrect");
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void ZeroReversable(int seed)
        {
            Random randomizer = new Random(seed);

            Position3D reference = new Position3D(
                new Latitude(randomizer.NextDouble() * 180 - 90),
                new Longitude(randomizer.NextDouble() * 360 - 180),
                new Distance(randomizer.NextDouble() * 1000, DistanceUnit.Meters));

            Position3D position = reference;
            NedPoint nedPoint = position.ToNedPoint(reference);
            Position3D reversed = nedPoint.ToPosition3D(reference);

            Assert.AreEqual(reference.Latitude.InDegrees(), reversed.Latitude.InDegrees(), 0.001, "Latitude incorrect");
            Assert.AreEqual(reference.Longitude.InDegrees(), reversed.Longitude.InDegrees(), 0.001, "Longitude incorrect");
            Assert.AreEqual(reference.Altitude.InMeters(), reversed.Altitude.InMeters(), 0.001, "Altitude incorrect");
        }
    }
}