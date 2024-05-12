using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReStack.Common.Helpers
{
    public static class GitHelper
    {
        public static string GetSlug(string str)
        {
            var segments = str.Split('/');
            var name = string.Empty;

            if (str.Contains("dev.azure.com"))
                name = $"{segments[segments.Length - 4]}_{segments[segments.Length - 1]}";
            else
                name = $"{segments[segments.Length - 2]}_{segments[segments.Length - 1]}";

            return name.Replace(".git", string.Empty);
        }
    }
}
