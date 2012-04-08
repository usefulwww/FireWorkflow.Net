using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Web;

using FireWorkflow.Net.Persistence.OracleDAL;
using FireWorkflow.Net.Persistence.SQLiteDAL;
using FireWorkflow.Net.Persistence.SqlServerDAL;

//using System.Linq;









namespace WebDemo.Example.GoodsDeliverProcess.Persistence
{
	public class TradeInfoDAO
	{
		public const String SN = "sn";
		public const String GOODS_NAME = "goodsName";
		public const String GOODS_TYPE = "goodsType";
		public const String QUANTITY = "quantity";
		public const String UNIT_PRICE = "unitPrice";
		public const String AMOUNT = "amount";
		public const String CUSTOMER_NAME = "customerName";
		public const String CUSTOMER_MOBILE = "customerMobile";
		public const String CUSTOMER_PHONE_FAX = "customerPhoneFax";
		public const String CUSTOMER_ADDRESS = "customerAddress";
		public const String STATE = "state";
		string connectionString = "User Id=ISS;Password=webiss;Data Source=ism";

		private string dbtype ="sqlserver";

		public TradeInfoDAO()
		{
			dbtype = ConfigurationManager.AppSettings["dbtype"];
			switch (dbtype)
			{
				case "sqlserver":
					connectionString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
					break;
				case "oracle":
					connectionString = ConfigurationManager.ConnectionStrings["OracleServer"].ConnectionString;
					break;
				case "sqlite":
					connectionString = ConfigurationManager.ConnectionStrings["Sqlite"].ConnectionString;
					break;
				default:
					connectionString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
					break;
			}
		}

		protected void initDao()
		{
			
			// do nothing
		}

		public bool save(TradeInfo transientInstance)
		{
			switch(dbtype){
				case "sqlserver":
					return save_sqlserver(transientInstance);
				case "oracle":
					return save_oracle(transientInstance);
				case "slite":
					return save_sqlite(transientInstance);
				default:
					return save_sqlserver(transientInstance);
			}
			
		}
		
		public void delete(TradeInfo persistentInstance)
		{
			try
			{
			}
			catch
			{
				throw;
			}
		}

		public TradeInfo findById(String id)
		{
			switch (dbtype)
			{
				case "sqlserver":
					return findById_sqlserver(id);
				case "oracle":
					return findById_oracle(id);
				case "sqlite":
					return findById_sqlite(id);
				default:
					return findById_sqlserver(id);
			}
		}

		#region save
		private bool save_oracle(TradeInfo transientInstance){
			transientInstance.Id = Guid.NewGuid().ToString().Replace("-", "");
			string insert = "INSERT INTO T_BIZ_TRADEINFO (" +
				"ID, SN, GOODS_NAME, GOODS_TYPE, QUANTITY, " +
				"UNIT_PRICE, AMOUNT, CUSTOMER_NAME, CUSTOMER_MOBILE, CUSTOMER_PHONE_FAX, " +
				"CUSTOMER_ADDRESS, STATE, PAYED_TIME, DELIVERED_TIME )VALUES(:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11, :12, :13, :14)";
			OracleParameter[] insertParms = {
				OracleHelper.NewOracleParameter(":1", OracleType.VarChar, 50, transientInstance.Id),
				OracleHelper.NewOracleParameter(":2", OracleType.VarChar, 50, transientInstance.Sn),
				OracleHelper.NewOracleParameter(":3", OracleType.VarChar, 100, transientInstance.GoodsName),
				OracleHelper.NewOracleParameter(":4", OracleType.VarChar, 50, transientInstance.GoodsType),
				OracleHelper.NewOracleParameter(":5", OracleType.Number, transientInstance.Quantity),
				OracleHelper.NewOracleParameter(":6", OracleType.Double, 22, transientInstance.UnitPrice),
				OracleHelper.NewOracleParameter(":7", OracleType.Double, 22, transientInstance.Amount),
				OracleHelper.NewOracleParameter(":8", OracleType.VarChar, 50, transientInstance.CustomerName),
				OracleHelper.NewOracleParameter(":9", OracleType.VarChar, 30, transientInstance.CustomerMobile),
				OracleHelper.NewOracleParameter(":10", OracleType.VarChar, 30, transientInstance.CustomerPhoneFax),
				OracleHelper.NewOracleParameter(":11", OracleType.VarChar, 150, transientInstance.CustomerAddress),
				OracleHelper.NewOracleParameter(":12", OracleType.VarChar, 15, transientInstance.State),
				OracleHelper.NewOracleParameter(":13", OracleType.Timestamp, 11, transientInstance.PayedTime),
				OracleHelper.NewOracleParameter(":14", OracleType.Timestamp, 11, transientInstance.DeliveredTime)
			};
			if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
				return false;
			else return true;
		}

