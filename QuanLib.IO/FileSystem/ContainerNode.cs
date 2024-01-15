using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO.FileSystem
{
    public abstract class ContainerNode : Node, IReadOnlyDictionary<string, Node>
    {
        protected ContainerNode()
        {
            _items = [];
        }

        protected readonly Dictionary<string, Node> _items;

        public Node this[string key] => _items[key];

        public IEnumerable<string> Keys => _items.Keys;

        public IEnumerable<Node> Values => _items.Values;

        public int Count => _items.Count;

        public virtual void Add(Node value)
        {
            _items.Add(value.Name, value);
            value.ParentNode = this;
        }

        public virtual bool Remvoe(string key)
        {
            ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

            if (_items.Remove(key, out var value))
            {
                value.ParentNode = null;
                return true;
            }

            return false;
        }

        public DirectoryNode[] GetDirectoryNodes()
        {
            return _items.Values.OfType<DirectoryNode>().ToArray();
        }

        public FileNode[] GetFileNodes()
        {
            return _items.Values.OfType<FileNode>().ToArray();
        }

        public DirectoryNode? GetAsDirectoryNode(string key)
        {
            return _items.TryGetValue(key, out var value) ? value as DirectoryNode : null;
        }

        public FileNode? GetAsFileNode(string key)
        {
            return _items.TryGetValue(key, out var value) ? value as FileNode : null;
        }

        public bool ContainsKey(string key)
        {
            return _items.ContainsKey(key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out Node value)
        {
            return _items.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, Node>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
        }
    }
}
