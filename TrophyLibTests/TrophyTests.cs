using TrophyLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrophyLib.Tests
{
    [TestClass()]
    public class TrophyTests
    {

        private Trophy good = new();
        private Trophy badNameNull = new();
        private Trophy badNameShort = new();
        private Trophy badYear = new();

        [TestInitialize()]
        public void Init()
        {
            good.Competition = "Swimming";
            good.Year = 1990;
            badNameNull.Competition = null;
            badNameNull.Year = 1990;
            badNameShort.Competition = "";
            badNameShort.Year = 1990;
            badYear.Competition = "RiffelSkydning";
            badYear.Year = 2050;
        }

    [TestMethod()]
        public void ValidateCompetitionTest()
        {
            good.ValidateCompetition();
            Assert.ThrowsException<ArgumentNullException>(badNameNull.ValidateCompetition);
            good.Competition = "Ski";
            good.ValidateCompetition();
            good.Competition = "Skin";
            good.ValidateCompetition();
        }

        [TestMethod()]
        [DataRow("")]
        [DataRow("S")]
        [DataRow("Sk")]
        public void ValidateCompetitionArgumentExceptionTest(string competition)
        {
            badNameShort.Competition = competition;
            Assert.ThrowsException<ArgumentException>(badNameShort.ValidateCompetition);
        }

        [TestMethod()]
        [DataRow(1970)]
        [DataRow(1971)]
        [DataRow(2024)]
        [DataRow(2023)]
        public void ValidateYearGoodTest(int year)
        {
            good.Year = year;
            good.ValidateYear();
        }

        [TestMethod()]
        [DataRow(2025)]
        [DataRow(1969)]
        public void ValidateYearOutOfRangeExceptionTest(int year)
        {
            badYear.Year = year;
            Assert.ThrowsException<ArgumentOutOfRangeException>(badYear.ValidateYear);
        }

        [TestMethod()]
        public void ValidateTest()
        {
            good.Validate();
            Assert.ThrowsException<ArgumentNullException>(badNameNull.Validate);
            Assert.ThrowsException<ArgumentException>(badNameShort.Validate);
            good.Year = 1970;
            good.Validate();
            good.Year = 1971;
            good.Validate();
            good.Year = 2023;
            good.Validate();
            good.Year = 2024;
            good.Validate();
            badYear.Year = 1969;
            Assert.ThrowsException<ArgumentOutOfRangeException>(badYear.Validate);
            badYear.Year = 2025;
            Assert.ThrowsException<ArgumentOutOfRangeException>(badYear.Validate);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            good.Year = 2024;
            Assert.AreEqual(good.ToString(), "0 Swimming 2024");
            Assert.AreEqual(badNameNull.ToString(), "0  1990");
            Assert.AreEqual(badNameShort.ToString(), "0  1990");
        }

        [TestMethod()]
        public void CopyTrophyTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Trophy(null));

            Assert.AreEqual<Trophy>(new() { Id = 0, Competition = "Swimming", Year = 1990 }, new(good));
            Assert.ThrowsException<ArgumentNullException>(() => new Trophy(badNameNull));
            Assert.ThrowsException<ArgumentException>(() => new Trophy(badNameShort));
            badYear.Year = 2026;
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Trophy(badYear));
            badYear.Year = 1960;
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Trophy(badYear));
        }
    }
}