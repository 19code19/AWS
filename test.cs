public static TResult MapTo<TResult>(this object source) where TResult : new()
{
    var target = new TResult();
    var sourceType = source.GetType();
    var targetType = typeof(TResult);

    sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Select(property => new
        {
            property,
            targetProperty = targetType.GetProperty(property.Name)
        })
        .Where(x => x.targetProperty != null && x.targetProperty.CanWrite)
        .ToList()
        .ForEach(x =>
        {
            var sourceValue = x.property.GetValue(source);
            var sourceTypeProp = x.property.PropertyType;
            var targetTypeProp = x.targetProperty.PropertyType;

            object? valueToSet = null;

            if (sourceValue == null)
            {
                valueToSet = null;
            }
            else if (targetTypeProp == sourceTypeProp)
            {
                valueToSet = sourceValue;
            }
            else if (targetTypeProp.IsEnum && sourceTypeProp.IsEnum)
            {
                // Enum to Enum mapping by name
                valueToSet = Enum.Parse(targetTypeProp, sourceValue.ToString());
            }
            else
            {
                return; // skip incompatible types
            }

            x.targetProperty.SetValue(target, valueToSet);
        });

    return target;
}
