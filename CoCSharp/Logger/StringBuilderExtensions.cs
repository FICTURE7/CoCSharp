using System.Text;

namespace CoCSharp.Logger
{
    // could make stuff moar fancy here?
    public static class StringBuilderExtensions
    {
        private const string IndentString = "    ";

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

        public static void IndentLine(this StringBuilder builder, object value)
        {
            Indent(builder, value);
            builder.AppendLine();
        }

        public static void IndentLine(this StringBuilder builder, object value, int indentLevel)
        {
            Indent(builder, value, indentLevel);
            builder.AppendLine();
        }
    }
}
