﻿using MT.UWP.Common;
using MT.UWP.Common.Extension;
using MTManga.Core.Entities;
using MTManga.Core.Enums;
using MTManga.Core.Extention;
using MTManga.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml.Media.Imaging;

namespace MTManga.UWP.ServicesImp {
    public class LocalMangaReading : IMangaReadingService {
        public object DataCore { get; set; }
        private MangaEntity _entity;
        private StorageFolder _folder;
        private ZipArchive _zip;
        private bool flag;
        private List<MangaInfo> _infos;
        private string saveName;
        private bool disposedValue;

        public async Task<BitmapImage> ReadAsync(int index) {
            await Init();
            if (index < 0)
                return null;
            if (_entity.Info.FileType == ItemType.ZipManga) {
                // StorageFile ZipArchive
                var entry = _zip.Entries[index];
                return await entry.Open().ToMemoryStream().AsRandomAccessStream().WriteBitmap();
            } else {
                // StorageFolder
                var file = await _folder.GetFilesAsync(CommonFileQuery.DefaultQuery, (uint)index, 1);
                var random = await file[0].OpenAsync(FileAccessMode.Read);
                return await random.WriteBitmap();
            }
        }

        public async Task SaveReadingProgressAsync() {
            _infos = await App.Helper.IO.GetLocalDataAsync<List<MangaInfo>>(saveName);
            _infos.ForEach(i => {
                if (i.Title == _entity.Info.Title) {
                    i.Current = _entity.Info.Current;
                }
            });
            await App.Helper.IO.SetLocalDataAsync(saveName, _infos);
        }

        public async Task Init() {
            if (!flag) {
                _entity = DataCore as MangaEntity;
                if (_entity.Info.FileType == ItemType.List)
                    throw new CustomException($"目录结构异常！{_entity.StorageItem.Name}");
                _folder = _entity.Info.FileType == ItemType.FolderManga ? _entity.StorageItem.Folder() : null;
                _zip = _entity.Info.FileType == ItemType.ZipManga ? await createZipZrchive() : null;
                saveName = _entity.Info.SavedName;
                flag = true;
            }
        }

        private async Task<ZipArchive> createZipZrchive() {
            var file = _entity.StorageItem.File();
            var fileStream = await file.OpenReadAsync();
            var zipArchive = new ZipArchive(fileStream.AsStream());
            return zipArchive;
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: 释放托管状态(托管对象)
                    _zip?.Dispose();
                }
                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                _entity = null;
                DataCore = null;
                disposedValue = true;
            }
        }
        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~LocalMangaReading()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose() {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}