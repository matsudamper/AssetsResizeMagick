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
            // 存在確認
            if (File.Exists(settingPath) == false)
            {
                Console.WriteLine("設定ファイルがありません");
                Console.ReadLine();
                return false;
            }

            // 取得
            var fs = new FileStream(settingPath, FileMode.Open);
            var serializer = new XmlSerializer(typeof(Settings));
            settings = (Settings)serializer.Deserialize(fs);

            // 表示
            for (int i = 0; i < settings.types.Count; i++)
            {
                var type = settings.types[i];
                Console.WriteLine("{0}, {1}", i + 1, type.name);
            }

            // 選択
            Console.WriteLine("どのサイズに変換しますか？");
            while (true)
            {
                int result;

                if (int.TryParse(Console.ReadLine(), out result)
                    && result > 0
                    && result < settings.types.Count + 1)
                {
                    Console.WriteLine("{0}番に変換します", result);

                    translateType = settings.types[result - 1];
                    break;
                }
                else
                {
                    Console.WriteLine("入力値が正しくありません");
                    continue;
                }
            }

            // 出力フォルダ作成
            if (String.IsNullOrEmpty(translateType.folderName) == false)
            {
                currentPath += @"\" + translateType.folderName;

                if (Directory.Exists(currentPath) == false)
                {
                    Directory.CreateDirectory(currentPath);
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

                var magicSettings = new ImageMagick.MagickReadSettings
                {
                    Density = new ImageMagick.Density(dpi),
                    BackgroundColor = new ImageMagick.MagickColor("#00000000")
                };

                using (var magickImage = new ImageMagick.MagickImage(path, magicSettings))
                {
                    foreach (var type in translateType.images)
                    {
                        string outPath = currentPath;

                        // 出力フォルダ作成
                        if (String.IsNullOrEmpty(type.folderName) == false)
                        {
                            outPath += @"\" + type.folderName;

                            if (Directory.Exists(outPath) == false)
                            {
                                Directory.CreateDirectory(outPath);
                            }
                        }

                        // 出力ファイル名追加
                        if (String.IsNullOrEmpty(type.filename) == false)
                        {
                            outPath += @"\" + type.filename;
                        }
                        else
                        {
                            outPath += String.Format(@"\{0}.png", Path.GetFileNameWithoutExtension(path));
                        }

                        // 出力
                        using (var editImage = new ImageMagick.MagickImage(magickImage))
                        {
                            editImage.Format = ImageMagick.MagickFormat.Png;
                            editImage.Scale(type.x, type.y);

                            // 変わらなっかたが念のため
                            editImage.Quality = 100;

                            editImage.Write(outPath);
                        }
                    }
                }
            }

            Console.WriteLine("終了しました");
            Console.ReadLine();
        }
    }
}
