namespace Ts.ShopIn.Dto.BlogDtos
{
    public class GetBlogForListViewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string BlogLink { get; set; }
        public string MainImageUrl { get; set; }
        public DateTime? PublishedOn { get; set; }
    }
}
