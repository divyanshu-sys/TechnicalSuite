using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ts.Common.Constant.AppConstants;
namespace Ts.DotCom.Client.ViewModels.StoryVms
{
    public class UpdateStoryVm
    {
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(70, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string Title { get; set; }

        [DisplayName("Story Link")]
        public string StoryLink { get; set; }

        [DisplayName("Meta Description")]
        [Required(ErrorMessage = ErrorMessageConstant.PleaseSpecify)]
        [MaxLength(160, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string MetaDescription { get; set; }

        [DisplayName("Category")]
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

        [DisplayName("Story Source")]
        [MaxLength(250, ErrorMessage = ErrorMessageConstant.MaxLength)]
        public string StorySource { get; set; }
    }
}
