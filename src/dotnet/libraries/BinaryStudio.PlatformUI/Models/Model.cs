using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Threading;

namespace BinaryStudio.PlatformUI.Models
    {
    public abstract class Model : NotifyPropertyChangedDispatcherObject
        {
        private static readonly IDictionary<Type,Type> Types = new Dictionary<Type,Type>();
        protected Model()
            {
            }

        protected Model(Dispatcher dispatcher)
            :base(dispatcher)
            {
            }

        #region M:RegisterModelTypes(Assembly)
        public static void RegisterModelTypes(Assembly assembly) {
            if (assembly == null) { throw new ArgumentNullException(nameof(assembly)); }
            foreach (var type in assembly.GetTypes().Where(i => i.IsSubclassOf(typeof(NotifyPropertyChangedDispatcherObject)) && !i.IsAbstract)) {
                foreach (var attribute in type.GetCustomAttributes(typeof(ModelAttribute), false).OfType<ModelAttribute>()) {
                    Types[attribute.Type] = type;
                    }
                }
            }
        #endregion
        #region M:RegisterModelType(Type,Type)
        public static void RegisterModelType(Type ObjectType,Type ModelType) {
            if (ObjectType == null) { throw new ArgumentNullException(nameof(ObjectType)); }
            if (ModelType == null) { throw new ArgumentNullException(nameof(ModelType)); }
            Types[ObjectType] = ModelType;
            }
        #endregion
        #region M:CreateModel(Object,Type):Object
        public static Object CreateModel(Object source, Type type) {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (Types.TryGetValue(type, out var Type)) {
                var ctor = Type.GetConstructor(
                    BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance,
                    null,new Type[]{
                    type
                    },null);
                if (ctor == null) { throw new MissingMethodException(); }
                return ctor.Invoke(new Object[]{ source });
                }
            return source;
            }
        #endregion
        #region M:CreateModel(Object):Object
        public static Object CreateModel(Object source) {
            return (source != null)
                ? CreateModel(source,source.GetType())
                : null;
            }
        #endregion
        }

    public abstract class Model<T> : Model
        {
        protected Model()
            {
            }

        protected Model(Dispatcher dispatcher)
            :base(dispatcher)
            {
            }
        }
    }