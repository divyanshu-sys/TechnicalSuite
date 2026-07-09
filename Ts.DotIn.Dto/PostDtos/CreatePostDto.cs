using System.ComponentModel.DataAnnotations;
using Ts.Common.AppCustomAttributes;
using Ts.Common.Constant.AppConstants;
using Ts.Dto;
namespace Ts.DotIn.Dto.PostDtos
{
    public class CreatePostDto : BaseInputDto
    {
        [SkipHtmlEncoding]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(80, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Title { get; set; }

        [SkipHtmlEncoding]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        [RegularExpression(RegxConstant.UrlLink, ErrorMessage = ErrorMessageConstant.UrlLinkRegx)]
        public string PostLink { get; set; }

        [SkipHtmlEncoding]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(180, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MetaDescription { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        public int SubCategoryId { get; set; }

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

        [SkipHtmlEncoding]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string PostSource { get; set; }
    }
}
