SET NOCOUNT ON
GO

--
-- Definition for table T_Biz_EmailMock : 
--

CREATE TABLE [dbo].[T_Biz_EmailMock] (
  [id] varchar(50) NOT NULL,
  [user_id] varchar(50) NOT NULL,
  [content] varchar(512) NOT NULL,
  [create_time] datetime NOT NULL
)
ON [PRIMARY]
GO

--
-- Definition for table T_Biz_LeaveApp_TASKINSTANCE : 
--

CREATE TABLE [dbo].[T_Biz_LeaveApp_TASKINSTANCE] (
  [TASKINSTANCE_ID] varchar(50) NOT NULL,
  [sn] varchar(255) NULL,
  [applicant] varchar(255) NULL,
  [leave_days] int NULL,
  [approval_flag] tinyint NULL,
  [from_date] varchar(255) NULL,
  [to_date] varchar(255) NULL
)
ON [PRIMARY]
GO

EXEC sp_addextendedproperty 'MS_Description', N'关联查询', N'schema', N'dbo', N'table', N'T_Biz_LeaveApp_TASKINSTANCE'
GO

--
-- Definition for table T_Biz_LeaveApplicationInfo : 
--

CREATE TABLE [dbo].[T_Biz_LeaveApplicationInfo] (
  [id] varchar(50) NOT NULL,
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
)
ON [PRIMARY]
GO

EXEC sp_addextendedproperty 'MS_Description', N'业务表的数据独立存储', N'schema', N'dbo', N'table', N'T_Biz_LeaveApplicationInfo'
GO

--
-- Definition for table T_Biz_LeaveApprovalInfo : 
--

CREATE TABLE [dbo].[T_Biz_LeaveApprovalInfo] (
  [id] varchar(50) NOT NULL,
  [sn] varchar(50) NOT NULL,
  [approver] varchar(50) NULL,
  [approver_flag] tinyint NULL,
  [detail] varchar(100) NULL,
  [approval_time] datetime NULL
)
ON [PRIMARY]
GO

--
-- Definition for table T_BIZ_LOAN_APPROVEINFO : 
--

CREATE TABLE [dbo].[T_BIZ_LOAN_APPROVEINFO] (
  [ID] varchar(50) NOT NULL,
  [SN] varchar(50) NOT NULL,
  [approver] varchar(50) NOT NULL,
  [decision] decimal(1, 0) NULL,
  [detail] varchar(50) NULL
)
ON [PRIMARY]
GO

--
-- Definition for table T_BIZ_LOANINFO : 
--

CREATE TABLE [dbo].[T_BIZ_LOANINFO] (
  [ID] varchar(50) NOT NULL,
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
)
ON [PRIMARY]
GO

--
-- Definition for table T_Biz_TradeInfo : 
--

CREATE TABLE [dbo].[T_Biz_TradeInfo] (
  [ID] varchar(50) NOT NULL,
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
)
ON [PRIMARY]
GO

--
-- Definition for table T_FF_DF_WORKFLOWDEF : 
--

