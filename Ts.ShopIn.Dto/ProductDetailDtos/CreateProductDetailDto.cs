using System.ComponentModel.DataAnnotations;
using Ts.Common.AppCustomAttributes;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
namespace Ts.ShopIn.Dto.ProductDetailDtos
{
    public class CreateProductDetailDto : BaseInputDto
    {
        [SkipHtmlEncoding]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(120, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Title { get; set; }

        [SkipHtmlEncoding]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.UrlLink, ErrorMessage = ErrorMessageConstant.UrlLinkRegx)]
        public string ProductDetailLink { get; set; }

        [SkipHtmlEncoding]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(180, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MetaDescription { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int ShopCategoryId { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int ExchangePolicyId { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int DeliveryPolicyId { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int ReturnPolicyId { get; set; }

        [SkipHtmlEncoding]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(100, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.Keyword, ErrorMessage = ErrorMessageConstant.KeywordRegx)]
        public string Keyword1 { get; set; }

        [SkipHtmlEncoding]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(100, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.Keyword, ErrorMessage = ErrorMessageConstant.KeywordRegx)]
        public string Keyword2 { get; set; }

        [SkipHtmlEncoding]
        [MaxLength(100, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.Keyword, ErrorMessage = ErrorMessageConstant.KeywordRegx)]
        public string Keyword3 { get; set; }

        [SkipHtmlEncoding]
        [MaxLength(100, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.Keyword, ErrorMessage = ErrorMessageConstant.KeywordRegx)]
        public string Keyword4 { get; set; }

        [SkipHtmlEncoding]
        [MaxLength(100, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.Keyword, ErrorMessage = ErrorMessageConstant.KeywordRegx)]
        public string Keyword5 { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMessageConstant.IntegerRange)]
        public int Stock { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMessageConstant.IntegerRange)]
        public decimal Mrp { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMessageConstant.IntegerRange)]
        public decimal Price { get; set; }

        public int? ProductId { get; set; }
    }
}
