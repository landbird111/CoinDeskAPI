CREATE TABLE [dbo].[CurrencyLang] (
    [CurrencyLangId]      INT            IDENTITY (1, 1) NOT NULL,
    [CurrencyInfoId]      INT            NOT NULL,
    [LangKey]             VARCHAR (10)   NOT NULL,
    [CurrencyName]        NVARCHAR (100) NOT NULL,
    [CurrencyShortName]   NVARCHAR (50)  NULL,
    [CurrencyDescription] NVARCHAR (200) NULL,
    CONSTRAINT [FK_CurrencyLang_CurrencyInfoId] FOREIGN KEY ([CurrencyInfoId]) REFERENCES [dbo].[CurrencyInfo] ([CurrencyInfoId])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UK_CurrencyLang]
    ON [dbo].[CurrencyLang]([CurrencyInfoId] ASC, [LangKey] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別語系', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CurrencyLang';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別語系Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CurrencyLang', @level2type = N'COLUMN', @level2name = N'CurrencyLangId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CurrencyLang', @level2type = N'COLUMN', @level2name = N'CurrencyInfoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別語系代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CurrencyLang', @level2type = N'COLUMN', @level2name = N'LangKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別語系名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CurrencyLang', @level2type = N'COLUMN', @level2name = N'CurrencyName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別簡稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CurrencyLang', @level2type = N'COLUMN', @level2name = N'CurrencyShortName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CurrencyLang', @level2type = N'COLUMN', @level2name = N'CurrencyDescription';