CREATE TABLE [dbo].[T_FF_DF_WORKFLOWDEF] (
  [id] varchar(50) NOT NULL,
  [definition_type] varchar(50) NOT NULL,
  [process_id] varchar(100) NOT NULL,
  [name] varchar(100) NOT NULL,
  [display_name] varchar(128) NULL,
  [description] varchar(1024) NULL,
  [version] int NOT NULL,
  [state] tinyint NOT NULL,
  [upload_user] varchar(50) NULL,
  [upload_time] datetime NULL,
  [publish_user] varchar(50) NULL,
  [publish_time] datetime NULL,
  [process_content] text NULL
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

EXEC sp_addextendedproperty 'MS_Description', N'存储流程定义文件。在Fire workflow 中，流程
   运行时的表不需要和流程定义表做关联，因此没有将
   流程定义文件分解，直接存为一个大字段。', N'schema', N'dbo', N'table', N'T_FF_DF_WORKFLOWDEF'
GO

EXEC sp_addextendedproperty 'MS_Description', N'流程定义文件的类型，默认为fpdl', N'schema', N'dbo', N'table', N'T_FF_DF_WORKFLOWDEF', N'column', N'definition_type'
GO

EXEC sp_addextendedproperty 'MS_Description', N'从1开始，逐步增加2，3，....', N'schema', N'dbo', N'table', N'T_FF_DF_WORKFLOWDEF', N'column', N'version'
GO

EXEC sp_addextendedproperty 'MS_Description', N'1已发布 0未发布', N'schema', N'dbo', N'table', N'T_FF_DF_WORKFLOWDEF', N'column', N'state'
GO

--
-- Definition for table T_FF_RT_PROCESSINSTANCE : 
--

CREATE TABLE [dbo].[T_FF_RT_PROCESSINSTANCE] (
  [id] varchar(50) NOT NULL,
  [process_id] varchar(100) NOT NULL,
  [version] varchar(50) NOT NULL,
  [name] varchar(100) NULL,
  [display_name] varchar(128) NULL,
  [state] int NOT NULL,
  [suspended] tinyint NOT NULL,
  [creator_id] varchar(50) NULL,
  [created_time] datetime NULL,
  [started_time] datetime NULL,
  [expired_time] datetime NULL,
  [end_time] datetime NULL,
  [parent_processinstance_id] varchar(50) NULL,
  [parent_taskinstance_id] varchar(50) NULL
)
ON [PRIMARY]
GO

--
-- Definition for table T_FF_HIST_TRACE : 
--

CREATE TABLE [dbo].[T_FF_HIST_TRACE] (
  [id] varchar(50) NOT NULL,
  [processinstance_id] varchar(50) NOT NULL,
  [step_number] int NOT NULL,
  [minor_number] int NOT NULL,
  [type] varchar(15) NOT NULL,
  [edge_id] varchar(100) NULL,
  [from_node_id] varchar(100) NOT NULL,
  [to_node_id] varchar(100) NOT NULL
)
ON [PRIMARY]
GO

EXEC sp_addextendedproperty 'MS_Description', N'记录每次流转的详细', N'schema', N'dbo', N'table', N'T_FF_HIST_TRACE'
GO

EXEC sp_addextendedproperty 'MS_Description', N'如果节点是activity节点，哪么为2，如果是同步器节点，那么为1', N'schema', N'dbo', N'table', N'T_FF_HIST_TRACE', N'column', N'minor_number'
GO

EXEC sp_addextendedproperty 'MS_Description', N'Transition和loop', N'schema', N'dbo', N'table', N'T_FF_HIST_TRACE', N'column', N'type'
GO

--
-- Definition for table T_FF_RT_PROCINST_VAR : 
--

CREATE TABLE [dbo].[T_FF_RT_PROCINST_VAR] (
  [processinstance_id] varchar(50) NOT NULL,
  [value] varchar(255) NULL,
  [name] varchar(255) NOT NULL
)
ON [PRIMARY]
GO

EXEC sp_addextendedproperty 'MS_Description', N'		<map lazy="false" name="processInstanceVariables" table="T_FF_RT_PROCINST_VAR">
   			<key column="PROCESSINSTANCE_ID" />
   			<map-key column="NAME" type="java.lang.String" />
   			<element column="VALUE"
   				type="org.fireflow.engine.persistence.hibernate.ProcessInstanceVariableType" />
   		</map>', N'schema', N'dbo', N'table', N'T_FF_RT_PROCINST_VAR'
GO

EXEC sp_addextendedproperty 'MS_Description', N'到T_FF_RT_ProcessInstance的外键', N'schema', N'dbo', N'table', N'T_FF_RT_PROCINST_VAR', N'column', N'processinstance_id'
GO

--
-- Definition for table T_FF_RT_TASKINSTANCE : 
--

CREATE TABLE [dbo].[T_FF_RT_TASKINSTANCE] (
  [id] varchar(50) NOT NULL,
  [biz_type] varchar(250) NOT NULL,
  [task_id] varchar(300) NOT NULL,
  [activity_id] varchar(200) NOT NULL,
  [name] varchar(100) NOT NULL,
  [display_name] varchar(128) NULL,
  [state] int NOT NULL,
  [suspended] tinyint NOT NULL,
  [task_type] varchar(10) NULL,
  [created_time] datetime NOT NULL,
  [started_time] datetime NULL,
  [expired_time] datetime NULL,
  [end_time] datetime NULL,
  [assignment_strategy] varchar(50) NULL,
  [processinstance_id] varchar(50) NOT NULL,
  [process_id] varchar(50) NOT NULL,
  [version] varchar(50) NOT NULL,
  [target_activity_id] varchar(100) NULL,
  [from_activity_id] varchar(600) NULL,
  [step_number] int NOT NULL,
  [can_be_withdrawn] tinyint NOT NULL,
  [BIZ_INFO] varchar(500) NULL
)
ON [PRIMARY]
GO

EXEC sp_addextendedproperty 'MS_Description', N'用于支持hibernate等orm工具进行父子对象映射，哪如何使用jdbc程序呢？', N'schema', N'dbo', N'table', N'T_FF_RT_TASKINSTANCE', N'column', N'biz_type'
GO

EXEC sp_addextendedproperty 'MS_Description', N'FormTask,ToolTask,SubflowTask', N'schema', N'dbo', N'table', N'T_FF_RT_TASKINSTANCE', N'column', N'task_type'
GO

EXEC sp_addextendedproperty 'MS_Description', N'取值ALL,ANY', N'schema', N'dbo', N'table', N'T_FF_RT_TASKINSTANCE', N'column', N'assignment_strategy'
GO

EXEC sp_addextendedproperty 'MS_Description', N'到T_FF_RT_ProcessInstance的外键', N'schema', N'dbo', N'table', N'T_FF_RT_TASKINSTANCE', N'column', N'processinstance_id'
GO

EXEC sp_addextendedproperty 'MS_Description', N'冗余字段', N'schema', N'dbo', N'table', N'T_FF_RT_TASKINSTANCE', N'column', N'process_id'
GO

EXEC sp_addextendedproperty 'MS_Description', N'冗余字段', N'schema', N'dbo', N'table', N'T_FF_RT_TASKINSTANCE', N'column', N'version'
GO

--
-- Definition for table T_FF_RT_TOKEN : 
--

CREATE TABLE [dbo].[T_FF_RT_TOKEN] (
  [id] varchar(50) NOT NULL,
  [alive] tinyint NOT NULL,
  [value] int NOT NULL,
  [node_id] varchar(200) NOT NULL,
  [processinstance_id] varchar(50) NOT NULL,
  [step_number] int NOT NULL,
  [from_activity_id] varchar(100) NULL
)
ON [PRIMARY]
GO

EXEC sp_addextendedproperty 'MS_Description', N'Token 表，一个流程实例同时可以有任意多个活
   动的Token；如果流程实例的活动Token 数量为0，
   即表示流程已经结束了。', N'schema', N'dbo', N'table', N'T_FF_RT_TOKEN'
GO

--
-- Definition for table T_FF_RT_WORKITEM : 
--

CREATE TABLE [dbo].[T_FF_RT_WORKITEM] (
  [id] varchar(50) NOT NULL,
  [taskinstance_id] varchar(50) NOT NULL,
  [state] int NOT NULL,
  [created_time] datetime NOT NULL,
  [claimed_time] datetime NULL,
  [end_time] datetime NULL,
  [actor_id] varchar(50) NULL,
  [comments] varchar(1024) NULL
)
ON [PRIMARY]
GO

EXEC sp_addextendedproperty 'MS_Description', N'到T_FF_RT_TaskInstance的外键', N'schema', N'dbo', N'table', N'T_FF_RT_WORKITEM', N'column', N'taskinstance_id'
GO

EXEC sp_addextendedproperty 'MS_Description', N',0=Initialized ,1=Running, 7=Completed,9=Canceled', N'schema', N'dbo', N'table', N'T_FF_RT_WORKITEM', N'column', N'state'
GO

--
-- Definition for indices : 
--

ALTER TABLE [dbo].[T_Biz_EmailMock]
ADD CONSTRAINT [PK_T_BIZ_EMAILMOCK] 
PRIMARY KEY CLUSTERED ([id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_Biz_LeaveApp_TASKINSTANCE]
ADD CONSTRAINT [PK_T_BIZ_LEAVEAPP_TASKINSTANCE] 
PRIMARY KEY CLUSTERED ([TASKINSTANCE_ID])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_Biz_LeaveApplicationInfo]
ADD CONSTRAINT [PK_T_BIZ_LEAVEAPPLICATIONINFO] 
PRIMARY KEY CLUSTERED ([id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_Biz_LeaveApprovalInfo]
ADD CONSTRAINT [PK_T_BIZ_LEAVEAPPROVALINFO] 
PRIMARY KEY CLUSTERED ([id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_BIZ_LOAN_APPROVEINFO]
ADD PRIMARY KEY CLUSTERED ([ID])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_BIZ_LOANINFO]
ADD PRIMARY KEY CLUSTERED ([ID])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_BIZ_LOANINFO]
ADD UNIQUE NONCLUSTERED ([SN])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_Biz_TradeInfo]
ADD PRIMARY KEY CLUSTERED ([ID])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_Biz_TradeInfo]
ADD UNIQUE NONCLUSTERED ([SN])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_FF_DF_WORKFLOWDEF]
ADD CONSTRAINT [PK_T_FF_DF_WORKFLOWDEF] 
PRIMARY KEY CLUSTERED ([id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_FF_RT_PROCESSINSTANCE]
ADD CONSTRAINT [PK_T_FF_RT_PROCESSINSTANCE] 
PRIMARY KEY CLUSTERED ([id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_FF_HIST_TRACE]
ADD CONSTRAINT [PK_T_FF_HIST_TRACE] 
PRIMARY KEY CLUSTERED ([id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_FF_RT_PROCINST_VAR]
ADD CONSTRAINT [PK_T_FF_RT_PROCINST_VAR] 
PRIMARY KEY CLUSTERED ([processinstance_id], [name])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_FF_RT_TASKINSTANCE]
ADD CONSTRAINT [PK_T_FF_RT_TASKINSTANCE] 
PRIMARY KEY CLUSTERED ([id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_FF_RT_TOKEN]
ADD CONSTRAINT [PK_T_FF_RT_TOKEN] 
PRIMARY KEY CLUSTERED ([id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[T_FF_RT_WORKITEM]
ADD CONSTRAINT [PK_T_FF_RT_WORKITEM] 
PRIMARY KEY CLUSTERED ([id])
WITH (
  PAD_INDEX = OFF,
  IGNORE_DUP_KEY = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

--
-- Definition for foreign keys : 
--

ALTER TABLE [dbo].[T_FF_HIST_TRACE]
ADD CONSTRAINT [trace_reference_process] FOREIGN KEY ([processinstance_id]) 
  REFERENCES [dbo].[T_FF_RT_PROCESSINSTANCE] ([id]) 
  ON UPDATE NO ACTION
  ON DELETE NO ACTION
GO

ALTER TABLE [dbo].[T_FF_RT_PROCINST_VAR]
ADD CONSTRAINT [var_reference_process] FOREIGN KEY ([processinstance_id]) 
  REFERENCES [dbo].[T_FF_RT_PROCESSINSTANCE] ([id]) 
  ON UPDATE NO ACTION
  ON DELETE NO ACTION
GO

ALTER TABLE [dbo].[T_FF_RT_TASKINSTANCE]
ADD CONSTRAINT [task_reference_process] FOREIGN KEY ([processinstance_id]) 
  REFERENCES [dbo].[T_FF_RT_PROCESSINSTANCE] ([id]) 
  ON UPDATE NO ACTION
  ON DELETE NO ACTION
GO

ALTER TABLE [dbo].[T_FF_RT_TOKEN]
ADD CONSTRAINT [token_reference_process] FOREIGN KEY ([processinstance_id]) 
  REFERENCES [dbo].[T_FF_RT_PROCESSINSTANCE] ([id]) 
  ON UPDATE NO ACTION
  ON DELETE NO ACTION
GO

ALTER TABLE [dbo].[T_FF_RT_WORKITEM]
ADD CONSTRAINT [workitem_reference_task] FOREIGN KEY ([taskinstance_id]) 
  REFERENCES [dbo].[T_FF_RT_TASKINSTANCE] ([id]) 
  ON UPDATE NO ACTION
  ON DELETE NO ACTION
GO

