using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace SL.DAO.Db
{
    /// <summary>
    /// 결과모델(신규 SP)
    /// </summary>
    public sealed class DataResultModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int onResult { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int onRowCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ovErrDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DataTable Data { get; set; }

        public static Boolean IsValid(DataResultModel model, bool checkEmptyRow = true)
        {
            if (model == null || model.onResult != 0)
            {
                return false;
            }

            DataTable tblData = model.Data;

            if (tblData == null || tblData.Rows == null)
            {
                return false;
            }

            if (checkEmptyRow == true && tblData.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// CSMS UI에서 사용하는 공용 Result 형식
    /// </summary>
    public sealed class DataResult
    {
        public int err { get; set; }
        public string errMsg { get; set; }
        public int rows { get; set; }
        public IList<Hashtable> data { get; set; }
        public Hashtable view { get; set; }
    }

    /// <summary>
    /// Return Model (CMS 타입)
    /// </summary>
    public sealed class DataResultTypeOfCMS
    {
        public int err { get; set; }
        public string errMsg { get; set; }
        public int rows { get; set; }

        public IList<Hashtable> data { get; set; }
        public Hashtable view { get; set; }
    }
}
