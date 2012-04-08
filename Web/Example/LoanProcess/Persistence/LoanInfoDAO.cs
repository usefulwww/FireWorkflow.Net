using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Data.SQLite;
using FireWorkflow.Net.Persistence.SQLiteDAL;
using System.Configuration;
using FireWorkflow.Net.Persistence.OracleDAL;
using System.Data.SqlClient;
using FireWorkflow.Net.Persistence.SqlServerDAL;

namespace WebDemo.Example.LoanProcess.Persistence
{
	public class LoanInfoDAO
	{
		string connectionString = "User Id=ISS;Password=webiss;Data Source=ism";
		private string dbtype = "sqlserver";
		
		public LoanInfoDAO()
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

		public bool attachDirty(LoanInfo instance)
		{
			switch (dbtype)
			{
				case "sqlserver":
					return attachDirty_sqlserver(instance);
				case "oracle":
					return attachDirty_oracle(instance);
				case "sqlite":
					return attachDirty_oracle(instance);
				default:
					return attachDirty_sqlserver(instance);
			}
		}
		
		public LoanInfo findBySn(Object sn)
		{
			switch (dbtype)
			{
				case "sqlserver":
					return findBySn_sqlserver(sn);
				case "oracle":
					return findBySn_oracle(sn);
				case "sqlite":
					return findBySn_sqlite(sn);
				default:
					return findBySn_sqlserver(sn);
			}
		}

