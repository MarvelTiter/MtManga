using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTManga.UWP.Extention {
    public static class StreamExtension {
        public static MemoryStream ToMemoryStream(this Stream self) {
            MemoryStream mem = new MemoryStream();
            byte[] buffer = new byte[1024 * 4];
            while (self.Read(buffer, 0, buffer.Length) > 0) {
                mem.Write(buffer, 0, buffer.Length);
            }
            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        }
    }
}
