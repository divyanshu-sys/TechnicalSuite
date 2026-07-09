using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.ShopIn.Client.ViewModels.ProductDetailVms
{
    public class CreateProductDetailVm
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(120, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Title { get; set; }

        [DisplayName("Product Detail Link")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.UrlLink, ErrorMessage = ErrorMessageConstant.UrlLinkRegx)]
        public string ProductDetailLink { get; set; }

        [DisplayName("Meta Description")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(180, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MetaDescription { get; set; }

        [DisplayName("Shop Category")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int ShopCategoryId { get; set; }

        [DisplayName("Exchange Policy")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int ExchangePolicyId { get; set; }

        [DisplayName("Delivery Policy")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int DeliveryPolicyId { get; set; }

        [DisplayName("Return Policy")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int ReturnPolicyId { get; set; }

        [DisplayName("Keyword 1")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(100, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.Keyword, ErrorMessage = ErrorMessageConstant.KeywordRegx)]
        public string Keyword1 { get; set; }

        [DisplayName("Keyword 2")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(100, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.Keyword, ErrorMessage = ErrorMessageConstant.KeywordRegx)]
        public string Keyword2 { get; set; }

        [DisplayName("Keyword 3")]
        [MaxLength(100, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.Keyword, ErrorMessage = ErrorMessageConstant.KeywordRegx)]
        public string Keyword3 { get; set; }

        [DisplayName("Keyword 4")]
        [MaxLength(100, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.Keyword, ErrorMessage = ErrorMessageConstant.KeywordRegx)]
        public string Keyword4 { get; set; }

        [DisplayName("Keyword 5")]
        [MaxLength(100, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.Keyword, ErrorMessage = ErrorMessageConstant.KeywordRegx)]
        public string Keyword5 { get; set; }

        [DisplayName("Is Available")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMessageConstant.IntegerRange)]
        public int Stock { get; set; }

        [DisplayName("MRP")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMessageConstant.IntegerRange)]
        public decimal Mrp { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMessageConstant.IntegerRange)]
        public decimal Price { get; set; }

        [DisplayName("Variant Of")]
        public int? ProductId { get; set; }
    }
}
