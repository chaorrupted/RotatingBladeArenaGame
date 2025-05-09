using UnityEngine;

namespace Utilities
{
    public class ScratchIDGenerator: MonoBehaviour
    {
        private int id = 3; // ids 0, 1, 2, 3 reserved for player and enemies

        public int GetID()
        {
            // Todo: others using this generator will need unique ids. this must be thread safe!
            // https://stackoverflow.com/questions/9011908/how-to-easy-make-this-counter-property-thread-safe
            id++;
            return id;
        }
    }
}