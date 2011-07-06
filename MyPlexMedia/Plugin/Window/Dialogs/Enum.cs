using System;
using System.Collections.Generic;

namespace MyPlexMedia.Plugin.Window.Dialogs {
    public static class Enum<T> {
        public static T Parse(string value) {
            return (T)Enum.Parse(typeof(T), value);
        }
        public static IList<T> GetValues() {
            IList<T> list = new List<T>();
            foreach (object value in Enum.GetValues(typeof(T))) {
                list.Add((T)value);
            }
            return list;
        }
    }
}