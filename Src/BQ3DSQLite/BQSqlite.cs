using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQ3DSQLite
{
    public class BQSqlite
    {
        private static string DBFullName = Environment.CurrentDirectory + @"\GameDB\gameInfo.db";

        public static bool CheckDBExist()
        {
            if (File.Exists(DBFullName))
            {
                return true;
            }

            return false;
        }

        public static bool CreateNewDB()
        {
            FileInfo lfi = new FileInfo(DBFullName);
            if (Directory.Exists(lfi.DirectoryName)==false)
            {
                Directory.CreateDirectory(lfi.DirectoryName);
            }

            File.Create(lfi.FullName);

            SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + DBFullName);
            lDBConnection.Open();

            CreateTables(lDBConnection);

            return true;
        }

        private static bool CreateTables(SQLiteConnection sqlConnection)
        {
            string lRomInfo = "";
            lRomInfo += "CREATE TABLE RomInfo(";
            lRomInfo += "serial varchar(128),";
            lRomInfo += "capacity varchar(128),";
            lRomInfo += "title_ID varchar(128),";
            lRomInfo += "card_Type varchar(128),";
            lRomInfo += "card_ID varchar(128),";
            lRomInfo += "chip_ID varchar(128),";
            lRomInfo += "manufacturer varchar(128)";
            lRomInfo += ")";
            string l3dsdbInfo = "";
            l3dsdbInfo += "CREATE TABLE Rom3dsdbInfo(";
            l3dsdbInfo += "serial varchar(128),";
            l3dsdbInfo += "id varchar(128),";
            l3dsdbInfo += "name varchar(128),";
            l3dsdbInfo += "publisher varchar(128),";
            l3dsdbInfo += "region varchar(128),";
            l3dsdbInfo += "languages varchar(128),";
            l3dsdbInfo += "group varchar(128),";
            l3dsdbInfo += "imagesize varchar(128),";
            l3dsdbInfo += "serial varchar(128),";
            l3dsdbInfo += "titleid varchar(128),";
            l3dsdbInfo += "imgcrc varchar(128),";
            l3dsdbInfo += "filename varchar(128),";
            l3dsdbInfo += "releasename varchar(128),";
            l3dsdbInfo += "trimmedsize varchar(128),";
            l3dsdbInfo += "firmware varchar(128),";
            l3dsdbInfo += "type varchar(128),";
            l3dsdbInfo += "card varchar(128)";
            l3dsdbInfo += ")";

            string lgamedb3dsInfo = "";
            l3dsdbInfo += "CREATE TABLE Romgamedb3dsInfo(";
            l3dsdbInfo += "serial varchar(128),";
            l3dsdbInfo += "id varchar(128),";
            l3dsdbInfo += "region varchar(128),";
            l3dsdbInfo += "type varchar(128),";
            l3dsdbInfo += "languages varchar(128),";
            l3dsdbInfo += "title_EN varchar(128),";
            l3dsdbInfo += "title_JP varchar(128),";
            l3dsdbInfo += "title_ZHTW varchar(128),";
            l3dsdbInfo += "developer varchar(128),";
            l3dsdbInfo += "publisher varchar(128),";
            l3dsdbInfo += "release_date varchar(128),";
            l3dsdbInfo += "genre varchar(128),";
            l3dsdbInfo += "rating varchar(128),";
            l3dsdbInfo += "players varchar(128),";
            l3dsdbInfo += "req_accessories varchar(128),";
            l3dsdbInfo += "accessories varchar(128),";
            l3dsdbInfo += "online_players varchar(128),";
            l3dsdbInfo += "save_blocks varchar(128),";
            l3dsdbInfo += "online_players varchar(128),";
            l3dsdbInfo += "case varchar(128)";
            l3dsdbInfo += ")";

            string lBQ3dsGameInfo = "";
            l3dsdbInfo += "CREATE TABLE BQ3dsGameInfo(";
            l3dsdbInfo += "serial varchar(8),"; //CTR-AG5W
            l3dsdbInfo += "languages varchar(128),"; //EN
            l3dsdbInfo += "title_EN varchar(128),"; //Ice Age - Continental Drift - Arctic Games
            l3dsdbInfo += "title_JP varchar(128),"; 
            l3dsdbInfo += "title_ZHTW varchar(128),";
            l3dsdbInfo += "title_ZHCN varchar(128),";
            l3dsdbInfo += "developer varchar(128),"; //
            l3dsdbInfo += "publisher varchar(128),"; // Nintendo
            l3dsdbInfo += "release_date varchar(32),"; // 2011
            l3dsdbInfo += "genre varchar(128),";
            l3dsdbInfo += "players varchar(128),"; // 1
            l3dsdbInfo += "imagesize varchar(128),"; //16384
            l3dsdbInfo += "firmware varchar(128),"; //5.1.0K
            l3dsdbInfo += "has3ds varchar(128),";
            l3dsdbInfo += "hasCIA varchar(128),";
            l3dsdbInfo += "has3dz varchar(128),";
            l3dsdbInfo += "card varchar(128)";
            l3dsdbInfo += "copyserial varchar(128)";
            l3dsdbInfo += ")";

            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = sqlConnection;
            
            cmd.CommandText = lRomInfo;
            cmd.ExecuteNonQuery();

            cmd.CommandText = l3dsdbInfo;
            cmd.ExecuteNonQuery();

            cmd.CommandText = lgamedb3dsInfo;
            cmd.ExecuteNonQuery();

            cmd.CommandText = lBQ3dsGameInfo;
            cmd.ExecuteNonQuery();

            return true;
        }
    }
}
