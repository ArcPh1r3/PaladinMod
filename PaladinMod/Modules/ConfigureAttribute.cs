using BepInEx.Configuration;
using System;
using System.Reflection;

namespace PaladinMod.Modules
{
    [AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ConfigureAttribute : Attribute
    {
        public string section { get; private set; }
        public System.ValueType defaultValue { get; private set; }
        public string name;
        public string description;
        public float min = 0;
        public float max = 20;
        public bool restartRequired = false;

        public ConfigureAttribute(string section, float defaultValue)
        {
            this.section = section;
            this.defaultValue = defaultValue;
        }
        public ConfigureAttribute(string section, int defaultValue)
        {
            this.section = section;
            this.defaultValue = defaultValue;
        }
        public ConfigureAttribute(string section, bool defaultValue)
        {
            this.section = section;
            this.defaultValue = defaultValue;
        }
        public ConfigureAttribute(string section, KeyboardShortcut defaultValue)
        {
            this.section = section;
            this.defaultValue = defaultValue;
        }

        public void InitFromField(FieldInfo fieldInfo)
        {
            if (string.IsNullOrEmpty(name)) name = fieldInfo.Name;
            if (string.IsNullOrEmpty(description)) description = name;
        }
    }
}
