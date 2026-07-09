using Ts.Dto;
namespace Ts.DotIn.Dto.StoryDtos
{
    public class StoryDto : BaseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string StoryLink { get; set; }
        public string MainImage { get; set; }
        public string MainImageUrl { get; set; }
        public string MainDescription { get; set; }
        public string MetaDescription { get; set; }
        public int SubCategoryId { get; set; }
        public string Description { get; set; }
        public string ImageBaseUrl { get; set; }
        public string Keyword1 { get; set; }
        public string Keyword2 { get; set; }
        public string Keyword3 { get; set; }
        public string Keyword4 { get; set; }
        public string Keyword5 { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedById { get; set; }
        public string StoryWorkerId { get; set; }
        public string MainImageSource { get; set; }
        public string StorySource { get; set; }
    }
}
