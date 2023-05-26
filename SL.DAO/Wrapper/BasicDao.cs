using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SL.DAO.Db;
using SL.Framework.Config;
using SL.Framework.Utility;

namespace SL.Dao.Services
{
    /// <summary>
    /// SingleTon 방식으로 호출한다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicDao<T> where T : class, new()
    {
        #region CONSTRUCTOR
        static T instance { get; set; }
        static readonly object lockObject = new object();

        public static T Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region DB TYPE

        /// <summary>
        /// 데이터베이스 종류
        /// </summary>
        protected enum TypeOfDatabase
        {
            Base,
            Csms,
            BaseService,
            CsmsService,
            INSA,
            CmsGlobal
        }

        #endregion

        #region CONNECTION STRING

        /// <summary>
        /// SP 실행시간 정의
        /// </summary>
        protected int? CommandExecuteTimeout { get; set; }

        /// <summary>
        /// OutPamram 추가여부
        /// </summary>
        protected bool isOutParam = true;

        #endregion

        #region GET CONNECTION STRING

        /// <summary>
        /// CONNECTION RETURN  
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        /// <remarks>
        /// 강제적으로 특정디비에 붙어야될 경우가 있어서 가상화함수로 정의한다.
        /// </remarks>
        protected string ConnectionString(TypeOfDatabase db)
        {
            return DaoConfig.DbConnectionString(string.Format("{0}Db", db.ToString()));
        }

        #endregion

        #region DATASET

        /// <summary>
        /// DATASET RETURN 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        protected virtual DataSet ExecuteDataset(TypeOfDatabase db, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (this.CommandExecuteTimeout != null)
                SqlHelper.CommandExecuteTimeout = CommandExecuteTimeout;
            return SqlHelper.ExecuteDataset(ConnectionString(db), commandType, commandText, commandParameters);
        }

        #endregion

        #region DATATABLE

        /// <summary>
        /// DATATABLE RETURN
        /// </summary>
        /// <param name="db"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        protected virtual DataTable ExecuteDataTable(TypeOfDatabase db, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (this.CommandExecuteTimeout != null)
                SqlHelper.CommandExecuteTimeout = CommandExecuteTimeout;

            List<SqlParameter> spParams = commandParameters.ToList();

