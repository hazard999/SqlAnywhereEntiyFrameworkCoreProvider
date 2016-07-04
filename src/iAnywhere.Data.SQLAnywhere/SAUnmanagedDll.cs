
// Type: iAnywhere.Data.SQLAnywhere.SAUnmanagedDll
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>Summary description for SAUnmanagedDll</summary>
  internal sealed class SAUnmanagedDll
  {
    private static string s_guid = "{16AA8FB8-4A98-4757-B7A5-0FF22C0A6E33}";
    private static IntPtr s_hModule = IntPtr.Zero;
    private static SAUnmanagedDll s_instance = null;
    private static bool s_lowMemory;
    private static string s_dllPath;

    public static SAUnmanagedDll Instance
    {
      get
      {
        lock (typeof (SAUnmanagedDll))
        {
          if (SAUnmanagedDll.s_instance == null)
            SAUnmanagedDll.s_instance = new SAUnmanagedDll();
          return SAUnmanagedDll.s_instance;
        }
      }
    }

    private SAUnmanagedDll()
    {
      SAUnmanagedDll.LoadDll();
      PInvokeMethods.Unmanaged_Init();
    }

    ~SAUnmanagedDll()
    {
      if (SAUnmanagedDll.s_instance == null)
        return;
      PInvokeMethods.Unmanaged_Fini();
    }

    private static void UnloadDll()
    {
      if (!(SAUnmanagedDll.s_hModule != IntPtr.Zero))
        return;
      PInvokeMethods.FreeLibrary(SAUnmanagedDll.s_hModule);
    }

    private static void LoadDll()
    {
      int num = 0;
      string tempDir = SAUnmanagedDll.GetTempDir();
      Version version = Assembly.GetExecutingAssembly().GetName().Version;
      string str1 = string.Format("{0}{1}{2}", version.Major, version.Minor, version.Build);
      string str2 = string.Format("{0}_{1}{2}", SAUnmanagedDll.s_guid, str1, SAUnmanagedDll.GetImageType());
      while (SAUnmanagedDll.s_hModule == IntPtr.Zero)
      {
        ++num;
        try
        {
          string path = string.Format("{0}{1}_{2}", tempDir, str2, num);
          SAUnmanagedDll.s_dllPath = string.Format("{0}\\{1}", path, "dbdata11.dll");
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
          else if (File.Exists(SAUnmanagedDll.s_dllPath))
          {
            try
            {
              File.Delete(SAUnmanagedDll.s_dllPath);
            }
            catch (Exception ex)
            {
              string @string = version.ToString();
              string str3 = @string.Substring(0, @string.Length - 1);
              SAUnmanagedDll.s_hModule = PInvokeMethods.LoadLibrary(SAUnmanagedDll.s_dllPath);
              if (SAUnmanagedDll.s_hModule != IntPtr.Zero)
              {
                foreach (ProcessModule module in (ReadOnlyCollectionBase) Process.GetCurrentProcess().Modules)
                {
                  if (string.Compare(module.ModuleName, "dbdata11.dll", true) == 0)
                  {
                    if (module.FileVersionInfo.FileVersion.StartsWith(str3))
                      return;
                    break;
                  }
                }
                PInvokeMethods.FreeLibrary(SAUnmanagedDll.s_hModule);
                SAUnmanagedDll.s_hModule = IntPtr.Zero;
                continue;
              }
              continue;
            }
          }
          SAUnmanagedDll.CreateDll();
          SAUnmanagedDll.s_hModule = PInvokeMethods.LoadLibrary(SAUnmanagedDll.s_dllPath);
        }
        catch (Exception ex)
        {
        }
        if (SAUnmanagedDll.s_lowMemory)
          throw new Exception(string.Format("Failed to load native dll ({0}), low disk space.", "dbdata11.dll"));
      }
    }

    private static string GetTempDir()
    {
      string[] strArray = new string[3]{ Path.GetTempPath().Trim(), Directory.GetCurrentDirectory().Trim(), Path.GetDirectoryName(SAUnmanagedDll.GetLocationFromCodeBase().Trim()) };
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
          FileStream fileStream = new FileStream(path2, FileMode.CreateNew);
          BinaryWriter binaryWriter = new BinaryWriter(fileStream);
          binaryWriter.Write(buffer);
          binaryWriter.Close();
          fileStream.Close();
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

    private static string GetImageType()
    {
      switch (SAUnmanagedDll.GetImageFileMachine())
      {
        case ImageFileMachine.AMD64:
          return ".x64";
        case ImageFileMachine.IA64:
          return ".ia64";
        default:
          return "";
      }
    }

    private static void CreateDll()
    {
      SAUnmanagedDll.s_lowMemory = false;
      string str = ".dbdata11.dll";
      using (FileStream fileStream = new FileStream(SAUnmanagedDll.s_dllPath, FileMode.CreateNew))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
        {
          using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof (SAUnmanagedDll).Namespace + str + SAUnmanagedDll.GetImageType()))
          {
            byte[] buffer = null;
            try
            {
              buffer = new byte[manifestResourceStream.Length];
              manifestResourceStream.Read(buffer, 0, (int) manifestResourceStream.Length);
            }
            catch (OutOfMemoryException ex)
            {
              SAUnmanagedDll.s_lowMemory = true;
            }
            if (SAUnmanagedDll.s_lowMemory)
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
      string str2 = Assembly.GetExecutingAssembly().GetName().CodeBase;
      if (str2.IndexOf(str1) == 0)
        str2 = str2.Substring(str1.Length);
      return str2.Replace('/', '\\');
    }

    private static ImageFileMachine GetImageFileMachine()
    {
      PortableExecutableKinds peKind;
      ImageFileMachine machine;
      Assembly.GetAssembly(typeof (int)).GetModules()[0].GetPEKind(out peKind, out machine);
      return machine;
    }
  }
}
