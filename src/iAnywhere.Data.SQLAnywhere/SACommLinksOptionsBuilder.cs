using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace iAnywhere.Data.SQLAnywhere
{

    public sealed class SACommLinksOptionsBuilder
    {
        private bool _useLongNameAsKeyword = true;
        private static SAUnmanagedDll s_unmanagedDll = SAUnmanagedDll.Instance;
        private static Hashtable s_ShortForms = new Hashtable();
        private const string c_sharedMemKey = "SharedMemory";
        private const string c_tcpipKey = "TCPIP";
        private const string c_allKey = "All";
        private Hashtable _linksOptions;

        public string ConnectionString
        {
            get
            {
                return ToString();
            }
            set
            {
                Init(value);
            }
        }

        public bool SharedMemory
        {
            get
            {
                return (bool)_linksOptions["SharedMemory"];
            }
            set
            {
                _linksOptions["SharedMemory"] = value;
            }
        }

        public bool All
        {
            get
            {
                return (bool)_linksOptions["All"];
            }
            set
            {
                _linksOptions["All"] = value;
            }
        }

        public SATcpOptionsBuilder TcpOptionsBuilder
        {
            get
            {
                return (SATcpOptionsBuilder)_linksOptions["TCPIP"];
            }
            set
            {
                _linksOptions["TCPIP"] = value;
            }
        }

        public string TcpOptionsString
        {
            get
            {
                return _linksOptions["TCPIP"].ToString();
            }
            set
            {
                SATcpOptionsBuilder tcpOptionsBuilder = new SATcpOptionsBuilder();
                tcpOptionsBuilder.SetUseLongNameAsKeyword(_useLongNameAsKeyword);
                tcpOptionsBuilder.ConnectionString = value;
                _linksOptions["TCPIP"] = tcpOptionsBuilder;
            }
        }

        static SACommLinksOptionsBuilder()
        {
            SACommLinksOptionsBuilder.s_ShortForms["SharedMemory"] = "shmem";
            SACommLinksOptionsBuilder.s_ShortForms["TCPIP"] = "tcp";
            SACommLinksOptionsBuilder.s_ShortForms["All"] = "all";
        }

        public SACommLinksOptionsBuilder()
          : this("")
        {
        }

        public SACommLinksOptionsBuilder(string options)
        {
            Init(options);
        }

        public void SetUseLongNameAsKeyword(bool useLongNameAsKeyword)
        {
            _useLongNameAsKeyword = useLongNameAsKeyword;
        }

        public bool GetUseLongNameAsKeyword()
        {
            return _useLongNameAsKeyword;
        }

        string FindKey(string key)
        {
            foreach (DictionaryEntry sShortForm in s_ShortForms)
            {
                if (string.Compare(key, sShortForm.Key as string, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(key, sShortForm.Value as string, StringComparison.OrdinalIgnoreCase) == 0)
                    return sShortForm.Key as string;
            }
            return null;
        }

        string GetKeyword(string key)
        {
            if (!_useLongNameAsKeyword)
                return SACommLinksOptionsBuilder.s_ShortForms[key] as string;
            return key;
        }

        void Init(string options)
        {
            _linksOptions = new Hashtable();
            _linksOptions["SharedMemory"] = false;
            _linksOptions["All"] = false;
            _linksOptions["TCPIP"] = null;
            int numResults = 0;
            IntPtr result = IntPtr.Zero;
            try
            {
                switch (PInvokeMethods.SAConnectionStringBuilder_ParseLinksOptions(options, ref numResults, ref result))
                {
                    case 1:
                        throw new SAException(SARes.GetString(7941));
                    case 6:
                        throw new SAException(SARes.GetString(14952, options));
                    case 4:
                        throw new SAException(SARes.GetString(17439, options));
                    default:
                        var saPortInfoPtr = Marshal.PtrToStructure<SAPortInfo>(result);
                        for (int index = 0; index < numResults; ++index)
                        {
                            string key = FindKey(Marshal.PtrToStructure<string>(saPortInfoPtr.Type));
                            if ("SharedMemory".Equals(key, StringComparison.OrdinalIgnoreCase))
                                SharedMemory = true;
                            else if ("TCPIP".Equals(key, StringComparison.OrdinalIgnoreCase))
                            {
                                TcpOptionsString = Marshal.PtrToStructure<string>(saPortInfoPtr.Options);
                            }
                            else
                            {
                                if (!"All".Equals(key, StringComparison.OrdinalIgnoreCase))
                                    throw new SAException(SARes.GetString(7760));
                                All = true;
                            }
                            //TODO: Fix SAConnectionStringBuilder_ParseLinksOptions
                            //++result;                            
                        }
                        break;
                }
            }
            finally
            {
                SAException.CheckException(PInvokeMethods.SAConnectionStringBuilder_FreeLinksOptions(numResults, result));
            }
        }

        /// <summary>
        ///     <para>Converts the SACommLinksOptionsBuilder object to a string representation.</para>
        /// </summary>
        /// <returns>
        /// <para>The options string being built.</para>
        ///    </returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (_linksOptions["SharedMemory"].Equals(true))
                stringBuilder.Append(GetKeyword("SharedMemory"));
            if (_linksOptions["All"].Equals(true))
            {
                stringBuilder.Append(stringBuilder.Length > 0 ? "," : "");
                stringBuilder.Append(GetKeyword("All"));
            }
            if (_linksOptions["TCPIP"] != null)
            {
                stringBuilder.Append(stringBuilder.Length > 0 ? "," : "");
                stringBuilder.Append(_linksOptions["TCPIP"].ToString());
            }
            return stringBuilder.ToString();
        }
    }
}
