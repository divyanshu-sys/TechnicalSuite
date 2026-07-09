using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.DotCom.Client.ViewModels.PostVms
{
    public class CreatePostVm
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(80, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Title { get; set; }

        [DisplayName("Post Link")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.UrlLink, ErrorMessage = ErrorMessageConstant.UrlLinkRegx)]
        public string PostLink { get; set; }

        [DisplayName("Meta Description")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(180, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MetaDescription { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int CategoryId { get; set; }

        [DisplayName("Sub Category")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int SubCategoryId { get; set; }

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

        [DisplayName("Post Source")]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string PostSource { get; set; }
    }
}
