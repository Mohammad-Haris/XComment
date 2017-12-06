using System;
using System.Text.RegularExpressions;
using EnvDTE;
using System.Windows.Forms;

namespace XComment
{
    internal sealed class Methods
    {

        #region Regex

        private static string C_CSharp_JScript(string current)
        {
            string blockComments = @"/\*(.*?)\*/";
            string lineComments = @"//(.*?)\r?.*";
            string strings = @"""((\\[^\n]|[^""\n])*)""";
            string verbatimStrings = @"@(""[^""]*"")+";
            string noComments = Regex.Replace(current, lineComments + "|" + strings + "|" + verbatimStrings, me =>
            {
                if (me.Value.StartsWith("//"))
                    return "";
                return me.Value;
            }, RegexOptions.Multiline);
            noComments = Regex.Replace(noComments, blockComments + "|" + strings + "|" + verbatimStrings, me =>
            {
                if (me.Value.StartsWith("/*"))
                    return "";
                return me.Value;
            }, RegexOptions.Singleline);
            return noComments;
        }

        private static string HTML(string current)
        {
            string comments = @"(<!--.*?-->|<!--|-->)";
            string noComments = Regex.Replace(current, comments, me =>
            {
                return "";
            }, RegexOptions.Singleline);
            return noComments;
        }

        private static string VBasic(string current)
        {
            string comments = @"'(.*?)\r?.*";
            string strings = @"""((\\[^\n]|[^""\n])*)""";
            string noComments = Regex.Replace(current, comments + "|" + strings, me =>
            {
                if (me.Value.StartsWith("'"))
                    return "";
                return me.Value;
            }, RegexOptions.Multiline);
            return noComments;
        }

        private static string CSS(string current)
        {
            string comments = @"/\*(.*?)\*/";
            string strings = @"""((\\[^\n]|[^""\n])*)""";
            string noComments = Regex.Replace(current, comments + "|" + strings, me =>
            {
                if (me.Value.StartsWith("/*"))
                    return "";
                return me.Value;
            }, RegexOptions.Singleline);
            return noComments;
        }

        private static string PHP(string current)
        {
            string slashSingleLine = @"//(.*?)\r?.*";
            string hashSingleLine = @"#(.*?)\r?.*";
            string starMultiLine = @"/\*(.*?)\*/";
            string singleQuote = @"'((\\[^\n]|[^''\n])*)'";
            string doubleQuote = @"""((\\[^\n]|[^""\n])*)""";
            string EOT_Singleline = @"([<][<][<][E][O][T].*\n*[E][O][T][;])";
            string EOT_Multiline = @"(<<<EOT(?s)(.*)EOT;)";
            string noComments = Regex.Replace(current, slashSingleLine + "|" + hashSingleLine
                + "|" + singleQuote + "|" + doubleQuote + "|" + EOT_Multiline, me =>
                {
                    if (me.Value.StartsWith("//") || me.Value.StartsWith("#"))
                        return "";
                    return me.Value;
                }, RegexOptions.Multiline);
            noComments = Regex.Replace(noComments, starMultiLine
                + "|" + singleQuote + "|" + doubleQuote + "|" + EOT_Singleline, me =>
                {
                    if (me.Value.StartsWith("/*"))
                        return "";
                    return me.Value;
                }, RegexOptions.Singleline);
            return noComments;
        }

        private static string SQL(string current)
        {
            string blockComments = @"/\*(.*?)\*/";
            string quoteStrings = @"""((\\[^\n]|[^""\n])*)""";
            string lineComments = @"--(.*?)\r?.*";
            string apostropheStrings = @"'((\\[^\n]|[^''\n])*)'";
            string noComments = Regex.Replace(current, lineComments + "|" + quoteStrings
                + "|" + apostropheStrings, me =>
                {
                    if (me.Value.StartsWith("--"))
                        return "";
                    return me.Value;
                }, RegexOptions.Multiline);
            noComments = Regex.Replace(noComments, blockComments
                + "|" + apostropheStrings + "|" + quoteStrings, me =>
                {
                    if (me.Value.StartsWith("/*"))
                        return "";
                    return me.Value;
                }, RegexOptions.Singleline);
            return noComments;
        }

