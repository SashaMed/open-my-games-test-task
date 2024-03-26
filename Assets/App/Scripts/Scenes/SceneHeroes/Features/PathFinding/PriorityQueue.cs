using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.App.Scripts.Scenes.SceneHeroes.Features.PathFinding
{
    public class PriorityQueue<TElement, TPriority> where TPriority : IComparable
    {
        private List<(TElement Element, TPriority Priority)> elements = new List<(TElement, TPriority)>();

        public void Enqueue(TElement element, TPriority priority)
        {
            elements.Add((element, priority));
            elements.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }

        public TElement Dequeue()
        {
            var item = elements[0];
            elements.RemoveAt(0);
            return item.Element;
        }

        public int Count => elements.Count;

        public bool Contains(TElement element)
        {
            return elements.Any(e => EqualityComparer<TElement>.Default.Equals(e.Element, element));
        }
    }
}
