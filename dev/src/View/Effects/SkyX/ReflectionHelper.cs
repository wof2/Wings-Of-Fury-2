
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace SkyX
{
    /// <summary>
    /// ReflectionHelper.
    /// </summary>
    public static class ReflectionHelper
    {

        private static BindingFlags defaultFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctorTypes"></param>
        /// <param name="ctorArgs"></param>
        /// <returns></returns>
        public static T Construct<T>(Type[] ctorTypes, object[] ctorArgs)
        {
            Type typeToCreate = typeof(T);

            ConstructorInfo ci = typeToCreate.GetConstructor(
                                    BindingFlags.NonPublic | BindingFlags.Instance,
                                    null, ctorTypes, new ParameterModifier[0]);

            return (T)ci.Invoke(ctorArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static T Construct<T>()
        {
            return (T)Construct(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static T Construct<T>(object[] args)
        {
            return (T)Construct(typeof(T),args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToCreate"></param>
        /// <returns></returns>
        public static Object Construct(Type typeToCreate)
        {
            Object instance = null;
            try
            {
                instance = Activator.CreateInstance(typeToCreate, defaultFlags, null, new object[] { }, CultureInfo.CurrentCulture);
            }
            catch (MissingMethodException)
            {
                Mogre.LogManager.Singleton.LogMessage("Missing default costructor for type " + typeToCreate);
            }

            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToCreate"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Object Construct(Type typeToCreate,object[] args)
        {
            Object instance = null;
            try
            {
                instance = Activator.CreateInstance(typeToCreate, defaultFlags, null, args, CultureInfo.CurrentCulture);
            }
            catch (MissingMethodException)
            {
                Mogre.LogManager.Singleton.LogMessage("Missing default costructor for type " + typeToCreate);
            }

            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static T GetField<T>(Object obj, string fieldName)
        {
            FieldInfo fi = obj.GetType().GetField(fieldName, defaultFlags);
            if (fi == null)
            {
                fi = obj.GetType().BaseType.GetField(fieldName, defaultFlags);
            }
            return (T)fi.GetValue(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetField<T>(T obj, string fieldName)
        {
            FieldInfo fi = obj.GetType().GetField(fieldName, defaultFlags);
            if (fi == null)
            {
                fi = obj.GetType().BaseType.GetField(fieldName, defaultFlags);
            }
            return fi.GetValue(obj);
        }

        /// <summary>
        /// Gets a valid pointer from given method.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static IntPtr GetFieldPointer(Object obj, string fieldName)
        {
            unsafe
            {
                void* handle = Pointer.Unbox(ReflectionHelper.GetField<Pointer>(obj, fieldName));
                return (IntPtr)handle;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetField<T>(T obj, string fieldName, object value)
        {
            FieldInfo fi = obj.GetType().GetField(fieldName, defaultFlags);
            if (fi == null)
            {
                fi = obj.GetType().BaseType.GetField(fieldName, defaultFlags);
            }
            fi.SetValue(obj, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static U GetProperty<T, U>(T obj, string fieldName)
        {
            PropertyInfo fi = typeof(T).GetProperty(fieldName, defaultFlags);
            if (fi == null)
            {
                fi = typeof(T).GetType().BaseType.GetProperty(fieldName, defaultFlags);
            }
            return (U)fi.GetValue(obj, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Object GetProperty<T>(T obj, string fieldName)
        {
            PropertyInfo fi = obj.GetType().GetProperty(fieldName, defaultFlags);
            if (fi == null)
            {
                fi = obj.GetType().BaseType.GetProperty(fieldName, defaultFlags);
            }
            return fi.GetValue(obj, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetProperty<T>(T obj, string fieldName, object value)
        {
            PropertyInfo fi = obj.GetType().GetProperty(fieldName, defaultFlags);
            if (fi == null)
            {
                fi = obj.GetType().BaseType.GetProperty(fieldName, defaultFlags);
            }
            fi.SetValue(obj, value, null);
        }

        [DllImport("SkyX.dll", EntryPoint = "Wrapper_FreeOutString", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Wrapper_FreeOutString(IntPtr Handle);


        public static string IntPtrToString(IntPtr strHandle)
        {
            if (strHandle != IntPtr.Zero)
            {
                string str = Marshal.PtrToStringAnsi(strHandle);
                Wrapper_FreeOutString(strHandle);
                return str;
            }
            return null;
        }

        public static IntPtr StringToIntPtr(string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                IntPtr result = Marshal.StringToHGlobalAnsi(str);
                return result;
            }
            return IntPtr.Zero;
        }

    }
}
