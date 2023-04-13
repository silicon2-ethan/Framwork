using System;

namespace SL.Framework.Extension
{
    /// <summary>
    /// EXCEPTION EXTENSION CLASS
    /// </summary>
    public class ExceptionExtension : Exception
    {
        /// <summary>
        /// SystemException
        /// </summary>
        public Exception SystemException { get; private set; }

        /// <summary>
        /// ExceptionType
        /// </summary>
        public ExceptionTypes ExceptionType { get; private set; }

        /// <summary>
        /// ExceptionMessage
        /// </summary>
        public string ExceptionMessage { get; private set; }

        ///// <summary>
        ///// ExceptionParam
        ///// </summary>
        //public string ExceptionParam { get; private set; }

        ///// <summary>
        ///// ExceptionMethod
        ///// </summary>
        //public string ExceptionMethod { get; private set; }

        ///// <summary>
        ///// ExceptionClass
        ///// </summary>
        //public string ExceptionClass { get; private set; }

        ///// <summary>
        ///// ExceptionLineNumber
        ///// </summary>
        //public int ExceptionLineNumber { get; private set; }

        /// <summary>
        /// ExceptionExtension
        /// </summary>
        /// <param name="systemException"></param>
        public ExceptionExtension(Exception systemException)
            : base(systemException.Message)
        {
            SystemException = systemException;
            ExceptionMessage = systemException.Message;
        }

        /// <summary>
        /// ExceptionExtension
        /// </summary>
        /// <param name="systemException"></param>
        /// <param name="exceptionMessage"></param>
        public ExceptionExtension(Exception systemException, string exceptionMessage)
            : base(systemException.Message)
        {
            SystemException = systemException;
            ExceptionMessage = exceptionMessage;
        }

        /// <summary>
        /// ExceptionExtension
        /// </summary>
        /// <param name="exceptionType"></param>
        public ExceptionExtension(ExceptionTypes exceptionType)
            : base(string.Empty)
        {
            ExceptionType = exceptionType;
        }

        /// <summary>
        /// ExceptionExtension
        /// </summary>
        /// <param name="exceptionType"></param>
        /// <param name="exceptionMessage"></param>
        public ExceptionExtension(ExceptionTypes exceptionType, string exceptionMessage)
            : base(exceptionMessage)
        {
            ExceptionType = exceptionType;
            ExceptionMessage = exceptionMessage;
        }

        /// <summary>
        /// ExceptionExtension
        /// </summary>
        /// <param name="exceptionMessage"></param>
        public ExceptionExtension(string exceptionMessage)
            : base(exceptionMessage)
        {
            ExceptionMessage = exceptionMessage;
        }

        ///// <summary>
        ///// ExceptionExtension
        ///// </summary>
        ///// <param name="exceptionMessage"></param>
        ///// <param name="exceptionParam"></param>
        //public ExceptionExtension(string exceptionMessage, string exceptionParam = null)
        //    : base(exceptionMessage)
        //{
        //    ExceptionMessage = exceptionMessage;
        //    ExceptionParam = exceptionParam;
        //}

        ///// <summary>
        ///// ExceptionExtension
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="exceptionMessage"></param>
        ///// <param name="exceptionParam"></param>
        ///// <param name="exceptionMethod"></param>
        ///// <param name="exceptionClass"></param>
        ///// <param name="exceptionLineNumber"></param>
        //public ExceptionExtension(ExceptionTypes type, string exceptionMessage = null, string exceptionParam = null,
        //    [CallerMemberName] string exceptionMethod = null, [CallerFilePath] string exceptionClass = null,
        //    [CallerLineNumber] int exceptionLineNumber = 0)
        //    : base(exceptionMessage)
        //{
        //    ExceptionType = type;
        //    ExceptionMessage = exceptionMessage;
        //    ExceptionParam = exceptionParam;
        //    ExceptionMethod = exceptionMethod;
        //    ExceptionClass = exceptionClass;
        //    ExceptionLineNumber = exceptionLineNumber;
        //}

        ///// <summary>
        ///// ExceptionExtension
        ///// </summary>
        ///// <param name="systemException"></param>
        ///// <param name="exceptionType"></param>
        ///// <param name="exceptionMessage"></param>
        ///// <param name="exceptionParam"></param>
        ///// <param name="exceptionMethod"></param>
        ///// <param name="exceptionClass"></param>
        ///// <param name="exceptionLineNumber"></param>
        //public ExceptionExtension(Exception systemException, ExceptionTypes exceptionType = ExceptionTypes.Undefined,
        //    string exceptionMessage = null, string exceptionParam = null,
        //    [CallerMemberName] string exceptionMethod = null, [CallerFilePath] string exceptionClass = null,
        //    [CallerLineNumber] int exceptionLineNumber = 0)
        //    : base(exceptionMessage, systemException)
        //{
        //    SystemException = systemException;
        //    ExceptionType = exceptionType;
        //    ExceptionMessage = exceptionMessage;
        //    ExceptionParam = exceptionParam;
        //    ExceptionMethod = exceptionMethod;
        //    ExceptionClass = exceptionClass;
        //    ExceptionLineNumber = exceptionLineNumber;
        //}

        ///// <summary>
        ///// ExceptionExtension message
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="exceptionMessage"></param>
        ///// <param name="exceptionParam"></param>
        ///// <param name="exceptionMethod"></param>
        ///// <param name="exceptionClass"></param>
        ///// <param name="exceptionLineNumber"></param>
        ///// <returns></returns>
        //private static string ExceptionBaseMessage(ExceptionTypes type, string exceptionMessage, string exceptionParam,
        //    string exceptionMethod, string exceptionClass, int exceptionLineNumber)
        //{
        //    return string.Format("{0} : {1}, {2} : {3}, {4} : {5}, {6} : {7}, {8} : {9}, {10} : {11}"
        //        , "type", type.ToString("")
        //        , "message", exceptionMessage
        //        , "param", exceptionParam
        //        , "method", exceptionMethod
        //        , "class", exceptionClass
        //        , "line", exceptionLineNumber);
        //}
    }
}