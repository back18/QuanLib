using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib
{
    /// <summary>
    /// 日期时间工具类
    /// </summary>
    public static class DateTimeUtil
    {
        /// <summary>
        /// 获取不依赖于区域性（固定）的 Calendar 对象
        /// </summary>
        public static readonly System.Globalization.Calendar InvariantCulture = System.Globalization.CultureInfo.InvariantCulture.Calendar;

        /// <summary>
        /// 平年每月天数
        /// </summary>
        private static readonly int[] ordinaryYearMonthDays = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        /// <summary>
        /// 获取平年每月天数
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetOrdinaryYearMonthDays(int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("超出月份范围，应为1~12");

            return ordinaryYearMonthDays[month - 1];
        }

        /// <summary>
        /// 解析纯数字日期字符串（最高到毫秒）
        /// </summary>
        /// <param name="timeNumber"></param>
        /// <returns></returns>
        public static DateTime ParseNumber(string timeNumber)
        {
            if (string.IsNullOrEmpty(timeNumber))
                throw new ArgumentException($"“{nameof(timeNumber)}”不能为 null 或空。", nameof(timeNumber));

            switch (timeNumber.Length)
            {
                case 1:
                    if (timeNumber.Equals("0"))
                        throw new ArgumentException("没有公元0年");
                    else goto case 3;
                case 2:
                case 3:
                    timeNumber = timeNumber.PadLeft(4, '0');
                    goto case 4;
                case 4:
                    timeNumber += "0101";
                    goto case 14;
                case 6:
                    timeNumber += "01";
                    goto case 14;
                case 8:
                case 10:
                case 12:
                case 14:
                    timeNumber = timeNumber.PadRight(17, '0');
                    break;
                case 17:
                    break;
                default: throw new ArgumentException("解析纯数字格式字符串时间精度最高到毫秒（最高17位）");
            }

            return new DateTime(
                int.Parse(timeNumber.Substring(0, 4)),
                int.Parse(timeNumber.Substring(4, 2)),
                int.Parse(timeNumber.Substring(6, 2)),
                int.Parse(timeNumber.Substring(8, 2)),
                int.Parse(timeNumber.Substring(10, 2)),
                int.Parse(timeNumber.Substring(12, 2)),
                int.Parse(timeNumber[14..]));
        }

        /// <summary>
        /// 解析Ticks字符串
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static DateTime ParseTicks(string ticks)
        {
            if (string.IsNullOrEmpty(ticks))
                throw new ArgumentException($"“{nameof(ticks)}”不能为 null 或空。", nameof(ticks));

            return new DateTime(long.Parse(ticks));
        }

        /// <summary>
        /// 解析字符串数组类型的日期
        /// </summary>
        /// <param name="timeArray"></param>
        /// <returns></returns>
        public static DateTime ParseArray(string[] timeArray)
        {
            int[] dataToInt = StringUtil.ToInts(timeArray);
            return timeArray.Length switch
            {
                3 => new DateTime(dataToInt[0], dataToInt[1], dataToInt[2]),
                6 => new DateTime(dataToInt[0], dataToInt[1], dataToInt[2], dataToInt[3], dataToInt[4], dataToInt[5]),
                7 => new DateTime(dataToInt[0], dataToInt[1], dataToInt[2], dataToInt[3], dataToInt[4], dataToInt[5], dataToInt[6]),
                _ => throw new ArgumentException("解析数组格式字符串精度最高到毫秒（最高7个元素）"),
            };
        }

        /// <summary>
        /// 增加指定单位长度的时间
        /// </summary>
        /// <param name="value">目标时间</param>
        /// <param name="count">加减的值</param>
        /// <param name="unit">单位</param>
        /// <returns></returns>
        public static DateTime Add(DateTime value, int count, DateTimeUnit unit)
        {
            return unit switch
            {
                DateTimeUnit.Year => InvariantCulture.AddYears(value, count),
                DateTimeUnit.Month => InvariantCulture.AddMonths(value, count),
                DateTimeUnit.Day => InvariantCulture.AddDays(value, count),
                DateTimeUnit.Hour => InvariantCulture.AddHours(value, count),
                DateTimeUnit.Minute => InvariantCulture.AddMinutes(value, count),
                DateTimeUnit.Second => InvariantCulture.AddSeconds(value, count),
                DateTimeUnit.Millisecond => InvariantCulture.AddMilliseconds(value, count),
                DateTimeUnit.Ticks => new DateTime(value.Ticks + count),
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// 获取指定等级的时间差
        /// </summary>
        /// <param name="arg1">被减数</param>
        /// <param name="arg2">减数</param>
        /// <param name="level">精度</param>
        /// <returns></returns>
        public static long GetLevelDifference(DateTime arg1, DateTime arg2, DateTimeUnit level)
        {
            switch (level)
            {
                case DateTimeUnit.Year:
                    return (new DateTime(arg1.Year, 0, 0).Ticks -
                            new DateTime(arg2.Year, 0, 0).Ticks) / DateTimeTicks.Year;
                case DateTimeUnit.Month:
                    int Result;
                    if (GetLevelTime(arg1, DateTimeUnit.Month).Ticks >= GetLevelTime(arg2, DateTimeUnit.Month).Ticks)
                    {
                        Result = arg1.Month - arg2.Month;
                        if (Result >= 0) Result += (arg1.Year - arg2.Year) * 12;
                        else Result = Result + 12 + (arg1.Year - arg2.Year - 1) * 12;
                        return Result;
                    }
                    else
                    {
                        Result = arg2.Month - arg1.Month;
                        if (Result >= 0) Result += (arg2.Year - arg1.Year) * 12;
                        else Result = Result + 12 + (arg2.Year - arg1.Year - 1) * 12;
                        return -Result;
                    }
                case DateTimeUnit.Day:
                    return (new DateTime(arg1.Year, arg1.Month, arg1.Day).Ticks -
                            new DateTime(arg2.Year, arg2.Month, arg2.Day).Ticks) / DateTimeTicks.Day;
                case DateTimeUnit.Hour:
                    return (new DateTime(arg1.Year, arg1.Month, arg1.Day, arg1.Hour, 0, 0).Ticks -
                            new DateTime(arg2.Year, arg2.Month, arg2.Day, arg2.Hour, 0, 0).Ticks) / DateTimeTicks.Hour;
                case DateTimeUnit.Minute:
                    return (new DateTime(arg1.Year, arg1.Month, arg1.Day, arg1.Hour, arg1.Minute, 0).Ticks -
                            new DateTime(arg2.Year, arg2.Month, arg2.Day, arg2.Hour, arg2.Minute, 0).Ticks) / DateTimeTicks.Minute;
                case DateTimeUnit.Second:
                    return (new DateTime(arg1.Year, arg1.Month, arg1.Day, arg1.Hour, arg1.Minute, arg1.Second).Ticks -
                            new DateTime(arg2.Year, arg2.Month, arg2.Day, arg2.Hour, arg2.Minute, arg2.Second).Ticks) / DateTimeTicks.Second;
                case DateTimeUnit.Millisecond:
                    return (new DateTime(arg1.Year, arg1.Month, arg1.Day, arg1.Hour, arg1.Minute, arg1.Second, arg1.Millisecond).Ticks -
                            new DateTime(arg2.Year, arg2.Month, arg2.Day, arg2.Hour, arg2.Minute, arg2.Second, arg2.Millisecond).Ticks) / DateTimeTicks.Minute;
                case DateTimeUnit.Ticks:
                    return arg1.Ticks - arg2.Ticks;
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 获取指定等级的时间
        /// </summary>
        /// <param name="value">时间</param>
        /// <param name="level">精度</param>
        /// <returns></returns>
        public static DateTime GetLevelTime(DateTime value, DateTimeUnit level)
        {
            return level switch
            {
                DateTimeUnit.Year => new DateTime(value.Year, 1, 1),
                DateTimeUnit.Month => new DateTime(value.Year, value.Month, 1),
                DateTimeUnit.Day => new DateTime(value.Year, value.Month, value.Day),
                DateTimeUnit.Hour => new DateTime(value.Year, value.Month, value.Day, value.Hour, 0, 0),
                DateTimeUnit.Minute => new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0),
                DateTimeUnit.Second => new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second),
                DateTimeUnit.Millisecond => new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond),
                DateTimeUnit.Ticks => value,
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// 判断时间等级
        /// </summary>
        /// <param name="value">时间</param>
        /// <param name="level">精度</param>
        /// <returns></returns>
        public static bool IsLevel(DateTime value, DateTimeUnit level)
        {
            switch (level)
            {
                case DateTimeUnit.Year:
                    if (value.Ticks % DateTimeTicks.Year == 0)
                        return true;
                    break;
                case DateTimeUnit.Month:
                    if (value.Day == 0)
                        goto case DateTimeUnit.Day;
                    break;
                case DateTimeUnit.Day:
                    if (value.Ticks % DateTimeTicks.Day == 0)
                        return true;
                    break;
                case DateTimeUnit.Hour:
                    if (value.Ticks % DateTimeTicks.Hour == 0)
                        return true;
                    break;
                case DateTimeUnit.Minute:
                    if (value.Ticks % DateTimeTicks.Minute == 0)
                        return true;
                    break;
                case DateTimeUnit.Second:
                    if (value.Ticks % DateTimeTicks.Second == 0)
                        return true;
                    break;
                case DateTimeUnit.Millisecond:
                    if (value.Ticks % DateTimeTicks.Millisecond == 0)
                        return true;
                    break;
                default: throw new NotImplementedException();
            }
            return false;
        }

        /// <summary>
        /// 转单位字符串，例如年月日
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUnitString(DateTimeUnit value)
        {
            return value switch
            {
                DateTimeUnit.Year => "年",
                DateTimeUnit.Month => "月",
                DateTimeUnit.Day => "日",
                DateTimeUnit.Hour => "时",
                DateTimeUnit.Minute => "分",
                DateTimeUnit.Second => "秒",
                DateTimeUnit.Millisecond => "毫秒",
                DateTimeUnit.Ticks => "刻",
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// 转时长字符串
        /// </summary>
        /// <param name="ticks">刻</param>
        /// <param name="setartLevel">开始单位</param>
        /// <param name="endLevel">结束单位</param>
        /// <returns></returns>
        public static string ToDurationstring(long ticks, DateTimeUnit setartLevel, DateTimeUnit endLevel)
        {
            if ((int)setartLevel < (int)endLevel)
                throw new ArgumentException("开始的单位小于结束的单位");
            if (ticks < 0)
                throw new ArgumentException("刻度数小于0");

            StringBuilder Result = new("");

            if ((int)setartLevel >= (int)DateTimeUnit.Year && (int)endLevel <= (int)DateTimeUnit.Year)
            {
                Result.Append(ticks / DateTimeTicks.Year + ToDurationString(DateTimeUnit.Year));
            }

            if ((int)setartLevel >= (int)DateTimeUnit.Day && (int)endLevel <= (int)DateTimeUnit.Day)
            {
                if (setartLevel == DateTimeUnit.Day)
                    Result.Append(ticks / DateTimeTicks.Day + ToDurationString(DateTimeUnit.Day));
                else Result.Append(ticks % DateTimeTicks.Year / DateTimeTicks.Day + ToDurationString(DateTimeUnit.Day));
            }

            if ((int)setartLevel >= (int)DateTimeUnit.Hour && (int)endLevel <= (int)DateTimeUnit.Hour)
            {
                if (setartLevel == DateTimeUnit.Hour)
                    Result.Append(ticks / DateTimeTicks.Hour + ToDurationString(DateTimeUnit.Hour));
                else Result.Append(ticks % DateTimeTicks.Day / DateTimeTicks.Hour + ToDurationString(DateTimeUnit.Hour));
            }

            if ((int)setartLevel >= (int)DateTimeUnit.Minute && (int)endLevel <= (int)DateTimeUnit.Minute)
            {
                if (setartLevel == DateTimeUnit.Minute)
                    Result.Append(ticks / DateTimeTicks.Minute + ToDurationString(DateTimeUnit.Minute));
                else Result.Append(ticks % DateTimeTicks.Hour / DateTimeTicks.Minute + ToDurationString(DateTimeUnit.Minute));
            }

            if ((int)setartLevel >= (int)DateTimeUnit.Second && (int)endLevel <= (int)DateTimeUnit.Second)
            {
                if (setartLevel == DateTimeUnit.Second)
                    Result.Append(ticks / DateTimeTicks.Second + ToDurationString(DateTimeUnit.Second));
                else Result.Append(ticks % DateTimeTicks.Minute / DateTimeTicks.Second + ToDurationString(DateTimeUnit.Second));
            }

            if ((int)setartLevel >= (int)DateTimeUnit.Millisecond && (int)endLevel <= (int)DateTimeUnit.Millisecond)
            {
                if (setartLevel == DateTimeUnit.Millisecond)
                    Result.Append(ticks / DateTimeTicks.Millisecond + ToDurationString(DateTimeUnit.Millisecond));
                else Result.Append(ticks % DateTimeTicks.Second / DateTimeTicks.Millisecond + ToDurationString(DateTimeUnit.Millisecond));
            }

            if ((int)setartLevel >= (int)DateTimeUnit.Ticks && (int)endLevel <= (int)DateTimeUnit.Ticks)
            {
                if (setartLevel == DateTimeUnit.Ticks)
                    Result.Append(ticks / DateTimeTicks.Ticks + ToDurationString(DateTimeUnit.Ticks));
                else Result.Append(ticks % DateTimeTicks.Millisecond / DateTimeTicks.Ticks + ToDurationString(DateTimeUnit.Ticks));
            }

            return Result.ToString();
        }

        /// <summary>
        /// 转时长字符串，例如一个月、一天、一小时、一分钟、一秒钟
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDurationString(DateTimeUnit value)
        {
            return value switch
            {
                DateTimeUnit.Year => "年",
                DateTimeUnit.Month => "个月",
                DateTimeUnit.Day => "天",
                DateTimeUnit.Hour => "小时",
                DateTimeUnit.Minute => "分钟",
                DateTimeUnit.Second => "秒钟",
                DateTimeUnit.Millisecond => "毫秒",
                DateTimeUnit.Ticks => "刻",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
