using CoinDeskAPI.Models.ViewModels.Currency;
using FluentValidation;

namespace CoinDeskAPI.Validators.ViewModels.Currency;

public class AddCurrencyVMValidator : AbstractValidator<AddCurrencyViewModel>
{
    public AddCurrencyVMValidator()
    {
        RuleFor(x => x).NotNull().WithMessage("新增幣別資料不可為空");

        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("貨幣代碼不可為空")
            .Length(3).WithMessage("貨幣代碼長度必須為3個字元");

        RuleFor(x => x.FullCurrencyName)
            .NotEmpty().WithMessage("全名貨幣名稱不可為空")
            .MaximumLength(100).WithMessage("全名貨幣名稱長度不可超過50個字元");


        RuleFor(x => x.ShortCurrencyName)
            .NotEmpty().WithMessage("簡短貨幣名稱不可為空")
            .MaximumLength(50).WithMessage("簡短貨幣名稱長度不可超過10個字元");

        RuleFor(x => x.CurrencyDesc)
            .MaximumLength(200).WithMessage("貨幣描述長度不可超過100個字元");
    }
}
