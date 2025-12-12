using System;

namespace MyLib
{
    public static class PlatformInfo
    {
        #if NETFRAMEWORK
        public static string GetRuntimeLabel() => "Framework-48";
        #else
            public static string GetRuntimeLabel() => "Modern-8";
        #endif
        /*
        public static string GetRuntimeLabel()
        {
            var runtime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            return $"Running on: {runtime}";
        }
        */
    }
}
