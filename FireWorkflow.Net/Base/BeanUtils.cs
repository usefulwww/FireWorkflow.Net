using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FireWorkflow.Net.Base
{
    /// <summary>
    /// 复制对象
    /// </summary>
    public class BeanUtils
    {
        /// <summary>
        /// Copies the properties.
        /// </summary>
        /// <param name="pobSrc">The pob SRC.</param>
        /// <param name="pobDest">The pob dest.</param>
        /// <param name="penOpt">The pen opt.</param>
        public static void CopyProperties(object pobSrc, object pobDest, OptionTyp penOpt)
        {
            SetProperties(GetProperties(pobSrc), pobDest, penOpt);
        }
        /// <summary>
        /// Copies the properties with map.
        /// </summary>
        /// <param name="pobSrc">The pob SRC.</param>
        /// <param name="pobDest">The pob dest.</param>
        /// <param name="pdiMap">The pdi map.</param>
        /// <param name="penOpt">The pen opt.</param>
        public static void CopyPropertiesWithMap(object pobSrc, object pobDest, Dictionary<String, string> pdiMap, OptionTyp penOpt)
        {
            List<String> strSrc = new List<String>();
            List<String> strDest = new List<String>();
            foreach (KeyValuePair<String, string> pair in pdiMap)
            {
                strSrc.Add(pair.Key);
                strDest.Add(pair.Value);
            }
            CopyPropertiesWithMap(pobSrc, pobDest, strSrc.ToArray(), strDest.ToArray(), penOpt);
        }
        /// <summary>
        /// Copies the properties with map.
        /// </summary>
        /// <param name="pobSrc">The pob SRC.</param>
        /// <param name="pobDest">The pob dest.</param>
        /// <param name="pstSrcPropertyNames">The PST SRC property names.</param>
        /// <param name="pstDestPropertyNames">The PST dest property names.</param>
        /// <param name="penOpt">The pen opt.</param>
        public static void CopyPropertiesWithMap(object pobSrc, object pobDest, string[] pstSrcPropertyNames, string[] pstDestPropertyNames, OptionTyp penOpt)
        {
            if (null == pobSrc || null == pobDest)
            { throw new ArgumentNullException("one argument is null!"); }
            if (pstDestPropertyNames.Length != pstSrcPropertyNames.Length)
                throw new ArgumentException("pstDestPropertyNames & pstSrcPropertyNames must same array length");
            for (int i = 0; i < pstDestPropertyNames.Length; i++)
            {
                CopyProperty(pobSrc, pobDest, pstSrcPropertyNames[i], pstDestPropertyNames[i], penOpt);
            }
        }
        /// <summary>
        /// Genernations the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pobSrc">The pob SRC.</param>
        /// <param name="penOpt">The pen opt.</param>
        /// <returns></returns>
        public static T GenernationObject<T>(object pobSrc, OptionTyp penOpt)
        {
            T lobDest = Activator.CreateInstance<T>();
            CopyProperties(pobSrc, lobDest, penOpt);
            return lobDest;
        }
        /// <summary>
        /// Genernations the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pdiProperties">The pdi properties.</param>
        /// <param name="penOpt">The pen opt.</param>
        /// <returns></returns>
        public static T GenernationObject<T>(Dictionary<String, object> pdiProperties, OptionTyp penOpt)
        {
            T lobDest = Activator.CreateInstance<T>();
            SetProperties(pdiProperties, lobDest, penOpt);
            return lobDest;
        }
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="pobObj">The pob obj.</param>
        /// <returns></returns>
        public static Dictionary<String, object> GetProperties(object pobObj)
        {
            Dictionary<String, object> list = new Dictionary<String, object>();
            string name;
            object val;
            if (null == pobObj) { throw new ArgumentNullException("pobObj can't be null"); }
            Type objType = pobObj.GetType();
            PropertyInfo[] objInfo = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < objInfo.Length; i++)
            {
                name = objInfo[i].Name;
                val = objInfo[i].GetValue(pobObj, null);
                list.Add(name, val);
            }
            return list;
        }
        /// <summary>
        /// Sets the properties.
        /// </summary>
        /// <param name="pdiProperties">The pdi properties.</param>
        /// <param name="pobObj">The pob obj.</param>
        /// <param name="penOpt">The pen opt.</param>
        public static void SetProperties(Dictionary<String, object> pdiProperties, object pobObj, OptionTyp penOpt)
        {
            foreach (KeyValuePair<String, object> pair in pdiProperties)
            {
                try
                {
                    SetProperty(pobObj, pair.Key, pair.Value, penOpt);
                }
                catch (MapPropertyException) { }
            }
        }
        /// <summary>
        /// Copies the property.
        /// </summary>
        /// <param name="pobSrc">The pob SRC.</param>
        /// <param name="pobDest">The pob dest.</param>
        /// <param name="pstPropertyName">Name of the PST property.</param>
        /// <param name="penOpt">The pen opt.</param>
        public static void CopyProperty(object pobSrc, object pobDest, string pstPropertyName, OptionTyp penOpt)
        {
            CopyProperty(pobSrc, pobDest, pstPropertyName, pstPropertyName, penOpt);
        }
        /// <summary>
        /// Copies the property.
        /// </summary>
        /// <param name="pobSrc">The pob SRC.</param>
        /// <param name="pobDest">The pob dest.</param>
        /// <param name="pstSrcPropertyName">Name of the PST SRC property.</param>
        /// <param name="pstDestPropertyName">Name of the PST dest property.</param>
        /// <param name="penOpt">The pen opt.</param>
        public static void CopyProperty(object pobSrc, object pobDest, string pstSrcPropertyName, string pstDestPropertyName, OptionTyp penOpt)
        {
            SetProperty(pobDest, pstDestPropertyName, GetProperty(pobSrc, pstSrcPropertyName, penOpt), penOpt);
        }
        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="pobObj">The pob obj.</param>
        /// <param name="pstPropertyName">Name of the PST property.</param>
        /// <param name="pobValue">The pob value.</param>
        /// <param name="penOpt">The pen opt.</param>
        public static void SetProperty(object pobObj, string pstPropertyName, object pobValue, OptionTyp penOpt)
        {
            if (null == pobObj || string.IsNullOrEmpty(pstPropertyName))
            {
                throw new ArgumentNullException("one argument is null!");
            }
            bool isIgnoreCase = ((penOpt & OptionTyp.IsIgnoreCase) == OptionTyp.IsIgnoreCase);
            bool isConvert = ((penOpt & OptionTyp.IsConvert) == OptionTyp.IsConvert);
            bool isThrowConvertException = ((penOpt & OptionTyp.IsThrowConvertException) == OptionTyp.IsThrowConvertException);
            Type t = pobObj.GetType();
            PropertyInfo objInfo = null;
            if (isIgnoreCase)
            {
                PropertyInfo[] objInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo p in objInfos)
                {
                    if (p.Name.ToUpperInvariant().Equals(pstPropertyName.ToUpperInvariant()))
                    {
                        objInfo = p;
                        break;
                    }
                }
            }
            else
            {
                objInfo = t.GetProperty(pstPropertyName, BindingFlags.Instance | BindingFlags.Public);
            }
            if (null == objInfo)
                throw new MapPropertyException("no mapping property");
            object descVal = null;
            if (null == pobValue || !isConvert)
            {
                descVal = pobValue;
            }
            else
            {
                Type srcPropertyType = pobValue.GetType();
                Type destPropertyType = objInfo.PropertyType;
                if (srcPropertyType.Equals(destPropertyType))
                {
                    descVal = pobValue;
                }
                else
                {
                    MethodInfo methodinfo = typeof(Convert).GetMethod("To" + destPropertyType.Name, new Type[] { srcPropertyType });
                    try
                    {
                        descVal = methodinfo.Invoke(null, new object[] { pobValue });
                    }
                    catch
                    {
                        if (isThrowConvertException)
                        {
                            throw new ConvertException("can't convert");
                        }
                        descVal = null;
                    }
                }
            }
            objInfo.SetValue(pobObj, descVal, null);
        }
        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="pobObj">The pob obj.</param>
        /// <param name="pstPropertyName">Name of the PST property.</param>
        /// <param name="penOpt">The pen opt.</param>
        /// <returns></returns>
        public static object GetProperty(object pobObj, string pstPropertyName, OptionTyp penOpt)
        {
            if (null == pobObj || string.IsNullOrEmpty(pstPropertyName))
            {
                throw new ArgumentNullException("one argument is null!");
            }
            bool isIgnoreCase = ((penOpt & OptionTyp.IsIgnoreCase) == OptionTyp.IsIgnoreCase);
            Type t = pobObj.GetType();
            PropertyInfo objInfo = null;
            if (isIgnoreCase)
            {
                PropertyInfo[] objInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo p in objInfos)
                {
                    if (p.Name.ToUpperInvariant().Equals(pstPropertyName.ToUpperInvariant()))
                    {
                        objInfo = p;
                        break;
                    }
                }
            }
            else
            {
                objInfo = t.GetProperty(pstPropertyName, BindingFlags.Instance | BindingFlags.Public);
            }
            if (null == objInfo)
                throw new MapPropertyException("no mapping property");
            object val = objInfo.GetValue(pobObj, null);
            return val;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum OptionTyp
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        IsIgnoreCase = 0x0001,

        /// <summary>
        /// 
        /// </summary>
        IsConvert = 0x0002,

        /// <summary>
        /// 
        /// </summary>
        IsThrowConvertException = 0x0004
    }
    /// <summary>
    /// 
    /// </summary>
    public class MapPropertyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPropertyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MapPropertyException(string message) : base(message) { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ConvertException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ConvertException(string message) : base(message) { }
    }

}
