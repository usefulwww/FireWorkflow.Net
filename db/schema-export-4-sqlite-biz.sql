CREATE TABLE [T_Biz_EmailMock] (
  [id] varchar(50) PRIMARY KEY NOT NULL,
  [user_id] varchar(50) NOT NULL,
  [content] varchar(512) NOT NULL,
  [create_time] datetime NOT NULL
);

CREATE TABLE [T_Biz_LeaveApp_TASKINSTANCE] (
  [TASKINSTANCE_ID] varchar(50) PRIMARY KEY NOT NULL,
  [sn] varchar(255) NULL,
  [applicant] varchar(255) NULL,
  [leave_days] int NULL,
  [approval_flag] tinyint NULL,
  [from_date] varchar(255) NULL,
  [to_date] varchar(255) NULL
);



CREATE TABLE [T_Biz_LeaveApplicationInfo] (
  [id] varchar(50) PRIMARY KEY NOT NULL,
  [sn] varchar(50) NOT NULL,
  [leaveReason] varchar(50) NULL,
  [fromDate] varchar(50) NULL,
  [toDate] varchar(50) NULL,
  [leaveDays] int NULL,
  [applicant_Id] varchar(50) NULL,
  [applicant_name] varchar(50) NULL,
  [submitTime] varchar(50) NULL,
  [approval_flag] tinyint NULL,
  [approval_detail] varchar(50) NULL,
  [approver] varchar(50) NULL,
  [approval_time] datetime NULL
);


CREATE TABLE [T_Biz_LeaveApprovalInfo] (
  [id] varchar(50) PRIMARY KEY NOT NULL,
  [sn] varchar(50) NOT NULL,
  [approver] varchar(50) NULL,
  [approver_flag] tinyint NULL,
  [detail] varchar(100) NULL,
  [approval_time] datetime NULL
);


CREATE TABLE [T_BIZ_LOAN_APPROVEINFO] (
  [ID] varchar(50) PRIMARY KEY NOT NULL,
  [SN] varchar(50) NOT NULL,
  [approver] varchar(50) NOT NULL,
  [decision] decimal(1, 0) NULL,
  [detail] varchar(50) NULL
);

CREATE TABLE [T_BIZ_LOANINFO] (
  [ID] varchar(50) PRIMARY KEY NOT NULL,
  [SN] varchar(50) NOT NULL,
  [APPLICANT_NAME] varchar(50) NOT NULL,
  [APPLICANT_ID] varchar(50) NOT NULL,
  [ADDRESS] varchar(256) NOT NULL,
  [SALARY] decimal(10, 0) NOT NULL,
  [LOAN_VALUE] decimal(10, 0) NOT NULL,
  [RETURN_DATE] varchar(10) NOT NULL,
  [LOANTELLER] varchar(50) NOT NULL,
  [APP_INFO_INPUT_DATE] datetime NOT NULL,
  [SALARY_IS_REAL] decimal(1, 0) NULL,
  [CREDIT_STATUS] decimal(1, 0) NULL,
  [RISK_FLAG] decimal(1, 0) NULL,
  [RISK_EVALUATOR] varchar(50) NULL,
  [RISK_INFO_INPUT_DATE] datetime NULL,
  [DECISION] decimal(1, 0) NULL,
  [examinerList] varchar(128) NULL,
  [approverList] varchar(128) NULL,
  [opponentList] varchar(128) NULL,
  [LEND_MONEY_INFO] varchar(256) NULL,
  [LEND_MONEY_OFFICER] varchar(50) NULL,
  [LEND_MONEY_INFO_INPUT_TIME] datetime NULL,
  [REJECT_INFO] varchar(256) NULL,
  [REJECT_INFO_INPUT_TIME] datetime NULL
);


CREATE TABLE [T_Biz_TradeInfo] (
  [ID] varchar(50) PRIMARY KEY NOT NULL,
  [SN] varchar(50) NULL,
  [GOODS_NAME] varchar(100) NULL,
  [GOODS_TYPE] varchar(50) NULL,
  [QUANTITY] decimal(19, 0) NULL,
  [UNIT_PRICE] decimal(18, 0) NULL,
  [AMOUNT] decimal(18, 0) NULL,
  [CUSTOMER_NAME] varchar(50) NULL,
  [CUSTOMER_MOBILE] varchar(30) NULL,
  [CUSTOMER_PHONE_FAX] varchar(30) NULL,
  [CUSTOMER_ADDRESS] varchar(150) NULL,
  [STATE] varchar(15) NULL,
  [PAYED_TIME] datetime NULL,
  [DELIVERED_TIME] datetime NULL
);