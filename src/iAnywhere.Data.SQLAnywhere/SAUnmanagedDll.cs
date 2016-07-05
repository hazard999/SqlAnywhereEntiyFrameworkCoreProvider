using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;

namespace iAnywhere.Data.SQLAnywhere
{
    sealed class SAUnmanagedDll
    {
        static string s_guid = "{16AA8FB8-4A98-4757-B7A5-0FF22C0A6E33}";
        static IntPtr s_hModule = IntPtr.Zero;
        static SAUnmanagedDll s_instance = null;
        static bool s_lowMemory;
        static string s_dllPath;

        public static SAUnmanagedDll Instance
        {
            get
            {
                lock (typeof(SAUnmanagedDll))
                {
                    if (s_instance == null)
                        s_instance = new SAUnmanagedDll();
                    return s_instance;
                }
            }
        }

        SAUnmanagedDll()
        {
            LoadDll();
            PInvokeMethods.Unmanaged_Init();
        }

        ~SAUnmanagedDll()
        {
            if (s_instance == null)
                return;
            PInvokeMethods.Unmanaged_Fini();
        }

        static void UnloadDll()
        {
            if (!(s_hModule != IntPtr.Zero))
                return;
            PInvokeMethods.FreeLibrary(s_hModule);
        }

        static void LoadDll()
        {
            int num = 0;
            string tempDir = GetTempDir();
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            string str1 = string.Format("{0}{1}{2}", version.Major, version.Minor, version.Build);
            string str2 = string.Format("{0}_{1}{2}", s_guid, str1, GetImageType());
            while (s_hModule == IntPtr.Zero)
            {
                ++num;
                try
                {
                    string path = string.Format("{0}{1}_{2}", tempDir, str2, num);
                    s_dllPath = string.Format("{0}\\{1}", path, "dbdata11.dll");
                    //if (!Directory.Exists(path))
                    //    Directory.CreateDirectory(path);
                    //else if (File.Exists(s_dllPath))
                    //{
                    //    try
                    //    {
                    //        File.Delete(s_dllPath);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        string @string = version.ToString();
                    //        string str3 = @string.Substring(0, @string.Length - 1);
                    //        s_hModule = PInvokeMethods.LoadLibrary(s_dllPath);
                    //        if (s_hModule != IntPtr.Zero)
                    //        {
                    //            //TODO: Fix ProcessModule
                    //            /*
                    //            foreach (ProcessModule module in (ReadOnlyCollectionBase)Process.GetCurrentProcess().Modules)
                    //            {
                    //                if (string.Compare(module.ModuleName, "dbdata11.dll", true) == 0)
                    //                {
                    //                    if (module.FileVersionInfo.FileVersion.StartsWith(str3))
                    //                        return;
                    //                    break;
                    //                }
                    //            }
                    //            */
                    //            PInvokeMethods.FreeLibrary(s_hModule);
                    //            s_hModule = IntPtr.Zero;
                    //            continue;
                    //        }
                    //        continue;
                    //    }
                    //}
                    //CreateDll();
                    s_hModule = PInvokeMethods.LoadLibrary(s_dllPath);
                }
                catch (Exception ex)
                {
                }
                if (s_lowMemory)
                    throw new Exception(string.Format("Failed to load native dll ({0}), low disk space.", "dbdata11.dll"));
            }
        }

        private static string GetTempDir()
        {
            string[] strArray = { Path.GetTempPath().Trim(), Directory.GetCurrentDirectory().Trim(), Path.GetDirectoryName(GetLocationFromCodeBase().Trim()) };
            string @string = Guid.NewGuid().ToString();
            for (int index = 0; index < strArray.GetLength(0); ++index)
            {
                string str = strArray[index];
                if (str[str.Length - 1] != 92 && str[str.Length - 1] != 47)
                    str += "\\";
                string path1 = str + @string;
                string path2 = string.Format("{0}\\{1}.dll", path1, @string);
                try
                {
                    Directory.CreateDirectory(path1);
                    byte[] buffer = new byte[1];

                    using (FileStream fileStream = new FileStream(path2, FileMode.CreateNew))
                    using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                        binaryWriter.Write(buffer);


                    File.Delete(path2);
                    Directory.Delete(path1);
                    return str;
                }
                catch (Exception ex)
                {
                }
            }
            throw new Exception(string.Format("SQL Anywhere ADO.NET Data Provider requires access permissions of one of the following directories:\r\n1. System temporary directory ({0})\r\n2. Current working directory ({1})\r\n3. SQL Anywhere ADO.NET Data Provider assembly directory ({2})", strArray[0], strArray[1], strArray[2]));
        }

        static string GetImageType()
        {
            if (IntPtr.Size == 8)
                return ".x64";

            return "";
        }

        private static void CreateDll()
        {
            s_lowMemory = false;
            string str = ".dbdata11.dll";
            using (FileStream fileStream = new FileStream(s_dllPath, FileMode.CreateNew))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    
                    using (Stream manifestResourceStream = Assembly.GetEntryAssembly().GetManifestResourceStream(typeof(SAUnmanagedDll).Namespace + str + SAUnmanagedDll.GetImageType()))
                    {
                        byte[] buffer = null;
                        try
                        {
                            buffer = new byte[manifestResourceStream.Length];
                            manifestResourceStream.Read(buffer, 0, (int)manifestResourceStream.Length);
                        }
                        catch (OutOfMemoryException ex)
                        {
                            s_lowMemory = true;
                        }
                        if (s_lowMemory)
                            return;
                        try
                        {
                            binaryWriter.Write(buffer);
                        }
                        catch (IOException ex)
                        {
                            SAUnmanagedDll.s_lowMemory = true;
                        }
                    }
                }
            }
        }

        private static string GetLocationFromCodeBase()
        {
            string str1 = "file:///";
            string str2 = Assembly.GetEntryAssembly().CodeBase;
            if (str2.IndexOf(str1) == 0)
                str2 = str2.Substring(str1.Length);
            return str2.Replace('/', '\\');
        }
    }
}
