using System;
using System.IO;
using System.Text;
using MySqlSugar;
using MySugar;
using Newtonsoft.Json.Linq;

namespace RunT4
{
    public class Program
    {
        static void Main(string[] args)
        {
            //运行T4模板
            RunT4();

        }

        /// <summary>
        /// 生成T4模板
        /// </summary>
        public static void RunT4()
        {
            Console.WriteLine("开始生产T4------模板");

            try
            {
                //获取appsettings.json路径
                string path = Path.GetFullPath("../../../..") + "\\ZC.Platform.API\\appsettings.json ";
                //获取Json报文
                string json = GetFileJson(path);
                //利用json获取连接字符串路径
                JObject ad = JObject.Parse(json);
                string ConnectionString = ad["ConnectionStrings"]["DefaultConnection"].ToString();

                using (var db = new SqlSugarClient(ConnectionString))
                {
                    //生成到ZC.Platform.Model层之下
                    string modelPath = Path.GetFullPath("../../../..") + "\\ZC.Platform.Model";
                    //编写新模板
                    db.ClassGenerating.ForeachTables(db, tableName =>
                    {
                        // string model = $"{modelPath}\\{tableName}.cs";
                        db.AddMappingTable(new KeyValue() { Key = tableName.ToUpper(), Value = tableName });

                    });
                    //删除所有.cs文件 非通用方法
                    DeleteAllCS(modelPath);

                    //建立全部
                    db.ClassGenerating.CreateClassFiles(db, modelPath, "ZC.Platform.Model");

                    //  db.ClassGenerating.CreateClassFiles(db, modelPath, "ZC.Platform.Model",
                    //null,
                    //className =>
                    //{
                    //    //生成文件之后的回调
                    //}, tableName =>
                    //{
                    //    //生成文件之前的回调
                    //});


                }
                Console.WriteLine("生成成功！");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误{ex.Message}");
                Console.ReadKey();
            }
            Console.WriteLine("1秒后退出");
            Exit(1);
        }

        public static string GetFileJson(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("utf-8")))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            return json;
        }

        public static void Exit(int seconds)
        {
            seconds--;
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine(seconds);
            if (seconds > 0)
            {
                Exit(seconds);
            };
        }

        public static void DeleteAllCS(string filePath)
        {
            DirectoryInfo folder = new DirectoryInfo(filePath);
            //获取文件夹下所有的文件
            FileInfo[] fileList = folder.GetFiles();
            foreach (FileInfo file in fileList)
            {
                //判断文件的扩展名是否为 .gif
                if (file.Extension == ".cs")
                {
                    file.Delete();  // 删除
                }
            }
        }
    }

}
