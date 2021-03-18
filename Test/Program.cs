using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using ProtoBuf;

namespace Test
{
    class Program
    {
        private Dictionary<string, StringBuilder> files = new Dictionary<string, StringBuilder>();

        private string Type2ProtoType(Type type)
        {
            switch (type.Name)
            {
                case "Int32":
                    return "int32";
                case "Int64":
                    return "int64";
                case "UInt32":
                    return "uint32";
                case "UInt64":
                    return "uint64";
                case "String":
                    return "string";
                case "List`1":
                    var genericType = type.GenericTypeArguments[0];
                    return Type2ProtoType(genericType);
                default:
                    return type.Name;
            }
        }

        private string GetProtoType(PropertyInfo info)
        {
            return Type2ProtoType(info.PropertyType);
        }

        private string GetDescriptor(PropertyInfo info)
        {
            var type = info.PropertyType;
            if (type.Name == "List`1" && type.IsGenericType)
                return "repeated";
            var attr = info.GetCustomAttribute<ProtoMemberAttribute>();
            return attr.IsRequired ? "required" : "optional";
        }

        public string GetDefaultValue(PropertyInfo info)
        {
            var attr = info.GetCustomAttribute<DefaultValueAttribute>();
            if(attr != null)
            {
                if(attr.Value == null)
                    return " [default=null]";
                var type = attr.Value.GetType().Name;
                switch(type)
                {
                    case "String":
                        return string.Format(" [default=\"{0}\"]", attr.Value);
                    case "Int64":
                    case "Single":
                        return string.Format(" [default={0}]", attr.Value);
                    default:
                        throw new Exception("Unknown default type! " + type);
                }
            }
            return "";
        }

        private Program()
        {
            var types = Assembly.Load("Assembly-CSharp").GetTypes();

            foreach (var i in types)
            {
                var attr = i.GetCustomAttribute<ProtoContractAttribute>();
                if (attr == null)
                    continue;
                if (i.IsEnum)
                    continue;

                var pkg = i.Namespace;
                var name = attr.Name;
                Console.WriteLine(pkg + "." + name);

                StringBuilder buff;
                if (files.ContainsKey(pkg))
                {
                    buff = files[pkg];
                }
                else
                {
                    buff = new StringBuilder();
                    buff.AppendFormat("package {0};\n\n", pkg);
                    files.Add(pkg, buff);
                }

                buff.AppendFormat("message {0} {{\n", name);

                var prop = i.GetProperties();
                foreach (var j in prop)
                {
                    var attr1 = j.GetCustomAttribute<ProtoMemberAttribute>();
                    var protoType = GetProtoType(j);
                    var descriptor = GetDescriptor(j);
                    var def = GetDefaultValue(j);

                    buff.AppendFormat("    {0} {1} {2} = {3}{4};\n", descriptor, protoType, attr1.Name, attr1.Tag, def);
                }

                buff.Append("}\n\n");
            }

            foreach(var i in files)
            {
                File.WriteAllText(i.Key + ".proto", i.Value.ToString());
            }

            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            new Program();
        }
    }
}
