using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQRomParsers
{
    public static class EnumExtensions
    {
        public static byte ByteValue(this Enum src)
        {
            return Convert.ToByte(src);
        }

        public static int IntValue(this Enum src)
        {
            return Convert.ToInt32(src);
        }

        public static TTarget ToEnum<TSource, TTarget>(this TSource src)
        {
            return (TTarget)Enum.ToObject(typeof(TTarget), src);
        }
    }
}
