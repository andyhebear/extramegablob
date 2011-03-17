using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace ExtraMegaBlob.References
{

    public class Vector3 : ISerializable
    {

        public float x, y, z;
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3()
        {
            this.x = 0f;
            this.y = 0f;
            this.z = 0f;
        }
        public Vector3(SerializationInfo info, StreamingContext ctxt)
        {
            x = (float)info.GetValue("x", typeof(float));
            y = (float)info.GetValue("y", typeof(float));
            z = (float)info.GetValue("z", typeof(float));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("x", x);
            info.AddValue("y", y);
            info.AddValue("z", z);
        }
        public Mogre.Vector3 toMogre
        {
            get
            {
                return new Mogre.Vector3(this.x, this.y, this.z);
            }
        }
        public static Vector3 FromMogre(Mogre.Vector3 loc)
        {
            return new Vector3(loc.x, loc.y, loc.z);
        }
        public static Vector3 FromString(String XmlString)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Vector3));
            MemoryStream memoryStream = new MemoryStream(Serialize.StringToUTF8ByteArray(XmlString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (Vector3)xs.Deserialize(memoryStream);
        }
        public override String ToString()
        {
            return Serialize.StaticallySerializeObject(this, typeof(Vector3));
        }
    }
    public static class Math
    {

        public static void floor(ref double x)
        {
            x = System.Math.Floor(x);
        }
        
        /// <summary>
        /// This is the Cartesian version of Pythagoras' theorem. In three-dimensional space,
        /// the distance between points (x1,y1,z1) and (x2,y2,z2) is
        /// http://upload.wikimedia.org/math/3/a/e/3ae1d79e0bfcc8f38223c7df4a7320c5.png
        /// which can be obtained by two consecutive applications of Pythagoras' theorem.
        /// http://en.wikipedia.org/wiki/Cartesian_coordinate_system#Distance_between_two_points
        /// the square root of (
        ///      ((point2.x - point1.x)squared) + 
        ///      ((point2.y - point1.y)squared) + 
        ///      ((point2.z - point1.z)squared)
        /// )
        /// </summary>
        public static float distanceBetweenPythagCartesian(Mogre.Vector3 point1, Mogre.Vector3 point2)
        {

            return Mogre.Math.Sqrt(
                (Mogre.Math.Sqr(point2.x - point1.x) +
                Mogre.Math.Sqr(point2.y - point1.y) +
                Mogre.Math.Sqr(point2.z - point1.z)
                ));
        }
        /// <summary>
        /// This is the Cartesian version of Pythagoras' theorem. In three-dimensional space,
        /// the distance between points (x1,y1,z1) and (x2,y2,z2) is
        /// http://upload.wikimedia.org/math/3/a/e/3ae1d79e0bfcc8f38223c7df4a7320c5.png
        /// which can be obtained by two consecutive applications of Pythagoras' theorem.
        /// http://en.wikipedia.org/wiki/Cartesian_coordinate_system#Distance_between_two_points
        /// the square root of (
        ///      ((point2.x - point1.x)squared) + 
        ///      ((point2.y - point1.y)squared) + 
        ///      ((point2.z - point1.z)squared)
        /// )
        /// </summary>
        public static float distanceBetweenPythagCartesian(References.Vector3 point1, References.Vector3 point2)
        {

            return Mogre.Math.Sqrt(
                (Mogre.Math.Sqr(point2.x - point1.x) +
                Mogre.Math.Sqr(point2.y - point1.y) +
                Mogre.Math.Sqr(point2.z - point1.z)
                ));
        }
        public static void clamp_lo(float low, ref float inval)
        {
            if (inval < low)
            {
                inval = low;
            }
            return;
        }
        public static void clamp_hi(float high, ref float inval)
        {
            if (inval > high)
            {
                inval = high;
            }
            return;
        }
        public static void clamp(float low, float high, ref float inval)
        {
            if (inval > high)
            {
                inval = high;
            }
            if (inval < low)
            {
                inval = low;
            }
            return;
        }
    }
}
