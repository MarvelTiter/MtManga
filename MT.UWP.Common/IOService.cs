﻿using MT.UWP.Common.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage.Search;
using Windows.Storage.Streams;

namespace MT.UWP.Common {
    public class IOService {
        IJsonConvertService jsonConvertService;
        public IOService() {
            jsonConvertService = new NewtonJson();
        }
        public async Task<StorageFile> OpenLocalFileAsync(params string[] types) {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // .XXX 
            Regex typeReg = new Regex(@"^\.[\w]+$");
            if (types.Length == 0)
                picker.FileTypeFilter.Add("*");
            else
                foreach (var type in types) {
                    if (type == "*" || typeReg.IsMatch(type))
                        picker.FileTypeFilter.Add(type);
                    else
                        throw new InvalidCastException("文件后缀名不正确");
                }

            var file = await picker.PickSingleFileAsync();
            return file;
        }

        public async Task<StorageFile> GetSaveFileAsync(string type, string name) {
            var save = new FileSavePicker();
            save.DefaultFileExtension = type;
            save.SuggestedFileName = name;
            save.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            var file = await save.PickSaveFileAsync();
            return file;
        }

        public async Task<T> GetLocalDataAsync<T>(string fileName, string defaultValue = "[]") {
            try {
                //var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appdata:///local/" + fileName));
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync(fileName);
                string content = await FileIO.ReadTextAsync(file);
                if (string.IsNullOrEmpty(content))
                    content = defaultValue;
                return jsonConvertService.DeserializeObject<T>(content);
            } catch (Exception) {
                return jsonConvertService.DeserializeObject<T>(defaultValue);
            }
        }

        public async Task ClearLocalDataAsync() {
            var folder = ApplicationData.Current.LocalFolder;
            var items = await folder.GetItemsAsync();
            foreach (var item in items) {
                await item.DeleteAsync();
            }
        }
        private SemaphoreSlim asyncLock = new SemaphoreSlim(1, 1);
        public async Task SetLocalDataAsync(string fileName, object content) {
            try {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                var contentString = jsonConvertService.SerializeObject(content);
                await FileIO.WriteTextAsync(file, contentString);//  
            } catch (Exception ex) {
            }
        }

        public async Task<T> LockLocalFolder<T>(Func<StorageFolder, Task<T>> action) {
            await asyncLock.WaitAsync();
            try {
                Debug.WriteLine($"---------LockLocalFolder--------CurrentCount:{asyncLock.CurrentCount}");
                var folder = ApplicationData.Current.LocalFolder;
                return await action.Invoke(folder);
            } catch {
                return default;
            }
        }

        public async Task<StorageFolder> GetUserFolderAsync(string token = null) {
            var picker = new FolderPicker();
            picker.FileTypeFilter.Add("*");
            StorageFolder folder;
            if (token == null) {
                folder = await picker.PickSingleFolderAsync();
            } else {
                folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);
            }
            return folder;
        }

        public string SaveFolder(StorageFolder folder) {
            var token = StorageApplicationPermissions.FutureAccessList.Add(folder);
            return token;
        }
    }
}
