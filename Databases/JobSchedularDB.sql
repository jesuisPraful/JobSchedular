Use [master]
GO

IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = N''OR name = N'JobSchedularDB')))
DROP DATABASE JobSchedularDB
GO

CREATE DATABASE JobSchedularDB
GO

Use JobSchedularDB
GO

CREATE TABLE Users (
    userId VARCHAR(50) PRIMARY KEY,
    Username VARCHAR(100) NOT NULL,
    Email VARCHAR(200) UNIQUE NOT NULL,
    Password VARCHAR(200) NOT NULL
);
GO

CREATE TABLE JobDefinition (
    jobId VARCHAR(50) PRIMARY KEY,
    userId VARCHAR(50) NOT NULL,
    jobName VARCHAR(200) NOT NULL,
    jobDescription TEXT NULL,
    jobParameters NVARCHAR(MAX) NULL, -- JSON Parameters
    status VARCHAR(50) CHECK (status IN ('Pending','Scheduled','Completed','Failed')),
    timestamps DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Job_User FOREIGN KEY (userId)
        REFERENCES Users(userId)
        ON DELETE CASCADE
);
GO

CREATE TABLE JobSchedule (
    jobId VARCHAR(50) PRIMARY KEY,
    scheduledExecutionTime DATETIME NOT NULL,

    schedulePattern VARCHAR(100) NULL,  -- NEW (for Cron)
    nextRunTime DATETIME NULL,          -- NEW (for Next Execution Time)

    status VARCHAR(50) CHECK (status IN ('Scheduled','Running','Completed','Failed')),

    CONSTRAINT FK_Schedule_Job FOREIGN KEY (jobId)
        REFERENCES JobDefinition(jobId)
        ON DELETE CASCADE
);
GO


CREATE TABLE JobExecutionLog (
    executionLogId VARCHAR(50) PRIMARY KEY,
    jobId VARCHAR(50) NOT NULL,
    executionStatus VARCHAR(50) CHECK (executionStatus IN ('Running','Success','Failed')),
    startTime DATETIME NOT NULL,
    endTime DATETIME NULL,
    executionNodeId VARCHAR(50) NOT NULL,

    CONSTRAINT FK_Log_Job FOREIGN KEY (jobId)
        REFERENCES JobDefinition(jobId)
        ON DELETE CASCADE
);
GO

CREATE TABLE ExecutionNode (
    nodeId VARCHAR(50) PRIMARY KEY,
    nodeName VARCHAR(200) NOT NULL,
    nodeIPAddress VARCHAR(100) NOT NULL UNIQUE,
    nodeStatus VARCHAR(50) CHECK (nodeStatus IN ('Active','Inactive','Busy'))
);
GO

CREATE TABLE ResourceAllocation (
    allocationId VARCHAR(50) PRIMARY KEY,
    jobId VARCHAR(50) NOT NULL,
    executionNodeId VARCHAR(50) NOT NULL,
    startTime DATETIME NOT NULL,
    endTime DATETIME NULL,

    CONSTRAINT FK_Resource_Job FOREIGN KEY (jobId)
        REFERENCES JobDefinition(jobId)
        ON DELETE CASCADE,

    CONSTRAINT FK_Resource_Node FOREIGN KEY (executionNodeId)
        REFERENCES ExecutionNode(nodeId)
        ON DELETE CASCADE
);
GO

CREATE TABLE JobRetry (
    retryId VARCHAR(50) PRIMARY KEY,
    jobId VARCHAR(50) NOT NULL,
    retryAttemptNumber INT NOT NULL,
    retryStatus VARCHAR(50) CHECK (retryStatus IN ('Pending','Running','Failed','Success')),
    retryTime DATETIME NOT NULL,

    CONSTRAINT FK_Retry_Job FOREIGN KEY (jobId)
        REFERENCES JobDefinition(jobId)
        ON DELETE CASCADE
);
GO
