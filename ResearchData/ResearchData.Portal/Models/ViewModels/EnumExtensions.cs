using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

public static class EnumExtensions
{
    /// <summary>
    /// Retrieves the <see cref="DisplayAttribute.Name" /> property on the <see cref="DisplayAttribute" />
    /// of the current enum value, or the enum's member name if the <see cref="DisplayAttribute" /> is not present.
    /// </summary>
    /// <param name="val">This enum member to get the name for.</param>
    /// <returns>The <see cref="DisplayAttribute.Name" /> property on the <see cref="DisplayAttribute" /> attribute, if present.</returns>
    public static string GetDisplayName(this Enum val)
    {
        return val.GetType()
                  .GetMember(val.ToString())
                  .FirstOrDefault()
                  ?.GetCustomAttribute<DisplayAttribute>(false)
                  ?.Name
                  ?? val.ToString();
    }
}



public static class EnumExtensionsGlobal
{
    /// <summary>
    /// Retrieves the <see cref="DisplayAttribute.Name" /> property on the <see cref="DisplayAttribute" />
    /// of the current enum value, or the enum's member name if the <see cref="DisplayAttribute" /> is not present.
    /// </summary>
    /// <param name="val">This enum member to get the name for.</param>
    /// <returns>The <see cref="DisplayAttribute.Name" /> property on the <see cref="DisplayAttribute" /> attribute, if present.</returns>
    public static string GetDisplayNameGlobal(this Enum val)
    {
        var lingua = System.Globalization.CultureInfo.CurrentCulture.Name;
        switch (val.ToString())
        {
            case "tint":
                if(lingua.Equals("en-US"))
                    return "Integers";
                return "Números inteiros";
            case "tfloat":
                if(lingua.Equals("en-US"))
                    return "Decimal numbers";
                return "Números Decimais";
            case "tstring":
                if(lingua.Equals("en-US"))
                    return "Text";
                return "Texto";
            case "tdata":
                if (lingua.Equals("en-US"))
                    return "Date";
                return "Data";
            case "tbool":
                if (lingua.Equals("en-US"))
                    return "True or false";
                return "Verdadeiro ou Falso";
        }
        return "";
    }
}
