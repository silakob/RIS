using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace UtilityByWaii
{
    public class Utility
    {
        public static D Map<S, D>(S model)
        {
            try
            {
                JsonSerializerOptions settings = new JsonSerializerOptions
                {
                    IgnoreNullValues = true,
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(model, settings);
                return JsonSerializer.Deserialize<D>(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void MapProp(object sourceObj, object targetObj)
        {
            Type T1 = sourceObj.GetType();
            Type T2 = targetObj.GetType();

            PropertyInfo[] sourceProprties = T1.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] targetProprties = T2.GetProperties(BindingFlags.Instance | BindingFlags.Public);


            for (int i = 0; i < sourceProprties.Length; i++)
            {
                var sourceProp = sourceProprties[i];
                int index = Array.FindIndex(targetProprties, a => a.Name == sourceProp.Name);

                if (index >= 0)
                {
                    object osourceVal = sourceProp.GetValue(sourceObj, null);

                    var targetProp = targetProprties[index];
                    targetProp.SetValue(targetObj, osourceVal);
                }

            }
        }
        public void Print<T>(IEnumerable<T> data)
        {
            var props = typeof(T).GetProperties();
            if (data is IList && data.GetType().IsGenericType)
            {
                foreach (var item in data)
                {
                    foreach (var prop in props)
                    {
                        Console.Write("{0}\t", prop.GetValue(item, null));
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                foreach (var prop in props)
                {
                    Console.Write("{0}\t", prop.Name);
                }
                Console.WriteLine();
            }
        }
    }
}
