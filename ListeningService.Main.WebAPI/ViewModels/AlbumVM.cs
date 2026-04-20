namespace ListeningService.Main.WebAPI.ViewModels
{
    public record AlbumVM(Guid Id, MultilingualString Name, Guid CategoryId)
    {
        public static AlbumVM? Create(Album? album)
        {
            if (album == null) return null;
            return new AlbumVM(album.Id, album.Name, album.CategoryId);
        }

        public static AlbumVM[] Create(IEnumerable<Album> albums)
        {
            return albums.Select(a => Create(a)).ToArray()!;
        }
    }
}
