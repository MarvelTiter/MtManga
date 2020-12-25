using MTManga.UWP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTManga.UWP.Services {
    public class LocalManga : IMangaService {
        public MangaEntity Subject { get; set; }
        public void LoadChapters() {
            throw new NotImplementedException();
        }
    }
}
