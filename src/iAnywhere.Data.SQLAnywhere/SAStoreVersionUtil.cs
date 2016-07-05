﻿using System;

namespace iAnywhere.Data.SQLAnywhere
{
    internal class SAStoreVersionUtil
    {
        private static string[] s_manifestTokens = new string[4] { "10", "11", "12", "15" };
        private static SAStoreVersion[] s_versions = new SAStoreVersion[4] { SAStoreVersion.SA10, SAStoreVersion.SA11, SAStoreVersion.SA12, SAStoreVersion.IQ15 };

        public static bool IsValidVersion(SAStoreVersion ver)
        {
            for (int index = 0; index < SAStoreVersionUtil.s_versions.GetLength(0); ++index)
            {
                if (ver == SAStoreVersionUtil.s_versions[index])
                    return true;
            }
            return false;
        }

        public static string GetManifestToken(string serVer)
        {
            foreach (string sManifestToken in SAStoreVersionUtil.s_manifestTokens)
            {
                if (serVer.StartsWith(sManifestToken, StringComparison.Ordinal))
                    return sManifestToken;
            }
            throw new ArgumentException("Server version not supported.");
        }

        public static SAStoreVersion GetVersion(string manifestToken)
        {
            for (int index = 0; index < SAStoreVersionUtil.s_manifestTokens.GetLength(0); ++index)
            {
                if (manifestToken.Equals(SAStoreVersionUtil.s_manifestTokens[index]))
                    return SAStoreVersionUtil.s_versions[index];
            }
            throw new ArgumentException("Server version not supported.");
        }
    }
}