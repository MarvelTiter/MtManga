using MT.UWP.Common.IServices;
using MT.UWP.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MT.UWP.Common {
    public class SettingService {
        private readonly Option option;
        IJsonConvertService jsonConvertService;

        public SettingService(Option option) {
            this.option = option;
            jsonConvertService = new NewtonJson();
        }
        /// <summary>
        /// 获取设置对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetSetting<T>(Enum key) where T : class, new() {
            var localcontainer = currentContainer();
            bool exit = localcontainer.Values.ContainsKey(key.ToString());
            if (exit) {
                var v = localcontainer.Values[key.ToString()].ToString();
                var setting = jsonConvertService.DeserializeObject<T>(v);
                return setting;
            } else {
                return new T();
            }
        }

        /// <summary>
        /// 保存设置对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public bool SaveSetting<T>(Enum key, T setting) {
            var localcontainer = currentContainer();
            var value = jsonConvertService.SerializeObject(setting);
            localcontainer.Values[key.ToString()] = value;
            return true;
        }

        /// <summary>
        /// 保存单个设置
        /// </summary>
        /// <param name="key">设置名</param>
        /// <param name="value">设置值</param>
        public void SaveLocalSetting(Enum key, object value) {
            var localcontainer = currentContainer();
            localcontainer.Values[key.ToString()] = value;
        }

        /// <summary>
        /// 获取单个设置
        /// </summary>
        /// <param name="key">设置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public string GetLocalSetting(Enum key, string defaultValue) {
            var localcontainer = currentContainer();
            bool isKeyExist = localcontainer.Values.ContainsKey(key.ToString());
            if (isKeyExist) {
                return localcontainer.Values[key.ToString()].ToString();
            } else {
                SaveLocalSetting(key, defaultValue);
                return defaultValue;
            }
        }

        public bool GetLocalSetting(Enum key, out string value) {
            var localcontainer = currentContainer();
            bool isKeyExist = localcontainer.Values.ContainsKey(key.ToString());
            if (isKeyExist) {
                value = localcontainer.Values[key.ToString()].ToString();
            } else
                value = "";
            return isKeyExist;
        }

        public T GetLocalSetting<T>(Enum key, T defaultValue) {
            var localcontainer = currentContainer();
            bool isKeyExist = localcontainer.Values.ContainsKey(key.ToString());
            if (isKeyExist) {
                var v = localcontainer.Values[key.ToString()];
                return (T)Convert.ChangeType(v, typeof(T));
            } else {
                SaveLocalSetting(key, defaultValue);
                return defaultValue;
            }
        }
        private ApplicationDataContainer currentContainer() {
            var localSetting = ApplicationData.Current.LocalSettings;
            var localcontainer = localSetting.CreateContainer(option.SettingContainerName, ApplicationDataCreateDisposition.Always);
            return localcontainer;
        }
    }
}
