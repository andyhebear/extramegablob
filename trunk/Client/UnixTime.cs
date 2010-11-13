using System;
using System.Collections.Generic;
using System.Text;

namespace thing.Parts
{
	public static class UnixTime
	{
		public static Double ToUnix(DateTime value)
		{
			DateTime d = Convert.ToDateTime("1970-01-01T00:00:00Z");
			d = d.ToUniversalTime();
			value = value.ToUniversalTime();
			return ( ( Double ) ( ( Double ) value.Ticks - ( Double ) d.Ticks ) / ( Double ) 10000000 );
		}
		public static DateTime FromUnix(Double TimeStamp , System.TimeZone Zone)
		{
			DateTime d = Convert.ToDateTime("1970-01-01T00:00:00Z");
			TimeSpan t = new TimeSpan(( long ) ( ( Double ) TimeStamp * ( Double ) 10000000 ));
			d = d.ToUniversalTime();
			d = d.Add(t);
			return Zone.ToLocalTime(d);
		}
		public static void test()
		{
			DateTime now = DateTime.Now;
			Double foo = ToUnix(now);
			DateTime bar = FromUnix(foo , System.TimeZone.CurrentTimeZone);
			//bar = bar.ToUniversalTime();
			System.Windows.Forms.MessageBox.Show("Unix Timestamp With Precision: " + foo.ToString());
			System.Windows.Forms.MessageBox.Show("DateTime from Precise Timestamp:: " + bar.ToString());
			//System.Windows.Forms.MessageBox.Show("Difference:: " + bar.CompareTo(bar).ToString());
		}
		public static Double PreciseEpochStampZulu
		{
			get
			{
                lock (typeof(Double))
                {
                    return ToUnix(DateTime.Now);
                }
			}
		}
		public static Int32 PreciseEpochStampZuluFraction
		{
			get
			{
				return GetFraction(ToUnix(DateTime.Now));
			}
		}
		private static Int32 GetFraction(Double d)
		{
			Decimal D = System.Math.Truncate(Convert.ToDecimal(d) * 10000000);
			
			return Convert.ToInt32(System.Math.Min(D,2147483647));
		}
	}
}
