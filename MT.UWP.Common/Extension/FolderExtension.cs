using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Search;

namespace MT.UWP.Common.Extension {
    public static class FolderExtension {
        public static async Task<IReadOnlyList<IStorageItem>> GetLocalItemInFolderAsync(this StorageFolder self, params string[] types) {
            QueryOptions itemQuery = FileExtensionQuery(types);
            //itemQuery.SortOrder.Add(new SortEntry { PropertyName = "System.FileName", AscendingOrder = true });

            //itemQuery.ApplicationSearchFilter = "新建";
            var queryResult = self.CreateItemQueryWithOptions(itemQuery);
            var storageItems = await queryResult.GetItemsAsync();
            return storageItems;
        }

        public static async Task<IReadOnlyList<IStorageItem>> GetLocalItemInFolderAsync(this StorageFolder self, uint index, uint size, params string[] types) {
            QueryOptions itemQuery = FileExtensionQuery(types);
            var queryResult = self.CreateItemQueryWithOptions(itemQuery);
            return await queryResult.GetItemsAsync(index * size, size);
        }

        public static async Task<int> GetFolderFileCount(this StorageFolder self, params string[] types) {
            QueryOptions itemQuery = FileExtensionQuery(types);
            var queryResult = self.CreateItemQueryWithOptions(itemQuery);
            var count = await queryResult.GetItemCountAsync();
            return (int)count;
        }

        private static QueryOptions FileExtensionQuery(string[] types) {
            QueryOptions itemQuery = new QueryOptions();
            Regex typeReg = new Regex(@"^\.[\w]+$");
            if (types.Length == 0)
                itemQuery.FileTypeFilter.Add("*");
            else
                foreach (var type in types) {
                    if (type == "*" || typeReg.IsMatch(type))
                        itemQuery.FileTypeFilter.Add(type);
                    else
                        throw new InvalidCastException("文件后缀名不正确");
                }

            return itemQuery;
        }

        public static StorageFolder Folder(this IStorageItem self) {
            return self as StorageFolder;
        }

        public static StorageFile File(this IStorageItem self) {
            return self as StorageFile;
        }

        public static async Task<IStorageItem> GetFirstItem(this StorageFolder self) {
            QueryOptions itemQuery = new QueryOptions();
            itemQuery.FileTypeFilter.Add("*");
            var queryResult = self.CreateItemQueryWithOptions(itemQuery);
            var result = await queryResult.GetItemsAsync(0, 1);
            return result.FirstOrDefault();
        }
    }
}
