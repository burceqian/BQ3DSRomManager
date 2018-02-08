using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQUtility
{
    public static class BQDirectory
    {
        const string ROM_CONVER = @"Conver\";
        const string ROM_ROM = @"Roms\";
        const string ROM_DB = @"DB\";
        const string ROM_ICON = @"ICO\";
        const string ROM_LARGE_ICON = @"ICO\Large\";
        const string ROM_SMALL_ICON = @"ICO\Small\";
        const string ROM_TEMP = @"Temp\";

        private static string _ConverDir;
        private static string _RomDir;
        private static string _DBDir;
        private static string _IcoDir;
        private static string _IcoLargeDir;
        private static string _IcoSmallDir;
        private static string _TempDir;

        public static string ConverDir { get { return _ConverDir; } }
        public static string RomDir { get { return _RomDir; } }
        public static string DBDir { get { return _DBDir; } }
        public static string IcoDir { get { return _IcoDir; } }
        public static string IcoLargeDir { get { return _IcoLargeDir; } }
        public static string IcoSmallDir { get { return _IcoSmallDir; } }
        public static string TempDir { get { return _TempDir; } }

        public static void Initilize()
        {
            _ConverDir = Path.Combine(Environment.CurrentDirectory, ROM_CONVER);
            _RomDir = Path.Combine(Environment.CurrentDirectory, ROM_ROM);
            _DBDir = Path.Combine(Environment.CurrentDirectory, ROM_DB);
            _IcoDir = Path.Combine(Environment.CurrentDirectory, ROM_ICON);
            _IcoLargeDir = Path.Combine(Environment.CurrentDirectory, ROM_LARGE_ICON);
            _IcoSmallDir = Path.Combine(Environment.CurrentDirectory, ROM_SMALL_ICON);
            _TempDir = Path.Combine(Environment.CurrentDirectory, ROM_TEMP);
            if (!Directory.Exists(_ConverDir))
            {
                Directory.CreateDirectory(_ConverDir);
            }
            if (!Directory.Exists(_RomDir))
            {
                Directory.CreateDirectory(_RomDir);
            }
            if (!Directory.Exists(_DBDir))
            {
                Directory.CreateDirectory(_DBDir);
            }
            if (!Directory.Exists(_IcoDir))
            {
                Directory.CreateDirectory(_IcoDir);
            }
            if (!Directory.Exists(_IcoLargeDir))
            {
                Directory.CreateDirectory(_IcoLargeDir);
            }
            if (!Directory.Exists(_IcoSmallDir))
            {
                Directory.CreateDirectory(_IcoSmallDir);
            }
            if (!Directory.Exists(_TempDir))
            {
                Directory.CreateDirectory(_TempDir);
            }
        }
    }
}
