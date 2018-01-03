using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BQ3DSRomLoader.UT
{
    public class CIAGame
    {
        private string _filePath;
        public static TitleLanguage DefaultLanguage = TitleLanguage.English;

        public CIAGame()
        {
            this.Titles = new List<ApplicationTitle>();
        }

        public CIAGame(string filePath) : this()
        {
            this.FilePath = filePath;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            CIAGame game = obj as CIAGame;
            if (game == null)
            {
                return false;
            }
            return (this.FilePath == game.FilePath);
        }

        public override int GetHashCode()
        {
            return string.Format("CIA {0}", this.FilePath).GetHashCode();
        }

        public void Reload()
        {
            try
            {
                this.ResetValues();
                using (FileStream stream = File.OpenRead(this.FilePath))
                {
                    CIA cia = new CIA();
                    if (cia.Read(stream))
                    {
                        this.TitleId = cia.TitleId;
                        this.Serial = cia.ProductCode;
                        this.MakerCode = cia.MakerCode;
                        this.MediaType = this.TitleId.StartsWith("0004008C") ? GameMediaType.DLC : (this.TitleId.StartsWith("0004000E") ? GameMediaType.Patch : GameMediaType.CIA);
                        if (cia.MetaSize >= 0x3ac0)
                        {
                            stream.Seek(cia.MetaOffset + 0x400L, SeekOrigin.Begin);
                            byte[] buffer = new byte[4];
                            stream.Read(buffer, 0, 4);
                            if (buffer.ToASCIIString() == "SMDH")
                            {
                                stream.Seek(4L, SeekOrigin.Current);
                                for (int i = 0; i < 0x10; i++)
                                {
                                    ApplicationTitle item = new ApplicationTitle();
                                    item.Read(stream);
                                    this.Titles.Add(item);
                                }
                                stream.Seek(0x38L, SeekOrigin.Current);
                                this.SmallIcon = ImageUtil.ReadImageFromStream(stream, 0x18, 0x18, ImageUtil.PixelFormat.RGB565);
                                this.LargeIcon = ImageUtil.ReadImageFromStream(stream, 0x30, 0x30, ImageUtil.PixelFormat.RGB565);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ResetValues()
        {
            string str;
            Bitmap bitmap;
            this.MakerCode = str = "N/A";
            this.TitleId = this.Serial = str;
            this.MediaType = GameMediaType.NA;
            this.LargeIcon = (Bitmap)(bitmap = null);
            this.SmallIcon = bitmap;
        }

        public void SaveIcon(string path)
        {
            try
            {
                if (this.LargeIcon != null)
                {
                    string str = string.Format("{0}_{1}.png", this.TitleId, this.Serial);
                    string filename = Path.Combine(path, str);
                    this.LargeIcon.Save(filename, ImageFormat.Png);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string FilePath
        {
            get
            {
                return this._filePath;
            }
            set
            {
                if (File.Exists(value))
                {
                    this._filePath = value;
                    string extension = Path.GetExtension(value);
                    if ((extension != null) && (extension.ToUpper() == ".CIA"))
                    {
                        this.Reload();
                    }
                }
            }
        }

        public Bitmap LargeIcon { get; private set; }

        public string LongName
        {
            get
            {
                if (this.Titles.Count < 12)
                {
                    return "";
                }
                return this.Titles[DefaultLanguage.IntValue()].LongDescription;
            }
        }

        public string MakerCode { get; private set; }

        public GameMediaType MediaType { get; private set; }

        public string Name
        {
            get
            {
                try
                {
                    return ((this.Titles.Count >= 12) ? this.Titles[DefaultLanguage.IntValue()].ShortDescription : Path.GetFileName(this.FilePath));
                }
                catch (Exception)
                {
                    return "N/A";
                }
            }
        }

        public string Publisher
        {
            get
            {
                if (this.Titles.Count < 12)
                {
                    return "";
                }
                return this.Titles[DefaultLanguage.IntValue()].Publisher;
            }
        }

        public string Serial { get; private set; }

        public Bitmap SmallIcon { get; private set; }

        public string TitleId { get; private set; }

        public List<ApplicationTitle> Titles { get; private set; }
    }
}
