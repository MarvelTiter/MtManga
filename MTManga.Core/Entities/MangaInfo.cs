using MTManga.Core.Enums;

namespace MTManga.Core.Entities {
    public class MangaInfo {
        public string Title { get; set; }
        public int Total { get; set; }
        public int Current { get; set; }
        public ItemType FileType { get; set; } = ItemType.UnSet;
        public int Offset { get; set; }
        public int Group { get; set; }
        public int GroupSize { get; set; }
        public string SavedName { get; set; }
    }
}
