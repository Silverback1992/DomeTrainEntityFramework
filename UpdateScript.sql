IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525142852_InitialSchema'
)
BEGIN
    CREATE TABLE [Genres] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Genres] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525142852_InitialSchema'
)
BEGIN
    CREATE TABLE [Pictures] (
        [Identifier] int NOT NULL IDENTITY,
        [Title] varchar(128) NOT NULL,
        [ReleaseDate] char(8) NOT NULL,
        [Plot] varchar(max) NULL,
        [AgeRating] varchar(32) NOT NULL,
        [MainGenreId] int NOT NULL,
        CONSTRAINT [PK_Pictures] PRIMARY KEY ([Identifier]),
        CONSTRAINT [FK_Pictures_Genres_MainGenreId] FOREIGN KEY ([MainGenreId]) REFERENCES [Genres] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525142852_InitialSchema'
)
BEGIN
    CREATE TABLE [Actors] (
        [MovieIdentifier] int NOT NULL,
        [Id] int NOT NULL IDENTITY,
        [FirstName] nvarchar(max) NULL,
        [LastName] nvarchar(max) NULL,
        CONSTRAINT [PK_Actors] PRIMARY KEY ([MovieIdentifier], [Id]),
        CONSTRAINT [FK_Actors_Pictures_MovieIdentifier] FOREIGN KEY ([MovieIdentifier]) REFERENCES [Pictures] ([Identifier]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525142852_InitialSchema'
)
BEGIN
    CREATE TABLE [Directors] (
        [MovieIdentifier] int NOT NULL,
        [FirstName] nvarchar(max) NULL,
        [LastName] nvarchar(max) NULL,
        CONSTRAINT [PK_Directors] PRIMARY KEY ([MovieIdentifier]),
        CONSTRAINT [FK_Directors_Pictures_MovieIdentifier] FOREIGN KEY ([MovieIdentifier]) REFERENCES [Pictures] ([Identifier]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525142852_InitialSchema'
)
BEGIN
    CREATE INDEX [IX_Pictures_MainGenreId] ON [Pictures] ([MainGenreId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525142852_InitialSchema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260525142852_InitialSchema', N'10.0.8');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525143243_AddedImdbRating'
)
BEGIN
    ALTER TABLE [Pictures] ADD [ImdbRating] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525143243_AddedImdbRating'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260525143243_AddedImdbRating', N'10.0.8');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525145557_ChangedToInternetRating'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Pictures]') AND [c].[name] = N'ImdbRating');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Pictures] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Pictures] ALTER COLUMN [ImdbRating] decimal(18,2) NOT NULL;
    ALTER TABLE [Pictures] ADD DEFAULT 0.0 FOR [ImdbRating];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525145557_ChangedToInternetRating'
)
BEGIN
    EXEC sp_rename N'[Pictures].[ImdbRating]', N'InternetRating', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525145557_ChangedToInternetRating'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260525145557_ChangedToInternetRating', N'10.0.8');
END;

COMMIT;
GO

