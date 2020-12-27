using MTManga.UWP.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTManga.UWP.Services {
    public interface IMangaCollectionService {
        object DataCore { get; set; }
        Task<ObservableCollection<MangaEntity>> LoadMangasAsync();
    }
}