        private static string XML(string current)
        {
            string comments = @"(<!--.*?-->|<!--|-->)";
            string noComments = Regex.Replace(current, comments, me =>
            {
                return "";
            }, RegexOptions.Singleline);
            return noComments;
        }

        private static string Ruby(string current)
        {
            string hashComments = @"#(.*?)\r?\n";
            string beginEndStrings = @"((=begin).*(=end))";
            string lineComments = @"--(.*?)\r?.*";
            string apostropheStrings = @"'((\\[^\n]|[^''\n])*)'";
            string quoteStrings = @"""((\\[^\n]|[^""\n])*)""";
            string noComments = Regex.Replace(current, lineComments + "|" + quoteStrings
                + "|" + apostropheStrings + "|" + hashComments + "|" + beginEndStrings, me =>
                {
                    if (me.Value.StartsWith("#") || me.Value.StartsWith("--") || me.Value.StartsWith("=begin"))
                        return "";
                    return me.Value;
                }, RegexOptions.Singleline);
            return noComments;
        }

        private static string Python(string current)
        {
            string quoteStrings = @"""((\\[^\n]|[^""\n])*)""";
            string hashComments = @"#[^!](.*?)\r?.*";
            string apostropheStrings = @"'((\\[^\n]|[^''\n])*)'";
            string triCommaComments = @"([""][""][""].*\n*[""][""][""])";
            string noComments = Regex.Replace(current, hashComments + "|" + quoteStrings
                + "|" + apostropheStrings, me =>
                {
                    if (me.Value.StartsWith("#"))
                        return "";
                    return me.Value;
                }, RegexOptions.Multiline);
            noComments = Regex.Replace(noComments, triCommaComments, me =>
            {
                return "";
            }, RegexOptions.Singleline);
            return noComments;
        }

        private static string Assembly(string current)
        {
            string comments = @"(;(.*?)\r?.*)";
            string strings = @"'((\\[^\n]|[^''\n])*)'";
            string noComments = Regex.Replace(current, comments + "|" + strings, me =>
            {
                if (me.Value.StartsWith(";"))
                    return "";
                return me.Value;
            }, RegexOptions.Multiline);
            return noComments;
        }

        private static string AppleScript(string current)
        {
            string dashComments = @"(--(.*?)\r?.*)";
            string hashComments = @"(#[^!](.*?)\r?.*)";
            string quoteStrings = @"""((\\[^\n]|[^""\n])*)""";
            string apostropheStrings = @"'((\\[^\n]|[^''\n])*)'";
            string blockComments = @"[(][*](.*?)[*][)]";
            string noComments = Regex.Replace(current, dashComments + "|" + hashComments + "|" + quoteStrings + "|" + apostropheStrings, me =>
            {
                if (me.Value.StartsWith("--") || me.Value.StartsWith("#"))
                    return "";
                return me.Value;
            }, RegexOptions.Multiline);
            noComments = Regex.Replace(noComments, blockComments + "|" + quoteStrings + "|" + apostropheStrings, me =>
            {
                if (me.Value.StartsWith("(*"))
                    return "";
                return me.Value;
            }, RegexOptions.Singleline);
            return noComments;
        }

        private static string ActionScript(string current)
        {
            string blockComments = @"/\*(.*?)\*/";
            string lineComments = @"//(.*?)\r?.*";
            string quoteStrings = @"""((\\[^\n]|[^""\n])*)""";
            string apostropheStrings = @"'((\\[^\n]|[^''\n])*)'";
            string noComments = Regex.Replace(current, lineComments + "|" + apostropheStrings + "|" + quoteStrings, me =>
            {
                if (me.Value.StartsWith("//"))
                    return "";
                return me.Value;
            }, RegexOptions.Multiline);
            noComments = Regex.Replace(noComments, blockComments + "|" + apostropheStrings + "|" + quoteStrings, me =>
            {
                if (me.Value.StartsWith("/*"))
                    return "";
                return me.Value;
            }, RegexOptions.Singleline);
            return noComments;
        }

