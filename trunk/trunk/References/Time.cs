using System;
using System.Text;

namespace ThingReferences
{
    public class timer
    {
        private DateTime startedat = DateTime.Now;
        private bool finished = true;
        private bool _reset = false;
        public TimeSpan interval;
        public void start()
        {
            startedat = DateTime.Now;
            finished = false;
            _reset = false;
        }
        public timer(TimeSpan interval)
        {
            this.interval = interval;
        }
        public bool elapsed
        {
            get
            {
                if (_reset) return false;
                if (finished) return true;
                else
                {
                    bool ret = (startedat.Add(interval).CompareTo(DateTime.Now) < 0) ? true : false;
                    if (ret) { finished = ret; }
                    return ret;
                }
            }
        }
        public void reset()
        {
            _reset = true;
        }
    }
    public static class Time
    {
        public static Double ToUnix(DateTime value)
        {
            DateTime d = Convert.ToDateTime("1970-01-01T00:00:00Z");
            d = d.ToUniversalTime();
            value = value.ToUniversalTime();
            return ((Double)((Double)value.Ticks - (Double)d.Ticks) / (Double)10000000);
        }
        public static DateTime FromUnix(Double TimeStamp, System.TimeZone Zone)
        {
            DateTime d = Convert.ToDateTime("1970-01-01T00:00:00Z");
            TimeSpan t = new TimeSpan((long)((Double)TimeStamp * (Double)10000000));
            d = d.ToUniversalTime();
            d = d.Add(t);
            return Zone.ToLocalTime(d);
        }
        public static void test()
        {
            DateTime now = DateTime.Now;
            Double foo = ToUnix(now);
            DateTime bar = FromUnix(foo, System.TimeZone.CurrentTimeZone);
            //bar = bar.ToUniversalTime();
            System.Windows.Forms.MessageBox.Show("Unix Timestamp With Precision: " + foo.ToString());
            System.Windows.Forms.MessageBox.Show("DateTime from Precise Timestamp:: " + bar.ToString());
            //System.Windows.Forms.MessageBox.Show("Difference:: " + bar.CompareTo(bar).ToString());
        }
        public static Double Now
        {
            get
            {
                return double.Parse(NowString);
            }
        }
        public static string NowString
        {
            get
            {
                DateTime x = DateTime.Now;
                double y = ToUnix(x);
                string k = x.ToString("0.fffffff");
                double h = double.Parse(k);
                return (string.Format(Config.NowTimeFormatSeparate, y, h));
            }
        }
        public static string NowStringFromDouble(double now)
        {
            double topd = now;
            Math.floor(ref topd);
            double botd = (now - topd);
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(Config.NowTimeFormatTop, topd));
            sb.Append(string.Format(Config.NowTimeFormatBottom, botd));
            return sb.ToString();
        }
    }
}
