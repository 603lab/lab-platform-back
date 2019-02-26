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
                string ConnectionString = ad["ConnectionStrings"]["PekoPekoConnection"].ToString();

                using (var db = new SqlSugarClient(ConnectionString))
                {

                    //生成到ZC.Platform.Model层之下
                    string modelPath = Path.GetFullPath("../../../..") + "\\ZC.Platform.Model";
                    string baseModelPath = Path.GetFullPath("../../../..") + "\\ZC.Platform.API\\BaseModel";

                    //删除baseModel模板
                    DeleteAllCS(baseModelPath);
                    //编写新模板
                    db.ClassGenerating.ForeachTables(db, tableName =>
                    {
                        // string model = $"{modelPath}\\{tableName}.cs";
                        db.AddMappingTable(new KeyValue() { Key = tableName.ToUpper(), Value = tableName });

                        //获取生成BaseModel模板
                        var tableModelTemplate = NewKey(db.ClassGenerating.TableNameToClass(db, tableName.ToUpper()), tableName.ToUpper(), false)
                        .Replace('~', '_').Replace("namespace System", "namespace ZC.Platform.API.BaseModel");

                        while (tableName.IndexOf('_') != -1)
                        {
                            tableName = tableName.Remove(tableName.IndexOf('_'), 1);
                        }
                        tableName += "Base";
                        //去T
                        tableName = tableName.Substring(1,tableName.Length-1);
                        CreateFile(baseModelPath.TrimEnd('\\') + "\\" + tableName.ToUpper() + ".cs", tableModelTemplate, Encoding.UTF8);

                        Console.WriteLine();
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

        /// <summary>
        /// 创建一个文件,并将字符串写入文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">字符串数据</param>
        /// <param name="encoding">字符编码</param>
        public static void CreateFile(string filePath, string text, Encoding encoding)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //获取文件目录路径
                    string directoryPath = GetDirectoryFromFilePath(filePath);

                    //如果文件的目录不存在，则创建目录
                    CreateDirectory(directoryPath);

                    //创建文件
                    FileInfo file = new FileInfo(filePath);
                    using (FileStream stream = file.Create())
                    {
                        using (StreamWriter writer = new StreamWriter(stream, encoding))
                        {
                            //写入字符串     
                            writer.Write(text);

                            //输出
                            writer.Flush();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 从文件绝对路径中获取目录路径
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static string GetDirectoryFromFilePath(string filePath)
        {
            //实例化文件
            FileInfo file = new FileInfo(filePath);

            //获取目录信息
            DirectoryInfo directory = file.Directory;

            //返回目录路径
            return directory.FullName;
        }

        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static void CreateDirectory(string directoryPath)
        {
            //如果目录不存在则创建该目录
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        #region 检测指定文件是否存在
        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion


        public static KeyValue GetNewKV(string Key, string Value)
        {
            Key = NewKey(Key, "", false);
            KeyValue kv = new KeyValue();
            kv.Key = Key;
            kv.Value = Value;
            return kv;
        }
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 获取驼峰式的命名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string NewKey(string key, string tableName, bool isSkip)
        {
            int index = key.IndexOf('_');
            if (index != -1)
            {
                //重命名表名
                string tConvert = key[index - 1].ToString();
                string nullMark = key[index - 2].ToString();
                if (!(tConvert == "T" && nullMark == " "))
                {
                    string ch = key[index + 1].ToString().ToUpper();

                    key = key.Remove(index, 2);
                    key = key.Insert(index, ch);

                    while (key.IndexOf('_') != -1)
                    {
                        key = NewKey(key, tableName, isSkip);
                    }
                }
                else
                {
                    if (!isSkip)
                    {
                        //确认该T是属于table的
                        int headIndex = key.IndexOf('{');
                        int endIndex = key.IndexOf('{', headIndex + 1);

                        //public class T_USERS_Mark -> public class Users_Mark
                        key = key.Remove(index - 1, 2);
                        //从T开始 到
                        int endTableNameIndex = key.IndexOf(' ', index);
                        key = key.Insert(endTableNameIndex - 2, "BASE");

                        //由于T_USERS的下滑先会被替换 所以寻找一个不会被替换的字符代替 '~'
                        tableName = tableName.Replace('_', '~');

                        //添加头部表格映射 [SugarMapping(TableName = "T_TASK")]
                        string sugarAttr = $"\n    [SugarMapping(TableName = \"{tableName.ToUpper()}\")]";
                        key = key = key.Insert(headIndex + 1, sugarAttr);

                        //添加引用
                        key = key.Insert(0, "using MySqlSugar;\n");
                        //完成任务不进来了
                        isSkip = true;
                    }


                    //处理完标题继续循环
                    while (key.IndexOf('_') != -1)
                    {
                        key = NewKey(key, tableName, isSkip);
                    }
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
