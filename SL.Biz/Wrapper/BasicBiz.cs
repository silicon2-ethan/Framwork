
using SL.CSMS.Common.Entities.DB;
using SL.CSMS.Dao.Services;
using SL.Framework.Utility;
using System;
using System.Linq;

namespace SL.Biz.Services
{
    public static class RepleaseProduct
    {
        /// <summary>
        /// 품목정보 조회
        /// </summary>
        /// <param name="prod_cd"></param>
        public static string GetProdData(this string prod_cd)
        {
            var e = new USP_PROD_SELECT() { mode = "R9A", prod_cd = prod_cd };
            var xml = CollectionHelper
                                .vMapperToDictionary<USP_PROD_SELECT>(e)
                                .GetXmlInputParameters(true);
            var data = ProductsDao.Instance.GetProdList(e.mode, xml, null);

            if (data.Data.Rows.Count <= 0) return prod_cd;
            return data.Data.Rows[0]["prod_cd"].ToString().vDefaultValue();
        }
    }

    /// <summary>
    /// SingleTon 방식으로 호출한다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicBiz<T> where T : class, new()
    {
        //AGV 사용가능한 사이트 정보
        readonly string[] isUseableSite = { "CO000001", "CO000011", "CO000012" }; //실리콘투, 3PL

        /// <summary>
        /// AGV 사용가능한 사이트 정보
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected bool IsUseableSite(string val)
        {
            if (isUseableSite.FirstOrDefault(x => x.Equals(val.vDefaultValue(""))) != null) //agv 를 연동할 수 있는 경우만 호출한다.
            {
                return true;
            }
            return false;
        }

        static readonly object lockObject = new object();
        static T instance { get; set; }

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

        /// <summary>
        /// SiteCd별 현지 시간 가지고 오기
        /// </summary>
        protected Func<string, string> GetLocalTime = ((siteCD) =>
        {
            string returnValue = string.Empty;
            switch (siteCD)
            {
                case "CO000007": //미주
                    returnValue = DateTime.UtcNow.ConvertTimeFromUtc(zone: "Pacific Standard Time");
                    break;
                case "CO000009": //인도네시아
                    returnValue = DateTime.UtcNow.ConvertTimeFromUtc(zone: "SE Asia Standard Time");
                    break;
                case "CO000010": //칠레
                    returnValue = DateTime.UtcNow.ConvertTimeFromUtc(zone: "Argentina Standard Time");
                    break;
                default:
                    returnValue = DateTime.UtcNow.ConvertTimeFromUtc(zone: "Korea Standard Time");
                    break;
            }
            return returnValue;
        });
    }
}