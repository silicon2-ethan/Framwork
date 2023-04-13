using System;
using System.Text;
using System.Web;

namespace SL.Framework.Utility
{
    public static class PagingHelper
    {
        /// <summary>
        /// PagingLink
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="blockSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageNoParameterName"></param>
        /// <param name="removeParams"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public static string PagingLink(int totalCount, int pageSize, int blockSize, int pageNo,
            string pageNoParameterName,
            string[] removeParams, PagingInfo pagingInfo)
        {
            // 페이징 파라메터명 //
            pageNoParameterName = string.IsNullOrEmpty(pageNoParameterName) ? "pageNo" : pageNoParameterName;

            // QueryString 중 제거 파라메터 설정 //
            if (removeParams == null)
                removeParams = new string[] {};

            // pageNoParameterName 추가 위해 배열크기 조정 //
            Array.Resize(ref removeParams, removeParams.Length + 1);
            removeParams[removeParams.Length - 1] = pageNoParameterName;

            // QueryString 생성 //
            var queryString = QueryStringHelper.Create("", removeParams) ?? string.Empty;
            if (!string.IsNullOrEmpty(queryString))
                queryString = "&" + queryString;

            // 총페이지 수 //
            var maxPageNo = ((totalCount - 1)/pageSize) + 1;

            // 현재 블록 번호 //
            var nowBlockNo = ((pageNo - 1)/blockSize) + 1;

            // 현재 블로 시작 페이지 번호 //
            var nowBlockStartPageNo = ((nowBlockNo - 1)*blockSize) + 1;

            // 현재 블록 종료 페이지 번호 //
            var nowBlockEndPageNo = nowBlockNo*blockSize;

            if (nowBlockEndPageNo > maxPageNo)
                nowBlockEndPageNo = maxPageNo;

            // 페이징 생성 //
            var pagingTag = new StringBuilder();
            var movePageUrl = HttpContext.Current.Request.Path;
            const string tagFormatMove = @"<a href=""{0}"" {1}>{2}</a>";
            const string tagFormatNonMove = @"<a href=""{0}"" onclick=""return false;"" {1}>{2}</a>";

            // 맨처음 //
            if (pagingInfo.IsFirstLastMoveUse)
            {
                if (pageNo == 1)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.FirstMoveCss, pagingInfo.FirstMoveTag) +
                        Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove, movePageUrl + "?" + pageNoParameterName + "=1" + queryString,
                            pagingInfo.FirstMoveCss, pagingInfo.FirstMoveTag) + Environment.NewLine);
            }

            // 이전 그룹 페이지 이동 //
            if (pagingInfo.IsGroupMoveUse)
            {
                if (nowBlockNo == 1)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.PreGroupMoveCss,
                            pagingInfo.PreGroupMoveTag) + Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove,
                            movePageUrl + "?" + pageNoParameterName + "=" + (nowBlockStartPageNo - 1) + queryString,
                            pagingInfo.PreGroupMoveCss, pagingInfo.PreGroupMoveTag) + Environment.NewLine);
            }

            // 1단위 이전 페이지 이동 //
            if (pagingInfo.IsPreNextMoveUse)
            {
                if (pageNo == 1)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.PreMoveCss, pagingInfo.PreMoveTag) +
                        Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove,
                            movePageUrl + "?" + pageNoParameterName + "=" + (pageNo - 1) + queryString,
                            pagingInfo.PreMoveCss, pagingInfo.PreMoveTag) + Environment.NewLine);
            }

            // 페이지 번호 //
            for (var i = nowBlockStartPageNo; i <= nowBlockEndPageNo; i++)
            {
                if (pageNo == i)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.NowPageCss, i) + Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove, movePageUrl + "?" + pageNoParameterName + "=" + i + queryString,
                            pagingInfo.NomalPageCss, i) + Environment.NewLine);
            }

            // 1단위 다음 페이지 이동 //
            if (pagingInfo.IsPreNextMoveUse)
            {
                if (pageNo == maxPageNo)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.NextMoveCss, pagingInfo.NextMoveTag) +
                        Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove,
                            movePageUrl + "?" + pageNoParameterName + "=" + (pageNo + 1) + queryString,
                            pagingInfo.NextMoveCss, pagingInfo.NextMoveTag) + Environment.NewLine);
            }

            // 다음 그룹 페이지 이동 //
            if (pagingInfo.IsGroupMoveUse)
            {
                if (nowBlockEndPageNo == maxPageNo)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.NextGroupMoveCss,
                            pagingInfo.NextGroupMoveTag) + Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove,
                            movePageUrl + "?" + pageNoParameterName + "=" + (nowBlockEndPageNo + 1) + queryString,
                            pagingInfo.NextGroupMoveCss, pagingInfo.NextGroupMoveTag) + Environment.NewLine);
            }

            // 맨마지막 //
            if (pagingInfo.IsFirstLastMoveUse)
            {
                if (pageNo == maxPageNo)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.LastMoveCss, pagingInfo.LastMoveTag) +
                        Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove,
                            movePageUrl + "?" + pageNoParameterName + "=" + maxPageNo + queryString,
                            pagingInfo.LastMoveCss, pagingInfo.LastMoveTag) + Environment.NewLine);
            }

            return pagingTag.ToString();
        }

        /// <summary>
        /// PagingCallBack
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="blockSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="callScript"></param>
        /// <returns></returns>
        public static string PagingCallBack(int totalCount, int pageSize, int blockSize, int pageNo,
            PagingInfo pagingInfo, string callScript)
        {
            // 총페이지 수 //
            var maxPageNo = ((totalCount - 1)/pageSize) + 1;

            // 현재 블록 번호 //
            var nowBlockNo = ((pageNo - 1)/blockSize) + 1;

            // 현재 블로 시작 페이지 번호 //
            var nowBlockStartPageNo = ((nowBlockNo - 1)*blockSize) + 1;

            // 현재 블록 종료 페이지 번호 //
            var nowBlockEndPageNo = nowBlockNo*blockSize;

            if (nowBlockEndPageNo > maxPageNo)
                nowBlockEndPageNo = maxPageNo;

            // 페이징 생성 //
            var pagingTag = new StringBuilder();
            const string movePageUrl = "javascript:;";
            const string tagFormatMove = @"<a href=""{0}"" {1}>{2}</a>";
            const string tagFormatNonMove = @"<a href=""{0}"" onclick=""return false;"" {1}>{2}</a>";

            // 맨처음 //
            if (pagingInfo.IsFirstLastMoveUse)
            {
                if (pageNo == 1)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.FirstMoveCss, pagingInfo.FirstMoveTag) +
                        Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove, movePageUrl,
                            "onclick=\"" + callScript + "('1'); return false;\" " + pagingInfo.FirstMoveCss,
                            pagingInfo.FirstMoveTag) + Environment.NewLine);
            }

            // 이전 그룹 페이지 이동 //
            if (pagingInfo.IsGroupMoveUse)
            {
                if (nowBlockNo == 1)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.PreGroupMoveCss,
                            pagingInfo.PreGroupMoveTag) + Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove, movePageUrl,
                            "onclick=\"" + callScript + "(" + (nowBlockStartPageNo - 1) + "); return false;\" " +
                            pagingInfo.PreGroupMoveCss, pagingInfo.PreGroupMoveTag) + Environment.NewLine);
            }

            // 1단위 이전 페이지 이동 //
            if (pagingInfo.IsPreNextMoveUse)
            {
                if (pageNo == 1)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.PreMoveCss, pagingInfo.PreMoveTag) +
                        Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove, movePageUrl,
                            "onclick=\"" + callScript + "(" + (pageNo - 1) + "); return false;\" " +
                            pagingInfo.PreMoveCss, pagingInfo.PreMoveTag) + Environment.NewLine);
            }

            // 페이지 번호 //
            for (var i = nowBlockStartPageNo; i <= nowBlockEndPageNo; i++)
            {
                if (pageNo == i)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.NowPageCss, i) + Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove, movePageUrl,
                            "onclick=\"" + callScript + "(" + i + "); return false;\" " + pagingInfo.NomalPageCss, i) +
                        Environment.NewLine);
            }

            // 1단위 다음 페이지 이동 //
            if (pagingInfo.IsPreNextMoveUse)
            {
                if (pageNo == maxPageNo)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.NextMoveCss, pagingInfo.NextMoveTag) +
                        Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove, movePageUrl,
                            "onclick=\"" + callScript + "(" + (pageNo + 1) + "); return false;\" " +
                            pagingInfo.NextMoveCss, pagingInfo.NextMoveTag) + Environment.NewLine);
            }

            // 다음 그룹 페이지 이동 //
            if (pagingInfo.IsGroupMoveUse)
            {
                if (nowBlockEndPageNo == maxPageNo)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.NextGroupMoveCss,
                            pagingInfo.NextGroupMoveTag) + Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove, movePageUrl,
                            "onclick=\"" + callScript + "(" + (nowBlockEndPageNo + 1) + "); return false;\" " +
                            pagingInfo.NextGroupMoveCss, pagingInfo.NextGroupMoveTag) + Environment.NewLine);
            }

            // 맨마지막 //
            if (pagingInfo.IsFirstLastMoveUse)
            {
                if (pageNo == maxPageNo)
                    pagingTag.Append(
                        string.Format(tagFormatNonMove, movePageUrl, pagingInfo.LastMoveCss, pagingInfo.LastMoveTag) +
                        Environment.NewLine);
                else
                    pagingTag.Append(
                        string.Format(tagFormatMove, movePageUrl,
                            "onclick=\"" + callScript + "(" + maxPageNo + "); return false;\" " + pagingInfo.LastMoveCss,
                            pagingInfo.LastMoveTag) + Environment.NewLine);
            }

            return pagingTag.ToString();
        }
    }

    public class PagingInfo
    {
        /// <summary>
        /// 맨처음 맨마지막 사용유무
        /// </summary>
        public bool IsFirstLastMoveUse { get; set; }

        /// <summary>
        /// 맨처음 이동 이미지(이미지테그전체) 또는 텍스트
        /// </summary>
        public string FirstMoveTag { get; set; }

        /// <summary>
        /// 맨처음 이동 이미지(이미지테그전체) 또는 텍스트의 A태그 css적용 class명
        /// Ex)class="aaa"
        /// </summary>
        public string FirstMoveCss { get; set; }

        /// <summary>
        /// 맨마지막 이동 이미지(이미지테그전체) 또는 텍스트
        /// </summary>
        public string LastMoveTag { get; set; }

        /// <summary>
        /// 맨마지막 이동 이미지(이미지테그전체) 또는 텍스트 A태그 css적용 class명
        /// Ex)class="aaa"
        /// </summary>
        public string LastMoveCss { get; set; }

        /// <summary>
        /// 그룹단위 이동 사용유무
        /// </summary>
        public bool IsGroupMoveUse { get; set; }

        /// <summary>
        /// 이전 그룹 이동 이미지(이미지테그전체) 또는 텍스트
        /// </summary>
        public string PreGroupMoveTag { get; set; }

        /// <summary>
        /// 이전 그룹 이동 이미지(이미지테그전체) 또는 텍스트 A태그 css적용 class명
        /// Ex)class="aaa"
        /// </summary>
        public string PreGroupMoveCss { get; set; }

        /// <summary>
        /// 다음 그룹 이동 이미지(이미지테그전체) 또는 텍스트
        /// </summary>
        public string NextGroupMoveTag { get; set; }

        /// <summary>
        /// 다음 그룹 이동 이미지(이미지테그전체) 또는 텍스트 A태그 css적용 class명
        /// Ex)class="aaa"
        /// </summary>
        public string NextGroupMoveCss { get; set; }

        /// <summary>
        /// 1단위 이동 사용유무
        /// </summary>
        public bool IsPreNextMoveUse { get; set; }

        /// <summary>
        /// 이전 1단위 이동 이미지(이미지테그전체) 또는 텍스트
        /// </summary>
        public string PreMoveTag { get; set; }

        /// <summary>
        /// 이전 1단위 이동 이미지(이미지테그전체) 또는 텍스트 A태그 css적용 class명
        /// Ex)class="aaa"
        /// </summary>
        public string PreMoveCss { get; set; }

        /// <summary>
        /// 다음 1단위 이동 이미지(이미지테그전체) 또는 텍스트
        /// </summary>
        public string NextMoveTag { get; set; }

        /// <summary>
        /// 다음 1단위 이동 이미지(이미지테그전체) 또는 텍스트 A태그 css적용 class명
        /// Ex)class="aaa"
        /// </summary>
        public string NextMoveCss { get; set; }

        /// <summary>
        /// 현재페이지의 A태그 css적용 class명
        /// Ex)class="aaa"
        /// </summary>
        public string NowPageCss { get; set; }

        /// <summary>
        /// 현재페이지가 아닌 A태그 css적용 class명
        /// Ex)class="aaa"
        /// </summary>
        public string NomalPageCss { get; set; }
    }
}