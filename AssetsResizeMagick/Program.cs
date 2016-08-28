using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace AssetsResizeMagick
{
    class Program
    {
        public static Settings settings;
        public static Type translateType;
        public static string[] files;

        public static string currentPath = Directory.GetCurrentDirectory();
        public static string settingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\settings.xml";
        public static int dpi = 1000;

        static void Main(string[] args)
        {
            if (!DropCheck()) return;

            if (!SettingsCheck()) return;

            Translate();
        }

        private static bool SettingsCheck()
        {
            if (File.Exists(settingPath) == false)
            {
                Console.WriteLine("設定ファイルがありません");
                Console.ReadLine();
                return false;
            }

            var fs = new FileStream(settingPath, FileMode.Open);
            var serializer = new XmlSerializer(typeof(Settings));
            settings = (Settings)serializer.Deserialize(fs);

            for (int i = 0; i < settings.types.Count; i++)
            {
                var type = settings.types[i];
                Console.WriteLine((i + 1) + ", " + type.name);
            }

            Console.WriteLine("どのサイズに変換しますか？");
            while (true)
            {
                int result;

                if (int.TryParse(Console.ReadLine(), out result)
                    && result > 0
                    && result < settings.types.Count + 1)
                {
                    Console.WriteLine(result + "番に変換します");

                    translateType = settings.types[result - 1];
                    break;
                }
                else
                {
                    Console.WriteLine("入力値が正しくありません");
                    continue;
                }
            }

            return true;
        }

        private static bool DropCheck()
        {
            files = Environment.GetCommandLineArgs();

            if (files.Length < 2)
            {
                Console.WriteLine("変換したいファイルをアイコンにドロップしてください");
                Console.ReadLine();
                return false;
            }

            for (int i = 1; i < files.Length; i++)
            {
                var path = files[i];
                Console.WriteLine(path);
            }

            return true;
        }

        private static void Translate()
        {
            for (int i = 1; i < files.Length; i++)
            {
                var path = files[i];

                var magicSettings = new ImageMagick.MagickReadSettings();
                magicSettings.Density = new ImageMagick.Density(dpi);
                magicSettings.BackgroundColor = new ImageMagick.MagickColor("#00000000");

                using (var magickImage = new ImageMagick.MagickImage(path, magicSettings))
                {
                    foreach (var item in translateType.images)
                    {
                        using (var editImage = new ImageMagick.MagickImage(magickImage))
                        {
                            editImage.Format = ImageMagick.MagickFormat.Png;
                            editImage.Scale(item.x, item.y);

                            // 変わらなっかたが念のため
                            editImage.Quality = 100;

                            editImage.Write(currentPath + @"\" + item.filename);
                        }
                    }
                }
            }

            Console.WriteLine("終了しました");
            Console.ReadLine();
        }
    }
}
