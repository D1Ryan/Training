using System;
using System.Collections.Generic;
using SAPI.Models;
using System.Linq;

namespace SAPI.Common
{
    public static class KnackHelper
    {
        /// <summary>
        /// 返回指定月份的天数，如未指定月份，则返回当月的天数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetMonthDays(string year, string month)
        {
            //int[] mdays = new int[12] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            //bool isYear = int.TryParse(year, out int tyear);
            //bool isMonth = int.TryParse(month, out int tmonth);
            //if (isYear == false)
            //{
            //    tyear = DateTime.Today.Year;
            //}
            //else
            //{
            //    if ((tyear < 1900) | (tyear > 2200))
            //    {
            //        tyear = DateTime.Today.Year;
            //    }
            //}
            //if (isMonth == false)
            //{
            //    tmonth = DateTime.Today.Month;
            //}
            //else
            //{
            //    if ((tmonth < 1) | (tmonth > 12))
            //    {
            //        tmonth = DateTime.Today.Month;
            //    }
            //}
            //if (tmonth == 2)
            //{
            //    DateTime today = new DateTime(tyear, tmonth, 1);
            //    DateTime nextMonth = today.AddMonths(1);
            //    return nextMonth.DayOfYear - today.DayOfYear;
            //}
            //else
            //{
            //    return mdays[tmonth - 1];
            //}
            return 1;
        }

        /// <summary>
        /// 返回指定月份的天数，需校验年月有效性(未来的年月)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetDaysByMonth(string year, string month, out int tyear, out int tmonth)
        {
            int tdays = 0;
            tyear = 0;
            tmonth = 0;
            bool isYear = int.TryParse(year, out tyear);
            bool isMonth = int.TryParse(month, out tmonth);
            if (isYear == false)
            {
                return tdays;
            }
            else
            {
                //年份介于今年和2200之间
                if ((tyear < DateTime.Today.Year) | (tyear > 2200))
                {
                    tyear = 0;
                    return tdays;
                }
                else
                {
                    if (isMonth == false)
                    {
                        return tdays;
                    }
                    else
                    {
                        ////月份介于1和12之间
                        if ((tmonth < 1) | (tmonth > 12))
                        {
                            tmonth = 0;
                            return tdays;
                        }
                        else
                        {
                            DateTime today = new DateTime(tyear, tmonth, 1);
                            DateTime nextMonth = today.AddMonths(1);
                            return nextMonth.DayOfYear - today.DayOfYear;
                        }
                    }
                }
            }
        }

        ///// <summary>
        ///// SMS根据公式组号及公式名，计算五险一金(Warning:公式可能会修改)
        ///// </summary>
        ///// <param name="list"></param>
        ///// <param name="groupId"></param>
        ///// <param name="iName"></param>
        ///// <returns></returns>
        //public static float GetStaffFund(List<SMS_Formula> list, int groupId, string iName, float fBase)
        //{
        //    var items = list.Where(w => w.Group_Id.Equals(groupId) && w.Item.Equals(iName));
        //    float iFoud = 0;
        //    if (items.Count() > 0)
        //    {
        //        //ROUND(缴费基数*0.02+0.04,1)
        //        int si = items.First().Formula.IndexOf("*") + 1;
        //        int ei = items.First().Formula.IndexOf(",");
        //        string[] formulas = items.First().Formula.Substring(si, ei - si).Split("+".ToCharArray());
        //        bool isBase = float.TryParse(formulas[0], out  fPoint);
        //        if (isBase)
        //        {
        //            iFoud = fPoint * fBase;
        //        }
        //        //如果计算部分还有加数，
        //        if (formulas.Length > 1)
        //        {
        //            bool hPlus = float.TryParse(formulas[1], out  fPlus);
        //            if (hPlus)
        //            {
        //                iFoud += fPlus;
        //            }
        //        }
        //        //保留两位小数，四舍五入
        //        iFoud = (float)Math.Round(iFoud, 2, MidpointRounding.AwayFromZero);
        //    }
        //    return iFoud;
        //}

    }
}