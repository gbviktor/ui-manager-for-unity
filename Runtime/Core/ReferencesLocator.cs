using System;
using System.Collections.Generic;

namespace com.gbviktor.UIManager.Core
{
    public class ReferencesLocator<T>
    {
        private Dictionary<object, T> refs = null;
        public ReferencesLocator() => refs = new Dictionary<object, T>();

        public bool Add(object key, T reference)
        {
            if (reference == null)
                throw new NullReferenceException("Refrence object is null");

            if (refs.ContainsKey(key))
                return false;

            refs.Add(key, reference);

            return true;
        }
        public bool HasReference(object key) => refs.ContainsKey(key);

        public T Get(object key)
        {
            try
            {
                return refs[key];
            } catch
            {
                throw new NullReferenceException($"Reference with key \"{key}\" muss be added before you try get it");
            }
        }
        public void Reset() => refs.Clear();

        internal void Remove(object key)
        {
            if (refs.ContainsKey(key))
            {
                refs.Remove(key);
            }
        }
    }
}