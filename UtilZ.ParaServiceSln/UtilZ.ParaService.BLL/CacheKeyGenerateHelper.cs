using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.ParaService.BLL
{
    internal class CacheKeyGenerateHelper
    {
        internal static string GetVerCacheKey(long projectId)
        {
            return $"prj_{projectId}_cache_key";
        }

        internal static string GetProjectModuleParaValueCacheKey(long projectId, long moduleId, long version)
        {
            return $"pmpvck_{projectId}_{moduleId}_{version}_cache_key";
        }

        internal static string GetProjectAliasCacheKey(string projectAlias)
        {
            return $"pack_{projectAlias}_cache_key";
        }

        internal static string GetModuleAliasCacheKey(string moduleAlias)
        {
            return $"mack_{moduleAlias}_cache_key";
        }
    }
}
