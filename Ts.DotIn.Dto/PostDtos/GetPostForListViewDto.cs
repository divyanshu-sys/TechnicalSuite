namespace Ts.DotIn.Dto.PostDtos
{
    public class GetPostForListViewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string PostLink { get; set; }
        public string MainImageUrl { get; set; }
        public DateTime? PublishedOn { get; set; }
    }
}
