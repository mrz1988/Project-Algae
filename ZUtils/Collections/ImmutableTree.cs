namespace ZUtils.Collections
{
    public class ImmutableTree<T>
    {
        public readonly T Value;
        public readonly ImmutableTree<T>[] Children;

        public bool HasChildren => Children.Length != 0;

        public ImmutableTree(T node,
            ImmutableTree<T>[] children)
        {
            Value = node;
            Children = children;
        }

        public ImmutableTree<T> ChildOrDefault(int index)
        {
            if (Children.Length <= index)
                return null;

            return Children[index];
        }
    }
}
