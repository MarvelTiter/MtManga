﻿using MT.UWP.Common.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace MT.UWP.Common {
    public class IOService  {
        IJsonConvertService jsonConvertService;
        public IOService() {
            jsonConvertService = new NewtonJson();
        }
        public async Task<StorageFile> OpenLocalFileAsync(params string[] types) {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // .XXX 
            Regex typeReg = new Regex(@"^\.[\w]+$");
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
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appdata:///local/" + fileName));
                string content = await FileIO.ReadTextAsync(file);
                if (string.IsNullOrEmpty(content))
                    content = defaultValue;
                return jsonConvertService.DeserializeObject<T>(content);
            } catch (Exception) {
                return jsonConvertService.DeserializeObject<T>(defaultValue);
            }
        }

        public async Task SetLocalDataAsync(string fileName, string content) {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, content);
        }
    }
}