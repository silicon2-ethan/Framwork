using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace SL.Framework.Utility
{
    public class ExcelHelper
    {
        // 서버가 64 BIT 일 경우 아래 2007 이상용으로 함께 작업가능
        // 확장명 XLS (Excel 97~2003 용)
        private const string Excel03ConnectString =
            "Provider=Microsoft.Jet.OLEDB.4.0;" +
            "Data Source=\"{0}\";" +
            "Mode=ReadWrite|Share Deny None;" +
            "Extended Properties='Excel 8.0; HDR={1};IMEX={2}';" +
            "Persist Security Info=False";

        // 확장명 XLSX (Excel 2007 이상용)
        private const string Excel07ConnectString =
            "Provider=Microsoft.ACE.OLEDB.12.0;" +
            "Data Source=\"{0}\";" +
            "Mode=ReadWrite|Share Deny None;" +
            "Extended Properties='Excel 12.0; HDR={1};IMEX={2}';" +
            "Persist Security Info=False";

        /// <summary>
        ///    Excel 파일의 형태를 반환한다.
        ///    -2 : Error  
        ///    -1 : 엑셀파일아님
        ///     0 : 97-2003 엑셀 파일 (xls)
        ///     1 : 2007 이상 파일 (xlsx)
        /// </summary>
        /// <param name="filePath">
        ///    Excel File 명 전체 경로입니다.
        /// </param>
        public static int ExcelFileType(string filePath)
        {
            byte[,] excelHeader =
            {
                {0xD0, 0xCF, 0x11, 0xE0, 0xA1}, // XLS  File Header
                {0x50, 0x4B, 0x03, 0x04, 0x14} // XLSX File Header
            };

            // result
            // -2=error
            // -1=not excel
            // 0=xls
            // 1=xlsx
            var result = -1;
            var fileInfo = new FileInfo(filePath);
            var fileStream = fileInfo.Open(FileMode.Open);

            try
            {
                var fh = new byte[5];
                fileStream.Read(fh, 0, 5);
                for (var i = 0; i < 2; i++)
                {
                    for (var j = 0; j < 5; j++)
                    {
                        if (fh[j] != excelHeader[i, j]) break;
                        if (j == 4) result = i;
                    }
                    if (result >= 0) break;
                }
            }
            catch
            {
                result = (-2);
            }
            finally
            {
                fileStream.Close();
            }
            return result;
        }

        /// <summary>
        /// EXCEL READER
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isHdr"></param>
        /// <returns></returns>
        public static DataSet ExcelReader(string filePath, bool isHdr)
        {
            DataSet ds;
            var connectString = string.Empty;
            var excelType = ExcelFileType(filePath);
            switch (excelType)
            {
                case (-2):
                    throw new Exception("형식검사중 오류가 발생하였습니다.");
                case (-1):
                    throw new Exception("엑셀 파일형식이 아닙니다.");
                case (0):
                case (1): /*OLE DB 12.0에서 오류가 발생함*/
                    if(Environment.Is64BitProcess == false)
                        connectString = string.Format($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={filePath};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"");
                    else
                        connectString = string.Format($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"");
                    break;
            }
            OleDbConnection oleDbConn = null;

            try
            {
                oleDbConn = new OleDbConnection(connectString);
                oleDbConn.Open();
                var schema = oleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] {null, null, null, "TABLE"});
                ds = new DataSet();
                foreach (DataRow dr in schema.Rows)
                {
                    var oleDbAdap = new OleDbDataAdapter(dr["TABLE_NAME"].ToString(), oleDbConn)
                    {
                        SelectCommand = {CommandType = CommandType.TableDirect},
                        AcceptChangesDuringFill = false
                    };
                    var tableName = dr["TABLE_NAME"].ToString().Replace("$", String.Empty).Replace("'", String.Empty);
                    if (dr["TABLE_NAME"].ToString().Contains("$")) oleDbAdap.Fill(ds, tableName);
                    break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (oleDbConn != null) oleDbConn.Close();
            }
            return ds;
        }

        /// <summary>
        ///    DataSet 을 Excel 파일로 저장한다.
        /// </summary>
        /// <param name="fileName">
        ///    Excel File 명 PullPath
        /// </param>
        /// <param name="ds">
        ///    Excel 로 저장할 대상 DataSet 객체.
        /// </param>
        /// <param name="existDel">
        ///    동일한 파일명이 있을 때 삭제 할 것인지 여부, 파일이 있고 false 면 저장안하고 그냥 false 를 리턴.
        /// </param>
        /// <param name="oldExcel">
        ///    xls 형태로 저장할 것인지 여부, false 이면 xlsx 형태로 저장함.
        /// </param>
        public static bool SaveExcel(string fileName, DataSet ds, bool existDel, bool oldExcel)
        {
            var result = true;
            if (!File.Exists(fileName))
            {
                if (existDel)
                    File.Delete(fileName);
                else
                    return false;
            }

            var tempFile = fileName;
            // 파일 확장자가 xls 이나 xlsx 가 아니면 아예 파일을 안만들어서
            // 템프파일로 생성후 지정한 파일명으로 변경..
            OleDbConnection oleDbConn = null;
            try
            {
                var connectString = "";
                if (oldExcel)
                {
                    tempFile = tempFile + ".xls";
                    connectString = string.Format(Excel07ConnectString, tempFile, "YES", "0");
                }
                else
                {
                    tempFile = tempFile + ".xlsx";
                    connectString = string.Format(Excel07ConnectString, tempFile, "YES", "0");
                }
                oleDbConn = new OleDbConnection(connectString);
                oleDbConn.Open();

                // Create Table(s).. : 테이블 단위 처리
                foreach (DataTable dt in ds.Tables)
                {
                    var tableName = dt.TableName;
                    var fldsInfo = new StringBuilder();
                    var flds = new StringBuilder();

                    // Create Field(s) String : 현재 테이블의 Field 명 생성
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (fldsInfo.Length > 0)
                        {
                            fldsInfo.Append(",");
                            flds.Append(",");
                        }
                        fldsInfo.Append("[" + column.ColumnName.Replace("'", "''") + "] CHAR(255)");
                        flds.Append(column.ColumnName.Replace("'", "''"));
                    }

                    // Table Create
                    var cmd = new OleDbCommand("CREATE TABLE " + tableName + "(" + fldsInfo.ToString() + ")", oleDbConn);
                    cmd.ExecuteNonQuery();

                    // Insert Data
                    foreach (DataRow dr in dt.Rows)
                    {
                        var values = new StringBuilder();
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (values.Length > 0) values.Append(",");
                            values.Append("'" + dr[column.ColumnName].ToString().Replace("'", "''") + "'");
                        }
                        cmd = new OleDbCommand(
                            "INSERT INTO [" + tableName + "$]" +
                            "(" + flds.ToString() + ") " +
                            "VALUES (" + values.ToString() + ")",
                            oleDbConn);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                if (oleDbConn != null) oleDbConn.Close();
                try
                {
                    if (File.Exists(tempFile))
                    {
                        File.Move(tempFile, fileName);
                    }
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// GC바로 호출하기  메모리 누수 방지
        /// </summary>
        /// <param name="obj"></param>
        public static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}