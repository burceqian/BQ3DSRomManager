using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQ3DSCore
{
    public static class BQDirectory
    {
        const string ROM_CONVER = @"\Conver\";
        const string ROM_ROM = @"\Roms\";
        const string ROM_DB = @"\DB\";
        const string ROM_ICON = @"\ICO\";

        private static string _ConverDir;
        private static string _RomDir;
        private static string _DBDir;
        private static string _IcoDir;

        public static string ConverDir { get { return _ConverDir; } }
        public static string RomDir { get { return _RomDir; } }
        public static string DBDir { get { return _DBDir; } }
        public static string IcoDir { get { return _IcoDir; } }

        public static void Initilize()
        {
            _ConverDir = Path.Combine(Environment.CurrentDirectory, ROM_CONVER);
            _RomDir = Path.Combine(Environment.CurrentDirectory, ROM_ROM);
            _DBDir = Path.Combine(Environment.CurrentDirectory, ROM_DB);
            _IcoDir = Path.Combine(Environment.CurrentDirectory, ROM_ICON);
            if (Directory.Exists(_ConverDir))
            {
                Directory.CreateDirectory(_ConverDir);
            }
            if (Directory.Exists(_RomDir))
            {
                Directory.CreateDirectory(_RomDir);
            }
            if (Directory.Exists(_DBDir))
            {
                Directory.CreateDirectory(_DBDir);
            }
            if (Directory.Exists(_IcoDir))
            {
                Directory.CreateDirectory(_IcoDir);
            }
        }
    }
}
