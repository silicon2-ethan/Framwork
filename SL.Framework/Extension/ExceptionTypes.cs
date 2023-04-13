
namespace SL.Framework.Extension
{
    /// <summary>
    /// EXCEPTION TYPES ENUM
    /// </summary>
    public enum ExceptionTypes : int
    {
        #region 정의되지 않은 에러

        Undefined = 10000,

        #endregion

        #region COMMON [20000~30000]

        InvalidParam = 20000,

        LoginAuthFail = 20001,

        #endregion

        #region BIZ [30000~40000]

        BIzCommonFail = 30000,

        BIzInsertFail = 30001,

        BIzSelectFail = 30002,

        BIzUpdateFail = 30003,

        BizDeleteFail = 30004,

        #endregion

        #region DB [40000~50000]

        DbCommonFail = 40000,

        DbInsertFail = 40001,

        DbSelectFail = 40002,

        DbUpdateFail = 40003,

        DbDeleteFail = 40004,

        #endregion

        #region FILE [50000~60000]

        FileCommonFail = 50000,

        FileCreateFail = 50001,

        FileReadFail = 50002,

        FileUpdateFail = 50003,

        FileDeleteFail = 50004

        #endregion
    }
}