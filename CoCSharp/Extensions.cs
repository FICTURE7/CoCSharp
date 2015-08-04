using System.IO;
using System.Text;

namespace CoCSharp
{
    internal static class StringBuilderExtensions
    {
        internal const string IndentString = "    ";

        public static void Indent(this StringBuilder builder)
        {
            builder.Append(IndentString);
        }

        public static void Indent(this StringBuilder builder, int indentLevel)
        {
            for (int i = 0; i < indentLevel; i++) builder.Append(IndentString);
        }

        public static void Indent(this StringBuilder builder, object value)
        {
            builder.Append(IndentString + value.ToString());
        }

        public static void Indent(this StringBuilder builder, object value, int indentLevel)
        {
            Indent(builder, indentLevel);
            builder.Append(value.ToString());
        }
    }
}
