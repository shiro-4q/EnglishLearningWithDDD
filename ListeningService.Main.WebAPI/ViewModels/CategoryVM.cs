namespace ListeningService.Main.WebAPI.ViewModels
{
    public record CategoryVM(Guid Id, MultilingualString Name, Uri CoverUrl)
    {
        public static CategoryVM? Create(Category? category)
        {
            if (category == null) return null;
            return new CategoryVM(category.Id, category.Name, category.CoverUrl);
        }

        public static CategoryVM[] Create(IEnumerable<Category> categories)
        {
            return categories.Select(c => Create(c)).ToArray()!;
        }
    }
}
