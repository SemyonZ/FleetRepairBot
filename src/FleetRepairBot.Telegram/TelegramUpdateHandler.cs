using System;
using System.Reflection;

namespace FleetRepairBot.Telegram
{
    public class TelegramUpdateHandler
    {
        /// <summary>
        /// Safely tries to set a property value using reflection. Handles nullable target types
        /// and basic conversion. Returns true if assignment succeeded, false otherwise.
        /// </summary>
        private bool TrySet(object entity, PropertyInfo prop, object value)
        {
            try
            {
                if (entity == null || prop == null) return false;

                var targetType = prop.PropertyType;

                // Unwrap Nullable<T> to underlying type
                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    targetType = Nullable.GetUnderlyingType(targetType);
                }

                object converted = null;
                if (value != null)
                {
                    // If value already is of target type, use directly
                    if (targetType.IsInstanceOfType(value))
                    {
                        converted = value;
                    }
                    else
                    {
                        converted = Convert.ChangeType(value, targetType);
                    }
                }

                prop.SetValue(entity, converted);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
