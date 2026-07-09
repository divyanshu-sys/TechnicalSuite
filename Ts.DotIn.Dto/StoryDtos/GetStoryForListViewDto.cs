namespace Ts.DotIn.Dto.StoryDtos
{
    public class GetStoryForListViewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string StoryLink { get; set; }
        public string MainImageUrl { get; set; }
        public DateTime? PublishedOn { get; set; }
    }
}
