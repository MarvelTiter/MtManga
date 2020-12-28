using MTManga.Core.Entities;
using MTManga.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTManga.UWP.ServicesImp {
    public class LocalManga : IMangaService {
        public MangaEntity Subject { get; set; }
        public void LoadChapters() {
            throw new NotImplementedException();
        }
    }
}