		private bool save_sqlserver(TradeInfo transientInstance)
		{
			transientInstance.Id = Guid.NewGuid().ToString("N");
			string insert = "INSERT INTO T_BIZ_TRADEINFO (" +
				"ID, SN, GOODS_NAME, GOODS_TYPE, QUANTITY, " +
				"UNIT_PRICE, AMOUNT, CUSTOMER_NAME, CUSTOMER_MOBILE, CUSTOMER_PHONE_FAX, " +
				"CUSTOMER_ADDRESS, STATE, PAYED_TIME, DELIVERED_TIME )VALUES(@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14)";
			SqlParameter[] insertParms = {
				SqlServerHelper.NewSqlParameter("@1",SqlDbType. VarChar, 50,transientInstance.Id),
				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, transientInstance.Sn),
				SqlServerHelper.NewSqlParameter("@3", SqlDbType.VarChar, 100, transientInstance.GoodsName),
				SqlServerHelper.NewSqlParameter("@4", SqlDbType.VarChar, 50, transientInstance.GoodsType),
				SqlServerHelper.NewSqlParameter("@5", SqlDbType.Decimal,38, transientInstance.Quantity),
				SqlServerHelper.NewSqlParameter("@6", SqlDbType.Decimal, 22, transientInstance.UnitPrice),
				SqlServerHelper.NewSqlParameter("@7", SqlDbType.Decimal, 22, transientInstance.Amount),
				SqlServerHelper.NewSqlParameter("@8", SqlDbType.VarChar, 50, transientInstance.CustomerName),
				SqlServerHelper.NewSqlParameter("@9", SqlDbType.VarChar, 30, transientInstance.CustomerMobile),
				SqlServerHelper.NewSqlParameter("@10", SqlDbType.VarChar, 30, transientInstance.CustomerPhoneFax),
				SqlServerHelper.NewSqlParameter("@11", SqlDbType.VarChar, 150, transientInstance.CustomerAddress),
				SqlServerHelper.NewSqlParameter("@12", SqlDbType.VarChar, 15, transientInstance.State),
				SqlServerHelper.NewSqlParameter("@13", SqlDbType.DateTime, 11, transientInstance.PayedTime),
				SqlServerHelper.NewSqlParameter("@14", SqlDbType.DateTime, 11, transientInstance.DeliveredTime)
			};

			if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
				return false;
			else return true;
		}

		private bool save_sqlite(TradeInfo transientInstance)
		{
			transientInstance.Id = Guid.NewGuid().ToString("N");
			string insert = "INSERT INTO T_BIZ_TRADEINFO (" +
				"ID, SN, GOODS_NAME, GOODS_TYPE, QUANTITY, " +
				"UNIT_PRICE, AMOUNT, CUSTOMER_NAME, CUSTOMER_MOBILE, CUSTOMER_PHONE_FAX, " +
				"CUSTOMER_ADDRESS, STATE, PAYED_TIME, DELIVERED_TIME )VALUES(@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14)";
			SqlParameter[] insertParms = {
				SqlServerHelper.NewSqlParameter("@1",SqlDbType. VarChar, 50,transientInstance.Id),
				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, transientInstance.Sn),
				SqlServerHelper.NewSqlParameter("@3", SqlDbType.VarChar, 100, transientInstance.GoodsName),
				SqlServerHelper.NewSqlParameter("@4", SqlDbType.VarChar, 50, transientInstance.GoodsType),
				SqlServerHelper.NewSqlParameter("@5", SqlDbType.Decimal,38, transientInstance.Quantity),
				SqlServerHelper.NewSqlParameter("@6", SqlDbType.Decimal, 22, transientInstance.UnitPrice),
				SqlServerHelper.NewSqlParameter("@7", SqlDbType.Decimal, 22, transientInstance.Amount),
				SqlServerHelper.NewSqlParameter("@8", SqlDbType.VarChar, 50, transientInstance.CustomerName),
				SqlServerHelper.NewSqlParameter("@9", SqlDbType.VarChar, 30, transientInstance.CustomerMobile),
				SqlServerHelper.NewSqlParameter("@10", SqlDbType.VarChar, 30, transientInstance.CustomerPhoneFax),
				SqlServerHelper.NewSqlParameter("@11", SqlDbType.VarChar, 150, transientInstance.CustomerAddress),
				SqlServerHelper.NewSqlParameter("@12", SqlDbType.VarChar, 15, transientInstance.State),
				SqlServerHelper.NewSqlParameter("@13", SqlDbType.DateTime, 11, transientInstance.PayedTime),
				SqlServerHelper.NewSqlParameter("@14", SqlDbType.DateTime, 11, transientInstance.DeliveredTime)
			};

