using HC.Common.HC.Common;
using System;
using System.Reflection;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Service
{
    public class BaseService
    {
        /// <summary>
        /// Use this method to group all the try catch blocks in one function.Create a base class if possible for whole project(hc.patient.service)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public T ExecuteFunctions<T>(Func<T> method)
        {
            T obj = default(T);
            try
            {
                return method();
            }
            catch (Exception ex)
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        switch (prop.Name)
                        {
                            case "data":
                                {
                                    prop.SetValue(obj, new object(), null);
                                    break;
                                }
                            case "Message":
                                {
                                    prop.SetValue(obj, StatusMessage.ServerError, null);
                                    break;
                                }
                            case "StatusCode":
                                {
                                    prop.SetValue(obj, HttpStatusCodes.InternalServerError, null);
                                    break;
                                }
                            case "AppError":
                                {
                                    prop.SetValue(obj, ex.Message, null);
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                return obj;
            }
        }

        
    }
}
