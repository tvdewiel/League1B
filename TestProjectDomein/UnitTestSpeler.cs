using League.BL.Domein;
using League.BL.Exceptions;
using System;
using Xunit;

namespace TestProjectDomein
{
    public class UnitTestSpeler
    {
        [Fact]
        public void ZetId_valid()
        {
            Speler s = new Speler(10,"Jos",180,80);
            Assert.Equal(10,s.Id);
            s.ZetId(1);
            Assert.Equal(1,s.Id);
        }
        [Fact]
        public void ZetId_invalid()
        {
            Speler s = new Speler(10, "Jos", 180, 80);
            Assert.Equal(10, s.Id);
            Assert.Throws<SpelerException>(()=>s.ZetId(0));
            Assert.Equal(10, s.Id);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(99)]
        public void ZetRugnummer_valid(int rugnr)
        {
            Speler s = new Speler(10, "Jos", 180, 80);
            
            s.ZetRugnummer(rugnr);
            Assert.Equal(rugnr, s.Rugnummer);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(100)]
        public void ZetRugnummer_invalid(int rugnr)
        {
            Speler s = new Speler(10, "Jos", 180, 80);
            Assert.Throws<SpelerException>(() => s.ZetRugnummer(rugnr));
        }
        [Theory]
        [InlineData("Jeff","Jeff")]
        [InlineData("Eden Hazard","Eden Hazard")]
        [InlineData("Eden Hazard ","Eden Hazard")]
        [InlineData("   Eden Hazard ", "Eden Hazard")]
        public void ZetNaam_valid(string naamin,string naamuit)
        {
            Speler s = new Speler(10, "Jos", 180, 80);

            s.ZetNaam(naamin);
            Assert.Equal(naamuit, s.Naam);
        }
        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  \n")]
        [InlineData("  \r ")]
        public void ZetNaam_invalid(string naam)
        {
            Speler s = new Speler(10, "Jos", 180, 80);

            Assert.Throws<SpelerException>(() => s.ZetNaam(naam));
        }
        [Fact]
        public void ctor_valid()
        {
            Speler s = new Speler(10, "Jos", 180, 80);

            Assert.Equal(10,s.Id);
            Assert.Equal("Jos",s.Naam);
            Assert.Equal(180,s.Lengte);
            Assert.Equal(80,s.Gewicht);

            s = new Speler(10, "Jos", null, 80);
            Assert.Equal(10, s.Id);
            Assert.Equal("Jos", s.Naam);
            Assert.Null(s.Lengte);
            Assert.Equal(80, s.Gewicht);

            s = new Speler(10, "Jos",180, null);
            Assert.Equal(10, s.Id);
            Assert.Equal("Jos", s.Naam);
            Assert.Null(s.Gewicht);
            Assert.Equal(180, s.Lengte);
        }
    }
}