			if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
				return false;
			else return true;
		}
		#endregion

		#region findById
		private TradeInfo findById_sqlserver(string id) {
			string select = "SELECT * FROM T_BIZ_TRADEINFO WHERE SN=@1";
			SqlConnection conn = new SqlConnection(connectionString);
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] selectParms = {
					SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, id)
				};
				reader = SqlServerHelper.ExecuteReader(conn, CommandType.Text, select, selectParms);
				if (reader.Read())
				{
					TradeInfo _TradeInfo = new TradeInfo()
					{
						Sn = Convert.ToString(reader["sn"]),
						GoodsName = Convert.ToString(reader["goods_name"]),
						UnitPrice = Convert.ToDouble(reader["unit_price"]),
						Quantity = Convert.ToInt64(reader["quantity"]),
						Amount = Convert.ToDouble(reader["amount"]),
						CustomerName = Convert.ToString(reader["customer_name"]),
						CustomerMobile = Convert.ToString(reader["customer_mobile"]),
						CustomerPhoneFax = Convert.ToString(reader["customer_phone_fax"])
					};
					return _TradeInfo;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (reader != null) reader.Close();
				if (conn.State != ConnectionState.Closed)
				{
					conn.Close();
					conn.Dispose();
				}
			}
			return null;
		}

		private TradeInfo findById_oracle(String id)
		{
			string select = "SELECT * FROM T_BIZ_TRADEINFO WHERE SN=:1";
			OracleConnection conn = new OracleConnection(connectionString);
			OracleDataReader reader = null;
			try
			{
				OracleParameter[] selectParms = {
					OracleHelper.NewOracleParameter(":1", OracleType.VarChar, 50, id)
				};
				reader = OracleHelper.ExecuteReader(conn, CommandType.Text, select, selectParms);
				if (reader.Read())
				{
					TradeInfo _TradeInfo = new TradeInfo()
					{
						Sn = Convert.ToString(reader["sn"]),
						GoodsName = Convert.ToString(reader["goods_name"]),
						UnitPrice = Convert.ToDouble(reader["unit_price"]),
						Quantity = Convert.ToInt64(reader["quantity"]),
						Amount = Convert.ToDouble(reader["amount"]),
						CustomerName = Convert.ToString(reader["customer_name"]),
						CustomerMobile = Convert.ToString(reader["customer_mobile"]),
						CustomerPhoneFax = Convert.ToString(reader["customer_phone_fax"])
					};
					return _TradeInfo;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (reader != null) reader.Close();
				if (conn.State != ConnectionState.Closed)
				{
					conn.Close();
					conn.Dispose();
				}
			}
			return null;
		}

		private TradeInfo findById_sqlite(string id) {
			string select = "SELECT * FROM T_BIZ_TRADEINFO WHERE SN=@1";
			SQLiteConnection conn = new SQLiteConnection(connectionString);
			SQLiteDataReader reader = null;
			try
			{
				SQLiteParameter[] selectParms = {
					SQLiteHelper.NewSqlParameter("@1", DbType.String, 50, id)
				};
				reader = SQLiteHelper.ExecuteReader(conn, CommandType.Text, select, selectParms);
				if (reader.Read())
				{
					TradeInfo _TradeInfo = new TradeInfo()
					{
						Sn = Convert.ToString(reader["sn"]),
						GoodsName = Convert.ToString(reader["goods_name"]),
						UnitPrice = Convert.ToDouble(reader["unit_price"]),
						Quantity = Convert.ToInt64(reader["quantity"]),
						Amount = Convert.ToDouble(reader["amount"]),
						CustomerName = Convert.ToString(reader["customer_name"]),
						CustomerMobile = Convert.ToString(reader["customer_mobile"]),
						CustomerPhoneFax = Convert.ToString(reader["customer_phone_fax"])
					};
					return _TradeInfo;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (reader != null) reader.Close();
				if (conn.State != ConnectionState.Closed)
				{
					conn.Close();
					conn.Dispose();
				}
			}
			return null;
		}
		#endregion

	}
}
