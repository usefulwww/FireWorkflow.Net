/* Copyright 2009 无忧lwz0721@gmail.com
 * @author 无忧lwz0721@gmail.com
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace FireWorkflow.Net.Persistence.SQLiteDAL
{
    public class QueryField
    {
        public IList<QueryFieldInfo> QueryFieldInfos { get; set; }
        public IList<QueryFieldInfo> QueryFieldInfosOr { get; set; }
        public QueryField()
        {
            QueryFieldInfos = new List<QueryFieldInfo>();
            QueryFieldInfosOr = new List<QueryFieldInfo>();
        }

        public void Add(QueryFieldInfo queryFieldInfo)
        {
            if (String.IsNullOrEmpty(queryFieldInfo.QueryString)) return;
            QueryFieldInfos.Add(queryFieldInfo);
        }
        public void AddOr(QueryFieldInfo queryFieldInfo)
        {
            if (String.IsNullOrEmpty(queryFieldInfo.QueryString.Trim())) return;
            QueryFieldInfosOr.Add(queryFieldInfo);
        }

        public int Count
        {
            get
            {
                return QueryFieldInfos.Count + QueryFieldInfosOr.Count;
            }
        }
    }

    /// <summary>查询参数实体类 QueryFieldInfo 。</summary>
    [Serializable]
    public class QueryFieldInfo
    {
        #region 构造函数

        /// <summary>构造函数</summary>
        /// <param name="FieldName">字段名称</param>
        /// <param name="FieldType">类型</param>
        /// <param name="QueryString">查询条件</param>
        public QueryFieldInfo(String FieldName, CSharpType FieldType, String QueryString)
        {
            this.FieldName = FieldName;
            this.FieldType = FieldType;
            this.QueryString = QueryString;
        }

        ///<summary>默认的构造函数。</summary>
        public QueryFieldInfo()
        {
        }
        #endregion

        #region 属性定义
        /// <summary>字段名称</summary>
        public String FieldName { get; set; }

        /// <summary>类型</summary>
        public CSharpType FieldType { get; set; }

        /// <summary>查询条件</summary>
        public String QueryString { get; set; }

        #endregion
    }

    public enum CSharpType
    {
        String,
        Int16,
        Int32,
        Boolean,
        DateTime,
        Decimal,
        Guid,
        Byte,
        //Bytes,
        Char,
        Chars,
        //Object,
        Double,
        Single
    }
}
