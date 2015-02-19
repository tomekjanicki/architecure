CREATE TABLE [dbo].[Users] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Login]     NVARCHAR (100) NOT NULL,
    [FirstName] NVARCHAR (100) NOT NULL,
    [LastName]  NVARCHAR (100) NOT NULL,
    [Version]   ROWVERSION     NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Users_Strings] CHECK ([Login] <> ''
                                             AND [FirstName] <> ''
                                             AND [LastName] <> '')
);

CREATE NONCLUSTERED INDEX [IX_Users_Login]
    ON [dbo].[Users]([Login] ASC);


CREATE TABLE [dbo].[Roles] (
    [Id]   INT NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Roles_Strings] CHECK ([Name] <> '')
);

CREATE UNIQUE NONCLUSTERED INDEX [IX_Roles_Name]
    ON [dbo].[Roles]([Name] ASC);

CREATE TABLE [dbo].[UsersRoles] (
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC, [UserId] ASC),
    CONSTRAINT [FK_UsersRoles_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id]),
    CONSTRAINT [FK_UsersRoles_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

CREATE TABLE [dbo].[Products] (
    [Id]      INT             IDENTITY (1, 1) NOT NULL,
    [Code]    NVARCHAR (50)   NOT NULL,
    [Name]    NVARCHAR (100)  NOT NULL,
    [Price]   DECIMAL (18, 2) NOT NULL,
    [Version] ROWVERSION      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Products_Strings] CHECK ([Code]<>'' AND [Name]<>'')
);

CREATE UNIQUE NONCLUSTERED INDEX [IX_Products_Code]
    ON [dbo].[Products]([Code] ASC);


CREATE TABLE [dbo].[Customers] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (50)  NOT NULL,
    [Mail]    NVARCHAR (100) NOT NULL,
    [Version] ROWVERSION     NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Customers_Strings] CHECK ([Name]<>'' AND [Mail]<>'')
);


CREATE UNIQUE NONCLUSTERED INDEX [IX_Customers_Mail]
    ON [dbo].[Customers]([Mail] ASC);

CREATE TABLE [dbo].[OrderStatuses] (
    [Id]   INT           NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_OrderStatuses_Strings] CHECK ([Name]<>'')
);

CREATE UNIQUE NONCLUSTERED INDEX [IX_OrderStatuses_Name]
    ON [dbo].[OrderStatuses]([Name] ASC);

CREATE TABLE [dbo].[Orders] (
    [Id]                  INT        IDENTITY (1, 1) NOT NULL,
    [Date]                DATETIME   NOT NULL,
    [CreatedDate]         DATETIME   DEFAULT (getdate()) NOT NULL,
    [CustomerId]          INT        NOT NULL,
    [StatusId]            INT        NOT NULL,
    [ReminderCreatedDate] DATETIME   NULL,
    [Version]             ROWVERSION NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Orders_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id]),
    CONSTRAINT [FK_Orders_OrderStatuses] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[OrderStatuses] ([Id])
);

CREATE TABLE [dbo].[OrdersDetails] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [OrderId]   INT NOT NULL,
    [ProductId] INT NOT NULL,
    [Qty]       INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OrdersDetails_Orders] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([Id]),
    CONSTRAINT [FK_OrdersDetails_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id])
);

CREATE TABLE [dbo].[Mails] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [Data]     VARBINARY (MAX) NOT NULL,
    [TryCount] INT             DEFAULT ((0)) NOT NULL,
    [Sent]     BIT             DEFAULT ((0)) NOT NULL,
    [Created]  DATETIME        DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
