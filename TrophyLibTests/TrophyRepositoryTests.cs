using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrophyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrophyLib.Tests
{
    [TestClass()]
    public class TrophyRepositoryTests
    {
        private TrophyRepository repo;

        [TestInitialize()]
        public void Init()
        {
            repo = new();
            repo.Add(new() { Competition = "Ridning", Year = 2012 });
            repo.Add(new() { Competition = "Basketball", Year = 2019 });
            repo.Add(new() { Competition = "Linedans", Year = 2008 });
            repo.Add(new() { Competition = "Kartoffelspisning", Year = 2018 });
            repo.Add(new() { Competition = "Åbn dåse", Year = 2015 });
        }

        [TestMethod()]
        public void GetTest()
        {
            IEnumerable<Trophy> trophies = repo.Get();
            Assert.AreEqual(5, trophies.Count());
            Assert.AreEqual(new Trophy() { Id = 1, Competition = "Ridning", Year = 2012 }, trophies.ElementAt(0));
            Assert.AreEqual(new Trophy() { Id = 2, Competition = "Basketball", Year = 2019 }, trophies.ElementAt(1));
            Assert.AreEqual(new Trophy() { Id = 3, Competition = "Linedans", Year = 2008 }, trophies.ElementAt(2));
            Assert.AreEqual(new Trophy() { Id = 4, Competition = "Kartoffelspisning", Year = 2018 }, trophies.ElementAt(3));
            Assert.AreEqual(new Trophy() { Id = 5, Competition = "Åbn dåse", Year = 2015 }, trophies.ElementAt(4));

            trophies = repo.Get(beforeYear:2015);
            Assert.AreEqual(2, trophies.Count());
            Assert.AreEqual(new Trophy() { Id = 1, Competition = "Ridning", Year = 2012 }, trophies.ElementAt(0));
            Assert.AreEqual(new Trophy() { Id = 3, Competition = "Linedans", Year = 2008 }, trophies.ElementAt(1));
            

            trophies = repo.Get(afterYear: 2015);
            Assert.AreEqual(2, trophies.Count());
            Assert.AreEqual(new Trophy() { Id = 2, Competition = "Basketball", Year = 2019 }, trophies.ElementAt(0));
            Assert.AreEqual(new Trophy() { Id = 4, Competition = "Kartoffelspisning", Year = 2018 }, trophies.ElementAt(1));

            trophies = repo.Get(beforeYear: 2018, afterYear: 2012);
            Assert.AreEqual(1, trophies.Count());
            Assert.AreEqual(new Trophy() { Id = 5, Competition = "Åbn dåse", Year = 2015 }, trophies.ElementAt(0));
        }

        [TestMethod()]
        [DataRow("ID", new int[] { 0, 1, 2, 3, 4 })]
        [DataRow("ID_ASC", new int[] { 0, 1, 2, 3, 4 })]
        [DataRow("ID_DESC", new int[] { 4, 3, 2, 1, 0 })]
        [DataRow("COMPETITION", new int[] { 3, 0, 2, 1, 4 })]
        [DataRow("COMPETITION_ASC", new int[] { 3, 0, 2, 1, 4 })]
        [DataRow("competition_DESC", new int[] { 1, 4, 2, 3, 0 })]
        [DataRow("YeAr", new int[] { 1, 4, 0, 3, 2 })]
        [DataRow("yEaR_aSc", new int[] { 1, 4, 0, 3, 2 })]
        [DataRow("yEaR_DeSc", new int[] { 3, 0, 4, 1, 2 })]
        public void SortedGetTest(string searchTerm, int[] expectedPositions)
        {
            IEnumerable<Trophy> trophies = repo.Get(sortBy: searchTerm);
            Assert.AreEqual(5, trophies.Count());
            Assert.AreEqual(new Trophy() { Id = 1, Competition = "Ridning", Year = 2012 }, trophies.ElementAt(expectedPositions[0]));
            Assert.AreEqual(new Trophy() { Id = 2, Competition = "Basketball", Year = 2019 }, trophies.ElementAt(expectedPositions[1]));
            Assert.AreEqual(new Trophy() { Id = 3, Competition = "Linedans", Year = 2008 }, trophies.ElementAt(expectedPositions[2]));
            Assert.AreEqual(new Trophy() { Id = 4, Competition = "Kartoffelspisning", Year = 2018 }, trophies.ElementAt(expectedPositions[3]));
            Assert.AreEqual(new Trophy() { Id = 5, Competition = "Åbn dåse", Year = 2015 }, trophies.ElementAt(expectedPositions[4]));
        }

        [TestMethod()]
        public void GetByIdTest()
        {
            Assert.AreEqual(new() { Id = 4, Competition = "Kartoffelspisning", Year = 2018 }, repo.GetById(4));
            Assert.AreEqual(5, repo.Get().Count());
        }

        [TestMethod()]
        public void AddTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => repo.Add(null));
            Assert.ThrowsException<ArgumentNullException>(() => repo.Add(new() { Competition = null, Year = 2000 }));
            Assert.ThrowsException<ArgumentException>(() => repo.Add(new() { Competition = "Ri", Year = 2000 }));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => repo.Add(new() { Competition = "Ridning", Year = 2050 }));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => repo.Add(new() { Competition = "Ridning", Year = 1950 }));

            Assert.AreEqual(new() { Id = 6, Competition = "Maraton", Year = 2019 }, repo.Add(new() { Competition = "Maraton", Year = 2019 }));
            IEnumerable<Trophy> trophies = repo.Get();
            Assert.AreEqual(6, trophies.Count());
            Assert.AreEqual(new() { Id = 1, Competition = "Ridning", Year = 2012 }, trophies.ElementAt(0));
            Assert.AreEqual(new() { Id = 2, Competition = "Basketball", Year = 2019 }, trophies.ElementAt(1));
            Assert.AreEqual(new() { Id = 3, Competition = "Linedans", Year = 2008 }, trophies.ElementAt(2));
            Assert.AreEqual(new() { Id = 4, Competition = "Kartoffelspisning", Year = 2018 }, trophies.ElementAt(3));
            Assert.AreEqual(new() { Id = 5, Competition = "Åbn dåse", Year = 2015 }, trophies.ElementAt(4));
            Assert.AreEqual(new() { Id = 6, Competition = "Maraton", Year = 2019 }, repo.GetById(6));
        }

        [TestMethod()]
        public void RemoveTest()
        {
            Assert.AreEqual(new() { Id = 4, Competition = "Kartoffelspisning", Year = 2018 }, repo.Remove(4));
            IEnumerable<Trophy> trophies = repo.Get();
            Assert.AreEqual(4, trophies.Count());
            Assert.AreEqual(new Trophy() { Id = 1, Competition = "Ridning", Year = 2012 }, trophies.ElementAt(0));
            Assert.AreEqual(new Trophy() { Id = 2, Competition = "Basketball", Year = 2019 }, trophies.ElementAt(1));
            Assert.AreEqual(new Trophy() { Id = 3, Competition = "Linedans", Year = 2008 }, trophies.ElementAt(2));
            Assert.AreEqual(new Trophy() { Id = 5, Competition = "Åbn dåse", Year = 2015 }, trophies.ElementAt(3));
        }

        [TestMethod()]
        public void UpdateTest()
        {
            Assert.AreEqual(null, repo.Update(-1, new() { Competition = "Ridning", Year = 2000 }));
            Assert.ThrowsException<ArgumentNullException>(() => repo.Update(3, null));
            Assert.ThrowsException<ArgumentNullException>(() => repo.Update(3, new() { Competition = null, Year = 2000 }));
            Assert.ThrowsException<ArgumentException>(() => repo.Update(3, new() { Competition = "Ri", Year = 2000 }));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => repo.Update(3, new() { Competition = "Ridning", Year = 2050 }));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => repo.Update(3, new() { Competition = "Ridning", Year = 1950 }));

            Assert.AreEqual(new() { Id = 2, Competition = "Maraton", Year = 2022 }, repo.Update(2,new() { Competition = "Maraton", Year = 2022 }));
            IEnumerable<Trophy> trophies = repo.Get();
            Assert.AreEqual(5, trophies.Count());
            Assert.AreEqual(new() { Id = 1, Competition = "Ridning", Year = 2012 }, trophies.ElementAt(0));
            Assert.AreEqual(new() { Id = 2, Competition = "Maraton", Year = 2022 }, trophies.ElementAt(1));
            Assert.AreEqual(new() { Id = 3, Competition = "Linedans", Year = 2008 }, trophies.ElementAt(2));
            Assert.AreEqual(new() { Id = 4, Competition = "Kartoffelspisning", Year = 2018 }, trophies.ElementAt(3));
            Assert.AreEqual(new() { Id = 5, Competition = "Åbn dåse", Year = 2015 }, trophies.ElementAt(4));
        }
    }
}