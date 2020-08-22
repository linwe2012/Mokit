using System;
using CppAst;
using System.Collections.Generic;
using System.Linq;

namespace Mokit
{
    

    public class ExportAttribute
    {
        public enum TargetLanguage
        {
            C,
            CSharp
        }

        public List<TargetLanguage> Languages
        {
            get { return GetLanguage(); }
            set { languages = value; }
        }


        public string DLLFile
        {
            get { return GetDLLFile(); }
            set { dll_file = value; }
        }

        public List<string> Namespace
        {
            get { return GetNamespace(); }
            set { name_space = value; }
        }


        ExportAttribute parent;
        List<TargetLanguage> languages;
        string dll_file;
        public List<string> name_space;

        List<TargetLanguage> GetLanguage()
        {
            if(languages == null)
            {
                if(parent == null)
                {
                    return new List<TargetLanguage>{ TargetLanguage.C };
                }
                return parent.Languages;
            }
            return languages;
        }

        string GetDLLFile()
        {
            if (dll_file == null)
            {
                if (parent == null)
                {
                    return "<not specified>";
                }

                return parent.DLLFile;
            }
            return dll_file;
        }

        List<string> GetNamespace()
        {
            if (name_space == null)
            {
                if (parent == null)
                {
                    return new List<string>();
                }

                return parent.Namespace;
            }
            return name_space;
        }

    }

    public class ScopeContext
    {
        public class Scope
        {
            public enum Type
            {
                Class,
                Function,
                Namespace
            };

            public Scope(CppClass cpp_class)
            {
                cpp_item = cpp_class;
                type = Type.Class;
                get_unique_name = get_name = () =>
                {
                    return cpp_class.Name;
                };

                // if(cpp_class.Classes)
            }

            public Scope(CppFunction cpp_func)
            {
                cpp_item = cpp_func;
                type = Type.Function;
                get_name = () =>
                {
                    return cpp_func.Name;
                };

                get_unique_name = () =>
                {
                    // cpp_func.TemplateParameters
                    return "";
                };
            }


            public Scope(CppNamespace cpp_ns)
            {
                cpp_item = cpp_ns;
                type = Type.Namespace;
                get_unique_name = get_name = () =>
                {
                    return cpp_ns.Name;
                };
            }

            public string Name
            {
                get { return get_name(); }
            }

            public bool IsTemplate;


            CppElement cpp_item;
            Func<string> get_name;
            Func<string> get_unique_name;
            public Type type;
        }

        List<Scope> scopes;

        List<CppClass> classes;
        List<CppFunction> functions;
        List<CppNamespace> namespaces;

        List<string> unique_names;

        public void Enter(CppClass cpp_class)
        {
            var scope = new Scope(cpp_class);

            scopes.Add(scope);
            classes.Add(cpp_class);
        }

        public void Enter(CppFunction cpp_fn)
        {
            var scope = new Scope(cpp_fn);

            scopes.Add(scope);
            functions.Add(cpp_fn);
        }

        public void Enter(CppNamespace cpp_ns)
        {
            var scope = new Scope(cpp_ns);

            scopes.Add(scope);
            namespaces.Add(cpp_ns);
        }

        public void Leave()
        {
            var last = scopes.Last();
            switch (last.type)
            {
                case Scope.Type.Class: classes.RemoveLast(); break;
                case Scope.Type.Function: functions.RemoveLast(); break;
                case Scope.Type.Namespace: namespaces.RemoveLast(); break;
            }

            scopes.RemoveLast();
        }

    }

    public class Context
    {
        public ScopeContext scopes;
        CppElement current;

        public static Context Instance;
        public static ScopeContext Scopes { get { return Instance.scopes; } }


        void LogError(string message)
        {
            
        }

        void LogWarning(string message)
        {

        }

        void LogInfo(string message)
        {

        }

        public static void Error(string message)
        {
            Instance.LogError(message);
        }

        public static void Warning(string message)
        {
            Instance.LogWarning(message);
        }

        public static void Info(string message)
        {
            Instance.LogInfo(message);
        }
    }


    public class ClassExportation
    {
        CppClass cpp_class;
        ExportAttribute attribute;


    }
}
