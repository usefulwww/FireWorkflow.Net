--流程定义
CREATE TABLE [T_FF_DF_WORKFLOWDEF] (
  [id] varchar(50) PRIMARY KEY NOT NULL,
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
);

--流程实例表
CREATE TABLE [T_FF_RT_PROCESSINSTANCE] (
  [id] varchar(50) PRIMARY KEY NOT NULL,
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
);


--流程实例执行情况跟踪表。记录每次流转的详细情况。
CREATE TABLE [T_FF_HIST_TRACE] (
  [id] varchar(50) PRIMARY KEY NOT NULL,
  [processinstance_id] varchar(50) NOT NULL,
  [step_number] int NOT NULL,
  [minor_number] int NOT NULL,
  [type] varchar(15) NOT NULL,
  [edge_id] varchar(100) NULL,
  [from_node_id] varchar(100) NOT NULL,
  [to_node_id] varchar(100) NOT NULL
);


--流程变量表
CREATE TABLE [T_FF_RT_PROCINST_VAR] (
  [processinstance_id] varchar(50) NOT NULL,
  [value] varchar(255) NULL,
  [name] varchar(255) NOT NULL
);


--任务实例表
CREATE TABLE [T_FF_RT_TASKINSTANCE] (
  [id] varchar(50) PRIMARY KEY NOT NULL,
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
);


--Token 表，一个流程实例同时可以有任意多个活动的 Token ；如果流程实例的活动 Token 数量为 0 ，即表示流程已经结束了。
CREATE TABLE [T_FF_RT_TOKEN] (
  [id] varchar(50) PRIMARY KEY NOT NULL,
  [alive] tinyint NOT NULL,
  [value] int NOT NULL,
  [node_id] varchar(200) NOT NULL,
  [processinstance_id] varchar(50) NOT NULL,
  [step_number] int NOT NULL,
  [from_activity_id] varchar(100) NULL
);


--工作项表
CREATE TABLE [T_FF_RT_WORKITEM] (
  [id] varchar(50) PRIMARY KEY NOT NULL,
  [taskinstance_id] varchar(50) NOT NULL,
  [state] int NOT NULL,
  [created_time] datetime NOT NULL,
  [claimed_time] datetime NULL,
  [end_time] datetime NULL,
  [actor_id] varchar(50) NULL,
  [comments] varchar(1024) NULL
);




