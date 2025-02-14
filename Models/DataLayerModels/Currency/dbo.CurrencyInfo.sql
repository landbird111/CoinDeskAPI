CREATE TABLE [dbo].[CurrencyInfo] (
    [CurrencyInfoId]   INT          IDENTITY (1, 1) NOT NULL,
    [CurrencyCode]     NVARCHAR (5) NOT NULL,
    [LastModifiedTime] DATETIME     DEFAULT (getdate()) NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [KEY_CurrencyInfo_CurrencyInfoI]
    ON [dbo].[CurrencyInfo]([CurrencyInfoId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別資料表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CurrencyInfo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CurrencyInfo', @level2type = N'COLUMN', @level2name = N'CurrencyInfoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CurrencyInfo', @level2type = N'COLUMN', @level2name = N'CurrencyCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CurrencyInfo', @level2type = N'COLUMN', @level2name = N'LastModifiedTime';

