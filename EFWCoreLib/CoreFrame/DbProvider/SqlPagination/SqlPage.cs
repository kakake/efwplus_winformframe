using System;


namespace EFWCoreLib.CoreFrame.DbProvider.SqlPagination
{
    /// <summary>
    /// SQL语句进行分页包装
    /// </summary>
    public class SqlPage
    {
        /// <summary>
        /// 格式化SQL语句
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="pageInfo"></param>
        /// <param name="oleDb"></param>
        /// <returns></returns>
        public static string FormatSql(string strsql, PageInfo pageInfo, AbstractDatabase oleDb)
        {
            switch (oleDb.DbType)
            {
                case DatabaseType.IbmDb2:
                   return Db2FormatSql(strsql, pageInfo, oleDb);
                case DatabaseType.MsAccess:
                   return MsAccessFormatSql(strsql, pageInfo, oleDb);
                case DatabaseType.MySQL:
                   return MySQLFormatSql(strsql, pageInfo, oleDb);
                case DatabaseType.Oracle:
                   return OracleFormatSql(strsql, pageInfo, oleDb);
                case DatabaseType.SqlServer2000:
                   return Sql2000FormatSql(strsql, pageInfo, oleDb);
                case DatabaseType.SqlServer2005:
                   return Sql2005FormatSql(strsql, pageInfo, oleDb);
            }
            return null;
        }

        private static string Db2FormatSql(string strsql, PageInfo pageInfo, AbstractDatabase oleDb)
        {
            
            if (pageInfo.KeyName == null || pageInfo.KeyName == "")
                throw new Exception("分页KeyName属性不能为空，如：pageInfo.KeyName==\"Id\" 或 pageInfo.KeyName==\"Id|Desc\"");

            int starRecordNum = pageInfo.startNum;
            int endRecordNum = pageInfo.endNum;
            //int index = strsql.ToLower().LastIndexOf("order by");
            //string _strsql = null;
            //if (index != -1)
            //    _strsql = strsql.Remove(index);
            //else
            //    _strsql = strsql;

            string _strsql = strsql;


            string sql_totalRecord = "select count(*) from (" + _strsql + ") A";
            Object obj = oleDb.GetDataResult(sql_totalRecord);
            pageInfo.totalRecord = Convert.ToInt32(obj == DBNull.Value ? 0 : obj);

            string _sql = _strsql;
            string[] orderbys = pageInfo.KeyName.Split(new char[] { '|' });
            string orderbyname, orderby;
            if (orderbys.Length != 2)
            {
                orderbyname = orderbys[0];
                orderby = "desc";
            }
            else
            {
                orderbyname = orderbys[0];
                orderby = orderbys[1];
            }

            strsql = @"select * from (
                                select rownumber() over(order by {3} {4}) as rowid,  t.* from ({0}) t
                            )as a where a.rowid >= {1} AND  a.rowid < {2}";

            strsql = String.Format(strsql, _sql, starRecordNum, endRecordNum, orderbyname, orderby);
            return strsql;
        }
        private static string Sql2000FormatSql(string strsql, PageInfo pageInfo, AbstractDatabase oleDb)
        {
            return null;
        }
        private static string Sql2005FormatSql(string strsql, PageInfo pageInfo, AbstractDatabase oleDb)
        {
            if (pageInfo.KeyName == null || pageInfo.KeyName == "")
                throw new Exception("分页KeyName属性不能为空，如：pageInfo.KeyName==\"Id\" 或 pageInfo.KeyName==\"Id|Desc\"");

            int starRecordNum = pageInfo.startNum;
            int endRecordNum = pageInfo.endNum;
            int index = strsql.ToLower().LastIndexOf("order by");
            string _strsql = null;
            if (index != -1)
                _strsql = strsql.Remove(index);
            else
                _strsql = strsql;


            string sql_totalRecord = "select TOP 1 count(*) from (" + _strsql + ") A";
            Object obj = oleDb.GetDataResult(sql_totalRecord);
            pageInfo.totalRecord = Convert.ToInt32(obj == DBNull.Value ? 0 : obj);

            string _sql = _strsql;
            string[] orderbys = pageInfo.KeyName.Split(new char[] { '|' });
            string orderbyname, orderby;
            if (orderbys.Length != 2)
            {
                orderbyname = orderbys[0];
                orderby = "desc";
            }
            else
            {
                orderbyname = orderbys[0];
                orderby = orderbys[1];
            }

            strsql = @"select * from
                        (
                        select row_number() over(order by {3} {4}) as rownum,t.* from ({0}) t
                        ) as a where rownum between {1} and {2}";

            strsql = String.Format(strsql, _sql, starRecordNum, endRecordNum, orderbyname, orderby);
            return strsql;
        }
        private static string MsAccessFormatSql(string strsql, PageInfo pageInfo, AbstractDatabase oleDb)
        {
            return null;
        }
        private static string MySQLFormatSql(string strsql, PageInfo pageInfo, AbstractDatabase oleDb)
        {
            if (pageInfo.KeyName == null || pageInfo.KeyName == "")
                throw new Exception("分页KeyName属性不能为空，如：pageInfo.KeyName==\"Id\" 或 pageInfo.KeyName==\"Id|Desc\"");

            int starRecordNum = pageInfo.startNum;
            int endRecordNum = pageInfo.endNum;
            //int index = strsql.ToLower().LastIndexOf("order by");
            //string _strsql = null;
            //if (index != -1)
            //    _strsql = strsql.Remove(index);
            //else
            //    _strsql = strsql;

            string _strsql = strsql;


            string sql_totalRecord = "select count(*) from (" + _strsql + ") A";
            Object obj = oleDb.GetDataResult(sql_totalRecord);
            pageInfo.totalRecord = Convert.ToInt32(obj == DBNull.Value ? 0 : obj);

            string _sql = _strsql;
            string[] orderbys = pageInfo.KeyName.Split(new char[] { '|' });
            string orderbyname, orderby;
            if (orderbys.Length != 2)
            {
                orderbyname = orderbys[0];
                orderby = "desc";
            }
            else
            {
                orderbyname = orderbys[0];
                orderby = orderbys[1];
            }

            strsql = @"select * from (
                                select rownumber() over(order by {3} {4}) as rowid,  t.* from ({0}) t
                            )as a where a.rowid >= {1} AND  a.rowid < {2}";

            strsql = String.Format(strsql, _sql, starRecordNum, endRecordNum, orderbyname, orderby);
            return strsql;
        }
        private static string OracleFormatSql(string strsql, PageInfo pageInfo, AbstractDatabase oleDb)
        {
            int starRecordNum = pageInfo.startNum;
            int endRecordNum = pageInfo.endNum;

            string sql_totalRecord = "select count(*) from (" + strsql + ") A";
            Object obj = oleDb.GetDataResult(sql_totalRecord);
            pageInfo.totalRecord = Convert.ToInt32(obj == DBNull.Value ? 0 : obj);

            strsql = " select * from( select a.*,rownum rn from ( " + strsql + " ) a )  where rn between " + starRecordNum.ToString() + " and " + endRecordNum.ToString();
           
            return strsql;
        }
    }
}
