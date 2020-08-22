using System;
using System.Collections.Generic;
using System.IO;
using System.Regex;
using System.Text;
// using System.Text.RegularExpressions;
using CppAst;

namespace Mokit
{
    struct AttributeArgument
    {
        public string name;
        public string value;

        public bool Try(out int result)
        {
            return int.TryParse(value, out result);
        }

        public bool Try(out float result)
        {
            return float.TryParse(value, out result);
        }

        public bool Try(out bool result)
        {
            if(name.Length != 0 && value.Length == 0)
            {
                result = true;
                return true;
            }

            return bool.TryParse(value, out result);
        }



        public AttributeArgument(string arg_name, string arg_value)
        {
            name = arg_name;
            value = arg_value;
        }
    }

    public class Application
    {

        public Dictionary<string, Action> class_attributes_handlers;

        public static void Main(string[] args)
        {
            var app = new Application();
            app.Process(args);
        }

        public void Process(string[] args)
        {
            var input_file = "test.cpp";

            var input_text = File.ReadAllText(input_file, encoding: Encoding.UTF8);

            var options = new CppParserOptions();
            //options.ParseAttributes = true;
            

            
            var compilation = CppParser.Parse(input_text, options);
            



            Context.Instance = new Context();

            foreach (var cpp_class in compilation.Classes)
            {
                HandleClass(cpp_class);
            }
        }

        bool TryParseArray(string input, out List<string> results)
        {
            results = new List<string>();
            input = input.Trim();

            if(input.Length < 2)
            {
                return false;
            }

            if(input[0] != '[' || input[input.Length - 1] != ']')
            {
                return false;
            }

            input = input.Substring(1, input.Length - 2);
            results = input.SplitEscaped(',');
            return true;
        }

        List<AttributeArgument> ParseArguments(string arguments)
        {
            var results = new List<AttributeArgument>();

            // split by commas, but not commas in quotes
            // var argument_list = Regex.Split(arguments, ",(?=([^\"]*\"[^\"]*\")*[^\"]*$)");
            var argument_list = arguments.SplitEscaped(',');

            foreach(var raw_arg in argument_list)
            {
                var arg = raw_arg.Trim();
                var arg_name = "";

                if(arg.Length == 0)
                {
                    continue;
                }

                if (!Char.IsLetter(arg[0]))
                {
                    results.Add(new AttributeArgument(arg_name, arg));
                    continue;
                }

                var i = 1;
                while (Char.IsLetterOrDigit(arg[i]))
                {
                    ++i;
                }

                arg_name = arg.Substring(0, i);
                arg = arg.Substring(i).Trim();
                if(arg.Length != 0 && arg[0] == '=')
                {
                    arg = arg.Substring(1).Trim();
                }
                else
                {
                    arg = "";
                }

                results.Add(new AttributeArgument(arg_name, arg));

            }

            return results;
        }




        void HandleClass(CppClass cpp_class)
        {
            Context.Scopes.Enter(cpp_class);

            var attr_list = cpp_class.Attributes;

            foreach(var attr in attr_list)
            {
                //attr.Arguments
            }
        }

        
        void OnClassExport(string arg)
        {
            var actions = new Dictionary<string, Action<AttributeArgument>>();

            actions.Add("TargetLanguage", (attribute) =>
            {

            });

            actions.Add("ClassResides", (attribute) =>
            {

            });
        }

    }
}
