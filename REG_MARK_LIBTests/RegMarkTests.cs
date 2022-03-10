using Microsoft.VisualStudio.TestTools.UnitTesting;
using REG_MARK_LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REG_MARK_LIB.Tests
{
    [TestClass()]
    public class RegMarkTests
    {
        [TestMethod()]
        [DataRow("E978YK159")]
        [DataRow("E978YK78")]
        [DataRow("Y111YY777")]
        public void CheckMark_CorrectMark3DigitRegions_ReturnsTrue(string mark)
        {
            bool actual = RegMark.CheckMark(mark);
            bool expected = true;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void CheckMark_CorrectMark2DigitRegions_ReturnsTrue()
        {
            bool actual = RegMark.CheckMark("E978YK59");
            bool expected = true;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void CheckMark_NonexistedRegions_ReturnsFalse()
        {
            bool actual = RegMark.CheckMark("E978YK888");
            bool expected = false;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void CheckMark_NoRegion_ReturnsFalse()
        {
            bool actual = RegMark.CheckMark("E978YK");
            bool expected = false;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void CheckMark_NumberInvalidLessDigits_ReturnsFalse()
        {
            bool actual = RegMark.CheckMark("E97YK159");
            bool expected = false;
            Assert.AreEqual(actual, expected);
        }
        [TestMethod()]
        public void CheckMark_NumberInvalidMoreDigits_ReturnsFalse()
        {
            bool actual = RegMark.CheckMark("E9745YK159");
            bool expected = false;
            Assert.AreEqual(actual, expected);
        }
        [TestMethod()]
        public void CheckMark_MixedSymbols_ReturnsFalse()
        {
            bool actual = RegMark.CheckMark("E97Y5K159");
            bool expected = false;
            Assert.AreEqual(actual, expected);
        }
        [TestMethod()]
        public void CheckMark_LessLetters_ReturnsFalse()
        {
            bool actual = RegMark.CheckMark("E975K159");
            bool expected = false;
            Assert.AreEqual(actual, expected);
        }
        [TestMethod()]
        public void CheckMark_MoreLetters_ReturnsFalse()
        {
            bool actual = RegMark.CheckMark("E975KKK159");
            bool expected = false;
            Assert.AreEqual(actual, expected);
        }
        [TestMethod()]
        public void CheckMark_InvalidLetters_ReturnsFalse()
        {
            bool actual = RegMark.CheckMark("E975KW159");
            bool expected = false;
            Assert.AreEqual(actual, expected);
        }
        [TestMethod()]
        public void CheckMark_InvalidZero_ReturnsFalse()
        {
            bool actual = RegMark.CheckMark("E000KY159");
            bool expected = false;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void GetNextMarkAfter_NumberPlus()
        {
            var expected = RegMark.GetNextMarkAfter("K001OT159");
            var actual = "K002OT159";
            Assert.AreEqual(actual, expected);
        }
        [TestMethod()]
        public void GetNextMarkAfter_Number999Plus()
        {
            var expected = RegMark.GetNextMarkAfter("K999OT159");
            var actual = "K001OY159";
            Assert.AreEqual(actual, expected);
        }
        [TestMethod()]
        public void GetNextMarkAfter_RegionPlus()
        {
            var expected = RegMark.GetNextMarkAfter("X999XX81");
            var actual = "A001AA159";
            Assert.AreEqual(actual, expected);
        }
        [TestMethod()]
        public void GetNextMarkAfter_RegionPlusSecond()
        {
            var expected = RegMark.GetNextMarkAfter("X999XX78");
            var actual = "A001AA98";
            Assert.AreEqual(actual, expected);
        }
        [TestMethod()]
        public void GetNextMarkAfter_WordLastPlus()
        {
            var expected = RegMark.GetNextMarkAfter("A999XX78");
            var actual = "B001AA78";
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void GetCombinationsCountInRange_SameNumbers_1()
        {
            var actual = RegMark.GetCombinationsCountInRange("A999XX78", "A999XX78");
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetCombinationsCountInRange_NumberOneMore_2()
        {
            var actual = RegMark.GetCombinationsCountInRange("A998XX78", "A999XX78");
            var expected = 2;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void GetCombinationsCountInRange_NumberOneLess_2()
        {
            var actual = RegMark.GetCombinationsCountInRange("A999XX78", "A998XX78");
            var expected = 2;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetCombinationsCountInRange_NumberNextLetter_2()
        {
            var actual = RegMark.GetCombinationsCountInRange("A999XX78", "B001AA78");
            var expected = 2;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetCombinationsCountInRange_FullDigitRange_999()
        {
            var actual = RegMark.GetCombinationsCountInRange("A001XX78", "A999XX78");
            var expected = 999;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetCombinationsCountInRange_RegionChange_2()
        {
            //var actual = RegMark.GetCombinationsCountInRange("X999XX78", "A001AA98");
            var actual = RegMark.GetCombinationsCountInRange("X999XX78", "A001AA98");
            var expected = 2;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void GetCombinationsCountInRange_RegionChangeHuge_3452546()
        {
            //var actual = RegMark.GetCombinationsCountInRange("X999XX78", "A001AA98");
            var actual = RegMark.GetCombinationsCountInRange("X999XX78", "A001AA178");
            var expected = 1726274;
            Assert.AreEqual(expected, actual);
        }
    }
}