            try
            {
                var ds = SqlHelper.ExecuteDataset(ConnectionString(db), commandType, commandText, spParams.ToArray());
                if (ds == null) return null;
                return ds.Tables.Count <= 0 ? null : ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Silicon 2 데이터 리턴방식
        /// </summary>
        /// <param name="db"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        protected virtual DataResultModel ExecuteDataResult(TypeOfDatabase db, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            DataResultModel returnValue = new DataResultModel();

            if (this.CommandExecuteTimeout != null)
                SqlHelper.CommandExecuteTimeout = CommandExecuteTimeout;
            List<SqlParameter> spParams = commandParameters.ToList();


            if (isOutParam)
            {
                try
                {
                    spParams.Add(new SqlParameter("@onResult", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    spParams.Add(new SqlParameter("@onRowCount", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    spParams.Add(new SqlParameter("@ovErrDesc", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output });
                }
                catch (Exception)
                {
                }
            }

            var ds = SqlHelper.ExecuteDataset(ConnectionString(db), commandType, commandText, spParams.ToArray());
            if (ds == null) return returnValue;
            try
            {
                returnValue.Data = ds.Tables.Count <= 0 ? null : ds.Tables[0];
                returnValue.onResult = Convert.ToInt32((spParams[spParams.Count - 3].Value == null) ? 0 : spParams[spParams.Count - 3].Value);
                returnValue.onRowCount = Convert.ToInt32((spParams[spParams.Count - 2].Value == null) ? 0 : spParams[spParams.Count - 2].Value);
                returnValue.ovErrDesc = spParams[spParams.Count - 1].Value.ToString();
            }
            catch (Exception)
            {
            }

            return returnValue;
        }

        /// <summary>
        /// UI에서 사용하는 방식으로 return한다.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected DataResult GeneratedResultData(DataResultModel dt)
        {
            #region [Data를 가공할 목적으로 사용]
            List<Hashtable> loadData = new List<Hashtable>();
            if (dt.Data == null) return new DataResult()
            {
                err = -1,
                data = new List<Hashtable>(),
                errMsg = "no data",
                rows = 0
            };

            try
            {
                foreach (DataRow item in dt.Data.Rows)
                {
                    Hashtable colData = new Hashtable();
                    foreach (DataColumn dc in dt.Data.Columns)
                    {
                        colData.Add(dc.ColumnName, item[dc.ColumnName].ToString().vDefaultValue());
                    }
                    loadData.Add(colData);
                }
                #endregion

                DataResult dr = new DataResult()
                {
                    err = dt.onResult,
                    errMsg = dt.ovErrDesc,
                    rows = dt.onRowCount,
                    data = loadData
                };
                return dr;
            }
            catch (Exception e)
            {
                return new DataResult()
                {
                    err = -1,
                    data = new List<Hashtable>(),
                    errMsg = e.Message,
                    rows = 0
                };
            }
        }

        #endregion

        #region DATAREADER

        /// <summary>
        /// DATAREDER RETURN
        /// </summary>
        /// <param name="db"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        protected virtual SqlDataReader ExecuteReader(TypeOfDatabase db, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteReader(ConnectionString(db), commandType, commandText, commandParameters);
        }

        #endregion

        #region NONQUERY

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        protected virtual int ExecuteNonQuery(TypeOfDatabase db, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString(db), commandType, commandText, commandParameters);
        }

        /// <summary>
        /// returnValue을 사용할 경우 사용한다.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        protected virtual int ExecuteNonQueryWithReturnValue(TypeOfDatabase db, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            var parameters = new List<SqlParameter>();
            for (int i = 0; i < commandParameters.Length; i++)
            {
                parameters.Add(commandParameters[i]);
            }
            parameters.Add(new SqlParameter("@QueryReturnValue", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
            return SqlHelper.ExecuteNonQuery(ConnectionString(db), commandType, commandText, parameters.ToArray());
        }

        #endregion

        #region ExecuteBulkInsert
        /// <summary>
        /// ExecuteBulkInsert : Bulk Insert를 실행한다.(데이터 Delete 후 Insert 진행)
        /// </summary>
        /// <param name="db">입력할 데이터</param>
        /// <param name="tableName">테이블명</param>
        /// <param name="dtData">DataTable Type(테이블의 컬럼명과 일치해야함)</param>
        /// <returns></returns>
        protected virtual DataResultModel ExecuteBulkInsert(TypeOfDatabase db, string tableName, DataTable dtData)
        {
            DataResultModel returnValue = new DataResultModel();

            try
            {
                //데이터 삭제
                SqlHelper.ExecuteScalar(ConnectionString(db), CommandType.Text, $"truncate table {tableName}");

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectionString(db), SqlBulkCopyOptions.TableLock))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(dtData);
                }

                returnValue.Data = dtData.Rows.Count <= 0 ? null : dtData;
                returnValue.onResult = 0;
                returnValue.onRowCount = dtData.Rows.Count;

            }
            catch (Exception e)
            {
                returnValue.Data = dtData.Rows.Count <= 0 ? null : dtData;
                returnValue.onResult = -1;
                returnValue.onRowCount = dtData.Rows.Count;
                returnValue.ovErrDesc = e.Message;
            }

            return returnValue;
        }
        #endregion
    }

    /// <summary>
    /// 현재 프로젝트 방식으로 Dao 호출
    /// </summary>
    public static class BasicDao
    {
        public static DataResultTypeOfCMS CallProcedure(Func<DataResultModel> callBack)
        {
            DataResultTypeOfCMS returnValue = new DataResultTypeOfCMS();
            try
            {
                var returnCallback = callBack();
                if (returnCallback == null)
                {
                    returnValue.err = 1;
                    returnValue.errMsg = "ERROR";
                    return returnValue;
                }
                else
                {
                    returnValue.err = returnCallback.onResult;
                    returnValue.errMsg = returnCallback.ovErrDesc;
                    returnValue.rows = (returnCallback.Data == null) ? 0 : returnCallback.Data.Rows.Count;
                    returnValue.data = returnCallback.Data.vMapperToHashtable();
                    returnValue.view = (returnCallback.Data == null) ? null : returnCallback.Data.vMapperToOneHashtable();
                    return returnValue;
                }
            }
            catch (Exception e)
            {
                returnValue.err = -1;
                returnValue.errMsg = e.Message;
                returnValue.rows = 0;
                return returnValue;
            }
        }
    }

}