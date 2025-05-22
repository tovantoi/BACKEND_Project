using chuyennganh.Domain.Constants;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.ExceptionEx;
using System.Net;
using System.Reflection;

namespace chuyennganh.Domain.DependencyInjection.Extensions
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Map data from source entity to target entity
        /// </summary>
        /// <typeparam name="TTarget">Destination entity, which data of source entity will be mapped to</typeparam>
        /// <param name="source">Current entity</param>
        /// <param name="ignoreNull">If true, null value of source properties will not be mapped to target and keep original data, otherwise, map null to target property</param>
        /// <returns>New value of target</returns>
        public static TTarget? MapTo<TTarget>(this object source, bool ignoreNull = false) where TTarget : class?, new()
        {
            TTarget target = new();
            return source.MapTo(target, ignoreNull);
        }

        /// <summary>
        /// Map data from source entity to target entity
        /// </summary>
        /// <typeparam name="TSource">Current entity</typeparam>
        /// <typeparam name="TTarget">Destination entity, which data of source entity will be mapped to</typeparam>
        /// <param name="source">Current entity</param>
        /// <param name="target">Destination entity, which data of source entity will be mapped to</param>
        /// <param name="ignoreNull">If true, null value of source will be mapped to target, otherwise, keep target original data</param>
        /// <returns>New value of target</returns>
        public static TTarget? MapTo<TSource, TTarget>(this TSource source, TTarget target, bool ignoreNull = false) where TSource : class? where TTarget : class?, new()
        {
            if (source is null) return null;

            // Create new target when target is null
            target ??= new TTarget();
            // Get source properties
            PropertyInfo[] sourceProperties = source.GetType().GetProperties();

            // Get target properties
            PropertyInfo[] targetProperties = target.GetType().GetProperties();

            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                // Find matched property name and type of source and target
                PropertyInfo? targetProperty = Array.Find(targetProperties, p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);
                if (targetProperty is null)
                    // Find by name property
                    targetProperty = Array.Find(targetProperties, p => p.Name == sourceProperty.Name);

                if (targetProperty is null || !AreTypesCompatible(sourceProperty.PropertyType, targetProperty.PropertyType))
                    continue;

                if (targetProperty != null && targetProperty.CanWrite)
                {
                    // Get source property value
                    object? value = sourceProperty.GetValue(source);
                    // If ignore null, will keep original target data
                    if (ignoreNull && value == null) value = targetProperty.GetValue(target);

                    try
                    {
                        // Mapping from source to target
                        targetProperty.SetValue(target, value);
                    }
                    catch (Exception)
                    {
                        string message = MsgConst.UN_SUP_MAP.FormatMsg(sourceProperty.Name, sourceProperty.PropertyType, targetProperty.PropertyType);
                        ShopException.ThrowException((int)HttpStatusCode.InternalServerError, MsgCode.ERR_INTERNAL_SERVER, message);
                    }
                }
            }

            return target;
        }

        /// <summary>
        /// Check ì source type and target type are compatible
        /// </summary>
        /// <param name="sourceType">Type of source</param>
        /// <param name="targetType">Type of target</param>
        /// <returns>True if source type and target type are compatible, otherwise false</returns>
        private static bool AreTypesCompatible(Type sourceType, Type targetType)
        {
            // Source type and target type are same
            if (sourceType == targetType)
                return true;

            // Source type is nullable, and target type is non-nullable
            if (Nullable.GetUnderlyingType(sourceType) == targetType)
                return true;

            // Source type is non-nullable, and target type is nullable
            if (Nullable.GetUnderlyingType(targetType) == sourceType)
                return true;

            return false;
        }
    }
}