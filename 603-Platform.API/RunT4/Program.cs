using System;
using System.Collections.Generic;
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

                        #region 废弃
                        //var kvList = new List<KeyValue>();
                        //var columsList = db.ClassGenerating.GetTableColumns(db, tableName);
                        //foreach (var item in columsList)
                        //{
                        //   // kvList.Add(GetNewKV(item.COLUMN_NAME.ToString(), item.COLUMN_NAME.ToString()));
                        //    db.AddMappingColumn(GetNewKV(item.COLUMN_NAME.ToString(), item.COLUMN_NAME.ToString()));
                        //}
                        
                        #endregion

                    });
                    //删除所有.cs文件 非通用方法
                    DeleteAllCS(modelPath);
                    //建立全部
                    db.ClassGenerating.CreateClassFiles(db, modelPath, "ZC.Platform.Model");

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

        public static KeyValue GetNewKV(string Key, string Value)
        {
            Key = NewKey(Key);
            KeyValue kv = new KeyValue();
            kv.Key = Key;
            kv.Value = Value;
            return kv;
        }

        /// <summary>
        /// 获取驼峰式的命名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string NewKey(string key)
        {
            int index = key.IndexOf('_');
            if (index != -1)
            {
                string ch = key[index + 1].ToString().ToUpper();

                key = key.Remove(index, 2);
                key = key.Insert(index, ch);
                while (key.IndexOf('_') != -1)
                {
                    key = NewKey(key);
                }
            }

            return key;
        }



        //获取文件json
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

        /// <summary>
        /// 定时退出
        /// </summary>
        /// <param name="seconds"></param>
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

        /// <summary>
        /// 删除所有的.cs文件
        /// </summary>
        /// <param name="filePath"></param>
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
