using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwimmingFunctions
{
    interface IComparePageService
    {
        Task<bool> GetPageAndCompare(string url, string pageName);
    }
}
