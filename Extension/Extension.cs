using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace Budget.Extension
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnMappingAttribute : Attribute
    {
        string source = string.Empty;

        public ColumnMappingAttribute() { }

        public string Source { get { return this.source; } set { this.source = value; } }
    }

    static public class DataReaderExtensions
    {
        static public List<T> ReadList<T>(this SqlDataReader reader) where T : new()
        {
            var results = new List<T>();
            while (reader.Read())
            {
                T result = new T();
                results.Add(result);
                Type t = result.GetType();

                PropertyInfo[] properties = t.GetProperties(BindingFlags.IgnoreCase
                    | BindingFlags.Public
                    | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    object[] attributes = property.GetCustomAttributes(typeof(ColumnMappingAttribute), true);
                    if (attributes != null && attributes.Length > 0)
                    {
                        ColumnMappingAttribute cma = attributes[0] as ColumnMappingAttribute;

                        object dataValue = reader[cma.Source];
                        if (DBNull.Value == dataValue) dataValue = null;

                        if (dataValue != null && property.PropertyType != dataValue.GetType())
                        {
                            try
                            {
                                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                {
                                    Type[] genericArgs = property.PropertyType.GetGenericArguments();
                                    if (genericArgs.Length > 0)
                                    {
                                        dataValue = Convert.ChangeType(dataValue, genericArgs[0]);
                                    }
                                }
                                else
                                {
                                    dataValue = Convert.ChangeType(dataValue, property.PropertyType);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                        }

                        try
                        {
                            property.SetValue(result, dataValue, null);
                        }
                        catch (ArgumentException)
                        {
                            if (!false)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            return results;
        }

        static public T Read<T>(this SqlDataReader reader) where T : new()
        {
            var result = new T();
            if (reader.Read())
            {
                Type t = result.GetType();

                PropertyInfo[] properties = t.GetProperties(BindingFlags.IgnoreCase
                    | BindingFlags.Public
                    | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    object[] attributes = property.GetCustomAttributes(typeof(ColumnMappingAttribute), true);
                    if (attributes != null && attributes.Length > 0)
                    {
                        ColumnMappingAttribute cma = attributes[0] as ColumnMappingAttribute;

                        object dataValue = reader[cma.Source];
                        if (DBNull.Value == dataValue) dataValue = null;

                        if (dataValue != null && property.PropertyType != dataValue.GetType())
                        {
                            try
                            {
                                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                {
                                    Type[] genericArgs = property.PropertyType.GetGenericArguments();
                                    if (genericArgs.Length > 0)
                                    {
                                        dataValue = Convert.ChangeType(dataValue, genericArgs[0]);
                                    }
                                }
                                else
                                {
                                    dataValue = Convert.ChangeType(dataValue, property.PropertyType);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                        }

                        try
                        {
                            property.SetValue(result, dataValue, null);
                        }
                        catch (ArgumentException)
                        {
                            if (!false)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