		#region attachDirty
		public bool attachDirty_sqlserver(LoanInfo instance)
		{
			if (string.IsNullOrEmpty(instance.Id))
			{
				instance.Id = Guid.NewGuid().ToString().Replace("-", "");
				string insert = "INSERT INTO T_BIZ_LOANINFO (" +
					"ID, SN, APPLICANT_NAME, APPLICANT_ID, ADDRESS, " +
					"SALARY, LOAN_VALUE, RETURN_DATE, LOANTELLER, APP_INFO_INPUT_DATE, " +
					"SALARY_IS_REAL, CREDIT_STATUS, RISK_FLAG, RISK_EVALUATOR, RISK_INFO_INPUT_DATE, " +
					"DECISION, EXAMINERLIST, APPROVERLIST, OPPONENTLIST, LEND_MONEY_INFO, " +
					"LEND_MONEY_OFFICER, LEND_MONEY_INFO_INPUT_TIME, REJECT_INFO, REJECT_INFO_INPUT_TIME )"+
					"VALUES(@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16, @17, @18, @19, @20, @21, @22, @23, @24)";
				SqlParameter[] insertParms = {
					SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, instance.Id),
					SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, instance.Sn),
					SqlServerHelper.NewSqlParameter("@3", SqlDbType.VarChar, 50, instance.ApplicantName),
					SqlServerHelper.NewSqlParameter("@4", SqlDbType.VarChar, 50, instance.ApplicantId),
					SqlServerHelper.NewSqlParameter("@5", SqlDbType.VarChar, 256, instance.Address),
					SqlServerHelper.NewSqlParameter("@6", SqlDbType.Int, instance.Salary),
					SqlServerHelper.NewSqlParameter("@7", SqlDbType.Int, instance.LoanValue),
					SqlServerHelper.NewSqlParameter("@8", SqlDbType.VarChar, 10, instance.ReturnDate),
					SqlServerHelper.NewSqlParameter("@9", SqlDbType.VarChar, 50, instance.Loanteller),
					SqlServerHelper.NewSqlParameter("@10", SqlDbType.DateTime, 11, instance.AppInfoInputDate),
					SqlServerHelper.NewSqlParameter("@11", SqlDbType.SmallInt, OracleHelper.OraBit(instance.SalaryIsReal)),
					SqlServerHelper.NewSqlParameter("@12", SqlDbType.SmallInt, OracleHelper.OraBit(instance.CreditStatus)),
					SqlServerHelper.NewSqlParameter("@13", SqlDbType.SmallInt, OracleHelper.OraBit(instance.RiskFlag)),
					SqlServerHelper.NewSqlParameter("@14", SqlDbType.VarChar, 50, instance.RiskEvaluator),
					SqlServerHelper.NewSqlParameter("@15", SqlDbType.DateTime, 11, instance.RiskInfoInputDate),
					SqlServerHelper.NewSqlParameter("@16", SqlDbType.SmallInt, OracleHelper.OraBit(instance.Decision)),
					SqlServerHelper.NewSqlParameter("@17", SqlDbType.VarChar, 128, instance.ExaminerList),
					SqlServerHelper.NewSqlParameter("@18", SqlDbType.VarChar, 128, instance.ApproverList),
					SqlServerHelper.NewSqlParameter("@19", SqlDbType.VarChar, 128, instance.OpponentList),
					SqlServerHelper.NewSqlParameter("@20", SqlDbType.VarChar, 256, instance.LendMoneyInfo),
					SqlServerHelper.NewSqlParameter("@21", SqlDbType.VarChar, 50, instance.LendMoneyOfficer),
					SqlServerHelper.NewSqlParameter("@22", SqlDbType.DateTime, 11, instance.LendMoneyInfoInputTime),
					SqlServerHelper.NewSqlParameter("@23", SqlDbType.VarChar, 256, instance.RejectInfo),
					SqlServerHelper.NewSqlParameter("@24", SqlDbType.DateTime, 11, instance.RejectInfoInputTime)
				};
				if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
					return false;
				else return true;
			}
			else
			{
				string update = "UPDATE T_BIZ_LOANINFO SET " +
					"APPLICANT_NAME=@3, APPLICANT_ID=@4, ADDRESS=@5, SALARY=@6, " +
					"LOAN_VALUE=@7, RETURN_DATE=@8, LOANTELLER=@9, APP_INFO_INPUT_DATE=@10, SALARY_IS_REAL=@11, " +
					"CREDIT_STATUS=@12, RISK_FLAG=@13, RISK_EVALUATOR=@14, RISK_INFO_INPUT_DATE=@15, DECISION=@16, " +
					"EXAMINERLIST=@17, APPROVERLIST=@18, OPPONENTLIST=@19, LEND_MONEY_INFO=@20, LEND_MONEY_OFFICER=@21, " +
					"LEND_MONEY_INFO_INPUT_TIME=@22, REJECT_INFO=@23, REJECT_INFO_INPUT_TIME=@24" +
					" WHERE ID=@1";
				SqlParameter[] updateParms = {
					//SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, instance.Sn),
					SqlServerHelper.NewSqlParameter("@3", SqlDbType.VarChar, 50, instance.ApplicantName),
					SqlServerHelper.NewSqlParameter("@4", SqlDbType.VarChar, 50, instance.ApplicantId),
					SqlServerHelper.NewSqlParameter("@5", SqlDbType.VarChar, 256, instance.Address),
					SqlServerHelper.NewSqlParameter("@6", SqlDbType.Int, instance.Salary),
					SqlServerHelper.NewSqlParameter("@7", SqlDbType.Int, instance.LoanValue),
					SqlServerHelper.NewSqlParameter("@8", SqlDbType.VarChar, 10, instance.ReturnDate),
					SqlServerHelper.NewSqlParameter("@9", SqlDbType.VarChar, 50, instance.Loanteller),
					SqlServerHelper.NewSqlParameter("@10", SqlDbType.DateTime, 11, instance.AppInfoInputDate),
					SqlServerHelper.NewSqlParameter("@11", SqlDbType.SmallInt, OracleHelper.OraBit(instance.SalaryIsReal)),
					SqlServerHelper.NewSqlParameter("@12", SqlDbType.SmallInt, OracleHelper.OraBit(instance.CreditStatus)),
					SqlServerHelper.NewSqlParameter("@13", SqlDbType.SmallInt, OracleHelper.OraBit(instance.RiskFlag)),
					SqlServerHelper.NewSqlParameter("@14", SqlDbType.VarChar, 50, instance.RiskEvaluator),
					SqlServerHelper.NewSqlParameter("@15", SqlDbType.DateTime, 11, instance.RiskInfoInputDate),
					SqlServerHelper.NewSqlParameter("@16", SqlDbType.SmallInt, OracleHelper.OraBit(instance.Decision)),
					SqlServerHelper.NewSqlParameter("@17", SqlDbType.VarChar, 128, instance.ExaminerList),
					SqlServerHelper.NewSqlParameter("@18", SqlDbType.VarChar, 128, instance.ApproverList),
					SqlServerHelper.NewSqlParameter("@19", SqlDbType.VarChar, 128, instance.OpponentList),
					SqlServerHelper.NewSqlParameter("@20", SqlDbType.VarChar, 256, instance.LendMoneyInfo),
					SqlServerHelper.NewSqlParameter("@21", SqlDbType.VarChar, 50, instance.LendMoneyOfficer),
					SqlServerHelper.NewSqlParameter("@22", SqlDbType.DateTime, 11, instance.LendMoneyInfoInputTime),
					SqlServerHelper.NewSqlParameter("@23", SqlDbType.VarChar, 256, instance.RejectInfo),
					SqlServerHelper.NewSqlParameter("@24", SqlDbType.DateTime, 11, instance.RejectInfoInputTime),
					SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, instance.Id)
				};
				if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
					return false;
				else return true;
			}
		}
		
		public bool attachDirty_oracle(LoanInfo instance)
		{
			if (string.IsNullOrEmpty(instance.Id))
			{
				instance.Id = Guid.NewGuid().ToString().Replace("-", "");
				string insert = "INSERT INTO T_BIZ_LOANINFO (" +
					"ID, SN, APPLICANT_NAME, APPLICANT_ID, ADDRESS, " +
					"SALARY, LOAN_VALUE, RETURN_DATE, LOANTELLER, APP_INFO_INPUT_DATE, " +
					"SALARY_IS_REAL, CREDIT_STATUS, RISK_FLAG, RISK_EVALUATOR, RISK_INFO_INPUT_DATE, " +
					"DECISION, EXAMINERLIST, APPROVERLIST, OPPONENTLIST, LEND_MONEY_INFO, " +
					"LEND_MONEY_OFFICER, LEND_MONEY_INFO_INPUT_TIME, REJECT_INFO, REJECT_INFO_INPUT_TIME )VALUES(:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11, :12, :13, :14, :15, :16, :17, :18, :19, :20, :21, :22, :23, :24)";
				OracleParameter[] insertParms = {
					OracleHelper.NewOracleParameter(":1", OracleType.VarChar, 50, instance.Id),
					OracleHelper.NewOracleParameter(":2", OracleType.VarChar, 50, instance.Sn),
					OracleHelper.NewOracleParameter(":3", OracleType.VarChar, 50, instance.ApplicantName),
					OracleHelper.NewOracleParameter(":4", OracleType.VarChar, 50, instance.ApplicantId),
					OracleHelper.NewOracleParameter(":5", OracleType.VarChar, 256, instance.Address),
					OracleHelper.NewOracleParameter(":6", OracleType.Int32, instance.Salary),
					OracleHelper.NewOracleParameter(":7", OracleType.Int32, instance.LoanValue),
					OracleHelper.NewOracleParameter(":8", OracleType.VarChar, 10, instance.ReturnDate),
					OracleHelper.NewOracleParameter(":9", OracleType.VarChar, 50, instance.Loanteller),
					OracleHelper.NewOracleParameter(":10", OracleType.Timestamp, 11, instance.AppInfoInputDate),
					OracleHelper.NewOracleParameter(":11", OracleType.Int16, OracleHelper.OraBit(instance.SalaryIsReal)),
					OracleHelper.NewOracleParameter(":12", OracleType.Int16, OracleHelper.OraBit(instance.CreditStatus)),
					OracleHelper.NewOracleParameter(":13", OracleType.Int16, OracleHelper.OraBit(instance.RiskFlag)),
					OracleHelper.NewOracleParameter(":14", OracleType.VarChar, 50, instance.RiskEvaluator),
					OracleHelper.NewOracleParameter(":15", OracleType.Timestamp, 11, instance.RiskInfoInputDate),
					OracleHelper.NewOracleParameter(":16", OracleType.Int16, OracleHelper.OraBit(instance.Decision)),
					OracleHelper.NewOracleParameter(":17", OracleType.VarChar, 128, instance.ExaminerList),
					OracleHelper.NewOracleParameter(":18", OracleType.VarChar, 128, instance.ApproverList),
					OracleHelper.NewOracleParameter(":19", OracleType.VarChar, 128, instance.OpponentList),
					OracleHelper.NewOracleParameter(":20", OracleType.VarChar, 256, instance.LendMoneyInfo),
					OracleHelper.NewOracleParameter(":21", OracleType.VarChar, 50, instance.LendMoneyOfficer),
					OracleHelper.NewOracleParameter(":22", OracleType.Timestamp, 11, instance.LendMoneyInfoInputTime),
					OracleHelper.NewOracleParameter(":23", OracleType.VarChar, 256, instance.RejectInfo),
					OracleHelper.NewOracleParameter(":24", OracleType.Timestamp, 11, instance.RejectInfoInputTime)
				};
				if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
					return false;
				else return true;
			}
			else
			{

				string update = "UPDATE T_BIZ_LOANINFO SET " +
					"APPLICANT_NAME=:3, APPLICANT_ID=:4, ADDRESS=:5, SALARY=:6, " +
					"LOAN_VALUE=:7, RETURN_DATE=:8, LOANTELLER=:9, APP_INFO_INPUT_DATE=:10, SALARY_IS_REAL=:11, " +
					"CREDIT_STATUS=:12, RISK_FLAG=:13, RISK_EVALUATOR=:14, RISK_INFO_INPUT_DATE=:15, DECISION=:16, " +
					"EXAMINERLIST=:17, APPROVERLIST=:18, OPPONENTLIST=:19, LEND_MONEY_INFO=:20, LEND_MONEY_OFFICER=:21, " +
					"LEND_MONEY_INFO_INPUT_TIME=:22, REJECT_INFO=:23, REJECT_INFO_INPUT_TIME=:24" +
					" WHERE ID=:1";
				OracleParameter[] updateParms = {
					//OracleHelper.NewOracleParameter(":2", OracleType.VarChar, 50, instance.Sn),
					OracleHelper.NewOracleParameter(":3", OracleType.VarChar, 50, instance.ApplicantName),
					OracleHelper.NewOracleParameter(":4", OracleType.VarChar, 50, instance.ApplicantId),
					OracleHelper.NewOracleParameter(":5", OracleType.VarChar, 256, instance.Address),
					OracleHelper.NewOracleParameter(":6", OracleType.Int32, instance.Salary),
					OracleHelper.NewOracleParameter(":7", OracleType.Int32, instance.LoanValue),
					OracleHelper.NewOracleParameter(":8", OracleType.VarChar, 10, instance.ReturnDate),
					OracleHelper.NewOracleParameter(":9", OracleType.VarChar, 50, instance.Loanteller),
					OracleHelper.NewOracleParameter(":10", OracleType.Timestamp, 11, instance.AppInfoInputDate),
					OracleHelper.NewOracleParameter(":11", OracleType.Int16, OracleHelper.OraBit(instance.SalaryIsReal)),
					OracleHelper.NewOracleParameter(":12", OracleType.Int16, OracleHelper.OraBit(instance.CreditStatus)),
					OracleHelper.NewOracleParameter(":13", OracleType.Int16, OracleHelper.OraBit(instance.RiskFlag)),
					OracleHelper.NewOracleParameter(":14", OracleType.VarChar, 50, instance.RiskEvaluator),
					OracleHelper.NewOracleParameter(":15", OracleType.Timestamp, 11, instance.RiskInfoInputDate),
					OracleHelper.NewOracleParameter(":16", OracleType.Int16, OracleHelper.OraBit(instance.Decision)),
					OracleHelper.NewOracleParameter(":17", OracleType.VarChar, 128, instance.ExaminerList),
					OracleHelper.NewOracleParameter(":18", OracleType.VarChar, 128, instance.ApproverList),
					OracleHelper.NewOracleParameter(":19", OracleType.VarChar, 128, instance.OpponentList),
					OracleHelper.NewOracleParameter(":20", OracleType.VarChar, 256, instance.LendMoneyInfo),
					OracleHelper.NewOracleParameter(":21", OracleType.VarChar, 50, instance.LendMoneyOfficer),
					OracleHelper.NewOracleParameter(":22", OracleType.Timestamp, 11, instance.LendMoneyInfoInputTime),
					OracleHelper.NewOracleParameter(":23", OracleType.VarChar, 256, instance.RejectInfo),
					OracleHelper.NewOracleParameter(":24", OracleType.Timestamp, 11, instance.RejectInfoInputTime),
					OracleHelper.NewOracleParameter(":1", OracleType.VarChar, 50, instance.Id)
				};
				if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
					return false;
				else return true;
			}
		}
		
		public bool attachDirty_sqlite(LoanInfo instance)
		{
			if (string.IsNullOrEmpty(instance.Id))
			{
				instance.Id = Guid.NewGuid().ToString().Replace("-", "");
				string insert = "INSERT INTO T_BIZ_LOANINFO (" +
					"ID, SN, APPLICANT_NAME, APPLICANT_ID, ADDRESS, " +
					"SALARY, LOAN_VALUE, RETURN_DATE, LOANTELLER, APP_INFO_INPUT_DATE, " +
					"SALARY_IS_REAL, CREDIT_STATUS, RISK_FLAG, RISK_EVALUATOR, RISK_INFO_INPUT_DATE, " +
					"DECISION, EXAMINERLIST, APPROVERLIST, OPPONENTLIST, LEND_MONEY_INFO, " +
					"LEND_MONEY_OFFICER, LEND_MONEY_INFO_INPUT_TIME, REJECT_INFO, REJECT_INFO_INPUT_TIME )"+
					"VALUES(@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16, @17, @18, @19, @20, @21, @22, @23, @24)";
				SQLiteParameter[] insertParms = {
					SQLiteHelper.NewSqlParameter("@1", DbType.String, 50, instance.Id),
					SQLiteHelper.NewSqlParameter("@2", DbType.String, 50, instance.Sn),
					SQLiteHelper.NewSqlParameter("@3", DbType.String, 50, instance.ApplicantName),
					SQLiteHelper.NewSqlParameter("@4", DbType.String, 50, instance.ApplicantId),
					SQLiteHelper.NewSqlParameter("@5", DbType.String, 256, instance.Address),
					SQLiteHelper.NewSqlParameter("@6", DbType.Int32, instance.Salary),
					SQLiteHelper.NewSqlParameter("@7", DbType.Int32, instance.LoanValue),
					SQLiteHelper.NewSqlParameter("@8", DbType.String, 10, instance.ReturnDate),
					SQLiteHelper.NewSqlParameter("@9", DbType.String, 50, instance.Loanteller),
					SQLiteHelper.NewSqlParameter("@10", DbType.DateTime, 11, instance.AppInfoInputDate),
					SQLiteHelper.NewSqlParameter("@11", DbType.Int16, OracleHelper.OraBit(instance.SalaryIsReal)),
					SQLiteHelper.NewSqlParameter("@12", DbType.Int16, OracleHelper.OraBit(instance.CreditStatus)),
					SQLiteHelper.NewSqlParameter("@13", DbType.Int16, OracleHelper.OraBit(instance.RiskFlag)),
					SQLiteHelper.NewSqlParameter("@14", DbType.String, 50, instance.RiskEvaluator),
					SQLiteHelper.NewSqlParameter("@15", DbType.DateTime, 11, instance.RiskInfoInputDate),
					SQLiteHelper.NewSqlParameter("@16", DbType.Int16, OracleHelper.OraBit(instance.Decision)),
					SQLiteHelper.NewSqlParameter("@17", DbType.String, 128, instance.ExaminerList),
					SQLiteHelper.NewSqlParameter("@18", DbType.String, 128, instance.ApproverList),
					SQLiteHelper.NewSqlParameter("@19", DbType.String, 128, instance.OpponentList),
					SQLiteHelper.NewSqlParameter("@20", DbType.String, 256, instance.LendMoneyInfo),
					SQLiteHelper.NewSqlParameter("@21", DbType.String, 50, instance.LendMoneyOfficer),
					SQLiteHelper.NewSqlParameter("@22", DbType.DateTime, 11, instance.LendMoneyInfoInputTime),
					SQLiteHelper.NewSqlParameter("@23", DbType.String, 256, instance.RejectInfo),
					SQLiteHelper.NewSqlParameter("@24", DbType.DateTime, 11, instance.RejectInfoInputTime)
				};
				if (SQLiteHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
					return false;
				else return true;
			}
			else
			{
				string update = "UPDATE T_BIZ_LOANINFO SET " +
					"APPLICANT_NAME=@3, APPLICANT_ID=@4, ADDRESS=@5, SALARY=@6, " +
					"LOAN_VALUE=@7, RETURN_DATE=@8, LOANTELLER=@9, APP_INFO_INPUT_DATE=@10, SALARY_IS_REAL=@11, " +
					"CREDIT_STATUS=@12, RISK_FLAG=@13, RISK_EVALUATOR=@14, RISK_INFO_INPUT_DATE=@15, DECISION=@16, " +
					"EXAMINERLIST=@17, APPROVERLIST=@18, OPPONENTLIST=@19, LEND_MONEY_INFO=@20, LEND_MONEY_OFFICER=@21, " +
					"LEND_MONEY_INFO_INPUT_TIME=@22, REJECT_INFO=@23, REJECT_INFO_INPUT_TIME=@24" +
					" WHERE ID=@1";
				SQLiteParameter[] updateParms = {
					//SQLiteHelper.NewSqlParameter("@2", DbType.String, 50, instance.Sn),
					SQLiteHelper.NewSqlParameter("@3", DbType.String, 50, instance.ApplicantName),
					SQLiteHelper.NewSqlParameter("@4", DbType.String, 50, instance.ApplicantId),
					SQLiteHelper.NewSqlParameter("@5", DbType.String, 256, instance.Address),
					SQLiteHelper.NewSqlParameter("@6", DbType.Int32, instance.Salary),
					SQLiteHelper.NewSqlParameter("@7", DbType.Int32, instance.LoanValue),
					SQLiteHelper.NewSqlParameter("@8", DbType.String, 10, instance.ReturnDate),
					SQLiteHelper.NewSqlParameter("@9", DbType.String, 50, instance.Loanteller),
					SQLiteHelper.NewSqlParameter("@10", DbType.DateTime, 11, instance.AppInfoInputDate),
					SQLiteHelper.NewSqlParameter("@11", DbType.Int16, OracleHelper.OraBit(instance.SalaryIsReal)),
					SQLiteHelper.NewSqlParameter("@12", DbType.Int16, OracleHelper.OraBit(instance.CreditStatus)),
					SQLiteHelper.NewSqlParameter("@13", DbType.Int16, OracleHelper.OraBit(instance.RiskFlag)),
					SQLiteHelper.NewSqlParameter("@14", DbType.String, 50, instance.RiskEvaluator),
					SQLiteHelper.NewSqlParameter("@15", DbType.DateTime, 11, instance.RiskInfoInputDate),
					SQLiteHelper.NewSqlParameter("@16", DbType.Int16, OracleHelper.OraBit(instance.Decision)),
					SQLiteHelper.NewSqlParameter("@17", DbType.String, 128, instance.ExaminerList),
					SQLiteHelper.NewSqlParameter("@18", DbType.String, 128, instance.ApproverList),
					SQLiteHelper.NewSqlParameter("@19", DbType.String, 128, instance.OpponentList),
					SQLiteHelper.NewSqlParameter("@20", DbType.String, 256, instance.LendMoneyInfo),
					SQLiteHelper.NewSqlParameter("@21", DbType.String, 50, instance.LendMoneyOfficer),
					SQLiteHelper.NewSqlParameter("@22", DbType.DateTime, 11, instance.LendMoneyInfoInputTime),
					SQLiteHelper.NewSqlParameter("@23", DbType.String, 256, instance.RejectInfo),
					SQLiteHelper.NewSqlParameter("@24", DbType.DateTime, 11, instance.RejectInfoInputTime),
					SQLiteHelper.NewSqlParameter("@1", DbType.String, 50, instance.Id)
				};
				if (SQLiteHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
					return false;
				else return true;
			}
		}
		#endregion
		
		#region findBySn
		public LoanInfo findBySn_oracle(Object sn)
		{
			string select = "SELECT * FROM t_biz_loaninfo WHERE SN=:1";
			OracleConnection conn = new OracleConnection(connectionString);
			OracleDataReader reader = null;
			try
			{
				OracleParameter[] selectParms = {
					OracleHelper.NewOracleParameter(":1", OracleType.VarChar, 50, sn)
				};
				reader = OracleHelper.ExecuteReader(conn, CommandType.Text, select, selectParms);
				if (reader.Read())
				{
					LoanInfo _LoanInfo = new LoanInfo()
					{
						Id = Convert.ToString(reader["id"]),
						Sn = Convert.ToString(reader["sn"]),
						ApplicantName = Convert.ToString(reader["applicant_name"]),
						ApplicantId = Convert.ToString(reader["applicant_id"]),
						Address = Convert.ToString(reader["address"]),
						Salary = Convert.ToInt32(reader["salary"]),
						LoanValue = Convert.ToInt32(reader["loan_value"]),
						ReturnDate = Convert.ToString(reader["return_date"]),
						Loanteller = Convert.ToString(reader["loanteller"]),

						AppInfoInputDate = Convert.ToDateTime(reader["app_info_input_date"]),
						SalaryIsReal = Convert.ToInt32(reader["salary_is_real"]) == 1 ? true : false,// Convert.ToBoolean(reader["SALARY_IS_REAL"]),
						CreditStatus = Convert.ToInt32(reader["credit_status"]) == 1 ? true : false,//Convert.ToBoolean(reader["credit_status"]),
						RiskFlag = Convert.ToInt32(reader["risk_flag"]) == 1 ? true : false,//Convert.ToBoolean(reader["risk_flag"]),
						RiskEvaluator = Convert.ToString(reader["risk_evaluator"]),
						RiskInfoInputDate = Convert.ToDateTime(reader["risk_info_input_date"]),
						Decision = Convert.ToInt32(reader["decision"]) == 1 ? true : false,//Convert.ToBoolean(reader["decision"]),
						ExaminerList = Convert.ToString(reader["examinerList"]),
						ApproverList = Convert.ToString(reader["approverList"]),
						OpponentList = Convert.ToString(reader["opponentList"]),
						LendMoneyInfo = Convert.ToString(reader["lend_money_info"]),
						LendMoneyOfficer = Convert.ToString(reader["lend_money_officer"]),
						LendMoneyInfoInputTime = Convert.ToDateTime(reader["lend_money_info_input_time"]),
						RejectInfo = Convert.ToString(reader["reject_info"]),
						RejectInfoInputTime = Convert.ToDateTime(reader["reject_info_input_time"]),
					};
					return _LoanInfo;
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

		public LoanInfo findBySn_sqlserver(Object sn)
		{
			string select = "SELECT * FROM t_biz_loaninfo WHERE SN=@1";
			SqlConnection conn = new SqlConnection(connectionString);
			SqlDataReader reader = null;
			try
			{
				SqlParameter[] selectParms = {
					SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, sn)
				};
				reader = SqlServerHelper.ExecuteReader(conn, CommandType.Text, select, selectParms);
				if (reader.Read())
				{
					LoanInfo _LoanInfo = new LoanInfo()
					{
						Id = Convert.ToString(reader["id"]),
						Sn = Convert.ToString(reader["sn"]),
						ApplicantName = Convert.ToString(reader["applicant_name"]),
						ApplicantId = Convert.ToString(reader["applicant_id"]),
						Address = Convert.ToString(reader["address"]),
						Salary = Convert.ToInt32(reader["salary"]),
						LoanValue = Convert.ToInt32(reader["loan_value"]),
						ReturnDate = Convert.ToString(reader["return_date"]),
						Loanteller = Convert.ToString(reader["loanteller"]),

						AppInfoInputDate = Convert.ToDateTime(reader["app_info_input_date"]),
						SalaryIsReal = Convert.ToInt32(reader["salary_is_real"]) == 1 ? true : false,// Convert.ToBoolean(reader["SALARY_IS_REAL"]),
						CreditStatus = Convert.ToInt32(reader["credit_status"]) == 1 ? true : false,//Convert.ToBoolean(reader["credit_status"]),
						RiskFlag = Convert.ToInt32(reader["risk_flag"]) == 1 ? true : false,//Convert.ToBoolean(reader["risk_flag"]),
						RiskEvaluator = Convert.ToString(reader["risk_evaluator"]),
						RiskInfoInputDate = Convert.ToDateTime(reader["risk_info_input_date"]),
						Decision = Convert.ToInt32(reader["decision"]) == 1 ? true : false,//Convert.ToBoolean(reader["decision"]),
						ExaminerList = Convert.ToString(reader["examinerList"]),
						ApproverList = Convert.ToString(reader["approverList"]),
						OpponentList = Convert.ToString(reader["opponentList"]),
						LendMoneyInfo = Convert.ToString(reader["lend_money_info"]),
						LendMoneyOfficer = Convert.ToString(reader["lend_money_officer"]),
						LendMoneyInfoInputTime = Convert.ToDateTime(reader["lend_money_info_input_time"]),
						RejectInfo = Convert.ToString(reader["reject_info"]),
						RejectInfoInputTime = Convert.ToDateTime(reader["reject_info_input_time"]),
					};
					return _LoanInfo;
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

		public LoanInfo findBySn_sqlite(Object sn)
		{
			string select = "SELECT * FROM t_biz_loaninfo WHERE SN=@1";
			SQLiteConnection conn = new SQLiteConnection(connectionString);
			SQLiteDataReader reader = null;
			try
			{
				SQLiteParameter[] selectParms = {
					SQLiteHelper.NewSqlParameter("@1", DbType.String, 50, sn)
				};
				reader = SQLiteHelper.ExecuteReader(conn, CommandType.Text, select, selectParms);
				if (reader.Read())
				{
					LoanInfo _LoanInfo = new LoanInfo()
					{
						Id = Convert.ToString(reader["id"]),
						Sn = Convert.ToString(reader["sn"]),
						ApplicantName = Convert.ToString(reader["applicant_name"]),
						ApplicantId = Convert.ToString(reader["applicant_id"]),
						Address = Convert.ToString(reader["address"]),
						Salary = Convert.ToInt32(reader["salary"]),
						LoanValue = Convert.ToInt32(reader["loan_value"]),
						ReturnDate = Convert.ToString(reader["return_date"]),
						Loanteller = Convert.ToString(reader["loanteller"]),

						AppInfoInputDate = Convert.ToDateTime(reader["app_info_input_date"]),
						SalaryIsReal = Convert.ToInt32(reader["salary_is_real"]) == 1 ? true : false,// Convert.ToBoolean(reader["SALARY_IS_REAL"]),
						CreditStatus = Convert.ToInt32(reader["credit_status"]) == 1 ? true : false,//Convert.ToBoolean(reader["credit_status"]),
						RiskFlag = Convert.ToInt32(reader["risk_flag"]) == 1 ? true : false,//Convert.ToBoolean(reader["risk_flag"]),
						RiskEvaluator = Convert.ToString(reader["risk_evaluator"]),
						RiskInfoInputDate = Convert.ToDateTime(reader["risk_info_input_date"]),
						Decision = Convert.ToInt32(reader["decision"]) == 1 ? true : false,//Convert.ToBoolean(reader["decision"]),
						ExaminerList = Convert.ToString(reader["examinerList"]),
						ApproverList = Convert.ToString(reader["approverList"]),
						OpponentList = Convert.ToString(reader["opponentList"]),
						LendMoneyInfo = Convert.ToString(reader["lend_money_info"]),
						LendMoneyOfficer = Convert.ToString(reader["lend_money_officer"]),
						LendMoneyInfoInputTime = Convert.ToDateTime(reader["lend_money_info_input_time"]),
						RejectInfo = Convert.ToString(reader["reject_info"]),
						RejectInfoInputTime = Convert.ToDateTime(reader["reject_info_input_time"]),
					};
					return _LoanInfo;
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
