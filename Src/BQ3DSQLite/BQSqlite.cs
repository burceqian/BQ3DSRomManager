using BQStructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQ3DSQLite
{
    public class BQSqlite
    {
        private static string DBFullName = Environment.CurrentDirectory + @"\DB\gameInfo.db";
        private const string TableRomInfo = "RomInfo";
        private const string TableRom3dsdbInfo = "Rom3dsdbInfo";
        private const string TableRomgamedb3dsInfo = "Romgamedb3dsInfo";
        private const string TableBQ3dsGameInfo = "BQ3dsGameInfo";

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
            SQLiteConnection lDBConnection = new SQLiteConnection(@"data source=""" + DBFullName + @""";Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10");
            try
            {
                lDBConnection.Open();

                CreateTables(lDBConnection);
            }
            catch (Exception ex)
            {
                return true;
            }
            finally
            {
                lDBConnection.Close();
            }
            return true;
        }

        private static bool CreateTables(SQLiteConnection sqlConnection)
        {
            string lRomInfo = "";
            lRomInfo += "CREATE TABLE " + TableRomInfo + "(";
            lRomInfo += "serial varchar(128),";
            lRomInfo += "capacity varchar(128),";
            lRomInfo += "title_ID varchar(128),";
            lRomInfo += "card_Type varchar(128),";
            lRomInfo += "card_ID varchar(128),";
            lRomInfo += "chip_ID varchar(128),";
            lRomInfo += "manufacturer varchar(128)";
            lRomInfo += ")";
            string l3dsdbInfo = "";
            l3dsdbInfo += "CREATE TABLE " + TableRom3dsdbInfo + "(";
            l3dsdbInfo += "serial varchar(128),";
            l3dsdbInfo += "id varchar(128),";
            l3dsdbInfo += "name varchar(128),";
            l3dsdbInfo += "publisher varchar(128),";
            l3dsdbInfo += "region varchar(128),";
            l3dsdbInfo += "languages varchar(128),";
            l3dsdbInfo += "[group] varchar(128),";
            l3dsdbInfo += "imagesize varchar(128),";
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
            lgamedb3dsInfo += "CREATE TABLE " + TableRomgamedb3dsInfo + "(";
            lgamedb3dsInfo += "serial varchar(128),";
            lgamedb3dsInfo += "id varchar(128),";
            lgamedb3dsInfo += "region varchar(128),";
            lgamedb3dsInfo += "type varchar(128),";
            lgamedb3dsInfo += "languages varchar(128),";
            lgamedb3dsInfo += "title_EN varchar(128),";
            lgamedb3dsInfo += "title_JA varchar(128),";
            lgamedb3dsInfo += "title_ZHTW varchar(128),";
            lgamedb3dsInfo += "developer varchar(128),";
            lgamedb3dsInfo += "publisher varchar(128),";
            lgamedb3dsInfo += "release_date varchar(128),";
            lgamedb3dsInfo += "genre varchar(128),";
            lgamedb3dsInfo += "rating varchar(128),";
            lgamedb3dsInfo += "players varchar(128),";
            lgamedb3dsInfo += "req_accessories varchar(128),";
            lgamedb3dsInfo += "accessories varchar(128),";
            lgamedb3dsInfo += "online_players varchar(128),";
            lgamedb3dsInfo += "save_blocks varchar(128),";
            lgamedb3dsInfo += "[case] varchar(128)";
            lgamedb3dsInfo += ")";

            string lBQ3dsGameInfo = "";
            lBQ3dsGameInfo += "CREATE TABLE " + TableBQ3dsGameInfo +  "(";
            lBQ3dsGameInfo += "serial varchar(8),"; //CTR-AG5W
            lBQ3dsGameInfo += "languages varchar(128),"; //EN
            lBQ3dsGameInfo += "title_EN varchar(128),"; //Ice Age - Continental Drift - Arctic Games
            lBQ3dsGameInfo += "title_JA varchar(128),"; 
            lBQ3dsGameInfo += "title_ZHTW varchar(128),";
            lBQ3dsGameInfo += "title_ZHCN varchar(128),";
            lBQ3dsGameInfo += "developer varchar(128),"; //
            lBQ3dsGameInfo += "publisher varchar(128),"; // Nintendo
            lBQ3dsGameInfo += "release_date varchar(32),"; // 2011
            lBQ3dsGameInfo += "genre varchar(128),";
            lBQ3dsGameInfo += "players varchar(128),"; // 1
            lBQ3dsGameInfo += "imagesize varchar(128),"; //16384
            lBQ3dsGameInfo += "firmware varchar(128),"; //5.1.0K
            lBQ3dsGameInfo += "has3ds varchar(128),";
            lBQ3dsGameInfo += "hasCIA varchar(128),";
            lBQ3dsGameInfo += "has3dz varchar(128),";
            lBQ3dsGameInfo += "card varchar(128),";
            lBQ3dsGameInfo += "copyserial varchar(128),";
            lBQ3dsGameInfo += "favorite bool";
            lBQ3dsGameInfo += ")";

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

        public static bool InsertRom3dsdbInfo(Rom3dsdbInfo pRom3dsdbInfo)
        {
            try
            {
                SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + DBFullName);
                lDBConnection.Open();

                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = lDBConnection;
                string sql = "INSERT INTO " + TableRom3dsdbInfo + "(";
                sql += "id,";
                sql += "name,";
                sql += "publisher,";
                sql += "region,";
                sql += "languages,";
                sql += "[group],";
                sql += "imagesize,";
                sql += "serial,";
                sql += "titleid,";
                sql += "imgcrc,";
                sql += "filename,";
                sql += "releasename,";
                sql += "trimmedsize,";
                sql += "firmware,";
                sql += "type,";
                sql += "card";
                sql += ")values(";
                sql += "@id,";
                sql += "@name,";
                sql += "@publisher,";
                sql += "@region,";
                sql += "@languages,";
                sql += "@group,";
                sql += "@imagesize,";
                sql += "@serial,";
                sql += "@titleid,";
                sql += "@imgcrc,";
                sql += "@filename,";
                sql += "@releasename,";
                sql += "@trimmedsize,";
                sql += "@firmware,";
                sql += "@type,";
                sql += "@card";
                sql += ")";

                cmd.CommandText = sql;

                cmd.Parameters.Add("@id",DbType.String);
                cmd.Parameters.Add("@name", DbType.String);
                cmd.Parameters.Add("@publisher", DbType.String);
                cmd.Parameters.Add("@region", DbType.String);
                cmd.Parameters.Add("@languages", DbType.String);
                cmd.Parameters.Add("@group", DbType.String);
                cmd.Parameters.Add("@imagesize", DbType.String);
                cmd.Parameters.Add("@serial", DbType.String);
                cmd.Parameters.Add("@titleid", DbType.String);
                cmd.Parameters.Add("@imgcrc", DbType.String);
                cmd.Parameters.Add("@filename", DbType.String);
                cmd.Parameters.Add("@releasename", DbType.String);
                cmd.Parameters.Add("@trimmedsize", DbType.String);
                cmd.Parameters.Add("@firmware", DbType.String);
                cmd.Parameters.Add("@type", DbType.String);
                cmd.Parameters.Add("@card", DbType.String);



                cmd.Parameters["@id"].Value = pRom3dsdbInfo.id;
                cmd.Parameters["@name"].Value = pRom3dsdbInfo.name;
                cmd.Parameters["@publisher"].Value = pRom3dsdbInfo.publisher;
                cmd.Parameters["@region"].Value = pRom3dsdbInfo.region;
                cmd.Parameters["@languages"].Value = pRom3dsdbInfo.languages;
                cmd.Parameters["@group"].Value = pRom3dsdbInfo.group;
                cmd.Parameters["@imagesize"].Value = pRom3dsdbInfo.imagesize;
                cmd.Parameters["@serial"].Value = pRom3dsdbInfo.serial;
                cmd.Parameters["@titleid"].Value = pRom3dsdbInfo.titleid;
                cmd.Parameters["@imgcrc"].Value = pRom3dsdbInfo.imgcrc;
                cmd.Parameters["@filename"].Value = pRom3dsdbInfo.filename;
                cmd.Parameters["@releasename"].Value = pRom3dsdbInfo.releasename;
                cmd.Parameters["@trimmedsize"].Value = pRom3dsdbInfo.trimmedsize;
                cmd.Parameters["@firmware"].Value = pRom3dsdbInfo.firmware;
                cmd.Parameters["@type"].Value = pRom3dsdbInfo.type;
                cmd.Parameters["@card"].Value = pRom3dsdbInfo.card;

 
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static bool InsertRom3dsdbInfo(List<Rom3dsdbInfo> pRom3dsdbInfo)
        {
            SQLiteTransaction trans = null;
            SQLiteCommand cmd = null;
            try
            {
                SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + DBFullName);
                lDBConnection.Open();

                trans = lDBConnection.BeginTransaction();

                cmd = new SQLiteCommand();
                cmd.Connection = lDBConnection;
                cmd.Transaction = trans;

                cmd.Parameters.Add("@id", DbType.String);
                cmd.Parameters.Add("@name", DbType.String);
                cmd.Parameters.Add("@publisher", DbType.String);
                cmd.Parameters.Add("@region", DbType.String);
                cmd.Parameters.Add("@languages", DbType.String);
                cmd.Parameters.Add("@group", DbType.String);
                cmd.Parameters.Add("@imagesize", DbType.String);
                cmd.Parameters.Add("@serial", DbType.String);
                cmd.Parameters.Add("@titleid", DbType.String);
                cmd.Parameters.Add("@imgcrc", DbType.String);
                cmd.Parameters.Add("@filename", DbType.String);
                cmd.Parameters.Add("@releasename", DbType.String);
                cmd.Parameters.Add("@trimmedsize", DbType.String);
                cmd.Parameters.Add("@firmware", DbType.String);
                cmd.Parameters.Add("@type", DbType.String);
                cmd.Parameters.Add("@card", DbType.String);

                string sql = "";

                sql = "INSERT INTO " + TableRom3dsdbInfo +  "(";
                sql += "id,";
                sql += "name,";
                sql += "publisher,";
                sql += "region,";
                sql += "languages,";
                sql += "[group],";
                sql += "imagesize,";
                sql += "serial,";
                sql += "titleid,";
                sql += "imgcrc,";
                sql += "filename,";
                sql += "releasename,";
                sql += "trimmedsize,";
                sql += "firmware,";
                sql += "type,";
                sql += "card";
                sql += ")values(";
                sql += "@id,";
                sql += "@name,";
                sql += "@publisher,";
                sql += "@region,";
                sql += "@languages,";
                sql += "@group,";
                sql += "@imagesize,";
                sql += "@serial,";
                sql += "@titleid,";
                sql += "@imgcrc,";
                sql += "@filename,";
                sql += "@releasename,";
                sql += "@trimmedsize,";
                sql += "@firmware,";
                sql += "@type,";
                sql += "@card";
                sql += ")";

                cmd.CommandText = sql;

                for (int i = 0; i < pRom3dsdbInfo.Count; i++)
                {
                    cmd.Parameters["@id"].Value = pRom3dsdbInfo[i].id;
                    cmd.Parameters["@name"].Value = pRom3dsdbInfo[i].name;
                    cmd.Parameters["@publisher"].Value = pRom3dsdbInfo[i].publisher;
                    cmd.Parameters["@region"].Value = pRom3dsdbInfo[i].region;
                    cmd.Parameters["@languages"].Value = pRom3dsdbInfo[i].languages;
                    cmd.Parameters["@group"].Value = pRom3dsdbInfo[i].group;
                    cmd.Parameters["@imagesize"].Value = pRom3dsdbInfo[i].imagesize;
                    cmd.Parameters["@serial"].Value = pRom3dsdbInfo[i].serial;
                    cmd.Parameters["@titleid"].Value = pRom3dsdbInfo[i].titleid;
                    cmd.Parameters["@imgcrc"].Value = pRom3dsdbInfo[i].imgcrc;
                    cmd.Parameters["@filename"].Value = pRom3dsdbInfo[i].filename;
                    cmd.Parameters["@releasename"].Value = pRom3dsdbInfo[i].releasename;
                    cmd.Parameters["@trimmedsize"].Value = pRom3dsdbInfo[i].trimmedsize;
                    cmd.Parameters["@firmware"].Value = pRom3dsdbInfo[i].firmware;
                    cmd.Parameters["@type"].Value = pRom3dsdbInfo[i].type;
                    cmd.Parameters["@card"].Value = pRom3dsdbInfo[i].card;
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            return true;
         }

        public static Rom3dsdbInfo GetRom3dsdbInfo(string serial)
        {
            SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + DBFullName);
            lDBConnection.Open();

            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = lDBConnection;
            cmd.CommandText = "SELECT * FROM Rom3dsdbInfo WHERE serial='" + serial + "'";

            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

            DataTable ds = new DataTable();
            da.Fill(ds);
            da.Dispose();
            cmd.Dispose();

            lDBConnection.Close();

            if (ds.Rows.Count == 0)
            {
                return null;
            }

            Rom3dsdbInfo lResult = new Rom3dsdbInfo();
            lResult.id = ds.Rows[0]["id"].ToString().Trim();
            lResult.name = ds.Rows[0]["name"].ToString().Trim();
            lResult.publisher = ds.Rows[0]["publisher"].ToString().Trim();
            lResult.region = ds.Rows[0]["region"].ToString().Trim();
            lResult.languages = ds.Rows[0]["languages"].ToString().Trim();
            lResult.group = ds.Rows[0]["group"].ToString().Trim();
            lResult.imagesize = ds.Rows[0]["imagesize"].ToString().Trim();
            lResult.serial = ds.Rows[0]["serial"].ToString().Trim();
            lResult.titleid = ds.Rows[0]["titleid"].ToString().Trim();
            lResult.imgcrc = ds.Rows[0]["imgcrc"].ToString().Trim();
            lResult.filename = ds.Rows[0]["filename"].ToString().Trim();
            lResult.releasename = ds.Rows[0]["releasename"].ToString().Trim();
            lResult.trimmedsize = ds.Rows[0]["trimmedsize"].ToString().Trim();
            lResult.firmware = ds.Rows[0]["firmware"].ToString().Trim();
            lResult.type = ds.Rows[0]["type"].ToString().Trim();
            lResult.card = ds.Rows[0]["card"].ToString().Trim();
            return lResult;
        }

        public static bool InsertRomgamedb3dsInfo(Romgamedb3dsInfo pRomgamedb3dsInfo)
        {
            try
            {
                SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + DBFullName);
                lDBConnection.Open();

                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = lDBConnection;
                string sql = "INSERT INTO " + TableRomgamedb3dsInfo + "(";
                sql += "serial,";
                sql += "id,";
                sql += "region,";
                sql += "type,";
                sql += "languages,";
                sql += "title_EN,";
                sql += "title_JA,";
                sql += "title_ZHTW,";
                sql += "developer,";
                sql += "publisher,";
                sql += "release_date,";
                sql += "genre,";
                sql += "rating,";
                sql += "players,";
                sql += "req_accessories,";
                sql += "accessories,";
                sql += "online_players,";
                sql += "save_blocks,";
                sql += "[case]";
                sql += ")values(";
                sql += "@serial,";
                sql += "@id,";
                sql += "@region,";
                sql += "@type,";
                sql += "@languages,";
                sql += "@title_EN,";
                sql += "@title_JA,";
                sql += "@title_ZHTW,";
                sql += "@developer,";
                sql += "@publisher,";
                sql += "@release_date,";
                sql += "@genre,";
                sql += "@rating,";
                sql += "@players,";
                sql += "@req_accessories,";
                sql += "@accessories,";
                sql += "@online_players,";
                sql += "@save_blocks,";
                sql += "@case";
                sql += ")";

                cmd.CommandText = sql;

                cmd.Parameters.Add("@serial", DbType.String);
                cmd.Parameters.Add("@id", DbType.String);
                cmd.Parameters.Add("@region", DbType.String);
                cmd.Parameters.Add("@type", DbType.String);
                cmd.Parameters.Add("@languages", DbType.String);
                cmd.Parameters.Add("@title_EN", DbType.String);
                cmd.Parameters.Add("@title_JA", DbType.String);
                cmd.Parameters.Add("@title_ZHTW", DbType.String);
                cmd.Parameters.Add("@developer", DbType.String);
                cmd.Parameters.Add("@publisher", DbType.String);
                cmd.Parameters.Add("@release_date", DbType.String);
                cmd.Parameters.Add("@genre", DbType.String);
                cmd.Parameters.Add("@rating", DbType.String);
                cmd.Parameters.Add("@players", DbType.String);
                cmd.Parameters.Add("@req_accessories", DbType.String);
                cmd.Parameters.Add("@accessories", DbType.String);
                cmd.Parameters.Add("@online_players", DbType.String);
                cmd.Parameters.Add("@save_blocks", DbType.String);
                cmd.Parameters.Add("@case", DbType.String);

                cmd.Parameters["@serial"].Value = pRomgamedb3dsInfo.serial;
                cmd.Parameters["@id"].Value = pRomgamedb3dsInfo.id==null?"": pRomgamedb3dsInfo.id;
                cmd.Parameters["@region"].Value = pRomgamedb3dsInfo.region == null ? "" : pRomgamedb3dsInfo.region;
                cmd.Parameters["@type"].Value = pRomgamedb3dsInfo.type == null ? "" : pRomgamedb3dsInfo.type;
                cmd.Parameters["@languages"].Value = pRomgamedb3dsInfo.languages == null ? "" : pRomgamedb3dsInfo.languages;
                cmd.Parameters["@title_EN"].Value = pRomgamedb3dsInfo.title_1EN_2 == null ? "" : pRomgamedb3dsInfo.title_1EN_2;
                cmd.Parameters["@title_JA"].Value = pRomgamedb3dsInfo.title_1JA_2 == null ? "" : pRomgamedb3dsInfo.title_1JA_2;
                cmd.Parameters["@title_ZHTW"].Value = pRomgamedb3dsInfo.title_1ZHTW_2 == null ? "" : pRomgamedb3dsInfo.title_1ZHTW_2;
                cmd.Parameters["@developer"].Value = pRomgamedb3dsInfo.developer == null ? "" : pRomgamedb3dsInfo.developer;
                cmd.Parameters["@publisher"].Value = pRomgamedb3dsInfo.publisher == null ? "" : pRomgamedb3dsInfo.publisher;
                cmd.Parameters["@release_date"].Value = pRomgamedb3dsInfo.release_3date == null ? "" : pRomgamedb3dsInfo.release_3date;
                cmd.Parameters["@genre"].Value = pRomgamedb3dsInfo.genre == null ? "" : pRomgamedb3dsInfo.genre;
                cmd.Parameters["@rating"].Value = pRomgamedb3dsInfo.rating == null ? "" : pRomgamedb3dsInfo.rating;
                cmd.Parameters["@players"].Value = pRomgamedb3dsInfo.players == null ? "" : pRomgamedb3dsInfo.players;
                cmd.Parameters["@req_accessories"].Value = pRomgamedb3dsInfo.req_4accessories == null ? "" : pRomgamedb3dsInfo.req_4accessories;
                cmd.Parameters["@accessories"].Value = pRomgamedb3dsInfo.accessories == null ? "" : pRomgamedb3dsInfo.accessories;
                cmd.Parameters["@online_players"].Value = pRomgamedb3dsInfo.online_3players == null ? "" : pRomgamedb3dsInfo.online_3players;
                cmd.Parameters["@save_blocks"].Value = pRomgamedb3dsInfo.save_3blocks == null ? "" : pRomgamedb3dsInfo.save_3blocks;
                cmd.Parameters["@case"].Value = pRomgamedb3dsInfo.__case == null ? "" : pRomgamedb3dsInfo.__case;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string xx = ex.ToString();
                //throw ex;
            }
            return true;
        }

        public static Romgamedb3dsInfo GetRomgamedb3dsInfo(string serial)
        {
            SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + DBFullName);
            lDBConnection.Open();

            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = lDBConnection;
            cmd.CommandText = "SELECT * FROM " + TableRomgamedb3dsInfo + " WHERE serial='" + serial + "'";

            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

            DataTable ds = new DataTable();
            da.Fill(ds);
            da.Dispose();
            cmd.Dispose();

            lDBConnection.Close();

            if (ds.Rows.Count == 0)
            {
                return null;
            }

            Romgamedb3dsInfo lResult = new Romgamedb3dsInfo();
            lResult.serial = ds.Rows[0]["serial"].ToString().Trim();
            lResult.id = ds.Rows[0]["id"].ToString().Trim();
            lResult.region = ds.Rows[0]["region"].ToString().Trim();
            lResult.type = ds.Rows[0]["type"].ToString().Trim();
            lResult.languages = ds.Rows[0]["languages"].ToString().Trim();
            lResult.title_1EN_2 = ds.Rows[0]["title_EN"].ToString().Trim();
            lResult.title_1JA_2 = ds.Rows[0]["title_JA"].ToString().Trim();
            lResult.title_1ZHTW_2 = ds.Rows[0]["title_ZHTW"].ToString().Trim();
            lResult.developer = ds.Rows[0]["developer"].ToString().Trim();
            lResult.publisher = ds.Rows[0]["publisher"].ToString().Trim();
            lResult.release_3date = ds.Rows[0]["release_date"].ToString().Trim();
            lResult.genre = ds.Rows[0]["genre"].ToString().Trim();
            lResult.rating = ds.Rows[0]["rating"].ToString().Trim();
            lResult.players = ds.Rows[0]["players"].ToString().Trim();
            lResult.req_4accessories = ds.Rows[0]["req_accessories"].ToString().Trim();
            lResult.accessories = ds.Rows[0]["accessories"].ToString().Trim();
            lResult.online_3players = ds.Rows[0]["online_players"].ToString().Trim();
            lResult.save_3blocks = ds.Rows[0]["save_blocks"].ToString().Trim();
            lResult.__case = ds.Rows[0]["case"].ToString().Trim();

            return lResult;
        }

        public static bool InsertRomInfo(RomInfo pRomInfo)
        {
            try
            {
                SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + DBFullName);
                lDBConnection.Open();

                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = lDBConnection;
                string sql = "INSERT INTO " + TableRomInfo + "(";
                sql += "serial,";
                sql += "capacity,";
                sql += "title_ID,";
                sql += "card_Type,";
                sql += "card_ID,";
                sql += "chip_ID,";
                sql += "manufacturer";
                sql += ")values(";
                sql += "@serial,";
                sql += "@capacity,";
                sql += "@title_ID,";
                sql += "@card_Type,";
                sql += "@card_ID,";
                sql += "@chip_ID,";
                sql += "@manufacturer";
                sql += ")";

                cmd.CommandText = sql;

                cmd.Parameters.Add("@serial", DbType.String);
                cmd.Parameters.Add("@capacity", DbType.String);
                cmd.Parameters.Add("@title_ID", DbType.String);
                cmd.Parameters.Add("@card_Type", DbType.String);
                cmd.Parameters.Add("@card_ID", DbType.String);
                cmd.Parameters.Add("@chip_ID", DbType.String);
                cmd.Parameters.Add("@manufacturer", DbType.String);

                cmd.Parameters["@serial"].Value = pRomInfo.Serial;
                cmd.Parameters["@capacity"].Value = pRomInfo.Capacity;
                cmd.Parameters["@title_ID"].Value = pRomInfo.Title_ID;
                cmd.Parameters["@card_Type"].Value = pRomInfo.Card_Type==CardType.Card1?"1":"2";
                cmd.Parameters["@card_ID"].Value = pRomInfo.Card_ID;
                cmd.Parameters["@chip_ID"].Value = pRomInfo.Chip_ID;
                cmd.Parameters["@manufacturer"].Value = pRomInfo.Manufacturer;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static RomInfo GetRomInfo(string serial)
        {
            SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + DBFullName);
            lDBConnection.Open();

            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = lDBConnection;
            cmd.CommandText = "SELECT * FROM " + TableRomInfo + " WHERE serial='" + serial + "'";

            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

            DataTable ds = new DataTable();
            da.Fill(ds);
            da.Dispose();
            cmd.Dispose();

            lDBConnection.Close();

            if (ds.Rows.Count == 0)
            {
                return null;
            }

            RomInfo lResult = new RomInfo();
            lResult.Serial = ds.Rows[0]["serial"].ToString().Trim();
            lResult.Capacity = ds.Rows[0]["capacity"].ToString().Trim();
            lResult.Title_ID = ds.Rows[0]["title_ID"].ToString().Trim();
            lResult.Card_Type = ds.Rows[0]["card_Type"].ToString().Trim()=="1"?CardType.Card1:CardType.Card2;
            lResult.Card_ID = ds.Rows[0]["card_ID"].ToString().Trim();
            lResult.Chip_ID = ds.Rows[0]["chip_ID"].ToString().Trim();
            lResult.Manufacturer = ds.Rows[0]["manufacturer"].ToString().Trim();


            return lResult;
        }

        public static void Delete3dsdb()
        {
            try
            {
                SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + DBFullName);
                lDBConnection.Open();

                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = lDBConnection;

                cmd.CommandText = "DELETE FROM " + TableRom3dsdbInfo;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ClearDB()
        {
            try
            {
                SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + DBFullName);
                lDBConnection.Open();

                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = lDBConnection;

                cmd.CommandText = "DELETE FROM " + TableRomInfo;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM " + TableRom3dsdbInfo;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM " + TableRomgamedb3dsInfo;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM " + TableBQ3dsGameInfo;
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
