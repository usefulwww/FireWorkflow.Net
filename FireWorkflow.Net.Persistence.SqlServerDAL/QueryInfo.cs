/* Copyright 2009 无忧lwz0721@gmail.com
 * @author 无忧lwz0721@gmail.com
 */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace FireWorkflow.Net.Persistence.SqlServerDAL
{
    public class QueryInfo
    {

        #region 构造函数
        ///<summary>默认的构造函数。</summary>
        public QueryInfo()
        {
            this.QueryString = "";
            //this.QueryParameters = null;
            this.ListQueryParameters = null;
        }
        public QueryInfo(String queryString, List<SqlParameter> queryParameters)
        {
            this.QueryString = queryString;
            this.ListQueryParameters = queryParameters;
        }
        //public QueryInfo(String queryString, SqlParameter[] queryParameters)
        //{
        //    this.QueryString = queryString;
        //    this.QueryParameters = queryParameters;
        //}
        #endregion

        #region 属性定义
        /// <summary>查询条件</summary>
        public String QueryString { get; set; }

        /// <summary>查询条件</summary>
        public String QueryStringWhere { get { return (String.IsNullOrEmpty(QueryString)) ? "" : " WHERE " + QueryString; } }

        /// <summary>查询条件</summary>
        public String QueryStringAnd { get { return (String.IsNullOrEmpty(QueryString)) ? "" : " AND " + QueryString; } }

        /// <summary>查询需要传入的参数集</summary>
        public List<SqlParameter> ListQueryParameters { get; set; }

        /// <summary>查询需要传入的参数集</summary>
        //public SqlParameter[] QueryParameters { get; set; }

        #endregion
    }
}
