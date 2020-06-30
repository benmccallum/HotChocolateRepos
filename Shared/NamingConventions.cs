using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using System;
using System.Text.RegularExpressions;

namespace Shared
{
    /// <summary>
    /// Custom naming conventions. Usage:
    /// <code>services.AddSingleton&lt;INamingConventions, NamingConventions&gt;()</code>
    /// </summary>
    public class NamingConventions : DefaultNamingConventions
    {
        private const string DtoSuffix = "Dto";
        private const string InputSuffix = "Input";

        /// <summary>
        /// Gets the type name to use for the graph type definition.
        /// Overrides the default by trimming "Dto" off the end, so we don't need to specify them all explicitly.
        /// </summary>
        /// <param name="type">Type to get name from.</param>
        /// <returns>Type name.</returns>
        public override NameString GetTypeName(Type type, TypeKind kind)
        {
            var typeName = base.GetTypeName(type).Value;

            typeName = kind switch
            {
                TypeKind.Object => TrimDtoSuffix(typeName),
                TypeKind.InputObject => EnsureInputSuffix(TrimDtoSuffix(typeName)),
                _ => typeName
            };

            return typeName;
        }

        private static string TrimDtoSuffix(string typeName)
            => typeName.EndsWith(DtoSuffix)
                ? typeName.Substring(0, typeName.Length - DtoSuffix.Length)
                : typeName;

        private static string EnsureInputSuffix(string typeName)
            => typeName.EndsWith(InputSuffix)
                ? typeName
                : typeName + InputSuffix;

        /// <summary>
        /// Gets the enum value name for the graph enum definition.
        /// Overrides the default by using the same naming convention graphql-dotnet was using,
        /// e.g. SOME_ENUM_VALUE, rather than SOMEENUMVALUE.
        /// </summary>
        /// <param name="value">Value to get name for.</param>
        /// <returns>Enum value name.</returns>
        public override NameString GetEnumValueName(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // Same convention as graphql-dotnet was using, e.g. SOME_ENUM_VALUE
            // See: https://github.com/graphql-dotnet/graphql-dotnet/blob/7c00e2a9a8cc9250fcdbe012bd9b40e110f6d5ab/src/GraphQL/Utilities/StringUtils.cs#L40
            var result = "";
            var index = 0;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            foreach (Match match in _reWords.Matches(value.ToString()))
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            {
                result += (index == 0 ? "" : "_") + match!.Value.ToUpperInvariant();
                index++;
            }

            return result;
        }

        private static readonly Regex _reWords = new Regex(@"[A-Z\xc0-\xd6\xd8-\xde]?[a-z\xdf-\xf6\xf8-\xff]+(?:['’](?:d|ll|m|re|s|t|ve))?(?=[\xac\xb1\xd7\xf7\x00-\x2f\x3a-\x40\x5b-\x60\x7b-\xbf\u2000-\u206f \t\x0b\f\xa0\ufeff\n\r\u2028\u2029\u1680\u180e\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200a\u202f\u205f\u3000]|[A-Z\xc0-\xd6\xd8-\xde]|$)|(?:[A-Z\xc0-\xd6\xd8-\xde]|[^\ud800-\udfff\xac\xb1\xd7\xf7\x00-\x2f\x3a-\x40\x5b-\x60\x7b-\xbf\u2000-\u206f \t\x0b\f\xa0\ufeff\n\r\u2028\u2029\u1680\u180e\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200a\u202f\u205f\u3000\d+\u2700-\u27bfa-z\xdf-\xf6\xf8-\xffA-Z\xc0-\xd6\xd8-\xde])+(?:['’](?:D|LL|M|RE|S|T|VE))?(?=[\xac\xb1\xd7\xf7\x00-\x2f\x3a-\x40\x5b-\x60\x7b-\xbf\u2000-\u206f \t\x0b\f\xa0\ufeff\n\r\u2028\u2029\u1680\u180e\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200a\u202f\u205f\u3000]|[A-Z\xc0-\xd6\xd8-\xde](?:[a-z\xdf-\xf6\xf8-\xff]|[^\ud800-\udfff\xac\xb1\xd7\xf7\x00-\x2f\x3a-\x40\x5b-\x60\x7b-\xbf\u2000-\u206f \t\x0b\f\xa0\ufeff\n\r\u2028\u2029\u1680\u180e\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200a\u202f\u205f\u3000\d+\u2700-\u27bfa-z\xdf-\xf6\xf8-\xffA-Z\xc0-\xd6\xd8-\xde])|$)|[A-Z\xc0-\xd6\xd8-\xde]?(?:[a-z\xdf-\xf6\xf8-\xff]|[^\ud800-\udfff\xac\xb1\xd7\xf7\x00-\x2f\x3a-\x40\x5b-\x60\x7b-\xbf\u2000-\u206f \t\x0b\f\xa0\ufeff\n\r\u2028\u2029\u1680\u180e\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200a\u202f\u205f\u3000\d+\u2700-\u27bfa-z\xdf-\xf6\xf8-\xffA-Z\xc0-\xd6\xd8-\xde])+(?:['’](?:d|ll|m|re|s|t|ve))?|[A-Z\xc0-\xd6\xd8-\xde]+(?:['’](?:D|LL|M|RE|S|T|VE))?|\d+|(?:[\u2700-\u27bf]|(?:\ud83c[\udde6-\uddff]){2}|[\ud800-\udbff][\udc00-\udfff])[\ufe0e\ufe0f]?(?:[\u0300-\u036f\ufe20-\ufe23\u20d0-\u20f0]|\ud83c[\udffb-\udfff])?(?:\u200d(?:[^\ud800-\udfff]|(?:\ud83c[\udde6-\uddff]){2}|[\ud800-\udbff][\udc00-\udfff])[\ufe0e\ufe0f]?(?:[\u0300-\u036f\ufe20-\ufe23\u20d0-\u20f0]|\ud83c[\udffb-\udfff])?)*");
    }
}
