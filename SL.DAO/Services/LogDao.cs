using SL.Dao.Services;
using SL.DAO.Db;
using SL.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.DAO.Services
{
    /// <summary>
    /// 로그를 남긴다.
    /// </summary>
    public sealed class LogDao : BasicDao<LogDao>
    {
        /// <summary>
        /// USP_LOG
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="xmlSrch"></param>
        /// <param name="xmlCrud"></param>
        /// <returns></returns>
        public DataResultModel LoggingProcess(string mode, Dictionary<string, string> xmlSrch, string xmlCrud)
        {
            const string spName = "USP_LOG";
            SqlParameter[] sqlParams =
            {
                new SqlParameter("@ivMode", mode)
                , new SqlParameter("@ivXmlSrch", xmlSrch.GetXmlInputParameters(true))
                , new SqlParameter("@ivXmlCrud", xmlCrud)
            };
            return base.ExecuteDataResult(TypeOfDatabase.Base, CommandType.StoredProcedure, spName, sqlParams);
        }
    }
}