        private static string Bash(string current)
        {
            string comment = @"(?<=[\n]|[\s])(#[^!](.*?)\r?\n)";
            string quoteStrings = @"""((\\[^\n]|[^""\n])*)""";
            string apostropheStrings = @"'((\\[^\n]|[^''\n])*)'";
            string blockString = @"(?:({((\\[^\n]|[^""\n])*)}))";
            string noComments = Regex.Replace(current, comment + "|" + quoteStrings + "|" + blockString + "|" + apostropheStrings, me =>
            {
                if (me.Value.StartsWith("#"))
                    return "" + Environment.NewLine;
                return me.Value;
            }, RegexOptions.Multiline);
            return noComments;
        }

        private static string fileType(string filePath)
        {
            Match match = Regex.Match(filePath, @"(\w+)$");
            string check = match.ToString();
            check = check.ToLower();
            switch (check)
            {
                case "c":
                case "cs":
                case "cpp":
                case "cxx":
                case "js":
                    return "C_CSharp_JScript";
                case "html":
                    return "HTML";
                case "vb":
                case "basic":
                    return "VBasic";
                case "css":
                    return "CSS";
                case "php":
                    return "PHP";
                case "sql":
                    return "SQL";
                case "xml":
                    return "XML";
                case "rb":
                    return "Ruby";
                case "py":
                    return "Python";
                case "asm":
                    return "Assembly";
                case "scriptsuite":
                    return "AppleScript";
                case "as":
                    return "ActionScript";
                case "sh":
                    return "Bash";
                default:
                    return "NULL";
            }
        }

        private static string delComments(string text, string fileType)
        {
            string check = fileType;
            string editted;
            switch (check)
            {
                case "C_CSharp_JScript":
                    editted = C_CSharp_JScript(text);
                    return editted;
                case "HTML":
                    editted = HTML(text);
                    return editted;
                case "VBasic":
                    editted = VBasic(text);
                    return editted;
                case "CSS":
                    editted = CSS(text);
                    return editted;
                case "PHP":
                    editted = PHP(text);
                    return editted;
                case "SQL":
                    editted = SQL(text);
                    return editted;
                case "XML":
                    editted = XML(text);
                    return editted;
                case "Ruby":
                    editted = Ruby(text);
                    return editted;
                case "Python":
                    editted = Python(text);
                    return editted;
                case "Assembly":
                    editted = Assembly(text);
                    return editted;
                case "AppleScript":
                    editted = AppleScript(text);
                    return editted;
                case "ActionScript":
                    editted = ActionScript(text);
                    return editted;
                default:
                    editted = Bash(text);
                    return editted;
            }
        }

        private static string removeWhiteSpace(string current)
        {
            string blankLines = @"(?:\r\n[\s-[\rn]]*){3,}";
            string whiteSpace = @"(?m)[\s-[\r]]+\r?$|\r\n\z";
            string noComments = Regex.Replace(current, blankLines, me =>
            {
                return "\r\n" + "\r\n" + "\r\n";
            }, RegexOptions.Multiline);
            noComments = Regex.Replace(noComments, whiteSpace, me =>
            {
                return "";
            }, RegexOptions.Multiline);
            return noComments;
        }

        #endregion

        #region Apply

        internal static void Apply(DTE dTE, TextDocument activeDoc)
        {
            string filePath = dTE.ActiveDocument.FullName;
            string filetype = fileType(filePath);
            if (filetype == "NULL")
                MessageBox.Show("Language Not Supported!");
            else
            {
                EditPoint objStart = activeDoc.StartPoint.CreateEditPoint();
                EditPoint objEnd = activeDoc.EndPoint.CreateEditPoint();
                string text = activeDoc.CreateEditPoint(objStart).GetText(objEnd);
                string editted = delComments(text, filetype);
                editted = removeWhiteSpace(editted);
                objStart.ReplaceText(objEnd, editted, (int)vsEPReplaceTextOptions.vsEPReplaceTextAutoformat);
            }
        }

        #endregion

    }
}