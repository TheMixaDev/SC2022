using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REG_MARK_LIB
{
    public class RegMark
    {
        static string valueData = "1;1-4;4-2;02,102-3;3-5;5-6;6-7;7-8;8-9;9-10;10-11;11,111-91;82-12;12-13;13,113-14;14-15;15-16;16,116-17;17-18;18-19;19-20;95-21;21,121-22;22-75;75,80-41;41-23;23,93,123-24;24,124,84,88-59;59,81,159-25;25,125-26;26,126-27;27-28;28-29;29-30;30-31;31-32;32-33;33-34;34,134-35;35-36;36,136-37;37-38;38,138,85-39;39,91-40;40-42;42,142-43;43-44;44-45;45-46;46-47;47-48;48-49;49-50;50,90,150,190,750-51;51-52;52,152-53;53-54;54,154-55;55-56;56-57;57-58;58-60;60-61;61,161-62;62-63;63,163-64;64,164-65;65-66;66,96,196-67;67-68;68-69;69-70;70-71;71-72;72-73;73,173-74;74,174-76;76-77;77,97,99,177,197,199,777-78;78,98,178-92;92-79;79-83;83-86;86,186-87;87-89;89-94;94";
        static Dictionary<int, List<string>> regionToKeys = new Dictionary<int, List<string>>();
        static List<string> allCodes = new List<string>();
        static string chars = "ABEKMHOPCTYX";
        private static void FillDictionary()
        {
            if (regionToKeys.Count > 0) return;
            foreach(string pair in valueData.Split('-'))
            {
                string[] info = pair.Split(';');
                List<string> result = info[1].Split(',').ToList();
                int key = int.Parse(info[0]);
                if (!regionToKeys.ContainsKey(key))
                {
                    regionToKeys[key] = result;
                }
                else regionToKeys[key].AddRange(result);
                allCodes.AddRange(result);
            }
        }
        /// <summary>
        /// Метод для проверки корректности номерного знака
        /// </summary>
        /// <param name="mark">Номер</param>
        /// <returns>Корректность</returns>
        public static bool CheckMark(string mark)
        {
            FillDictionary();
            string numeric = "0123456789";
            try
            {
                if (!chars.Contains(mark[0]) ||
                    !chars.Contains(mark[4]) ||
                    !chars.Contains(mark[5])) return false;
                if (!numeric.Contains(mark[1]) ||
                    !numeric.Contains(mark[2]) ||
                    !numeric.Contains(mark[3])) return false;
                if (mark.Substring(1, 3).Equals("000")) return false;
                string region = mark.Substring(6);
                if (!allCodes.Contains(region)) return false;
            } catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Метод для получения следующего номера после определенного
        /// </summary>
        /// <param name="mark">Номер</param>
        /// <returns>Следующий за ним номер</returns>
        public static string GetNextMarkAfter(string mark)
        {
            FillDictionary();
            if (!CheckMark(mark)) return null;
            int charsMax = 1727; //Max Number in 12-system
            int numericValue = int.Parse(mark.Substring(1,3));
            string letterValue = mark[0] + mark.Substring(4, 2);
            int letterNumeric = From12to10(letterValue);
            string region = mark.Substring(6);
            numericValue++;
            if (numericValue == 1000)
            {
                numericValue = 1;
                letterNumeric++;
                if (letterNumeric > charsMax)
                {
                    letterNumeric = 0;
                    foreach (int regionNum in regionToKeys.Keys)
                    {
                        if (regionToKeys[regionNum].Contains(region))
                        {
                            int newRegionIndex = regionToKeys[regionNum].IndexOf(region) + 1;
                            if (newRegionIndex != regionToKeys[regionNum].Count) region = regionToKeys[regionNum][newRegionIndex];
                            else return null;
                            break;
                        }
                    }
                }
                letterValue = From10to12(letterNumeric);
            }
            string numericFinal = addZeros(numericValue);
            string result = letterValue[0]+numericFinal+letterValue[1]+letterValue[2]+region;
            return result;
        }
        private static int From12to10(string numberIn12)
        {
            return chars.IndexOf(numberIn12[0]) * 144 + chars.IndexOf(numberIn12[1]) * 12 + chars.IndexOf(numberIn12[2]);
        }
        private static string From10to12(int numberIn10)
        {
            string result = "";
            result += chars[numberIn10 % 12];
            numberIn10 = (int)Math.Floor((double)numberIn10/ 12d);
            result += chars[numberIn10 % 12];
            numberIn10 = (int)Math.Floor((double)numberIn10 / 12d);
            result += chars[numberIn10 % 12];
            char[] listChar = result.ToCharArray();
            Array.Reverse(listChar);
            return new string(listChar);
        }

        private static string addZeros(int number)
        {
            string first = number.ToString();
            if (first.Length == 2) return "0" + first;
            if (first.Length == 1) return "00" + first;
            return first;
        }
        /// <summary>
        /// Метод для подсчета количества номеров в диапозоне
        /// </summary>
        /// <param name="mark1">Номер начала диапозона</param>
        /// <param name="mark2">Номер конца диапозона</param>
        /// <returns>Количество</returns>
        public static int GetCombinationsCountInRange(string mark1, string mark2)
        {
            FillDictionary();
            if (!CheckMark(mark1) || !CheckMark(mark2)) return -1;
            int fromLetters1 = From12to10(mark1[0] + mark1.Substring(4, 2));
            int fromLetters2 = From12to10(mark2[0] + mark2.Substring(4, 2));
            int numeric1 = fromLetters1 * 1000 + int.Parse(mark1.Substring(1, 3));
            int numeric2 = fromLetters2 * 1000 + int.Parse(mark2.Substring(1, 3));
            string region1 = mark1.Substring(6);
            string region2 = mark2.Substring(6);
            if(region1 != region2)
            {
                int regionCode = -1;
                foreach(int key in regionToKeys.Keys)
                {
                    if(regionToKeys[key].Contains(region1) && regionToKeys[key].Contains(region2))
                    {
                        regionCode = key;
                        break;
                    }
                }
                if (regionCode == -1) return -1;
                int region1Index = regionToKeys[regionCode].IndexOf(region1);
                int region2Index = regionToKeys[regionCode].IndexOf(region2);
                int numbersPerRegion = 1726272; //Numbers in one region
                if(region1Index > region2Index)
                    return GetCombinationsCountInRange(mark2, "X999XX" + region2) + GetCombinationsCountInRange("A001AA" + region1, mark1) + (region1Index - region2Index - 1) * numbersPerRegion;
                return GetCombinationsCountInRange(mark1, "X999XX" + region1) + GetCombinationsCountInRange("A001AA" + region2, mark2) + (region2Index-region1Index-1) * numbersPerRegion;
            }
            return Math.Abs(numeric2-numeric1)-Math.Abs(fromLetters2 - fromLetters1)+1;
        }
    }
}
