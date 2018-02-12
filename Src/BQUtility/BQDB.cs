using BQStructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQUtility
{
    public class BQDB
    {
        private static string _DBFileFullName = Path.Combine(BQDirectory.DBDir, "game.db");
        private const string _TableName = "GAME_INFO";

        public static bool CreateNewDB()
        {
            FileInfo lfi = new FileInfo(_DBFileFullName);
            if (Directory.Exists(lfi.DirectoryName) == false)
            {
                Directory.CreateDirectory(lfi.DirectoryName);
            }
            //if (!lfi.Exists)
            //{
            //    var lt = lfi.Create();
            //    lt.Close();
            //}

            SQLiteConnection lDBConnection = new SQLiteConnection(@"data source=""" + _DBFileFullName + @""";Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10");
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
            //Serial
            //Capacity
            //Region
            //Region_Lockout
            //Languages
            //Title_ID
            //Game_Title
            //English_Title
            //Japanese_Title
            //French_Title
            //German_Title
            //Italian_Title
            //Spanish_Title
            //Simplified_Chinese_Title
            //Korean_Title
            //Dutch_Title
            //Portuguese_Title
            //Russian_Title
            //Traditional_Chinese_Title
            //Platform
            //Card_Type
            //Card_ID
            //Chip_ID
            //Manufacturer
            //OriginalName
            //Developer
            //Publisher
            //ReleaseDate
            //Genre
            //Rating
            //Players
            //Imagesize
            //Firmware
            //Favorite
            //IsCustomsizeRom
            //SourceSerial


            lRomInfo += "CREATE TABLE " + _TableName + "(";
            lRomInfo += "Serial varchar(10),";
            lRomInfo += "Capacity varchar(128),";
            lRomInfo += "Region varchar(128),";
            lRomInfo += "Region_Lockout varchar(128),";
            lRomInfo += "Languages varchar(128),";
            lRomInfo += "Title_ID varchar(128),";
            lRomInfo += "Game_Title varchar(128),";
            lRomInfo += "English_Title varchar(128),";
            lRomInfo += "Japanese_Title varchar(128),";
            lRomInfo += "French_Title varchar(128),";
            lRomInfo += "German_Title varchar(128),";
            lRomInfo += "Italian_Title varchar(128),";
            lRomInfo += "Spanish_Title varchar(128),";
            lRomInfo += "Simplified_Chinese_Title varchar(128),";
            lRomInfo += "Korean_Title varchar(128),";
            lRomInfo += "Dutch_Title varchar(128),";
            lRomInfo += "Portuguese_Title varchar(128),";
            lRomInfo += "Russian_Title varchar(128),";
            lRomInfo += "Traditional_Chinese_Title varchar(128),";
            lRomInfo += "Platform varchar(128),";
            lRomInfo += "Card_Type varchar(128),";
            lRomInfo += "Card_ID varchar(128),";
            lRomInfo += "Chip_ID varchar(128),";
            lRomInfo += "Manufacturer varchar(128),";
            lRomInfo += "OriginalName varchar(128),";
            lRomInfo += "Developer varchar(128),";
            lRomInfo += "Publisher varchar(128),";
            lRomInfo += "ReleaseDate varchar(128),";
            lRomInfo += "Genre varchar(128),";
            lRomInfo += "Rating varchar(128),";
            lRomInfo += "Players varchar(128),";
            lRomInfo += "Imagesize varchar(128),";
            lRomInfo += "Firmware varchar(128),";
            lRomInfo += "Favorite varchar(128),"; 
            lRomInfo += "IsCustomsizeRom varchar(128),";
            lRomInfo += "SourceSerial varchar(10)";
            lRomInfo += ")";

            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = sqlConnection;

            cmd.CommandText = lRomInfo;
            cmd.ExecuteNonQuery();

            return true;
        }

        public static bool InsertRomInfo(RomBasicInfo pRomInfo)
        {
            try
            {
                SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + _DBFileFullName);
                lDBConnection.Open();

                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = lDBConnection;
                string sql = "INSERT INTO " + _TableName + "(";
                sql += "Serial ,";
                sql += "Capacity ,";
                sql += "Region ,";
                sql += "Region_Lockout ,";
                sql += "Languages ,";
                sql += "Title_ID ,";
                sql += "Game_Title ,";
                sql += "English_Title ,";
                sql += "Japanese_Title ,";
                sql += "French_Title ,";
                sql += "German_Title ,";
                sql += "Italian_Title ,";
                sql += "Spanish_Title ,";
                sql += "Simplified_Chinese_Title ,";
                sql += "Korean_Title ,";
                sql += "Dutch_Title ,";
                sql += "Portuguese_Title ,";
                sql += "Russian_Title ,";
                sql += "Traditional_Chinese_Title ,";
                sql += "Platform ,";
                sql += "Card_Type ,";
                sql += "Card_ID ,";
                sql += "Chip_ID ,";
                sql += "Manufacturer ,";
                sql += "OriginalName ,";
                sql += "Developer ,";
                sql += "Publisher ,";
                sql += "ReleaseDate ,";
                sql += "Genre ,";
                sql += "Rating ,";
                sql += "Players ,";
                sql += "Imagesize ,";
                sql += "Firmware ,";
                sql += "Favorite, ";
                sql += "IsCustomsizeRom ,";
                sql += "SourceSerial ";
                sql += ")values(";
                sql += "@Serial ,";
                sql += "@Capacity ,";
                sql += "@Region ,";
                sql += "@Region_Lockout ,";
                sql += "@Languages ,";
                sql += "@Title_ID ,";
                sql += "@Game_Title ,";
                sql += "@English_Title ,";
                sql += "@Japanese_Title ,";
                sql += "@French_Title ,";
                sql += "@German_Title ,";
                sql += "@Italian_Title ,";
                sql += "@Spanish_Title ,";
                sql += "@Simplified_Chinese_Title ,";
                sql += "@Korean_Title ,";
                sql += "@Dutch_Title ,";
                sql += "@Portuguese_Title ,";
                sql += "@Russian_Title ,";
                sql += "@Traditional_Chinese_Title ,";
                sql += "@Platform ,";
                sql += "@Card_Type ,";
                sql += "@Card_ID ,";
                sql += "@Chip_ID ,";
                sql += "@Manufacturer ,";
                sql += "@OriginalName ,";
                sql += "@Developer ,";
                sql += "@Publisher ,";
                sql += "@ReleaseDate ,";
                sql += "@Genre ,";
                sql += "@Rating ,";
                sql += "@Players ,";
                sql += "@Imagesize ,";
                sql += "@Firmware ,";
                sql += "@Favorite ,";
                sql += "@IsCustomsizeRom ,";
                sql += "@SourceSerial ";
                sql += ")";

                cmd.CommandText = sql;

                cmd.Parameters.Add("@Serial", DbType.String);
                cmd.Parameters.Add("@Capacity", DbType.String);
                cmd.Parameters.Add("@Region", DbType.String);
                cmd.Parameters.Add("@Region_Lockout", DbType.String);
                cmd.Parameters.Add("@Languages", DbType.String);
                cmd.Parameters.Add("@Title_ID", DbType.String);
                cmd.Parameters.Add("@Game_Title", DbType.String);
                cmd.Parameters.Add("@English_Title", DbType.String);
                cmd.Parameters.Add("@Japanese_Title", DbType.String);
                cmd.Parameters.Add("@French_Title", DbType.String);
                cmd.Parameters.Add("@German_Title", DbType.String);
                cmd.Parameters.Add("@Italian_Title", DbType.String);
                cmd.Parameters.Add("@Spanish_Title", DbType.String);
                cmd.Parameters.Add("@Simplified_Chinese_Title", DbType.String);
                cmd.Parameters.Add("@Korean_Title", DbType.String);
                cmd.Parameters.Add("@Dutch_Title", DbType.String);
                cmd.Parameters.Add("@Portuguese_Title", DbType.String);
                cmd.Parameters.Add("@Russian_Title", DbType.String);
                cmd.Parameters.Add("@Traditional_Chinese_Title", DbType.String);
                cmd.Parameters.Add("@Platform", DbType.String);
                cmd.Parameters.Add("@Card_Type", DbType.String);
                cmd.Parameters.Add("@Card_ID", DbType.String);
                cmd.Parameters.Add("@Chip_ID", DbType.String);
                cmd.Parameters.Add("@Manufacturer", DbType.String);
                cmd.Parameters.Add("@OriginalName", DbType.String);
                cmd.Parameters.Add("@Developer", DbType.String);
                cmd.Parameters.Add("@Publisher", DbType.String);
                cmd.Parameters.Add("@ReleaseDate", DbType.String);
                cmd.Parameters.Add("@Genre", DbType.String);
                cmd.Parameters.Add("@Rating", DbType.String);
                cmd.Parameters.Add("@Players", DbType.String);
                cmd.Parameters.Add("@Imagesize", DbType.String);
                cmd.Parameters.Add("@Firmware", DbType.String);
                cmd.Parameters.Add("@Favorite", DbType.String);
                cmd.Parameters.Add("@IsCustomsizeRom", DbType.String);
                cmd.Parameters.Add("@SourceSerial", DbType.String);

                cmd.Parameters["@Serial"].Value = pRomInfo.Serial;
                cmd.Parameters["@Capacity"].Value = pRomInfo.Capacity;
                cmd.Parameters["@Region"].Value = pRomInfo.Region;
                cmd.Parameters["@Region_Lockout"].Value = pRomInfo.Region_Lockout;
                cmd.Parameters["@Languages"].Value = pRomInfo.Languages;
                cmd.Parameters["@Title_ID"].Value = pRomInfo.Title_ID;
                cmd.Parameters["@Game_Title"].Value = pRomInfo.Game_Title;
                cmd.Parameters["@English_Title"].Value = pRomInfo.English_Title;
                cmd.Parameters["@Japanese_Title"].Value = pRomInfo.Japanese_Title;
                cmd.Parameters["@French_Title"].Value = pRomInfo.French_Title;
                cmd.Parameters["@German_Title"].Value = pRomInfo.German_Title;
                cmd.Parameters["@Italian_Title"].Value = pRomInfo.Italian_Title;
                cmd.Parameters["@Spanish_Title"].Value = pRomInfo.Spanish_Title;
                cmd.Parameters["@Simplified_Chinese_Title"].Value = pRomInfo.Simplified_Chinese_Title;
                cmd.Parameters["@Korean_Title"].Value = pRomInfo.Korean_Title;
                cmd.Parameters["@Dutch_Title"].Value = pRomInfo.Dutch_Title;
                cmd.Parameters["@Portuguese_Title"].Value = pRomInfo.Portuguese_Title;
                cmd.Parameters["@Russian_Title"].Value = pRomInfo.Russian_Title;
                cmd.Parameters["@Traditional_Chinese_Title"].Value = pRomInfo.Traditional_Chinese_Title;
                cmd.Parameters["@Platform"].Value = pRomInfo.Platform;
                cmd.Parameters["@Card_Type"].Value = pRomInfo.Card_Type;
                cmd.Parameters["@Card_ID"].Value = pRomInfo.Card_ID;
                cmd.Parameters["@Chip_ID"].Value = pRomInfo.Chip_ID;
                cmd.Parameters["@Manufacturer"].Value = pRomInfo.Manufacturer;
                cmd.Parameters["@OriginalName"].Value = pRomInfo.OriginalName;
                cmd.Parameters["@Developer"].Value = pRomInfo.Developer;
                cmd.Parameters["@Publisher"].Value = pRomInfo.Publisher;
                cmd.Parameters["@ReleaseDate"].Value = pRomInfo.ReleaseDate;
                cmd.Parameters["@Genre"].Value = pRomInfo.Genre;
                cmd.Parameters["@Rating"].Value = pRomInfo.Rating;
                cmd.Parameters["@Players"].Value = pRomInfo.Players;
                cmd.Parameters["@Imagesize"].Value = pRomInfo.Imagesize;
                cmd.Parameters["@Firmware"].Value = pRomInfo.Firmware;
                cmd.Parameters["@Favorite"].Value = pRomInfo.Favorite;
                cmd.Parameters["@IsCustomsizeRom"].Value = pRomInfo.IsCustomsizeRom;
                cmd.Parameters["@SourceSerial"].Value = pRomInfo.SourceSerial;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static bool InsertRomInfoList(List<RomInformation> pRomInfoList)
        {
            SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + _DBFileFullName);
            SQLiteCommand cmd = new SQLiteCommand();
            lDBConnection.Open();

            SQLiteTransaction lTransaction = lDBConnection.BeginTransaction();
            cmd.Connection = lDBConnection;

            try
            {

                string sql = "INSERT INTO " + _TableName + "(";
                sql += "Serial ,";
                sql += "Capacity ,";
                sql += "Region ,";
                sql += "Region_Lockout ,";
                sql += "Languages ,";
                sql += "Title_ID ,";
                sql += "Game_Title ,";
                sql += "English_Title ,";
                sql += "Japanese_Title ,";
                sql += "French_Title ,";
                sql += "German_Title ,";
                sql += "Italian_Title ,";
                sql += "Spanish_Title ,";
                sql += "Simplified_Chinese_Title ,";
                sql += "Korean_Title ,";
                sql += "Dutch_Title ,";
                sql += "Portuguese_Title ,";
                sql += "Russian_Title ,";
                sql += "Traditional_Chinese_Title ,";
                sql += "Platform ,";
                sql += "Card_Type ,";
                sql += "Card_ID ,";
                sql += "Chip_ID ,";
                sql += "Manufacturer ,";
                sql += "OriginalName ,";
                sql += "Developer ,";
                sql += "Publisher ,";
                sql += "ReleaseDate ,";
                sql += "Genre ,";
                sql += "Rating ,";
                sql += "Players ,";
                sql += "Imagesize ,";
                sql += "Firmware ,";
                sql += "Favorite, ";
                sql += "IsCustomsizeRom ,";
                sql += "SourceSerial ";
                sql += ")values(";
                sql += "@Serial ,";
                sql += "@Capacity ,";
                sql += "@Region ,";
                sql += "@Region_Lockout ,";
                sql += "@Languages ,";
                sql += "@Title_ID ,";
                sql += "@Game_Title ,";
                sql += "@English_Title ,";
                sql += "@Japanese_Title ,";
                sql += "@French_Title ,";
                sql += "@German_Title ,";
                sql += "@Italian_Title ,";
                sql += "@Spanish_Title ,";
                sql += "@Simplified_Chinese_Title ,";
                sql += "@Korean_Title ,";
                sql += "@Dutch_Title ,";
                sql += "@Portuguese_Title ,";
                sql += "@Russian_Title ,";
                sql += "@Traditional_Chinese_Title ,";
                sql += "@Platform ,";
                sql += "@Card_Type ,";
                sql += "@Card_ID ,";
                sql += "@Chip_ID ,";
                sql += "@Manufacturer ,";
                sql += "@OriginalName ,";
                sql += "@Developer ,";
                sql += "@Publisher ,";
                sql += "@ReleaseDate ,";
                sql += "@Genre ,";
                sql += "@Rating ,";
                sql += "@Players ,";
                sql += "@Imagesize ,";
                sql += "@Firmware ,";
                sql += "@Favorite ,";
                sql += "@IsCustomsizeRom ,";
                sql += "@SourceSerial ";
                sql += ")";

                cmd.CommandText = sql;

                cmd.Parameters.Add("@Serial", DbType.String);
                cmd.Parameters.Add("@Capacity", DbType.String);
                cmd.Parameters.Add("@Region", DbType.String);
                cmd.Parameters.Add("@Region_Lockout", DbType.String);
                cmd.Parameters.Add("@Languages", DbType.String);
                cmd.Parameters.Add("@Title_ID", DbType.String);
                cmd.Parameters.Add("@Game_Title", DbType.String);
                cmd.Parameters.Add("@English_Title", DbType.String);
                cmd.Parameters.Add("@Japanese_Title", DbType.String);
                cmd.Parameters.Add("@French_Title", DbType.String);
                cmd.Parameters.Add("@German_Title", DbType.String);
                cmd.Parameters.Add("@Italian_Title", DbType.String);
                cmd.Parameters.Add("@Spanish_Title", DbType.String);
                cmd.Parameters.Add("@Simplified_Chinese_Title", DbType.String);
                cmd.Parameters.Add("@Korean_Title", DbType.String);
                cmd.Parameters.Add("@Dutch_Title", DbType.String);
                cmd.Parameters.Add("@Portuguese_Title", DbType.String);
                cmd.Parameters.Add("@Russian_Title", DbType.String);
                cmd.Parameters.Add("@Traditional_Chinese_Title", DbType.String);
                cmd.Parameters.Add("@Platform", DbType.String);
                cmd.Parameters.Add("@Card_Type", DbType.String);
                cmd.Parameters.Add("@Card_ID", DbType.String);
                cmd.Parameters.Add("@Chip_ID", DbType.String);
                cmd.Parameters.Add("@Manufacturer", DbType.String);
                cmd.Parameters.Add("@OriginalName", DbType.String);
                cmd.Parameters.Add("@Developer", DbType.String);
                cmd.Parameters.Add("@Publisher", DbType.String);
                cmd.Parameters.Add("@ReleaseDate", DbType.String);
                cmd.Parameters.Add("@Genre", DbType.String);
                cmd.Parameters.Add("@Rating", DbType.String);
                cmd.Parameters.Add("@Players", DbType.String);
                cmd.Parameters.Add("@Imagesize", DbType.String);
                cmd.Parameters.Add("@Firmware", DbType.String);
                cmd.Parameters.Add("@Favorite", DbType.String);
                cmd.Parameters.Add("@IsCustomsizeRom", DbType.String);
                cmd.Parameters.Add("@SourceSerial", DbType.String);

                foreach (var rominfo in pRomInfoList)
                {
                    RomBasicInfo tBasic = rominfo.BasicInfo;
                    cmd.Parameters["@Serial"].Value = tBasic.Serial;
                    cmd.Parameters["@Capacity"].Value = tBasic.Capacity;
                    cmd.Parameters["@Region"].Value = tBasic.Region;
                    cmd.Parameters["@Region_Lockout"].Value = tBasic.Region_Lockout;
                    cmd.Parameters["@Languages"].Value = tBasic.Languages;
                    cmd.Parameters["@Title_ID"].Value = tBasic.Title_ID;
                    cmd.Parameters["@Game_Title"].Value = tBasic.Game_Title;
                    cmd.Parameters["@English_Title"].Value = tBasic.English_Title;
                    cmd.Parameters["@Japanese_Title"].Value = tBasic.Japanese_Title;
                    cmd.Parameters["@French_Title"].Value = tBasic.French_Title;
                    cmd.Parameters["@German_Title"].Value = tBasic.German_Title;
                    cmd.Parameters["@Italian_Title"].Value = tBasic.Italian_Title;
                    cmd.Parameters["@Spanish_Title"].Value = tBasic.Spanish_Title;
                    cmd.Parameters["@Simplified_Chinese_Title"].Value = tBasic.Simplified_Chinese_Title;
                    cmd.Parameters["@Korean_Title"].Value = tBasic.Korean_Title;
                    cmd.Parameters["@Dutch_Title"].Value = tBasic.Dutch_Title;
                    cmd.Parameters["@Portuguese_Title"].Value = tBasic.Portuguese_Title;
                    cmd.Parameters["@Russian_Title"].Value = tBasic.Russian_Title;
                    cmd.Parameters["@Traditional_Chinese_Title"].Value = tBasic.Traditional_Chinese_Title;
                    cmd.Parameters["@Platform"].Value = tBasic.Platform;
                    cmd.Parameters["@Card_Type"].Value = tBasic.Card_Type;
                    cmd.Parameters["@Card_ID"].Value = tBasic.Card_ID;
                    cmd.Parameters["@Chip_ID"].Value = tBasic.Chip_ID;
                    cmd.Parameters["@Manufacturer"].Value = tBasic.Manufacturer;
                    cmd.Parameters["@OriginalName"].Value = tBasic.OriginalName;
                    cmd.Parameters["@Developer"].Value = tBasic.Developer;
                    cmd.Parameters["@Publisher"].Value = tBasic.Publisher;
                    cmd.Parameters["@ReleaseDate"].Value = tBasic.ReleaseDate;
                    cmd.Parameters["@Genre"].Value = tBasic.Genre;
                    cmd.Parameters["@Rating"].Value = tBasic.Rating;
                    cmd.Parameters["@Players"].Value = tBasic.Players;
                    cmd.Parameters["@Imagesize"].Value = tBasic.Imagesize;
                    cmd.Parameters["@Firmware"].Value = tBasic.Firmware;
                    cmd.Parameters["@Favorite"].Value = tBasic.Favorite;
                    cmd.Parameters["@IsCustomsizeRom"].Value = tBasic.IsCustomsizeRom;
                    cmd.Parameters["@SourceSerial"].Value = tBasic.SourceSerial;
                    cmd.ExecuteNonQuery();
                }

                lTransaction.Commit();
            }
            catch (Exception ex)
            {
                lTransaction.Rollback();
                throw ex;
            }
            return true;
        }

        public static RomInformation GetGameInfo(string pSerial)
        {
            SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + _DBFileFullName);
            lDBConnection.Open();

            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = lDBConnection;
            cmd.CommandText = "SELECT * FROM " + _TableName + " WHERE serial='" + pSerial + "'";

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

            RomInformation lResult = ConverDataRowToRomInfo(ds.Rows[0]);
            return lResult;
        }
        private static RomInformation ConverDataRowToRomInfo(DataRow pDBDataRow)
        {
            RomInformation lResult = new RomInformation();
            lResult.BasicInfo.Serial = pDBDataRow["Serial"].ToString().Trim();
            lResult.BasicInfo.Capacity = pDBDataRow["Capacity"].ToString().Trim();
            lResult.BasicInfo.Region = pDBDataRow["Region"].ToString().Trim();
            lResult.BasicInfo.Region_Lockout = pDBDataRow["Region_Lockout"].ToString().Trim();
            lResult.BasicInfo.Languages = pDBDataRow["Languages"].ToString().Trim();
            lResult.BasicInfo.Title_ID = pDBDataRow["Title_ID"].ToString().Trim();
            lResult.BasicInfo.Game_Title = pDBDataRow["Game_Title"].ToString().Trim();
            lResult.BasicInfo.English_Title = pDBDataRow["English_Title"].ToString().Trim();
            lResult.BasicInfo.Japanese_Title = pDBDataRow["Japanese_Title"].ToString().Trim();
            lResult.BasicInfo.French_Title = pDBDataRow["French_Title"].ToString().Trim();
            lResult.BasicInfo.German_Title = pDBDataRow["German_Title"].ToString().Trim();
            lResult.BasicInfo.Italian_Title = pDBDataRow["Italian_Title"].ToString().Trim();
            lResult.BasicInfo.Spanish_Title = pDBDataRow["Spanish_Title"].ToString().Trim();
            lResult.BasicInfo.Simplified_Chinese_Title = pDBDataRow["Simplified_Chinese_Title"].ToString().Trim();
            lResult.BasicInfo.Korean_Title = pDBDataRow["Korean_Title"].ToString().Trim();
            lResult.BasicInfo.Dutch_Title = pDBDataRow["Dutch_Title"].ToString().Trim();
            lResult.BasicInfo.Portuguese_Title = pDBDataRow["Portuguese_Title"].ToString().Trim();
            lResult.BasicInfo.Russian_Title = pDBDataRow["Russian_Title"].ToString().Trim();
            lResult.BasicInfo.Traditional_Chinese_Title = pDBDataRow["Traditional_Chinese_Title"].ToString().Trim();
            lResult.BasicInfo.Platform = pDBDataRow["Platform"].ToString().Trim();
            lResult.BasicInfo.Card_Type = pDBDataRow["Card_Type"].ToString().Trim();
            lResult.BasicInfo.Card_ID = pDBDataRow["Card_ID"].ToString().Trim();
            lResult.BasicInfo.Chip_ID = pDBDataRow["Chip_ID"].ToString().Trim();
            lResult.BasicInfo.Manufacturer = pDBDataRow["Manufacturer"].ToString().Trim();
            lResult.BasicInfo.OriginalName = pDBDataRow["OriginalName"].ToString().Trim();
            lResult.BasicInfo.Developer = pDBDataRow["Developer"].ToString().Trim();
            lResult.BasicInfo.Publisher = pDBDataRow["Publisher"].ToString().Trim();
            lResult.BasicInfo.ReleaseDate = pDBDataRow["ReleaseDate"].ToString().Trim();
            lResult.BasicInfo.Genre = pDBDataRow["Genre"].ToString().Trim();
            lResult.BasicInfo.Rating = pDBDataRow["Rating"].ToString().Trim();
            lResult.BasicInfo.Players = pDBDataRow["Players"].ToString().Trim();
            lResult.BasicInfo.Imagesize = pDBDataRow["Imagesize"].ToString().Trim();
            lResult.BasicInfo.Firmware = pDBDataRow["Firmware"].ToString().Trim();
            lResult.BasicInfo.Favorite = pDBDataRow["Favorite"].ToString().Trim();
            lResult.BasicInfo.IsCustomsizeRom = pDBDataRow["IsCustomsizeRom"].ToString().Trim();
            lResult.BasicInfo.SourceSerial = pDBDataRow["SourceSerial"].ToString().Trim();
            return lResult;
        }
        public static List<RomInformation> GetAllGameInfo()
        {
            SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + _DBFileFullName);
            lDBConnection.Open();

            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = lDBConnection;
            cmd.CommandText = "SELECT * FROM " + _TableName + " Limit 20";

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
            List<RomInformation> lResult = new List<RomInformation>();
            foreach (DataRow dataRow in ds.Rows)
            {
                lResult.Add(ConverDataRowToRomInfo(dataRow));
            }
            return lResult;
        }
        public static void UpdateGameInfo(RomBasicInfo pRomInfo)
        {
            string lSqlHeader = "UPDATE " + _TableName + " SET ";
            string lSqlFooter = "WHERE Serial = '" + pRomInfo.Serial + "' ";
            // FirstName = 'Fred', City = 'Nanjing'
            string lSqlBody = "";

            try
            {
                SQLiteConnection lDBConnection = new SQLiteConnection("data source=" + _DBFileFullName);
                lDBConnection.Open();

                RomInformation lExistRomInfo = GetGameInfo(pRomInfo.Serial);

                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = lDBConnection;
                string lSql = lSqlHeader;

                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Serial, pRomInfo.Serial, "Serial");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Capacity, pRomInfo.Capacity, "Capacity");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Region, pRomInfo.Region, "Region");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Region_Lockout, pRomInfo.Region_Lockout, "Region_Lockout");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Languages, pRomInfo.Languages, "Languages");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Title_ID, pRomInfo.Title_ID, "Title_ID");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Game_Title, pRomInfo.Game_Title, "Game_Title");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.English_Title, pRomInfo.English_Title, "English_Title");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Japanese_Title, pRomInfo.Japanese_Title, "Japanese_Title");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.French_Title, pRomInfo.French_Title, "French_Title");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.German_Title, pRomInfo.German_Title, "German_Title");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Italian_Title, pRomInfo.Italian_Title, "Italian_Title");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Spanish_Title, pRomInfo.Spanish_Title, "Spanish_Title");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Simplified_Chinese_Title, pRomInfo.Simplified_Chinese_Title, "Simplified_Chinese_Title");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Korean_Title, pRomInfo.Korean_Title, "Korean_Title");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Dutch_Title, pRomInfo.Dutch_Title, "Dutch_Title");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Portuguese_Title, pRomInfo.Portuguese_Title, "Portuguese_Title");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Russian_Title, pRomInfo.Russian_Title, "Russian_Title");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Traditional_Chinese_Title, pRomInfo.Traditional_Chinese_Title, "Traditional_Chinese_Title");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Platform, pRomInfo.Platform, "Platform");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Card_Type, pRomInfo.Card_Type, "Card_Type");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Card_ID, pRomInfo.Card_ID, "Card_ID");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Chip_ID, pRomInfo.Chip_ID, "Chip_ID");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Manufacturer, pRomInfo.Manufacturer, "Manufacturer");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.OriginalName, pRomInfo.OriginalName, "OriginalName");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Developer, pRomInfo.Developer, "Developer");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Publisher, pRomInfo.Publisher, "Publisher");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.ReleaseDate, pRomInfo.ReleaseDate, "ReleaseDate");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Genre, pRomInfo.Genre, "Genre");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Rating, pRomInfo.Rating, "Rating");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Players, pRomInfo.Players, "Players");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Imagesize, pRomInfo.Imagesize, "Imagesize");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Firmware, pRomInfo.Firmware, "Firmware");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.Favorite, pRomInfo.Favorite, "Favorite");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.IsCustomsizeRom, pRomInfo.IsCustomsizeRom, "IsCustomsizeRom");
                lSqlBody += AddProp(cmd, lExistRomInfo.BasicInfo.SourceSerial, pRomInfo.SourceSerial, "SourceSerial");

                lSqlBody.TrimEnd(',');

                lSql += lSqlBody;

                lSql += lSqlFooter;

                cmd.CommandText = lSql;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static string AddProp(SQLiteCommand pSqlCmd,string pExistValue,string pTarValue,string pItemName)
        {
            string lResult = "";

            if (pExistValue != pTarValue)
            {
                lResult = " " + pItemName + " = '@" + pItemName + "', ";
                pSqlCmd.Parameters.Add("@" + pItemName, DbType.String);
                pSqlCmd.Parameters["@" + pItemName].Value = pTarValue;
            }

            return lResult;
        }
        public static bool CheckDBExist()
        {
            FileInfo lFile = new FileInfo(_DBFileFullName);
            if (lFile.Exists && lFile.Length > 0)
            {
                return true;
            }

            if (lFile.Exists)
            {
                lFile.Delete();
            }

            return false;
        }
        public void SaveRomInfoToDB(RomInformation pRomInfo)
        {

        }
    }
}
