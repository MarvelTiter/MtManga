using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MT.MVVM.Core.Ioc {
    public partial class MIoC : IMIocContainter, IMIocController {
        private Dictionary<Type, Type> _interfaceClassMapper = new Dictionary<Type, Type>();
        private Dictionary<Type, ConstructorInfo> _ctorInfos = new Dictionary<Type, ConstructorInfo>();
        private Dictionary<Type, Dictionary<string, Delegate>> _factories = new Dictionary<Type, Dictionary<string, Delegate>>();
        private readonly string _defaultKey = Guid.NewGuid().ToString();
        private Dictionary<Type, Dictionary<string, object>> _instancesRegistry = new Dictionary<Type, Dictionary<string, object>>();

        private static readonly object _instanceLock = new object();
        private readonly object _syncLock = new object();

        private static MIoC _default;
        public static MIoC Default {
            get {
                if (_default == null) {
                    lock (_instanceLock) {
                        if (_default == null) {
                            _default = new MIoC();
                        }
                    }
                }

                return _default;
            }
        }

        #region Container member

        public void RegisterScope<TI, T>()
          where TI : class
          where T : class, TI {
            Register<TI, T>(false);
        }

        public void RegisterSingle<TI, T>()
            where TI : class
            where T : class, TI {
            Register<TI, T>(true);
        }

        public void RegisterSingle<TI, T>(string key)
            where TI : class
            where T : class, TI {
            Register<TI, T>(true, key);
        }

        public void RegisterScope<T>()
          where T : class {
            Register<T>(false);
        }
        public void RegisterSingle<T>()
           where T : class {
            Register<T>(true);
        }

        public void RegisterSingle<T>(string key)
           where T : class {
            Register<T>(true, key);
        }

        public void RegisterSingle<TI, T>(T ins)
            where TI : class
            where T : class, TI {
            Register<TI, T>(_defaultKey, ins);
        }

        public void RegisterSingle<TI, T>(T ins, string key)
            where TI : class
            where T : class, TI {
            Register<TI, T>(key, ins);
        }

        #endregion

        #region controller member

        public T GetScope<T>() {
            return (T)DoGetService(typeof(T), _defaultKey, false);
        }

        public T GetSingleton<T>() {
            return (T)DoGetService(typeof(T), _defaultKey);
        }
        public T GetScope<T>(string key) {
            return (T)DoGetService(typeof(T), key, false);
        }

        public T GetSingleton<T>(string key) {
            return (T)DoGetService(typeof(T), key);
        }

        #endregion              

    }

    /// <summary>
    /// private member
    /// </summary>
    public partial class MIoC {
        private void Register<TI, T>(bool single, string key = null)
            where TI : class
            where T : class, TI {
            lock (_syncLock) {
                var interfaceType = typeof(TI);
                var classType = typeof(T);

                if (_interfaceClassMapper.ContainsKey(interfaceType)) {
                    if (_interfaceClassMapper[interfaceType] != classType) {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "There is already a class registered for {0}.",
                                interfaceType.FullName));
                    }
                } else {
                    _interfaceClassMapper.Add(interfaceType, classType);
                    _ctorInfos.Add(classType, GetConstructorInfo(classType));
                }

                Func<TI> factory = MakeInstance<TI>;
                DoRegister(interfaceType, factory, key ?? _defaultKey);
                if (single) {
                    CacheInstance(interfaceType, key ?? _defaultKey);
                }
            }
        }

        private void Register<TI, T>(string key, T ins) where T : TI {
            lock (_syncLock) {
                var interfaceType = typeof(TI);
                var classType = typeof(T);

                if (_interfaceClassMapper.ContainsKey(interfaceType)) {
                    if (_interfaceClassMapper[interfaceType] != classType) {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "There is already a class registered for {0}.",
                                interfaceType.FullName));
                    }
                } else {
                    _interfaceClassMapper.Add(interfaceType, classType);
                    _ctorInfos.Add(classType, GetConstructorInfo(classType));
                }

                Func<TI> factory = () => ins;
                DoRegister(interfaceType, factory, key ?? _defaultKey);
                CacheInstance(interfaceType, key ?? _defaultKey);
            }
        }

        private void Register<T>(bool single, string key = null) {
            var classType = typeof(T);

            if (classType.IsInterface) {
                throw new ArgumentException("An interface cannot be registered alone.");
            }

            lock (_syncLock) {
                if (_factories.ContainsKey(classType)
                    && _factories[classType].ContainsKey(_defaultKey)) {
                    if (!_ctorInfos.ContainsKey(classType)) {
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                            "Class {0} is already registered.",
                            classType));
                    }
                    return;
                }

                if (!_interfaceClassMapper.ContainsKey(classType)) {
                    _interfaceClassMapper.Add(classType, null);
                }

                _ctorInfos.Add(classType, GetConstructorInfo(classType));
                Func<T> factory = MakeInstance<T>;
                DoRegister(classType, factory, key ?? _defaultKey);

                if (single) {
                    CacheInstance(classType, key ?? _defaultKey);
                }
            }
        }

        private T MakeInstance<T>() {
            var serviceType = typeof(T);
            var constructor = _ctorInfos.ContainsKey(serviceType)
                                  ? _ctorInfos[serviceType]
                                  : GetConstructorInfo(serviceType);

            var parameterInfos = constructor.GetParameters();

            if (parameterInfos.Length == 0) {
                return (T)constructor.Invoke(null);
            }

            var parameters = new object[parameterInfos.Length];

            foreach (var parameterInfo in parameterInfos) {
                parameters[parameterInfo.Position] = GetService(parameterInfo.ParameterType);
            }

            return (T)constructor.Invoke(parameters);
        }

        private ConstructorInfo GetConstructorInfo(Type serviceType) {
            Type resolveTo;

            if (_interfaceClassMapper.ContainsKey(serviceType)) {
                resolveTo = _interfaceClassMapper[serviceType] ?? serviceType;
            } else {
                resolveTo = serviceType;
            }

            var constructorInfos = resolveTo.GetConstructors();

            if (constructorInfos.Length == 0 || (constructorInfos.Length == 1 && !constructorInfos[0].IsPublic)) {
                throw new Exception(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Cannot register: No public constructor found in {0}.",
                        resolveTo.Name));
            }
            if (constructorInfos.Length > 1) {
                if (constructorInfos.Length > 2) {
                    return GetPreferredConstructorInfo(constructorInfos);
                }

                if (constructorInfos.FirstOrDefault(i => i.Name == ".cctor") == null) {
                    return GetPreferredConstructorInfo(constructorInfos);
                }

                var first = constructorInfos.FirstOrDefault(i => i.Name != ".cctor");

                if (first == null || !first.IsPublic) {
                    throw new Exception(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Cannot register: No public constructor found in {0}.",
                            resolveTo.Name));
                }
                return first;
            }
            return constructorInfos[0];
        }
        private ConstructorInfo GetPreferredConstructorInfo(IEnumerable<ConstructorInfo> ctors) {
            //ctors.Where(c => {
            //    c.GetCustomAttribute(typeof)
            //})

            throw new Exception(string.Format(
                        CultureInfo.InvariantCulture,
                        "Cannot register: NotImplemented"));
        }
        private void DoRegister<T>(Type classType, Func<T> factory, string key) {
            if (_factories.ContainsKey(classType)) {
                if (_factories[classType].ContainsKey(key)) {
                    return;
                }
                _factories[classType].Add(key, factory);
            } else {
                var list = new Dictionary<string, Delegate>();
                list.Add(key, factory);
                _factories.Add(classType, list);
            }
        }
        private object GetService(Type serviceType) {
            return DoGetService(serviceType, _defaultKey);
        }
        private object DoGetService(Type serviceType, string key, bool cache = true) {
            lock (_syncLock) {
                if (string.IsNullOrEmpty(key)) {
                    key = _defaultKey;
                }
                if (cache)
                    return CacheInstance(serviceType, key);
                else
                    return CreateInstance(serviceType, key);

            }
        }
        private object CreateInstance(Type serviceType, string key) {
            if (_factories.ContainsKey(serviceType)) {
                if (_factories[serviceType].ContainsKey(key)) {
                    return _factories[serviceType][key].DynamicInvoke(null);
                }

                if (_factories[serviceType].ContainsKey(_defaultKey)) {
                    return _factories[serviceType][_defaultKey].DynamicInvoke(null);
                }
            }
            throw new Exception(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Type not found: {0}",
                    serviceType.FullName));

        }
        private object CacheInstance(Type serviceType, string key) {
            Dictionary<string, object> instances = null;
            if (!_instancesRegistry.ContainsKey(serviceType)) {
                if (!_interfaceClassMapper.ContainsKey(serviceType)) {
                    throw new Exception(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Type not found in cache: {0}.",
                            serviceType.FullName));
                }
                instances = new Dictionary<string, object>();
                _instancesRegistry.Add(serviceType, instances);
            } else {
                instances = _instancesRegistry[serviceType];
            }

            if (instances != null
                && instances.ContainsKey(key)) {
                return instances[key];
            }
            var instance = CreateInstance(serviceType, key);

            if (instances != null) {
                instances.Add(key, instance);
            }
            return instance;
        }
    }
}
